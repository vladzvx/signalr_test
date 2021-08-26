using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UMKBNeedStuff.Responses.UMKB;
using UMKBRequests;
using UMKBRequests.Models.API.Satellite;
using UMKBRequests.Models.API.Semantic;
using fieldsDescriptions = UMKBNeedStuff.Models.FieldsDescriptions;

namespace IASK.InterviewerEngine
{
    public partial class Interviewer
    {
        public class Factory
        {
            private readonly ConcurrentDictionary<ulong, Interviewer> cache = new ConcurrentDictionary<ulong, Interviewer>();
            private readonly IKeyProvider keyProvider;

            public Factory (IKeyProvider keyProvider)
            {
                this.keyProvider = keyProvider;
            }
            public bool TryGetInterviewer(ulong CheckerId, out Interviewer interviewer)
            {
                if (cache.TryGetValue(CheckerId, out interviewer))
                {
                    return true;
                }
                else
                {
                    interviewer = Create(CheckerId,
                        keyProvider.SemanticKey,
                        keyProvider.DescriptionsListKey,
                        keyProvider.VarcharListKey,
                        keyProvider.BoolSattliteKey,
                        keyProvider.GetNamesKey);
                    return cache.TryAdd(CheckerId, interviewer);
                }
            }

            /// <summary>
            /// Создание экземпляра вопросно-ответной системы.
            /// </summary>
            /// <param name="CheckerId">ulong (NewBigId чекера)</param>
            /// <param name="lang">Язык для загрузки. Дефолтное значение - null - загржает все доступные языки.</param>
            /// <param name="SemanticKey">Ключ доступа к запросам семантики. 
            /// Используется <see href= "https://wiki.socmedica.me/doku.php?id=umkb:api:get:semantic">NewBigId API</see></param>
            /// <param name="DescriptionsListKey">Ключ доступа для получения описаний концептов.
            /// Используется <see href= "https://wiki.socmedica.me/doku.php?id=umkb:api:get:getdescriptionsdb">
            /// API для получения описаний концептов из БД</see>
            ///  </param>
            /// <param name="VarcharListKey">Ключ доступа для получения varchar концептов. 
            /// Используется <see href= "https://wiki.socmedica.me/doku.php?id=umkb:api:get:varcharlist">getVarcharList API</see>
            /// </param>
            /// <returns></returns>
            public Interviewer Create(ulong CheckerId, string SemanticKey, string DescriptionsListKey, string VarcharListKey, string BoolSattliteKey, string GetNamesKey, string lang = null)
            {
                Builder Builder = new Builder(CheckerId);
                HashSet<ulong> TriggerIDs = new HashSet<ulong>();
                HashSet<ulong> TriggersToSensorsLinksIDs = new HashSet<ulong>();
                Dictionary<ulong, ulong> TriggerRebroIDs = new Dictionary<ulong, ulong>();
                HashSet<ulong> TriggersToLogicIDs = new HashSet<ulong>();
                HashSet<ulong> SensorIDs = new HashSet<ulong>();
                HashSet<ulong> SensorToLogicIDs = new HashSet<ulong>();
                HashSet<ulong> CollectorIDs = new HashSet<ulong>();
                HashSet<ulong> CollectorToLogicIDs = new HashSet<ulong>();
                List<ulong> TreeNodes = new List<ulong>();
                HashSet<ulong> IdbsDescriptions = new HashSet<ulong>();
                Dictionary<ulong, ulong> Varchars = new Dictionary<ulong, ulong>();

                #region Сбор тригеров

                ResultSemanticNewBigId plexusTriggers = CacheForming.GetTriggersByCheckerId(SemanticKey, 10, CheckerId);

                Builder.SetType(plexusTriggers.graph[0].level);
                Builder.OnePageMode = CacheForming.CheckBoolValue(CheckerId, Constants.OnePageInterfaceField, BoolSattliteKey);

                ResultSemanticNewBigId plexusSensor;
                ResultSemanticNewBigId plexusCollector;

                ComparerPlexByDeepUpSortIncreases comparer1 = new ComparerPlexByDeepUpSortIncreases();
                plexusTriggers.graph.Sort(comparer1);

                if (plexusTriggers.graph.Count > 0)
                {
                    foreach (PlexNewBigId plex in plexusTriggers.graph)
                        if (plex.deep > 0)
                        {
                            TriggerIDs.Add(plex.idb);
                            IdbsDescriptions.Add(plex.idb);
                            TriggerRebroIDs.Add(plex.id, plex.idb);
                            Builder.TryAddCheckerTriggerIDs(new CheckerItemBuilder(plex, plexusTriggers));
                            if (plexusTriggers.names.ContainsKey(plex.idb))
                                Builder.Triggers.Add(plex.idb, new TriggerBuilder(plex.idb, plexusTriggers.names[plex.idb]));
                            else
                                Builder.Triggers.Add(plex.idb, new TriggerBuilder(plex.idb, plex.idb.ToString()));
                        }
                        else if (Builder.Type == InterviewerType.SinglePage)
                        {
                            TriggerIDs.Add(plex.id);
                            IdbsDescriptions.Add(plex.id);
                            Builder.TryAddTriggerId(new CheckerItemBuilder(plex, plexusTriggers));
                            if (plexusTriggers.names.ContainsKey(plex.idb))
                                Builder.Triggers.Add(plex.id, new TriggerBuilder(plex.id, plexusTriggers.names[plex.id]));
                            else
                                Builder.Triggers.Add(plex.id, new TriggerBuilder(plex.id, plex.id.ToString()));
                            //TriggerRebroIDs.Add(plex.id, plex.idb);
                        }

                    if (TriggerIDs.Count > 0)
                    {
                        #region Сбор сенсоров
                        plexusSensor = CacheForming.GetSensors(SemanticKey, TriggerIDs.ToList(), 1);
                        if (plexusSensor.graph.Count > 0)
                        {
                            ComparerPlexByDeepUpSortIncreases comparer = new ComparerPlexByDeepUpSortIncreases();
                            plexusSensor.graph.Sort(comparer);
                            List<CheckerItemBuilder> items = new List<CheckerItemBuilder>();
                            foreach (PlexNewBigId plex in plexusSensor.graph)
                            {
                                if (plex.deep > 0)
                                {
                                    SensorIDs.Add(plex.id);
                                    IdbsDescriptions.Add(plex.id);
                                    IdbsDescriptions.Add(plex.idb);
                                    Varchars.TryAdd(plex.idb, plex.id);
                                    //VarcharIds.Add(plex.id);
                                    if (plex.value_a != null && Convert.ToInt32(Math.Round((double)plex.value_a)) == 12)
                                        TreeNodes.Add(plex.idb);
                                    CheckerItemBuilder eTItem = new CheckerItemBuilder(plex, plexusSensor);
                                    if (eTItem != null && Builder.Triggers.ContainsKey(eTItem.MasterPlex.ida))
                                    {
                                        Builder.Triggers[eTItem.MasterPlex.ida].TryAddSensor(eTItem);
                                        items.Add(eTItem);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Сбор коллекторов
                        plexusCollector = CacheForming.GetCollectors(SemanticKey, TriggerIDs.ToList(), 1);
                        if (plexusCollector.graph.Count > 0)
                        {
                            ComparerPlexByDeepUpSortIncreases comparer = new ComparerPlexByDeepUpSortIncreases();
                            plexusCollector.graph.Sort(comparer);

                            foreach (PlexNewBigId PlexNewBigId in plexusCollector.graph)
                                if (PlexNewBigId.deep > 0)
                                {
                                    CollectorIDs.Add(PlexNewBigId.id);
                                    CheckerItemBuilder eTItem = new CheckerItemBuilder(PlexNewBigId, plexusCollector);
                                    if (eTItem != null && Builder.Triggers.ContainsKey(eTItem.MasterPlex.ida))
                                        Builder.Triggers[eTItem.MasterPlex.ida].TryAddCollector(eTItem);
                                }
                        }
                        #endregion

                        #region Сбор варчаров
                        var idbs = Varchars.Keys.Select(id => IdParser.ParseNewBigIdToBigId(id)).ToList();
                        var ids = Varchars.Values.Select(id => IdParser.ParseNewBigIdToBigId(id)).ToList();
                        var varcharList = CacheForming.GetVarcharList(VarcharListKey, idbs.Select(id => id.ToString()).ToList());
                        var varcharList2 = CacheForming.GetVarcharList(VarcharListKey, ids.Select(id => id.ToString()).ToList());
                        if (varcharList != null && varcharList2 != null && varcharList.Count == varcharList2.Count)
                        {
                            for (int i = 0; i < varcharList.Count; i++)
                            {
                                List<FieldsVarChar> vsb = varcharList[i];
                                List<FieldsVarChar> vs = varcharList2[i];
                                foreach (FieldsVarChar fvch in vsb)
                                {
                                    if (vs.FindIndex(item => item.field != null && item.field.Equals(fvch.field)) < 0)
                                    {
                                        vs.Add(fvch);
                                    }
                                }
                                ulong idb = Varchars.Keys.ElementAt(i);
                                ulong id = Varchars.Values.ElementAt(i);
                                foreach (var v in vs)
                                {
                                    if (Constants.VarcharFieldsToContent.Contains(v.field))
                                    {
                                        Builder.AddContentIfNeed(idb, v);
                                    }
                                    else if (Constants.INPUT_MASK.Equals(v.field))
                                    {
                                        Builder.AddRangesContent(idb, v.value);
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }


                #endregion

                #region Сбор описаний
                List<ulong> _ids = IdbsDescriptions.ToList();//.Where((id) => IdParser.ParseNewBigId(id).lib != 912).ToList();
                ResponseGetDescriptionsDB descriptions = CacheForming.GetplexusDescriptions(DescriptionsListKey, _ids, lang);
                for (int i = 0; i < descriptions.Result.Count; ++i)
                    if (descriptions.Result[i].Count > 0)
                        foreach (fieldsDescriptions item in descriptions.Result[i])
                        {
                            Builder.TryAddDescription(IdbsDescriptions.ElementAt(i), item);
                        }
                #endregion

                #region Сбор Условия срабатывания тригера
                ResultSemanticNewBigId plexusTriggerTurnOn = CacheForming.GetTurnOnConditions(SemanticKey, TriggerRebroIDs.Keys.ToList()) ?? new ResultSemanticNewBigId();
                if (plexusTriggerTurnOn.alerts != null && plexusTriggerTurnOn.alerts.code == 200 && plexusTriggerTurnOn.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusTriggerTurnOn.graph)
                        if (PlexNewBigId.deep > 0)
                            if (Builder.ChekerTriggerIDs.ContainsKey(TriggerRebroIDs[PlexNewBigId.ida]))
                                if (Builder.ChekerTriggerIDs[TriggerRebroIDs[PlexNewBigId.ida]].TryAddSensorTurnOn(PlexNewBigId))
                                    TriggersToLogicIDs.Add(PlexNewBigId.id);
                }
                #endregion

                #region Сбор Условия срабатывания сенсора
                ResultSemanticNewBigId plexusTriggerSensorTurnOn = CacheForming.GetTurnOnConditions(SemanticKey, SensorIDs.ToList()) ?? new ResultSemanticNewBigId();
                if (plexusTriggerSensorTurnOn.alerts != null && plexusTriggerSensorTurnOn.alerts.code == 200 && plexusTriggerSensorTurnOn.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusTriggerSensorTurnOn.graph)
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong id in Builder.Triggers.Keys)
                                if (Builder.Triggers[id].Sensors.ContainsKey(PlexNewBigId.ida))
                                    if (Builder.Triggers[id].Sensors[PlexNewBigId.ida].TryAddSensorTurnOn(PlexNewBigId))
                                        SensorToLogicIDs.Add(PlexNewBigId.id);
                }
                #endregion

                #region Сбор Условия срабатывания коллектора
                ResultSemanticNewBigId plexusTriggerCollectorTurnOn = CacheForming.GetTurnOnConditions(SemanticKey, CollectorIDs.ToList()) ?? new ResultSemanticNewBigId();
                if (plexusTriggerCollectorTurnOn.alerts != null && plexusTriggerCollectorTurnOn.alerts.code == 200 && plexusTriggerCollectorTurnOn.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusTriggerCollectorTurnOn.graph)
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong id in Builder.Triggers.Keys)
                                if (Builder.Triggers[id].Collectors.ContainsKey(PlexNewBigId.ida))
                                    if (Builder.Triggers[id].Collectors[PlexNewBigId.ida].TryAddSensorTurnOn(PlexNewBigId))
                                        CollectorToLogicIDs.Add(PlexNewBigId.id);
                }
                #endregion

                #region Сбор связей для групировки сенсоров
                ResultSemanticNewBigId plexusTriggerSensorGroup = CacheForming.GetSensorsGrouping(SemanticKey, SensorIDs.ToList());
                if (plexusTriggerSensorGroup.alerts != null && plexusTriggerSensorGroup.alerts.code == 200 && plexusTriggerSensorGroup.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusTriggerSensorGroup.graph)
                    {
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong id in Builder.Triggers.Keys)
                            {
                                if (Builder.Triggers[id].Sensors.ContainsKey(PlexNewBigId.ida))
                                    if (Builder.Triggers[id].TryAddSensorGroup(PlexNewBigId))
                                    {
                                    }
                            }
                    }
                }
                #endregion

                #region Сбор связей для групировки коллекторов
                ResultSemanticNewBigId plexusTriggerCollectorGroup = CacheForming.GetSensorsGrouping(SemanticKey, SensorIDs.ToList()) ?? new ResultSemanticNewBigId();
                if (plexusTriggerCollectorGroup.alerts != null && plexusTriggerCollectorGroup.alerts.code == 200 && plexusTriggerCollectorGroup.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusTriggerCollectorGroup.graph)
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong id in Builder.Triggers.Keys)
                                if (Builder.Triggers[id].Collectors.ContainsKey(PlexNewBigId.ida))
                                    if (Builder.Triggers[id].TryAddCollectorGroup(PlexNewBigId))
                                    {
                                    }
                }
                #endregion

                #region Сбор Логических отношений по TriggerSensorTurnOn
                ResultSemanticNewBigId plexusLogicTriggerSensorTurnOn = CacheForming.GetLogicTurnOn(SemanticKey, SensorToLogicIDs.ToList());
                if (plexusLogicTriggerSensorTurnOn.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusLogicTriggerSensorTurnOn.graph)
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong idT in Builder.Triggers.Keys)
                            {
                                foreach (ulong idS in Builder.Triggers[idT].Sensors.Keys)
                                {
                                    if (Builder.Triggers[idT].Sensors[idS].TryAddLogicSensorTurnOn(PlexNewBigId))
                                    {
                                    }
                                }
                            }
                }
                #endregion

