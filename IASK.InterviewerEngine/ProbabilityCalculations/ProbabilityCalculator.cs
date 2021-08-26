using IASK.InterviewerEngine.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UMKBNeedStuff.Models;
using UMKBNeedStuff.Responses.UMKB;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    public class ProbabilityCalculator : IProbabilityCalculator
    {
        internal ETDataFromNosology DataToSymptoms = new ETDataFromNosology(true, 0.01, 0.2);
        internal ETDataFromNosology DataToRiskFactor = new ETDataFromNosology(true, 0.01, 0.2);
        internal ETDataFromNosology DataToPrevention = new ETDataFromNosology(true, 0.01, 0.2);
        internal Dictionary<ulong, ComplexDescription.Builder> Descriptions = new Dictionary<ulong, ComplexDescription.Builder>();

        public bool TryGetProbability(InterviewerState.ProbabilityType probabilityType, ulong NosologyId, out double result)
        {
            result = 0;
            if (probabilityType == InterviewerState.ProbabilityType.Nosology)
            {
                return TryGetProbabilityResults(NosologyId, out result);
            }
            else if (probabilityType == InterviewerState.ProbabilityType.Risk)
            {
                return TryGetRiskResults(NosologyId, out result);
            }
            return false;
        }
        public bool TryGetNosologies(out Dictionary<ulong, string> result)
        {
            result = new Dictionary<ulong, string>();
            try
            {
                if (DataToSymptoms.RootIDs != null)
                    foreach (ulong id in DataToSymptoms.RootIDs)
                    {
                        if (DataToSymptoms.AllElement.ContainsKey(id))
                        {
                            result.TryAdd(id, DataToSymptoms.AllElement[id].Name);
                        }

                    }
                if (DataToRiskFactor.RootIDs != null)
                    foreach (ulong id in DataToRiskFactor.RootIDs)
                    {
                        if (DataToRiskFactor.AllElement.ContainsKey(id))
                        {
                            result.TryAdd(id, DataToRiskFactor.AllElement[id].Name);
                        }

                    }
                if (DataToPrevention.RootIDs != null)
                    foreach (ulong id in DataToPrevention.RootIDs)
                    {
                        if (DataToPrevention.AllElement.ContainsKey(id))
                        {
                            result.TryAdd(id, DataToPrevention.AllElement[id].Name);
                        }

                    }
                return result.Count > 0;
            }
            catch
            {
                return false;
            }



        }
        public void TrySetAnswersFromIsolationSensors(Dictionary<ulong, IsolationSensor> DataToIsolationSensor)
        {
            DataToSymptoms.TrySetAnswersFromIsolationSensors(DataToIsolationSensor);
            DataToRiskFactor.TrySetAnswersFromIsolationSensors(DataToIsolationSensor);
            DataToPrevention.TrySetAnswersFromIsolationSensors(DataToIsolationSensor);
        }
        internal bool TryAddDescription(ulong bigID, UMKBNeedStuff.Models.FieldsDescriptions fieldsDescriptions)
        {
            if (fieldsDescriptions == null)
                return false;

            if (this.Descriptions.ContainsKey(bigID))
            {
                this.Descriptions[bigID].TryAddDescriptions(fieldsDescriptions);
            }
            else
            {
                ComplexDescription.Builder desc = new ComplexDescription.Builder();
                desc.TryAddDescriptions(fieldsDescriptions);
                this.Descriptions.Add(bigID, desc);
            }
            return false;
        }
        internal bool TryGetProbabilityResults(ulong NosologyId, out double re)
        {
            re = 0;
            if (DataToSymptoms.RootIDs.Contains(NosologyId))
            {
                ETElement element = DataToSymptoms.AllElement[NosologyId];
                re = element.CalcValueSum_AToStepA;
                return true;
            }
            return false;
        }
        internal bool TryGetRiskResults(ulong NosologyId, out double re)
        {

            re = 0;
            if (DataToRiskFactor.RootIDs.Contains(NosologyId))
            {
                ETElement element = DataToRiskFactor.AllElement[NosologyId];
                double valueA_RF = 0.0;
                if (DataToPrevention.RootIDs.Contains(element.ID))
                {
                    valueA_RF = element.CalcValueSum_AToStepA - DataToPrevention.AllElement[element.ID].CalcValueSum_AToStepA;
                }
                else
                {
                    valueA_RF = element.CalcValueSum_AToStepA;
                }

                re = valueA_RF;

                double valueA_NS = 0.0;
                if (DataToSymptoms.AllElement.ContainsKey(element.ID))
                    valueA_NS = DataToSymptoms.AllElement[element.ID].CalcValueSum_AToStepA;
                return true;
            }

            return false;


        }
        public IProbabilityCalculator Copy()
        {
            return new ProbabilityCalculator()
            {
                DataToPrevention = this.DataToPrevention.Copy(),
                DataToRiskFactor = this.DataToRiskFactor.Copy(),
                DataToSymptoms = this.DataToSymptoms.Copy()
            };
        }

        public class Factory : IProbabilityCalculatorFactory
        {
            private readonly ConcurrentDictionary<ulong, IProbabilityCalculator> cache = new ConcurrentDictionary<ulong, IProbabilityCalculator>();
            private readonly IKeyProvider keyProvider;
            public Factory(IKeyProvider keyProvider)
            {
                this.keyProvider = keyProvider;
            }

            public bool TryGetProbabilityCalculator(ulong idb, out IProbabilityCalculator calculator)
            {
                if (cache.TryGetValue(idb, out calculator))
                {
                    return true;
                }
                else
                {
                    calculator = Create(idb, keyProvider.SemanticKey, keyProvider.DescriptionsListKey);
                    cache.TryAdd(idb, calculator);
                    return true;
                }
            }

            internal static ProbabilityCalculator Create(ulong idb, string GraphPass, string DescriptionsList)
            {
                ProbabilityCalculator calculator = new ProbabilityCalculator();

                HashSet<ulong> TriggerIDs = new HashSet<ulong>();
                HashSet<ulong> TriggersToSensorsLinksIDs = new HashSet<ulong>();
                Dictionary<ulong, ulong> TriggerRebroIDs = new Dictionary<ulong, ulong>();
                HashSet<ulong> TriggersToLogicIDs = new HashSet<ulong>();
                HashSet<ulong> SensorIDs = new HashSet<ulong>();
                HashSet<ulong> SensorToLogicIDs = new HashSet<ulong>();
                HashSet<ulong> CollectorIDs = new HashSet<ulong>();
                HashSet<ulong> CollectorToLogicIDs = new HashSet<ulong>();
                HashSet<ulong> TreeNodes = new HashSet<ulong>();
                HashSet<ulong> DescriptionIDs = new HashSet<ulong>();
                HashSet<ulong> VarcharIDs = new HashSet<ulong>();


                HashSet<ushort> libs2 = new HashSet<ushort>();

                #region Сбор IDs нозологии
                List<ulong> NosologyIDs = new List<ulong>();
                ResultSemanticNewBigId plexusNosology = CacheForming.GetNosologyByCheckerID(GraphPass, idb, 1);
                if (plexusNosology.alerts != null && plexusNosology.alerts.code == 200 && plexusNosology.graph.Count > 0)
                {
                    List<PlexNewBigId> temp = new List<PlexNewBigId>();
                    foreach (PlexNewBigId PlexNewBigId in plexusNosology.graph)
                        if (PlexNewBigId.deep > 0 && PlexNewBigId.value_a != null && Math.Abs((double)PlexNewBigId.value_a - 1) <= double.Epsilon)
                            temp.Add(PlexNewBigId);

                    ComparerPlexByDeepUpSortIncreases comparer = new ComparerPlexByDeepUpSortIncreases();
                    temp.Sort(comparer);

                    foreach (PlexNewBigId PlexNewBigId in temp)
                    {
                        NosologyIDs.Add(PlexNewBigId.idb);
                        DescriptionIDs.Add(PlexNewBigId.idb);
                    }
                }
                else
                {
                }


                #endregion

                #region Сбор симптомов 
                ResultSemanticNewBigId plexusSymptoms = CacheForming.GetToSymptoms(GraphPass, NosologyIDs, 1) ?? new ResultSemanticNewBigId();
                if (plexusSymptoms.alerts != null && plexusSymptoms.alerts.code == 200 && plexusSymptoms.graph.Count > 0)
                {
                    ComparerPlexByDeepUpSortUpValueAIncreases comparer = new ComparerPlexByDeepUpSortUpValueAIncreases();
                    plexusSymptoms.graph.Sort(comparer);

                    List<ulong> correctionIDs = plexusSymptoms.graph.Where(p => p.deep > 0).Select(p => p.id).ToList();
                    ResultSemanticNewBigId plexusSymptomsCorrection = CacheForming.GetCorrection(GraphPass, correctionIDs) ?? new ResultSemanticNewBigId();

                    Dictionary<ulong, List<PlexNewBigId>> symptomsCorrection = null;
                    if (plexusSymptomsCorrection.alerts != null && plexusSymptomsCorrection.alerts.code == 200 && plexusSymptomsCorrection.graph.Count > 0)
                    {
                        symptomsCorrection = plexusSymptomsCorrection.graph.Where(p => p.deep > 0).GroupBy(g => g.ida).ToDictionary(g => g.Key, g => g.ToList());
                    }
                    else
                    {

                    }

                    foreach (PlexNewBigId PlexNewBigId in plexusSymptoms.graph)
                    {
                        ETItem eTItem = CacheForming.CreateETItem(PlexNewBigId, plexusSymptoms, false);
                        if (eTItem != null)
                        {
                            if (symptomsCorrection != null)
                                if (symptomsCorrection.ContainsKey(PlexNewBigId.id))
                                    foreach (PlexNewBigId p in symptomsCorrection[PlexNewBigId.id])
                                    {
                                        bool done = eTItem.TryAddCorrection(p);
                                        if (done)
                                        {
                                            calculator.DataToSymptoms.AddCorrection(eTItem.id, p.id, p.idb);
                                        }

                                    }
                            calculator.DataToSymptoms.Add(eTItem);
                        }
                    }
                }
                else
                {

                }
                #endregion

                #region Сбор факторов риска
                ResultSemanticNewBigId plexusRiskFactor = CacheForming.GetToRiskFactor(GraphPass, NosologyIDs, 1) ?? new ResultSemanticNewBigId();
                if (plexusRiskFactor.alerts != null && plexusRiskFactor.alerts.code == 200 && plexusRiskFactor.graph.Count > 0)
                {
                    ComparerPlexByDeepUpSortUpValueAIncreases comparer = new ComparerPlexByDeepUpSortUpValueAIncreases();
                    plexusRiskFactor.graph.Sort(comparer);

                    List<ulong> correctionIDs = plexusRiskFactor.graph.Where(p => p.deep > 0).Select(p => p.id).ToList();
                    ResultSemanticNewBigId plexusRiskFactorCorrection = CacheForming.GetCorrection(GraphPass, correctionIDs) ?? new ResultSemanticNewBigId();

                    Dictionary<ulong, List<PlexNewBigId>> riskFactorCorrection = null;
                    if (plexusRiskFactorCorrection.alerts != null && plexusRiskFactorCorrection.alerts.code == 200 && plexusRiskFactorCorrection.graph.Count > 0)
                    {
                        riskFactorCorrection = plexusRiskFactorCorrection.graph.Where(p => p.deep > 0).GroupBy(g => g.ida).ToDictionary(g => g.Key, g => g.ToList());
                    }
                    else
                    {
                    }

                    foreach (PlexNewBigId PlexNewBigId in plexusRiskFactor.graph)
                    {
                        ETItem eTItem = CacheForming.CreateETItem(PlexNewBigId, plexusRiskFactor, false);
                        if (eTItem != null)
                        {
                            if (riskFactorCorrection != null)
                                if (riskFactorCorrection.ContainsKey(PlexNewBigId.id))
                                    foreach (PlexNewBigId p in riskFactorCorrection[PlexNewBigId.id])
                                    {
                                        bool done = eTItem.TryAddCorrection(p);
                                        if (done)
                                        {
                                            calculator.DataToRiskFactor.AddCorrection(eTItem.idb, eTItem.id, p.idb);
                                        }
                                    }
                            calculator.DataToRiskFactor.Add(eTItem);
                        }

                    }
                }
                else
                {

                }
                #endregion

                #region Сбор Профилактики
                ResultSemanticNewBigId plexusPrevention = CacheForming.GetPrevention(GraphPass, NosologyIDs, 1) ?? new ResultSemanticNewBigId();
                if (plexusPrevention.alerts != null && plexusPrevention.alerts.code == 200 && plexusPrevention.graph.Count > 0)
                {
                    ComparerPlexByDeepUpSortUpValueAIncreases comparer = new ComparerPlexByDeepUpSortUpValueAIncreases();
                    plexusPrevention.graph.Sort(comparer);

                    List<ulong> correctionIDs = plexusPrevention.graph.Where(p => p.deep > 0).Select(p => p.id).ToList();
                    ResultSemanticNewBigId plexusPreventionCorrection = CacheForming.GetCorrection(GraphPass, correctionIDs) ?? new ResultSemanticNewBigId();

                    Dictionary<ulong, List<PlexNewBigId>> preventionCorrection = null;
                    if (plexusPreventionCorrection.alerts != null && plexusPreventionCorrection.alerts.code == 200 && plexusPreventionCorrection.graph.Count > 0)
                    {
                        preventionCorrection = plexusPreventionCorrection.graph.Where(p => p.deep > 0).GroupBy(g => g.ida).ToDictionary(g => g.Key, g => g.ToList());
                    }
                    else
                    {

                    }

                    foreach (PlexNewBigId PlexNewBigId in plexusPrevention.graph)
                    {
                        ETItem eTItem = CacheForming.CreateETItem(PlexNewBigId, plexusRiskFactor, false);
                        if (eTItem != null)
                        {
                            if (preventionCorrection != null)
                                if (preventionCorrection.ContainsKey(PlexNewBigId.id))
                                    foreach (PlexNewBigId p in preventionCorrection[PlexNewBigId.id])
                                    {
                                        bool done = eTItem.TryAddCorrection(p);
                                        if (done)
                                            calculator.DataToPrevention.AddCorrection(eTItem.id, p.id, p.idb);
                                    }
                            calculator.DataToPrevention.Add(eTItem);
                        }
                    }
                }
                else
                {

                }
                #endregion

                #region Сбор для заболеваемости связи

                double toParent = 0.1;
                double toBinarization = 0.2;
                short deepParent = 10;
                List<UMKBRequests.Models.API.Semantic.Alert> alertsRF = calculator.DataToRiskFactor.AddRelateMain(
                    GraphPass,
                    calculator.DataToRiskFactor.RootIDs,
                    true,
                    toParent,
                    deepParent,
                    true,
                    true,
                    toBinarization);

                List<UMKBRequests.Models.API.Semantic.Alert> alertsNS = calculator.DataToSymptoms.AddRelateMain(
                    GraphPass,
                    calculator.DataToSymptoms.RootIDs,
                    true,
                    toParent,
                    deepParent,
                    true,
                    true,
                    toBinarization);

                List<UMKBRequests.Models.API.Semantic.Alert> alertsPr = calculator.DataToPrevention.AddRelateMain(
                    GraphPass,
                    calculator.DataToSymptoms.RootIDs,
                    true,
                    toParent,
                    deepParent,
                    true,
                    true,
                    toBinarization);

                #endregion

                #region Запрос Заболеваемости.
                List<UMKBRequests.Models.API.Semantic.Alert> alertsPFR = calculator.DataToRiskFactor.FillPrevalence(GraphPass);
                List<UMKBRequests.Models.API.Semantic.Alert> alertsPNS = calculator.DataToSymptoms.FillPrevalence(GraphPass);
                List<UMKBRequests.Models.API.Semantic.Alert> alertsPPr = calculator.DataToPrevention.FillPrevalence(GraphPass);

                #endregion

                #region Сбор описаний
                List<ulong> _ids = DescriptionIDs.ToList();//.Where((id) => IdParser.ParseNewBigId(id).lib != 912).ToList();
                ResponseGetDescriptionsDB descriptions = CacheForming.GetplexusDescriptions(DescriptionsList, _ids);
                for (int i = 0; i < descriptions.Result.Count; ++i)
                    if (descriptions.Result[i].Count > 0)
                        foreach (FieldsDescriptions item in descriptions.Result[i])
                        {
                            //eTSession.TryAddDescription(DescriptionIDs.ElementAt(i), item, "ru");
                            calculator.TryAddDescription(DescriptionIDs.ElementAt(i), item);
                        }

                #endregion

                return calculator;
            }
        }
    }
}
