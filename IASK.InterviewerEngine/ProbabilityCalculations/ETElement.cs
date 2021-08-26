using System;
using System.Collections.Generic;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    internal class ETElement
    {
        #region fields and properities
        private readonly ETItem MasterETItem;//todo удалить

        public Dictionary<ulong, ETItem> StepToA = new Dictionary<ulong, ETItem>();

        public Dictionary<ulong, ETItem> StepToB = new Dictionary<ulong, ETItem>();

        public ulong ID;
        public short Type;

        public short Deep;

        public string Name;

        public Dictionary<ulong, string> CountRootID;

        private bool _isCalc;
        public bool IsCalc { get => _isCalc; }

        private bool _isWaitCalc;
        public bool IsWaitCalc { get => _isWaitCalc; }
        public HashSet<ulong> ParentIDs;

        public HashSet<ulong> EkvivalentIDs;

        public HashSet<ulong> BinarizationIDs;

        private readonly double _defaultPrevalence;
        public bool ContainsParentPrevalence;

        public double ParentPrevalence;

        public double Prevalence
        {
            get
            {
                if (!IsPrevalenceCalc)
                    return 1.0;

                CheckerValue checker = new CheckerValue();

                if (this.Prevalences.Count > 0)
                {
                    double valuePrevalence = 0.0;
                    int count = 0;
                    foreach (ulong id in Prevalences.Keys)
                    {
                        double valueIdPrevalence = 0.0;
                        int idCount = 0;
                        foreach (PlexNewBigId PlexNewBigId in Prevalences[id].Values)
                            if (checker.CheckPrevalenceValue_a(PlexNewBigId.value_a))
                            {
                                double temp = checker.GetCheckedPrevalenceValue_a(PlexNewBigId.value_a);
                                if (Math.Abs(-1.0 - temp) <= double.Epsilon)
                                    return -1.0;

                                valueIdPrevalence += temp;
                                idCount++;
                            }

                        if (idCount > 0)
                        {
                            valuePrevalence += valueIdPrevalence / idCount;
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        valuePrevalence /= count;
                        return valuePrevalence;
                    }
                }

                if (this.MainPrevalences.Count > 0)
                {
                    double valuePrevalence = 0.0;
                    int count = 0;
                    foreach (PlexNewBigId PlexNewBigId in MainPrevalences.Values)
                        if (checker.CheckPrevalenceValue_a(PlexNewBigId.value_a))
                        {
                            double temp = checker.GetCheckedPrevalenceValue_a(PlexNewBigId.value_a);
                            if (Math.Abs(-1.0 - temp) > double.Epsilon)
                            {
                                valuePrevalence += temp;
                                count++;
                            }
                        }
                    if (count > 0)
                    {
                        valuePrevalence /= count;
                        return valuePrevalence;
                    }
                }

                if (this.ContainsParentPrevalence)
                    return this.ParentPrevalence;

                return _defaultPrevalence;
            }
        }

        public bool IsPrevalenceCalc;

        public bool IsMyPrevalence
        {
            get
            {
                CheckerValue checker = new CheckerValue();

                if (this.Prevalences.Count > 0)
                    foreach (ulong id in Prevalences.Keys)
                        foreach (PlexNewBigId PlexNewBigId in Prevalences[id].Values)
                            if (checker.CheckPrevalenceValue_a(PlexNewBigId.value_a))
                                return true;

                if (this.MainPrevalences.Count > 0)
                    foreach (PlexNewBigId PlexNewBigId in MainPrevalences.Values)
                        if (checker.CheckPrevalenceValue_a(PlexNewBigId.value_a))
                            return true;

                return false;
            }
        }

        public Dictionary<ulong, PlexNewBigId> MainPrevalences = new Dictionary<ulong, PlexNewBigId>();

        public Dictionary<ulong, Dictionary<ulong, PlexNewBigId>> Prevalences = new Dictionary<ulong, Dictionary<ulong, PlexNewBigId>>();



        internal Dictionary<ulong, DataCalcValue> CalcValueByID_A;


        internal Dictionary<ulong, DataCalcValue> CalcValueByID_B;


        #endregion

        internal ETElement Copy()
        {
            ETElement temp = new ETElement(this.MasterETItem, this.IsPrevalenceCalc, this._defaultPrevalence);

            foreach (ulong key in this.CountRootID.Keys)
                if (!temp.CountRootID.ContainsKey(key))
                    temp.CountRootID.Add(key, this.CountRootID[key]);

            foreach (ulong key in this.StepToA.Keys)
                if (!temp.StepToA.ContainsKey(key))
                    temp.StepToA.Add(key, this.StepToA[key].Copy());
                else
                    temp.StepToA[key] = this.StepToA[key].Copy();

            foreach (ulong key in this.StepToB.Keys)
                if (!temp.StepToB.ContainsKey(key))
                    temp.StepToB.Add(key, this.StepToB[key].Copy());
                else
                    temp.StepToB[key] = this.StepToB[key].Copy();

            temp._isWaitCalc = this._isWaitCalc;
            temp._isCalc = this._isCalc;

            foreach (ulong key in this.CalcValueByID_A.Keys)
                if (!temp.CalcValueByID_A.ContainsKey(key))
                    temp.CalcValueByID_A.Add(key, this.CalcValueByID_A[key].Copy());
                else
                    temp.CalcValueByID_A[key] = this.CalcValueByID_A[key].Copy();

            foreach (ulong key in this.CalcValueByID_B.Keys)
                if (!temp.CalcValueByID_B.ContainsKey(key))
                    temp.CalcValueByID_B.Add(key, this.CalcValueByID_B[key].Copy());
                else
                    temp.CalcValueByID_B[key] = this.CalcValueByID_B[key].Copy();

            foreach (ulong key in this.ParentIDs)
                if (!temp.ParentIDs.Contains(key))
                    temp.ParentIDs.Add(key);

            foreach (ulong key in this.EkvivalentIDs)
                if (!temp.EkvivalentIDs.Contains(key))
                    temp.EkvivalentIDs.Add(key);

            foreach (ulong key in this.BinarizationIDs)
                if (!temp.BinarizationIDs.Contains(key))
                    temp.BinarizationIDs.Add(key);

            temp.ContainsParentPrevalence = this.ContainsParentPrevalence;
            temp.ParentPrevalence = ParentPrevalence;

            foreach (ulong key in this.Prevalences.Keys)
            {
                Dictionary<ulong, PlexNewBigId> tempLocal = new Dictionary<ulong, PlexNewBigId>();

                foreach (ulong keyLocal in this.Prevalences[key].Keys)
                    tempLocal.Add(keyLocal, this.Prevalences[key][keyLocal].Copy());

                temp.Prevalences.Add(key, tempLocal);
            }

            foreach (ulong key in this.MainPrevalences.Keys)
                temp.MainPrevalences.Add(key, this.MainPrevalences[key].Copy());

            temp.CalcValueSum_A_Forced = this.CalcValueSum_A_Forced;
            temp.CalcValueSum_D_Forced = this.CalcValueSum_D_Forced;

            return temp;
        }

        public ETElement(ETItem eTItem, bool isPrevalence, double defaultPrevalence)
        {
            if (eTItem != null)
            {
                MasterETItem = eTItem.Copy();
                Type = eTItem.type;
                Deep = eTItem.deep;
                Name = eTItem.name;
                CountRootID = new Dictionary<ulong, string>();
                StepToA = new Dictionary<ulong, ETItem>();
                StepToB = new Dictionary<ulong, ETItem>();
                _isWaitCalc = false;
                _isCalc = false;
                CalcValueByID_A = new Dictionary<ulong, DataCalcValue>();
                CalcValueByID_B = new Dictionary<ulong, DataCalcValue>();
                ParentIDs = new HashSet<ulong>();
                EkvivalentIDs = new HashSet<ulong>();
                BinarizationIDs = new HashSet<ulong>();
                IsPrevalenceCalc = isPrevalence;
                ContainsParentPrevalence = false;
                ParentPrevalence = new double();
                Prevalences = new Dictionary<ulong, Dictionary<ulong, PlexNewBigId>>();
                MainPrevalences = new Dictionary<ulong, PlexNewBigId>();
                _defaultPrevalence = defaultPrevalence;
                CalcValueSum_A_Forced = new double();
                CalcValueSum_D_Forced = new double();

                if (Type == 0)
                {
                    if (eTItem.route == 2)
                    {
                        ID = eTItem.idb;
                        StepToA.Add(eTItem.ida, eTItem);
                    }
                    else if (eTItem.route == 1)
                    {
                        ID = eTItem.idb;
                        StepToA.Add(eTItem.ida, eTItem);
                    }
                    else if (eTItem.route == 0)
                    {
                        ID = eTItem.idb;
                        StepToA.Add(eTItem.ida, eTItem);
                    }
                    else
                    {
                        ID = eTItem.idb;
                        StepToA.Add(eTItem.ida, eTItem);
                    }
                }
                else if (Type == 1)
                {
                    ID = eTItem.id;
                    CountRootID.Add(this.ID, this.Name);
                    this.SetCalcForType1((double)eTItem.Value_A, (double)eTItem.Value_D);
                    this.ItIsCalculated();
                }
                else { ID = new ulong(); }
            }
        }

        #region calc

        private double CalcValueSum_A_Forced { get; set; }

        public double CalcValueSum_AToStepB
        {
            get
            {
                if (IsCalc)
                {
                    if (this.Type == 1)
                        return this.CalcValueSum_A_Forced;

                    Calculations calculations = new Calculations();
                    List<double> temp = new List<double>();

                    foreach (ulong id_a in this.CalcValueByID_A.Keys)
                        temp.Add(this.CalcValueByID_A[id_a].CalcValue_A);

                    double value = calculations.SummVeroiat(temp.ToArray());

                    if (this.IsPrevalenceCalc)
                    {
                        if (this.IsMyPrevalence)
                        {
                            if (Math.Abs(-1.0 - Prevalence) > double.Epsilon)
                                return Math.Pow(value, (1 / Prevalence));
                            else
                                return 0.0;
                        }

                        if (this.ContainsParentPrevalence)
                        {
                            if (Math.Abs(-1.0 - ParentPrevalence) > double.Epsilon)
                                return Math.Pow(value, (1 / ParentPrevalence));
                            else
                                return 0.0;
                        }

                        return Math.Pow(value, (1 / _defaultPrevalence));
                    }

                    return value;
                }
                else { return 0; }
            }
        }

        public double CalcValueSum_AToStepA
        {
            get
            {
                if (IsCalc)
                {
                    //if (this.Type == 1)
                    //    return this.CalcValueSum_A_Forced;

                    Calculations calculations = new Calculations();
                    List<double> temp = new List<double>();

                    foreach (ulong id_b in this.CalcValueByID_B.Keys)
                        temp.Add(this.CalcValueByID_B[id_b].CalcValue_A);

                    double value = calculations.SummVeroiat(temp.ToArray());

                    if (this.IsPrevalenceCalc)
                    {
                        if (this.IsMyPrevalence)
                        {
                            if (Math.Abs(-1.0 - Prevalence) > double.Epsilon)
                                return Math.Pow(value, (1 / Prevalence));
                            else
                                return 0.0;
                        }

                        if (this.ContainsParentPrevalence)
                        {
                            if (Math.Abs(-1.0 - ParentPrevalence) > double.Epsilon)
                                return Math.Pow(value, (1 / ParentPrevalence));
                            else
                                return 0.0;
                        }

                        return Math.Pow(value, (1 / _defaultPrevalence));
                    }

                    return value;
                }
                else { return 0; }
            }
        }

        private double CalcValueSum_D_Forced { get; set; }

        public double CalcValueSum_DToStepB
        {
            get
            {
                if (IsCalc)
                {
                    if (this.Type == 1)
                        return this.CalcValueSum_D_Forced;

                    Calculations calculations = new Calculations();
                    List<double> temp = new List<double>();

                    foreach (ulong id_a in this.CalcValueByID_A.Keys)
                        temp.Add(this.CalcValueByID_A[id_a].CalcValue_D);

                    return calculations.SummVeroiat(temp.ToArray());
                }
                else { return 0; }
            }
        }

        public double CalcValueSum_DToStepA
        {
            get
            {
                if (IsCalc)
                {
                    //if (this.Type == 1)
                    //    return this.CalcValueSum_D_Forced;

                    Calculations calculations = new Calculations();
                    List<double> temp = new List<double>();

                    foreach (ulong id_b in this.CalcValueByID_B.Keys)
                        temp.Add(this.CalcValueByID_B[id_b].CalcValue_D);

                    return calculations.SummVeroiat(temp.ToArray());
                }
                else { return 0; }
            }
        }

        public bool AddStepToB(ulong ID_B, ETItem eTItem)
        {
            if (!this.StepToB.ContainsKey(ID_B))
            {
                this.StepToB.Add(ID_B, eTItem);
                return true;
            }
            else return false;
        }

        public bool AddStepToA(ulong ID_A, ETItem eTItem)
        {
            if (!this.StepToA.ContainsKey(ID_A))
            {
                this.StepToA.Add(ID_A, eTItem);
                return true;
            }
            else return false;
        }

        public bool AddToCountRootID(ETElement eTElement)
        {
            bool temp = false;

            foreach (ulong key in eTElement.CountRootID.Keys)
            {
                if (!this.CountRootID.ContainsKey(key))
                {
                    this.CountRootID.Add(key, eTElement.CountRootID[key]);
                    temp = true;
                }
                if (!this.StepToA[eTElement.ID].countRootID.ContainsKey(key))
                {
                    this.StepToA[eTElement.ID].countRootID.Add(key, eTElement.CountRootID[key]);
                    temp = true;
                }
            }

            return temp;
        }

        public bool SetCalcForType1(double value_a, double value_d)
        {
            if (this.Type == 1)
            {
                this.CalcValueSum_A_Forced = value_a;
                this.CalcValueSum_D_Forced = value_d;

                ETItem eTItem = new ETItem()
                {
                    id = this.ID,
                    type = this.Type,
                    name = this.Name,
                    countRootID = this.CountRootID,
                    CalcValue_A = value_a,
                    CalcValue_D = value_d,
                    deep = this.Deep,
                };

                this.StepToA.Add(this.ID, eTItem);

                KeyValuePair<ulong, DataCalcValue> temp = new KeyValuePair<ulong, DataCalcValue>(this.ID, new DataCalcValue(value_a, value_d));

                this.CalcValueByID_A.Add(temp.Key, temp.Value);
                return true;
            }
            return false;
        }

        public bool ItIsCalculated()
        {
            this._isCalc = true;
            return this.IsCalc;
        }

        public bool ResetCalc()
        {
            this._isCalc = false;
            return this.IsCalc;
        }

        public bool ToWaitCalc()
        {
            this._isWaitCalc = true;
            return this.IsWaitCalc;
        }

        public bool ToDoneWaiting()
        {
            this._isWaitCalc = false;
            return true;
        }

        public void AddCalcValueByStepAID(ulong id, double value_a, double value_d)
        {
            if (this.CalcValueByID_A.ContainsKey(id))
            {
                this.CalcValueByID_A.Remove(id);
                this.CalcValueByID_A.Add(id, new DataCalcValue(value_a, value_d));
            }
            else
                this.CalcValueByID_A.Add(id, new DataCalcValue(value_a, value_d));
        }

        public void AddCalcValueByStepBID(ulong id, double value_a, double value_d)
        {
            if (this.CalcValueByID_B.ContainsKey(id))
            {
                this.CalcValueByID_B.Remove(id);
                this.CalcValueByID_B.Add(id, new DataCalcValue(value_a, value_d));
            }
            else
                this.CalcValueByID_B.Add(id, new DataCalcValue(value_a, value_d));
        }

        public void ClearCalcValueByStepB()
        {
            this.CalcValueByID_B.Clear();
        }

        #endregion

        #region class building
        public bool TryAddPrevalence(PlexNewBigId PlexNewBigId)
        {
            if (PlexNewBigId != null && PlexNewBigId.value_a != null && PlexNewBigId.ida == this.ID && Constants.LevelPrevalence.Contains(PlexNewBigId.level))
            {
                if (PlexNewBigId.idb == Constants.GetPrevalenceIDMain)
                {
                    if (!this.MainPrevalences.ContainsKey(PlexNewBigId.id))
                        this.MainPrevalences.Add(PlexNewBigId.id, PlexNewBigId);
                    else
                        return false;
                }
                else
                {
                    if (this.Prevalences.ContainsKey(PlexNewBigId.idb))
                    {
                        if (!this.Prevalences[PlexNewBigId.idb].ContainsKey(PlexNewBigId.id))
                            this.Prevalences[PlexNewBigId.idb].Add(PlexNewBigId.id, PlexNewBigId);
                        else
                            return false;
                    }
                    else
                        this.Prevalences.Add(PlexNewBigId.idb, new Dictionary<ulong, PlexNewBigId>() { { PlexNewBigId.id, PlexNewBigId } });
                }
                return true;
            }
            return false;
        }

        public bool SetParentPrevalence(double parentPrevalence)
        {
            CheckerValue checker = new CheckerValue();

            if (checker.CheckPrevalenceValue_a(parentPrevalence))
            {
                this.ParentPrevalence = parentPrevalence;
                this.ContainsParentPrevalence = true;
                return true;
            }
            return false;
        }

        public void AddParentIDs(HashSet<ulong> newParentIDs)
        {
            foreach (ulong id in newParentIDs)
                this.ParentIDs.Add(id);
        }

        public void AddEkvivalentIDs(HashSet<ulong> newEkvivalentIDs)
        {
            foreach (ulong id in newEkvivalentIDs)
                this.EkvivalentIDs.Add(id);
        }

        public void AddBinarizationIDs(HashSet<ulong> newBinarizationIDs)
        {
            foreach (ulong id in newBinarizationIDs)
                this.BinarizationIDs.Add(id);
        }
    }
    #endregion
}
