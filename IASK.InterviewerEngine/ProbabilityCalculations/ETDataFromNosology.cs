using System;
using System.Collections.Generic;
using System.Linq;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal class ETDataFromNosology
    {

        private readonly Dictionary<ulong, ETElement> _allElement;

        public Dictionary<ulong, Dictionary<ulong, ulong>> _lookUpToCorrection;

        public Dictionary<ulong, Dictionary<ulong, ulong>> LookUpToCorrection { get => _lookUpToCorrection; }

        public Dictionary<ulong, ETElement> AllElement { get => _allElement; }

        private readonly List<HashSet<ulong>> _listByDeep;
        public List<HashSet<ulong>> ListByDeep { get => _listByDeep; }

        private readonly HashSet<ulong> _rootID;
        public HashSet<ulong> RootIDs { get => _rootID; }

        private readonly HashSet<ulong> _parentId;
        public HashSet<ulong> ParentId { get => _parentId; }

        private readonly HashSet<ulong> _ekvivalentId;
        public HashSet<ulong> EkvivalentId { get => _ekvivalentId; }

        private readonly HashSet<ulong> _binarizationId;
        public HashSet<ulong> BinarizationId { get => _binarizationId; }

        private readonly double _defaultPrevalence;
        public double DefaultPrevalence { get => _defaultPrevalence; }

        private readonly bool _isPrevalence;
        public bool IsPrevalence { get => _isPrevalence; }

        private readonly double _toPrevalenceStep;
        public double ToPrevalenceStep { get => _toPrevalenceStep; }
        internal ETDataFromNosology Copy()
        {
            ETDataFromNosology temp = new ETDataFromNosology(this._isPrevalence, this._defaultPrevalence, this._toPrevalenceStep);

            foreach (var key in _lookUpToCorrection.Keys)
            {
                if (!temp._lookUpToCorrection.ContainsKey(key))
                    temp._lookUpToCorrection.Add(key, new Dictionary<ulong, ulong>());

                foreach (var d in _lookUpToCorrection[key])
                {
                    temp._lookUpToCorrection[key].Add(d.Key, d.Value);
                }
            }

            foreach (ulong key in this._allElement.Keys)
                temp._allElement.Add(key, this._allElement[key].Copy());

            foreach (ulong key in this._rootID)
                temp._rootID.Add(key);

            foreach (HashSet<ulong> hash in this._listByDeep)
            {
                HashSet<ulong> tempLocal = new HashSet<ulong>();

                foreach (ulong key in hash)
                    tempLocal.Add(key);

                temp._listByDeep.Add(tempLocal);
            }

            foreach (ulong key in this._parentId)
                temp._parentId.Add(key);

            foreach (ulong key in this._ekvivalentId)
                temp._ekvivalentId.Add(key);

            foreach (ulong key in this._binarizationId)
                temp._binarizationId.Add(key);

            return temp;
        }
        public ETDataFromNosology(bool isPrevalence, double defaultPrevalence, double toPrevalenceStep)
        {
            this._lookUpToCorrection = new Dictionary<ulong, Dictionary<ulong, ulong>>();
            this._allElement = new Dictionary<ulong, ETElement>();
            this._rootID = new HashSet<ulong>();
            this._listByDeep = new List<HashSet<ulong>>();
            this._parentId = new HashSet<ulong>();
            this._ekvivalentId = new HashSet<ulong>();
            this._binarizationId = new HashSet<ulong>();
            this._defaultPrevalence = defaultPrevalence;
            this._isPrevalence = isPrevalence;
            this._toPrevalenceStep = toPrevalenceStep;
        }


        public void TrySetAnswersFromIsolationSensors(Dictionary<ulong, IsolationSensor> DataToIsolationSensor)
        {
            foreach (ulong id in DataToIsolationSensor.Keys)
            {
                if (DataToIsolationSensor[id].IsTurnOn)
                    TrySetAnswer(DataToIsolationSensor, id);
            }
        }
        public void TrySetAnswer(Dictionary<ulong, IsolationSensor> DataToIsolationSensor, ulong id, double value_a = 1, double value_d = 1)
        {
            if (this.AllElement.ContainsKey(id))
            {
                this.AllElement[id].AddCalcValueByStepBID(id, value_a, value_d);
                this.AllElement[id].ItIsCalculated();
                this.Calc(DataToIsolationSensor);
            }
        }



        public bool Add(ETItem newETItem)
        {
            ETElement temp = new ETElement(newETItem, IsPrevalence, DefaultPrevalence);
            return this.Add(temp);
        }

        public bool Add(params ETItem[] newETItems)
        {
            bool result = false;

            foreach (ETItem item in newETItems)
            {
                ETElement temp = new ETElement(item, IsPrevalence, DefaultPrevalence);
                if (this.Add(temp))
                    result = true;
            }

            return result;
        }

        public bool Add(ETElement newETElement)
        {
            ulong id = newETElement.ID;

            if (!this.AllElement.ContainsKey(id))
            {
                this._allElement.Add(id, newETElement);

                while (this._listByDeep.Count < newETElement.Deep + 1)
                    this._listByDeep.Add(new HashSet<ulong>());

                this._listByDeep[newETElement.Deep].Add(id);

                if (newETElement.Type == 1)
                    this._rootID.Add(id);

                this.AddConnectByETElement(newETElement);

                return true;
            }
            else
            {
                if (newETElement.StepToA.Keys.Count == 0)
                    return false;

                return this.AddConnectByETElement(newETElement);
            }
        }

        public bool AddConnectByETElement(ETElement eTElement)
        {
            if (!this.AllElement.ContainsKey(eTElement.ID))
                return false;

            bool result = false;

            foreach (ulong ID_A in eTElement.StepToA.Keys)
            {
                if (AddConnectByID_A_StepA(eTElement, ID_A))
                    result = true;

                if (AddConnectByID_A_StepB(eTElement, ID_A))
                    result = true;
            }

            return result;
        }

        private bool AddConnectByID_A_StepB(ETElement eTElement, ulong ID_A)
        {
            if (!this.AllElement.ContainsKey(ID_A))
            {
                throw new Exception();
            }

            if (!this.AllElement[ID_A].StepToB.ContainsKey(eTElement.ID))
            {
                if (ID_A == eTElement.ID)
                    return false;

                this._allElement[ID_A].AddStepToB(eTElement.ID, this.AllElement[eTElement.ID].StepToA[ID_A]);
                this._allElement[eTElement.ID].AddToCountRootID(this.AllElement[ID_A]);

                return true;
            }

            return false;
        }

        private bool AddConnectByID_A_StepA(ETElement eTElement, ulong ID_A)
        {
            if (!this.AllElement[eTElement.ID].StepToA.ContainsKey(ID_A))
            {
                if (ID_A == eTElement.ID)
                {
                    if (eTElement.Type == 1)
                        this._allElement[eTElement.ID].AddStepToA(ID_A, eTElement.StepToA[ID_A]);
                    else
                        return false;
                }
                else
                    this._allElement[eTElement.ID].AddStepToA(ID_A, eTElement.StepToA[ID_A]);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Метод подсчёта вероятностей.
        /// </summary>
        public void Calc(Dictionary<ulong, IsolationSensor> DataToIsolationSensor)
        {
            foreach (ulong id in this.RootIDs)
            {
                this.AllElement[id].ResetCalc();
                this.AllElement[id].ClearCalcValueByStepB();
                CalcValueByID(DataToIsolationSensor, id);
            }
        }

        private bool CalcValueByID(Dictionary<ulong, IsolationSensor> DataToIsolationSensor, ulong id)
        {
            if (this._allElement[id].IsWaitCalc)
                return false;

            if (!this.AllElement[id].IsCalc)
            {
                this._allElement[id].ToWaitCalc();

                if (this.AllElement[id].StepToB.Count > 0)
                {
                    foreach (ulong ID_B in this.AllElement[id].StepToB.Keys)
                    {
                        if (!this.AllElement[ID_B].IsCalc)
                            if (!CalcValueByID(DataToIsolationSensor, ID_B))
                            {
                                //this._allElement[id].ToDoneWaiting();
                                //return false;
                                //break;
                            }

                        if (this.AllElement[ID_B].IsCalc)
                        {
                            CheckerValue checker = new CheckerValue();
                            ETItem eTItem = this.AllElement[id].StepToB[ID_B];
                            if (eTItem.Correction.Count > 0)
                            {
                                if (DataToIsolationSensor == null)
                                    continue;
                                if (DataToIsolationSensor.ContainsKey(ID_B))
                                {
                                    IsolationSensor iSensor = DataToIsolationSensor[ID_B];
                                    if (_lookUpToCorrection.ContainsKey(ID_B))
                                        if (_lookUpToCorrection[ID_B].ContainsKey(eTItem.id))
                                        {
                                            ulong key = _lookUpToCorrection[ID_B][eTItem.id];
                                            bool? res = eTItem.IsCorrectionTurnOn(iSensor.sensor, true, iSensor.ValueD);
                                            if (res == null || !(bool)res)
                                                continue;
                                        }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            double value_A;
                            if (checker.CheckValue_a(eTItem.Value_A))
                                value_A = checker.GetCheckedValue_a(eTItem.Value_A) * this.AllElement[ID_B].CalcValueSum_AToStepA;
                            else
                                value_A = .0;

                            double value_D;
                            if (checker.CheckValue_d(eTItem.Value_D))
                                value_D = checker.GetCheckedValue_d(eTItem.Value_D) * this.AllElement[ID_B].CalcValueSum_DToStepA;
                            else
                                value_D = .0;

                            this.AllElement[id].AddCalcValueByStepBID(ID_B, value_A, value_D);
                            //this.AllElement[id].AddToCountRootID(this.AllElement[ID_A]);
                        }
                    }
                }
                else
                {
                    this.AllElement[id].ToDoneWaiting();
                    return false;
                }

                CheckTryPutPrevalence(id);

                if (this.AllElement[id].CalcValueByID_B.Count > 0)
                {
                    this.AllElement[id].ItIsCalculated();
                }

                this.AllElement[id].ToDoneWaiting();
            }
            return true;
        }

        public bool SetCalcValueByID(ulong id, double value_a, double value_d)
        {
            this.AllElement[id].AddCalcValueByStepBID(id, value_a, value_d);
            return true;
        }

        private bool CheckTryPutPrevalence(ulong id)
        {
            bool temp = false;
            Dictionary<ulong, HashSet<ulong>> parents = new Dictionary<ulong, HashSet<ulong>>();
            parents.Add(id, new HashSet<ulong>());
            HashSet<ulong> step = new HashSet<ulong>();

            foreach (ulong idParent in this.AllElement[id].ParentIDs)
            {
                if (!parents.ContainsKey(idParent))
                    parents.Add(idParent, new HashSet<ulong>());
                parents[idParent].Add(id);
                step.Add(idParent);
            }

            while (step.Count > 0)
            {
                HashSet<ulong> newStep = new HashSet<ulong>();

                foreach (ulong idb in step)
                {
                    if (this.AllElement.ContainsKey(idb))
                    {
                        if (this.AllElement[idb].IsMyPrevalence || this.AllElement[idb].ContainsParentPrevalence)
                        {
                            return PutPrevalence(idb, parents);
                        }
                        else
                        {
                            foreach (ulong idParent in this.AllElement[idb].ParentIDs)
                            {
                                if (!parents.ContainsKey(idParent))
                                    parents.Add(idParent, new HashSet<ulong>());

                                if (parents[idParent].Add(idb))
                                    newStep.Add(idParent);
                            }
                        }
                    }
                }
                step = newStep;
            }
            return temp;
        }

        internal void AddCorrection(ulong ida, ulong id, ulong idb)
        {
            if (!LookUpToCorrection.ContainsKey(ida))
                LookUpToCorrection.Add(ida, new Dictionary<ulong, ulong>());
            if (!LookUpToCorrection[ida].ContainsKey(id))
                LookUpToCorrection[ida].Add(id, idb);
        }

        private bool PutPrevalence(ulong idParent, Dictionary<ulong, HashSet<ulong>> dicParentIDs)
        {
            bool temp = false;
            double Prevalence = this.AllElement[idParent].Prevalence * (1 - ToPrevalenceStep);
            HashSet<ulong> step = new HashSet<ulong>();

            foreach (ulong ida in dicParentIDs[idParent])
                step.Add(ida);

            while (step.Count > 0)
            {
                HashSet<ulong> newStep = new HashSet<ulong>();

                foreach (ulong ida in step)
                {
                    if (!this.AllElement[ida].IsMyPrevalence && !this.AllElement[ida].ContainsParentPrevalence)
                    {
                        foreach (ulong newIDA in dicParentIDs[ida])
                            if (!newStep.Contains(newIDA))
                                newStep.Add(newIDA);

                        if (this.AllElement.ContainsKey(ida))
                        {
                            if (this.AllElement[ida].SetParentPrevalence(Prevalence))
                                temp = true;
                        }
                    }
                }

                Prevalence *= (1 - ToPrevalenceStep);
                step = newStep;
            }

            return temp;
        }

        /// <summary>
        /// Метод для получения списка элементов по родовидовым связям.
        /// </summary>
        /// <param name="relateIDHash">Хеши BigId по которых необходим запрос для родовидовых связей.</param>
        /// <param name="NewIsParent">Учёт родо-видовой связи по предкам.</param>
        /// <param name="NewToParent">Содержит значение % в долях, вычитания за каждый шаг по родо-видовой связи по предкам.</param>
        /// <param name="NewDeepParent">Глубина вхождения графа по родо-видовой связи по предкам.</param>
        /// <param name="NewIsEkvivalent">Учёт Эквивалентной связи.</param>
        /// <param name="IsZeroStep">Является запрос для нулевого шага.</param>
        /// <param name="NewIsBinarization">Учёт Бинаризации.</param>
        /// <param name="NewToBinarization">Содержит значение % в долях, вычитания за каждый шаг по связи Бинаризации.</param>
        /// <returns>Список элементов по родовидовым связям.</returns>
        public List<Alert> AddRelateMain(
            string graphpass,
            HashSet<ulong> relateIDHash,
            bool NewIsParent,
            double NewToParent,
            short NewDeepParent,
            bool NewIsEkvivalent,
            bool NewIsBinarization,
            double NewToBinarization)
        {
            List<Alert> alerts = new List<Alert>();

            HashSet<ulong> newRelateIDHash = new HashSet<ulong>();
            HashSet<ulong> newEkvivalentIDHash = new HashSet<ulong>();
            HashSet<ulong> newBinarizationIDHash = new HashSet<ulong>();

            foreach (ulong key in relateIDHash)
            {
                if (!EkvivalentId.Contains(key))
                    newEkvivalentIDHash.Add(key);
                else
                { }

                if (!ParentId.Contains(key))
                    newRelateIDHash.Add(key);
                else
                { }

                if (!BinarizationId.Contains(key))
                    newBinarizationIDHash.Add(key);
                else
                { }
            }

            if (NewIsEkvivalent /*&& (alerts.Count == 0 || alerts[0].code == 200)*/)
                this.AddETDataEkvivalentRelate(ref alerts, ref newEkvivalentIDHash, graphpass);

            foreach (ulong key in newEkvivalentIDHash)
            {
                if (!newBinarizationIDHash.Contains(key) && !BinarizationId.Contains(key))
                    newBinarizationIDHash.Add(key);
                else
                { }
            }

            HashSet<ulong> newStepEkvivalentIDHash = new HashSet<ulong>();

            if (NewIsParent /*&& (alerts.Count == 0 || alerts[0].code == 200)*/)
            {
                HashSet<ulong> temp = this.GetAndAddETDataRelateParent(
                    ref alerts,
                    newRelateIDHash,
                    "Parent",
                    NewToParent,
                    NewDeepParent,
                    ParentId,
                    true,
                    graphpass);

                foreach (ulong id in temp)
                    newStepEkvivalentIDHash.Add(id);
            }

            foreach (ulong key in relateIDHash)
            {
                if (!EkvivalentId.Contains(key))
                    newStepEkvivalentIDHash.Add(key);

                if (!newBinarizationIDHash.Contains(key) && !BinarizationId.Contains(key))
                    newBinarizationIDHash.Add(key);
            }

            if (NewIsEkvivalent /*&& (alerts.Count == 0 || alerts[0].code == 200)*/)
                this.AddETDataEkvivalentRelate(ref alerts, ref newStepEkvivalentIDHash, graphpass);

            foreach (ulong key in newStepEkvivalentIDHash)
            {
                if (!newBinarizationIDHash.Contains(key) && !BinarizationId.Contains(key))
                    newBinarizationIDHash.Add(key);
            }

            if (NewIsBinarization)
                this.AddETDataBinarizationRelate(ref alerts, ref newBinarizationIDHash, NewToBinarization, graphpass);

            return alerts;
        }

        /// <summary>
        /// Метод для получения списка элементов по определённой родовидовой связи.
        /// </summary>
        /// <param name="alerts">Общий список ошибок.</param>
        /// <param name="MainIDHash">Хеши BigId по которых необходимо сделать запрос.</param>
        /// <param name="nameRelate">Название родовидовой связи.</param>
        /// <param name="toMinus">Кол-во потери веса связи.</param>
        /// <param name="deep">Глубина шагов.</param>
        /// <param name="HSRelate">ПЕРЕДАТЬ относящиеся к предкам или потомкам свойство структуры. Хеш BigId свойство структуры для которой запоминается вершины, что это родовидовая вершина.</param>
        /// <param name="isRequestParents">Поиск по предуам.</param>
        /// <returns></returns>
        private HashSet<ulong> GetAndAddETDataRelateParent(
            ref List<Alert> alerts,
            HashSet<ulong> MainIDHash,
            string nameRelate,
            double toMinus,
            short deep,
            HashSet<ulong> HSRelate,
            bool isRequestParents,
            string graphpass)
        {
            HashSet<ulong> result = new HashSet<ulong>();

            HashSet<ulong> ignore = new HashSet<ulong>();

            ResultSemanticNewBigId ResultSemanticNewBigId = CacheForming.GetToParentByListID(graphpass, /*relateIDHash.ToList()*/ MainIDHash.ToList(), deep);

            if (ResultSemanticNewBigId.alerts != null && ResultSemanticNewBigId.alerts.code == 200)
            {
                alerts.Add(ResultSemanticNewBigId.alerts);

                if (ResultSemanticNewBigId.graph.Count > 0)
                    foreach (PlexNewBigId PlexNewBigId in ResultSemanticNewBigId.graph)
                    {
                        ETItem temp = CacheForming.CreateETItem(PlexNewBigId, ResultSemanticNewBigId, true);
                        if (temp != null)
                        {
                            if (temp.deep > 0)
                            {
                                if (temp.type == 0)
                                {
                                    temp.id = temp.idb;
                                    //relateIDHash.Add(temp.idb);
                                }
                                else if (temp.type == 1)
                                {
                                    //relateIDHash.Add(temp.id);
                                }

                                temp.name += "_!" + nameRelate;

                                if (Constants.Level_bSymptoms.Contains(temp.levelb))
                                    temp.Value_A = 1 - toMinus;
                                else
                                    temp.Value_A = 1;

                                //////////if (!ETGetPlexus.Level_bNext.Contains(temp.levelb) || ETGetPlexus.Level_bStop.Contains(temp.levelb)) // Времено паока что
                                //////////    ignore.Add(temp.idb);

                                if (!ignore.Contains(temp.ida))
                                {
                                    ETElement eT = new ETElement(temp, IsPrevalence, DefaultPrevalence);

                                    if (isRequestParents)
                                        eT.AddParentIDs(GetParentsIDs(eT.ID, ResultSemanticNewBigId.graph));

                                    result.Add(eT.ID);

                                    if (!AllElement.ContainsKey(eT.ID) && !HSRelate.Contains(eT.ID))
                                        HSRelate.Add(eT.ID);

                                    Add(eT);
                                }
                                else
                                    ignore.Add(temp.idb);
                            }
                            else
                            {
                                if (isRequestParents)
                                    if (temp.deep == 0 && temp.type == 1 && AllElement.ContainsKey(temp.id))
                                        AllElement[temp.id].AddParentIDs(GetParentsIDs(temp.id, ResultSemanticNewBigId.graph));
                            }
                        }
                        else
                        {
                            ignore.Add(PlexNewBigId.idb);
                            //break; //-------------------------------
                        }
                    }
            }
            else if (ResultSemanticNewBigId.alerts != null)
            {
                alerts = new List<Alert>() { ResultSemanticNewBigId.alerts };
            }
            else
            {
                alerts = new List<Alert>() { new Alert()
                {
                    code = 500,
                    title = "Internal Server Error",
                    level = "error",
                    sticky = false,
                    message = "Не вернул Alert. метод GetRelate по связи: " + nameRelate
                } };
            }
            return result;
        }

        /// <summary>
        /// Метод для получения списка элементов по Эквивалентной связи.
        /// </summary>
        /// <param name="alerts">Общий список ошибок.</param>
        /// <param name="MainIDHash">Хеши BigId по которых необходимо сделать запрос.</param>
        private void AddETDataEkvivalentRelate(
            ref List<Alert> alerts,
            ref HashSet<ulong> MainIDHash,
            string graphpass)
        {
            string nameRelate = "Ekvivalent";

            HashSet<ulong> ignore = new HashSet<ulong>();

            ResultSemanticNewBigId ResultSemanticNewBigId = CacheForming.GetByEkvivalent(graphpass, MainIDHash.ToList(), 1);

            if (ResultSemanticNewBigId.alerts != null && ResultSemanticNewBigId.alerts.code == 200)
            {
                alerts.Add(ResultSemanticNewBigId.alerts);

                if (ResultSemanticNewBigId.graph != null && ResultSemanticNewBigId.graph.Count > 0)
                    foreach (PlexNewBigId PlexNewBigId in ResultSemanticNewBigId.graph)
                    {
                        ETItem temp = CacheForming.CreateETItem(PlexNewBigId, ResultSemanticNewBigId, true);
                        if (temp != null)
                        {
                            if (temp.deep > 0)
                            {
                                if (temp.type == 0)
                                {
                                    temp.id = temp.idb;
                                    MainIDHash.Add(temp.idb);
                                }
                                else if (temp.type == 1)
                                    MainIDHash.Add(temp.id);

                                temp.name += "_!" + nameRelate;

                                if (temp.deep > 0)
                                    temp.Value_A = 1;

                                //////////if (!ETGetPlexus.Level_bNext.Contains(temp.levelb) || ETGetPlexus.Level_bStop.Contains(temp.levelb))
                                //////////    ignore.Add(temp.idb);

                                if (!ignore.Contains(temp.ida))
                                {
                                    ETElement eT = new ETElement(temp, IsPrevalence, DefaultPrevalence);
                                    eT.AddEkvivalentIDs(GetEkvivalintIDs(eT.ID, ResultSemanticNewBigId.graph));

                                    ETElement eTReverse = new ETElement(temp.GetReverseID_AB(), IsPrevalence, DefaultPrevalence);
                                    eTReverse.AddEkvivalentIDs(GetEkvivalintIDsReverse(eTReverse.ID, ResultSemanticNewBigId.graph));

                                    if (!AllElement.ContainsKey(eT.ID) && !EkvivalentId.Contains(eT.ID))
                                        EkvivalentId.Add(eT.ID);

                                    this.Add(eT);
                                    this.Add(eTReverse);
                                }
                                else
                                    ignore.Add(temp.idb);
                            }
                            else
                            {
                                if (temp.deep == 0 && temp.type == 1 && AllElement.ContainsKey(temp.id))
                                    AllElement[temp.id].AddEkvivalentIDs(GetEkvivalintIDs(temp.id, ResultSemanticNewBigId.graph));
                            }
                        }
                        else
                        {
                            ignore.Add(PlexNewBigId.idb);
                        }
                    }
            }
            else if (ResultSemanticNewBigId.alerts != null)
            {
                alerts = new List<Alert>() { ResultSemanticNewBigId.alerts };
            }
            else
            {
                alerts = new List<Alert>() { new Alert()
                {
                    code = 500,
                    title = "Internal Server Error",
                    level = "error",
                    sticky = false,
                    message = "Не вернул Alert. метод GetByEkvivalent по связи: " + nameRelate
                } };
            }
        }

        /// <summary>
        /// Метод для получения списка элементов по Бинаризационной связи.
        /// </summary>
        /// <param name="alerts">Общий список ошибок.</param>
        /// <param name="MainIDHash">Хеши BigId по которых необходимо сделать запрос.</param>
        /// <param name="NewToBinarization">Содержит значение % в долях, вычитания за каждый шаг по связи Бинаризации.</param>
        private void AddETDataBinarizationRelate(ref List<Alert> alerts, ref HashSet<ulong> MainIDHash, double NewToBinarization, string graphpass)
        {
            string nameRelate = "Binarization";

            ResultSemanticNewBigId ResultSemanticNewBigId = CacheForming.GetByBinarization(graphpass, MainIDHash.ToList());

            if (ResultSemanticNewBigId.alerts != null && ResultSemanticNewBigId.alerts.code == 200)
            {
                alerts.Add(ResultSemanticNewBigId.alerts);

                if (ResultSemanticNewBigId.graph != null && ResultSemanticNewBigId.graph.Count > 0)
                    foreach (PlexNewBigId PlexNewBigId in ResultSemanticNewBigId.graph)
                    {
                        ETItem temp = CacheForming.CreateETItem(PlexNewBigId, ResultSemanticNewBigId, true);
                        if (temp != null)
                        {
                            if (temp.type == 2 && MainIDHash.Contains(temp.id))
                            {
                                string strTemp = temp.name_ida;
                                temp.idb = temp.ida;
                                temp.name_ida = temp.name;
                                temp.ida = temp.id;
                                temp.name = strTemp;

                                temp.levelb = 0;
                                temp.levelbName = string.Empty;

                                temp.type = 0;
                                temp.name += "_!" + nameRelate;
                                temp.Value_A = 1 - NewToBinarization;

                                ETElement eT = new ETElement(temp, IsPrevalence, DefaultPrevalence);
                                eT.AddBinarizationIDs(GetBinarizationIDs(temp.id, ResultSemanticNewBigId.graph));

                                if (!AllElement.ContainsKey(eT.ID) && !BinarizationId.Contains(eT.ID))
                                    BinarizationId.Add(eT.ID);

                                Add(eT);
                            }
                        }
                    }
            }
            else if (ResultSemanticNewBigId.alerts != null)
            {
                alerts = new List<Alert>() { ResultSemanticNewBigId.alerts };
            }
            else
            {
                alerts = new List<Alert>() { new Alert()
                    {
                        code = 500,
                        title = "Internal Server Error",
                        level = "error",
                        sticky = false,
                        message = "Не вернул Alert. метод GetBinarizationRelate по связи: " + nameRelate
                    } };
            }
        }

        private HashSet<ulong> GetParentsIDs(ulong iD, List<PlexNewBigId> graph)
        {
            HashSet<ulong> temp = new HashSet<ulong>();

            foreach (PlexNewBigId PlexNewBigId in graph)
                if (PlexNewBigId.ida == iD)
                    if (!temp.Contains(PlexNewBigId.idb))
                        temp.Add(PlexNewBigId.idb);

            return temp;
        }

        private HashSet<ulong> GetEkvivalintIDsReverse(ulong iD, List<PlexNewBigId> graph)
        {
            HashSet<ulong> temp = new HashSet<ulong>();

            foreach (PlexNewBigId PlexNewBigId in graph)
                if (PlexNewBigId.idb == iD)
                    if (!temp.Contains(PlexNewBigId.ida))
                        temp.Add(PlexNewBigId.ida);

            return temp;
        }

        private HashSet<ulong> GetEkvivalintIDs(ulong iD, List<PlexNewBigId> graph)
        {
            HashSet<ulong> temp = new HashSet<ulong>();

            foreach (PlexNewBigId PlexNewBigId in graph)
                if (PlexNewBigId.ida == iD)
                    if (!temp.Contains(PlexNewBigId.idb))
                        temp.Add(PlexNewBigId.idb);

            return temp;
        }

        private HashSet<ulong> GetBinarizationIDs(ulong iD, List<PlexNewBigId> graph)
        {
            HashSet<ulong> temp = new HashSet<ulong>();

            foreach (PlexNewBigId PlexNewBigId in graph)
                if (PlexNewBigId.id == iD)
                    if (!temp.Contains(PlexNewBigId.ida))
                        temp.Add(PlexNewBigId.ida);

            return temp;
        }

        public List<Alert> FillPrevalence(string graphpass)
        {
            List<Alert> alerts = new List<Alert>();

            ResultSemanticNewBigId plexusPFR = CacheForming.GetByPrevalence(graphpass, AllElement.Keys.ToList()) ?? new ResultSemanticNewBigId();
            if (plexusPFR.alerts != null)
            {
                alerts.Add(plexusPFR.alerts);

                if (plexusPFR.alerts.code == 200)
                    if (plexusPFR.graph.Count > 0)
                        foreach (PlexNewBigId PlexNewBigId in plexusPFR.graph)
                        {
                            if (AllElement.ContainsKey(PlexNewBigId.ida))
                            {
                                //if (GetPlexus.GetPrevalenceIDMain == PlexNewBigId.idb)
                                AllElement[PlexNewBigId.ida].TryAddPrevalence(PlexNewBigId);
                                //else if (listPPID.Count > 0)
                                //{
                                //    foreach (PersonParameterId person in listPPID)
                                //        if (person.IdConcepts.Contains(PlexNewBigId.idb))
                                //            eTSession.DataToRiskFactor.AllElement[PlexNewBigId.ida].TryAddPrevalence(PlexNewBigId);
                                //}
                            }

                        }
            }
            else
            {
                alerts.Add(new Alert()
                {
                    code = 500,
                    title = "Internal Server Error",
                    level = "error",
                    sticky = false,
                    message = "Запрос Заболеваемости Не Вернул Alerts \n"
                });
            }
            return alerts;
        }

    }
}