                #region Сбор Логических отношений по TriggerCollectorTurnOn
                ResultSemanticNewBigId plexusLogicTriggerCollectorTurnOn = CacheForming.GetLogicTurnOn(SemanticKey, CollectorToLogicIDs.ToList()) ?? new ResultSemanticNewBigId();
                if (plexusLogicTriggerCollectorTurnOn.alerts != null && plexusLogicTriggerCollectorTurnOn.alerts.code == 200 && plexusLogicTriggerCollectorTurnOn.graph.Count > 0)
                {
                    foreach (PlexNewBigId PlexNewBigId in plexusLogicTriggerCollectorTurnOn.graph)
                        if (PlexNewBigId.deep > 0)
                            foreach (ulong idT in Builder.Triggers.Keys)
                            {
                                foreach (ulong idS in Builder.Triggers[idT].Collectors.Keys)
                                {
                                    if (Builder.Triggers[idT].Collectors[idS].TryAddLogicSensorTurnOn(PlexNewBigId))
                                    {
                                    }
                                }
                            }
                }
                #endregion

                #region Сбор дерева
                ResultSemanticNewBigId TreePlexus = CacheForming.GetTree(SemanticKey, TreeNodes, Constants.TreeDeep, Constants.LevelToParent, Constants.RouteChild, Constants.TreeLib);
                if (TreePlexus.graph.Count > 0)
                {
                    foreach (PlexNewBigId plex in TreePlexus.graph)
                    {
                        string val = string.Empty;
                        if (plex.deep == 0)
                        {
                            TreePlexus.names.TryGetValue(plex.id, out val);

                            Builder.AddContentIfNeed(plex.id, new Content(
                                Id: plex.id.ToString(),
                                ParentId: null,
                                Value: val,
                                Type: Content.ContentType.tree_element
                            ));
                        }
                        else
                        {
                            TreePlexus.names.TryGetValue(plex.idb, out val);
                            Builder.AddContentIfNeed(plex.idb, new Content(
                                Id: plex.idb.ToString(),
                                ParentId: plex.ida.ToString(),
                                Value: val,
                                Type: Content.ContentType.tree_element
                            ));
                        }
                    }

                }
                #endregion

                #region Сбор названий
                GetNamesResponse names = CacheForming.GetNames(GetNamesKey, IdbsDescriptions.ToList());

                for (int i = 0; i < names.names.Count; ++i)
                    if (names.names[i].Count > 0)
                        foreach (var item in names.names[i])
                        {
                            Builder.TryAddName(IdbsDescriptions.ElementAt(i), item);
                        }
                #endregion

                Builder.LoadContents(SemanticKey);
                Interviewer result = new Interviewer(Builder,this);
                return result;
            }

        }
    }
}
