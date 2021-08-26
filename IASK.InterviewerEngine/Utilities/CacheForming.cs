using IASK.InterviewerEngine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UMKBNeedStuff.Responses.UMKB;
using UMKBRequests;
using UMKBRequests.Models.API.Satellite;
using UMKBRequests.Models.API.Semantic;

namespace IASK.InterviewerEngine
{
    /// <summary>
    /// Класс с подготовленными запросами к семантике для формирования различных частей вопросно-ответной системы.
    /// </summary>
    public static class CacheForming
    {
        /// <summary>
        /// Проверка сателитного поля-переключателя для концепта. 
        /// Запрос <see cref="UMKBRequests.GetBoolean"/>
        /// При ошибке запроса бросает  <see cref="UMKBRequests.GetBoolean.GetBooleanException"></see>
        /// </summary>
        /// <param name="Id">Id чекера/анкеты в формате longid (NewBigId)</param>
        /// <param name="field">Поле в БД по которому осуществляется проверка</param>
        /// <param name="booleanSattliteKey">Ключ доступа к API</param>
        /// <returns></returns>
        internal static bool CheckBoolValue(ulong Id, string field, string booleanSattliteKey)
        {
            RequestSatellite requestSattelite = new RequestSatellite()
            {
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = booleanSattliteKey
                },
                SelectField = new List<string>() { field },
                bigids = new List<string>() { IdParser.ParseNewBigIdToBigId(Id).ToString() }
            };
            ResponseBoolean responseBoolean = UMKBRequests.GetBoolean.Get(requestSattelite);
            if (responseBoolean.alerts!=null&& responseBoolean.alerts.Count==1&& responseBoolean.alerts[0].code.Equals("200"))
            {
                if (responseBoolean.result[0].Count == 0) return false;
                bool? value = responseBoolean.result[0][0].value;
                return value != null && (bool)value;
            }
            else if (TryCheckBooleanAccess(requestSattelite, out string message))
            {
                throw new GetBoolean.GetBooleanException(message);
            }
            else 
            {

                string mess = string.Empty;
                foreach (var alert in responseBoolean.alerts)
                {
                    mess += string.Format("code: {0}; message: {1}; ",alert.code,alert.message);
                }
                throw new GetBoolean.GetBooleanException(mess);
            }
        }
        /// <summary>
        /// Метод для проверки результата запроса семантики. 
        /// При некорректном результате бросает <see cref="Exceptions.GetSemanticException"></see>
        /// </summary>
        /// <param name="result">Результат запроса.</param>
        /// <returns></returns>
        private static ResultSemanticNewBigId CheckSemanticRequestResult(ResultSemanticNewBigId result, RequestSemantic request = null)
        {
            if (result.alerts != null && result.alerts.code != 200)
            {
                if (request != null && result.alerts.code == 405 && TryCheckSemanticAccess(request, out string message))
                {
                    throw new GetSemanticException(result.alerts, message);
                }
                else throw new GetSemanticException(result.alerts);
            }
            return result;
        }
        /// <summary>
        /// Метод для проверки результата запроса названий. 
        /// При некорректном результате бросает <see cref="Exceptions.Get"></see>
        /// </summary>
        /// <param name="result">Результат запроса.</param>
        /// <returns></returns>
        private static ResponseGetDescriptionsDB CheckGetDescriptionsResult(ResponseGetDescriptionsDB result, RequestSatellite request = null)
        {
            if (result.Alert != null && !result.Alert.code.Equals("200"))
            {
                if (request != null && result.Alert.code.Equals("405") && TryCheckDescriptionsAccess(request, out string message))
                {
                    throw new GetDescriptionsException(result.Alert, message);
                }
                else throw new Exceptions.GetDescriptionsException(result.Alert);
            }
            return result;
        }


        /// <summary>
        /// Метод для проверки результата запроса названий. 
        /// При некорректном результате бросает <see cref="Exceptions.GetNamesException"></see>
        /// </summary>
        /// <param name="result">Результат запроса.</param>
        /// <returns></returns>
        private static GetNamesResponse CheckGetNamesResult(GetNamesResponse result)
        {
            if (result.alerts != null && result.alerts.Count>0&& !result.alerts[0].code.Equals("200"))
            {
                throw new GetNamesException(result.alerts[0]);
            }
            return result;
        }

        /// <summary>
        /// Метод для проверки результата запроса varchar-ов. 
        /// При некорректном результате бросает <see cref="Exceptions.GetVarcharListException"></see>
        /// </summary>
        /// <param name="result">Результат запроса.</param>
        /// <returns></returns>
        private static ResultVarCharList CheckGetVarCharListResult(ResultVarCharList result, RequestVarCharList requestVarCharList = null)
        {
            if (result.alerts != null && result.alerts != null)
            {
                if (requestVarCharList != null && result.alerts.code == 500 &&
                    TryCheckVarcharListAccess(requestVarCharList, out string message) && !string.IsNullOrEmpty(message))
                {
                    throw new Exceptions.GetVarcharListException(result.alerts, message);
                }
                else throw new Exceptions.GetVarcharListException(result.alerts);
            }
            return result;
        }


        public static GetNamesResponse GetNames(string keyGetNames, List<ulong> idsList, string locale = null)
        {
            UMKBRequests.Models.API.Satellite.RequestSatellite requestSatellite = new UMKBRequests.Models.API.Satellite.RequestSatellite()
            {
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = keyGetNames
                },
                bigids = idsList.Select(item => item.ToString()),
                SelectField = Constants.NamesFilters,
                locale = locale
            };

            var Names = UMKBRequests.GetNames.Get(requestSatellite, true);
            return CheckGetNamesResult(Names);
        }

        internal static ResponseGetDescriptionsDB GetplexusDescriptions(string keyDescriptionsList, List<ulong> list, string locale = null)
        {
            if (list.Count > 0)
            {
                List<string> listId_string = list.Select(id => IdParser.ParseNewBigIdToBigId(id).ToString()).ToList();
                GetDescriptionsDB getDescriptionsDB = new GetDescriptionsDB();

                RequestSatellite request = new RequestSatellite()
                {
                    permit = new UMKBRequests.Models.API.Codes.Permit()
                    {
                        key = keyDescriptionsList
                    },
                    bigids = listId_string,
                    locale= locale

                };
                ResponseGetDescriptionsDB rd = getDescriptionsDB.Get(request);
                return CheckGetDescriptionsResult(rd, request);
            }
            else return new ResponseGetDescriptionsDB() { Alert = new UMKBNeedStuff.Alert() { code = "200" }, Result = new List<List<UMKBNeedStuff.Models.FieldsDescriptions>>() };

        }

        /// <summary>
        /// Метод получения коллекции массивов семантики по симптомам.
        /// </summary>
        /// <param name="keyVarchar">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="bigids">Cписок внешних (сквозных) индексов концептов.</param>
        /// <param name="libids">список внутренних (двухкомпонентных) индексов концептов.</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultVarCharList GetVarcharList(string keyVarchar, List<string> bigids)
        {
            UMKBRequests.GetVarCharList gvcl = new GetVarCharList();
            RequestVarCharList requestVarCharList = new RequestVarCharList()
            {
                bigids = bigids,
                permit = new UMKBRequests.Models.API.Codes.Permit()
                {
                    key = keyVarchar
                },
            };
            var res = gvcl.Get(requestVarCharList);
            return CheckGetVarCharListResult(res, requestVarCharList);
        }

        /// <summary>
        /// Метод получения коллекции массивов семантики к симптомам.
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="Symptoms">Список содержащий BIG ID.</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetToSymptoms(string KeyGraph, List<ulong> Symptoms, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();// Symptoms.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in Symptoms)
                listId_string.Add((IDTrigger).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelSymptoms,
                route = Constants.RouteReverseSymptoms,
                lib = Constants.LibSymptoms,
                libid = listId_string,
                deep = Deep,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// Метод получения коллекции массивов семантики к факторам риска.
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="RiskFactor">Строка содержащая "Факторы Риска".</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetToRiskFactor(string KeyGraph, List<ulong> RiskFactor, short Deep)
        {
            GetSemanticNewBigId getSemanticNewBigId = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();// RiskFactor.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in RiskFactor)
                listId_string.Add((IDTrigger).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelRiskFactor,
                route = Constants.RouteToRiskFactor,
                lib = Constants.LibRiskFactor,
                libid = listId_string,
                deep = Deep,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemanticNewBigId.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// Метод получения коллекции массивов семантики к профилактики.
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="Prevention">Строка содержащая "Факторы Риска".</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetPrevention(string KeyGraph, List<ulong> Prevention, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();// Prevention.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in Prevention)
                listId_string.Add((IDTrigger).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelPrevention,
                route = Constants.RoutePrevention,
                lib = Constants.LibPrevention,
                libid = listId_string,
                deep = Deep,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// Метод получения объекта промежуточных данных по PlexNewBigId из ResultSemanticNewBigId.
        /// </summary>
        /// <param name="_plex">Коллекция параметров сплетения.</param>
        /// <param name="_plexus">Коллекция массивов семантики.</param>
        /// <param name="isChkLevelB">Проверка на LevelB.</param>
        /// <returns>Объект с промежуточными данными.</returns>
        internal static ETItem CreateETItem(PlexNewBigId _plex, ResultSemanticNewBigId _plexus, bool isChkLevelB)
        {
            ETItem temp = new ETItem(_plex)
            {
                id = _plex.id,
                deep = _plex.deep,
                type = _plex.type,
                ida = _plex.ida,
                idb = _plex.idb,
                level = _plex.level,
                levelb = _plex.levelb,
                route = _plex.route,
                countRootID = new Dictionary<ulong, string>(),
            };

            return ETItemFilling(temp, _plex, _plexus, ref isChkLevelB);
        }


        /// <summary>
        /// Метод получения объекта с промежуточными данными с добавленными недостающих параметров.
        /// </summary>
        /// <param name="temp">Класс промежуточных данных нозологии.</param>
        /// <param name="_plex">Коллекция параметров сплетения.</param>
        /// <param name="_plexus">Коллекция массивов семантики.</param>
        /// <param name="isChkLevelB">Проверка на LevelB.</param>
        /// <returns>Класс промежуточных данных нозологии.</returns>
        private static ETItem ETItemFilling(ETItem temp, PlexNewBigId _plex, ResultSemanticNewBigId _plexus, ref bool isChkLevelB)
        {
            if (_plex.type == 1)
            {
                if (_plexus.names.ContainsKey(_plex.id))
                {
                    temp.name = _plexus.names[_plex.id];

                    if (_plexus.levels.ContainsKey(_plex.level))
                        temp.levelbName = _plexus.levels[_plex.level];
                }
                else
                    temp.name = _plex.id.ToString();
            }
            else if (_plex.type == 0)
            {
                if (isChkLevelB && !Constants.Level_bWork.Contains(_plex.levelb))
                    return null;

                if (_plexus.levels.ContainsKey(_plex.levelb))
                    temp.levelbName = _plexus.levels[_plex.levelb];

                if (_plexus.names.ContainsKey(_plex.ida))
                    temp.name_ida = _plexus.names[_plex.ida];
                else
                    temp.name_ida = _plex.ida.ToString();

                if (_plexus.names.ContainsKey(_plex.idb))
                    temp.name = _plexus.names[_plex.idb];
                else
                    temp.name = _plex.idb.ToString();
            }
            else if (_plex.type == 2)
            {
                if (_plexus.levels.ContainsKey(_plex.levelb))
                    temp.levelbName = _plexus.levels[_plex.levelb];

                if (_plexus.names.ContainsKey(_plex.ida))
                    temp.name_ida = _plexus.names[_plex.ida];
                else
                    temp.name_ida = _plex.ida.ToString();

                if (_plexus.names.ContainsKey(_plex.id))
                    temp.name = _plexus.names[_plex.id];
                else
                    temp.name = _plex.id.ToString();
            }

            if (_plex.value_a != null) temp.Value_A = _plex.value_a;
            if (_plex.value_d != null) temp.Value_D = _plex.value_d;
            return temp;
        }

        internal static ResultSemanticNewBigId GetTriggersByCheckerId(string KeyGraph, short Deep, ulong CheckerId)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            string[] listId_string = { CheckerId.ToString() };
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelCheckerTrigger,
                route = Constants.RouteChild,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = Deep,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }


        /// <summary>
        /// Сбор семантики для нозологий
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="IDChecker">BIG ID.</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetNosologyByCheckerID(string KeyGraph, ulong IDChecker, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = new List<string>() { (IDChecker).ToString() };

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelCheckerNosology,
                route = Constants.RouteCheackerNosology,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = Deep,
            };

            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="TriggersIds">Список содержащий BIG ID.</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetSensors(string KeyGraph, List<ulong> TriggersIds, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();
            foreach (var IDTrigger in TriggersIds)
                listId_string.Add((IDTrigger).ToString());

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelTriggerSensor,
                route = Constants.RouteTriggerSensor,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = Deep,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="SensorsIds">Список ID индексов элементов графа.</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetSensorsGrouping(string KeyGraph, List<ulong> SensorsIds)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();
            foreach (var id in SensorsIds)
                listId_string.Add((id).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = new List<ushort>() { Constants.LevelToParent.First() },
                route = Constants.RouteParent,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = 1,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="IDTriggers">Список содержащий BIG ID.</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetTurnOnConditions(string KeyGraph, List<ulong> IDTriggers)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = new List<string>();// IDTriggers.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in IDTriggers)
                listId_string.Add((IDTrigger).ToString());


            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelCheckerSensorTurnOn,
                route = Constants.RouteCheackerSensorTurnOn,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = 1,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="IDs">Список содержащий BIG ID.</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetCorrection(string KeyGraph, List<ulong> IDs)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();// IDs.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in IDs)
                listId_string.Add((IDTrigger).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelCorrection,
                route = Constants.RouteCorrection,
                lib = Constants.LibChecker,
                libid = listId_string,
                deep = 1,
                valmore = true
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }


        /// <summary>
        /// Сбор логических отношений в условиях срабатывания
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="ListId">Список ID индексов элементов графа.</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetLogicTurnOn(string KeyGraph, List<ulong> ListId)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = new List<string>();//  ListId.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in ListId)
                listId_string.Add((IDTrigger).ToString());


            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = new List<ushort>() { Constants.LevelToParent.First() },
                route = Constants.RouteParent,
                lib = Constants.LibChecker.ToList(),
                libid = listId_string,
                deep = 1,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// Сбор коллекторов
        /// </summary>
        /// <param name="KeyGraph">Строка содержащая "Лицензионный ключ".</param>
        /// <param name="TriggerIDs">Список содержащий BIG ID.</param>
        /// <param name="Deep">short содержит "Глубину хождения графа".</param>
        /// <returns>Коллекция массивов семантики.</returns>
        internal static ResultSemanticNewBigId GetCollectors(string KeyGraph, List<ulong> TriggerIDs, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = new List<string>();// IDTriggerCollector.ConvertAll(new Converter<ulong, string>(Convert.ToString));
            foreach (var IDTrigger in TriggerIDs)
                listId_string.Add((IDTrigger).ToString());

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelTriggerCollector.ToList(),
                route = Constants.RouteTriggerCollector,
                lib = Constants.LibChecker.ToList(),
                libid = listId_string,
                deep = Deep,
                valmore = true
            };

            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        /// <summary>
        /// Little universal method ... 
        /// </summary>
        /// <param name="KeyGraph">Пароль</param>
        /// <param name="ElementsID"> BIG000000ID  для поиска  </param>
        /// <param name="Deep"> глубина происков </param>
        /// <param name="Level"> левелы связи </param>
        /// <param name="Route">  0  вперёд, 2 - назад, 1 - оба конца</param>
        /// <param name="Lib"> либы, где ищем </param>
        /// <returns></returns>
        internal static ResultSemanticNewBigId GetTree(string KeyGraph, IEnumerable<ulong> ElementsID, short Deep, IEnumerable<ushort> Level, short Route, IEnumerable<ushort> Lib)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = new List<string>();// ElementsID.ConvertAll(new Converter<ulong, string>(Convert.ToString));

            foreach (var IDTrigger in ElementsID)
                listId_string.Add((IDTrigger).ToString());
            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Level,
                route = Route,
                lib = Lib,
                libid = listId_string,
                deep = Deep,
            };

            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }


        #region ForET
        internal static ResultSemanticNewBigId GetByPrevalence(string KeyGraph, List<ulong> ListId)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = ListId.ConvertAll(new Converter<ulong, string>(Convert.ToString));

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelPrevalence,
                route = Constants.RoutePrevalence,
                lib = Constants.libPrevalence,
                libid = listId_string,
                deep = 10,
            };

            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }
        internal static ResultSemanticNewBigId GetToParentByListID(string KeyGraph, List<ulong> ListId, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();

            List<string> listId_string = ListId.ConvertAll(new Converter<ulong, string>(Convert.ToString));

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelToParent,
                route = Constants.RouteParent,
                lib = Constants.LibSymptomsParent,
                libid = listId_string,
                deep = Deep,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        internal static ResultSemanticNewBigId GetByEkvivalent(string KeyGraph, List<ulong> ListId, short Deep)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = ListId.ConvertAll(new Converter<ulong, string>(Convert.ToString));

            RequestSemantic settingGraph = new RequestSemantic()
            {
                key = KeyGraph,
                level = Constants.LevelToEkvivalent,
                route = Constants.RouteEkvivalent,
                lib = Constants.LibEkvivalent,
                libid = listId_string,
                deep = Deep,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }

        internal static ResultSemanticNewBigId GetByBinarization(string KeyGraph, List<ulong> ListId)
        {
            GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
            List<string> listId_string = ListId.ConvertAll(new Converter<ulong, string>(Convert.ToString));

            RequestSemantic settingGraph = new RequestSemantic()
            {
                binarization = 1,
                key = KeyGraph,
                level = new List<ushort>(),
                route = Constants.RouteBinarization,
                lib = Constants.LibBinarization,
                libid = listId_string,
                deep = 0,
            };
            return CheckSemanticRequestResult(getSemantic.Get(settingGraph), settingGraph);
        }
        #endregion
        private static IEnumerable<RequestSemantic> SingleRequestsGenerator(RequestSemantic requestSemantic)
        {
            RequestSemantic temp = new RequestSemantic()
            {
                key = requestSemantic.key,
                deep = 1,
                valmore = requestSemantic.valmore,
                locale = requestSemantic.locale,
            };
            foreach (ushort lib in requestSemantic.lib)
            {
                temp.lib = new ushort[] { lib };
                foreach (ushort level in requestSemantic.level)
                {
                    temp.level = new ushort[] { level };
                    foreach (string libid in requestSemantic.libid)
                    {
                        temp.libid = new string[] { libid };
                        yield return temp;
                    }
                }
            }

        }

        internal static bool TryCheckSemanticAccess(RequestSemantic requestSemantic, out string message)
        {
            message = string.Empty;
            try
            {
                if (TryCheckSemanticAccess(requestSemantic, out List<ushort> RestrictedLibs, out List<ushort> RestrictedLevels))
                {
                    if (RestrictedLevels.Count > 0)
                    {
                        message += "Potentially unallowed levels: ";
                        foreach (short level in RestrictedLevels)
                        {
                            message += level.ToString() + ", ";
                        }
                        message = message.Remove(message.Length - 2, 2);
                        message += "; ";
                    }
                    if (RestrictedLibs.Count > 0)
                    {
                        message += "Potentially unallowed libs: ";
                        foreach (short level in RestrictedLibs)
                        {
                            message += level.ToString() + ", ";
                        }
                        message = message.Remove(message.Length - 2, 2);
                        message += "; ";
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool TryCheckSemanticAccess(RequestSemantic requestSemantic,
            out List<ushort> RestrictedLibs, out List<ushort> RestrictedLevels)
        {
            RestrictedLibs = new List<ushort>();
            RestrictedLevels = new List<ushort>();
            try
            {
                List<ushort> AllowedLibs = new List<ushort>();
                List<ushort> AllowedLevels = new List<ushort>();
                GetSemanticNewBigId getSemantic = new GetSemanticNewBigId();
                foreach (RequestSemantic req in SingleRequestsGenerator(requestSemantic))
                {
                    Thread.Sleep(100);
                    ResultSemanticNewBigId res = getSemantic.Get(req);
                    if (res.alerts.code == 200)
                    {
                        if (!AllowedLibs.Contains(req.lib.First())) AllowedLibs.Add(req.lib.First());
                        if (!AllowedLevels.Contains(req.level.First())) AllowedLevels.Add(req.level.First());
                    }
                    else if (res.alerts.code == 405)
                    {
                        if (!RestrictedLibs.Contains(req.lib.First())) RestrictedLibs.Add(req.lib.First());
                        if (!RestrictedLevels.Contains(req.level.First())) RestrictedLevels.Add(req.level.First());
                    }
                }
                foreach (ushort lib in AllowedLibs)
                {
                    RestrictedLibs.Remove(lib);
                }
                foreach (ushort level in AllowedLevels)
                {
                    RestrictedLevels.Remove(level);
                }

                if (RestrictedLevels.Count == 0 && RestrictedLibs.Count == 0)
                {
                    return false;
                }
                else return true;
            }
            catch
            {
                return false;
            }
        }
        internal static bool TryCheckDescriptionsAccess(RequestSatellite request, out List<ushort> RequiredLibs)
        {
            RequiredLibs = new List<ushort>();
            try
            {
                GetDescriptionsDB getDescriptionsDB = new GetDescriptionsDB();
                Dictionary<ushort, List<string>> RequestSortedByLibs = new Dictionary<ushort, List<string>>();
                foreach (string bigid in request.bigids)
                {
                    ushort lib = IdParser.ParseBigId(bigid).lib;
                    if (!RequestSortedByLibs.ContainsKey(lib))
                    {
                        RequestSortedByLibs[lib] = new List<string>() { bigid };
                    }
                    else
                    {
                        RequestSortedByLibs[lib].Add(bigid);
                    }
                }
                foreach (var key in RequestSortedByLibs.Keys)
                {
                    RequestSatellite requestSatellite = new RequestSatellite()
                    {
                        permit = new UMKBRequests.Models.API.Codes.Permit()
                        {
                            key = request.permit.key
                        },
                        bigids = RequestSortedByLibs[key],
                        locale = "ru"

                    };
                    ResponseGetDescriptionsDB rd = getDescriptionsDB.Get(requestSatellite);

                    if (rd.Alert.code.Equals("405"))
                    {
                        if (!RequiredLibs.Contains(key)) RequiredLibs.Add(key);
                    }

                }
                return RequiredLibs.Count != 0;
            }
            catch
            {
                return false;
            }
        }
        internal static bool TryCheckDescriptionsAccess(RequestSatellite request, out string message)
        {
            message = string.Empty;
            if (TryCheckDescriptionsAccess(request, out List<ushort> libs) && libs != null && libs.Count != 0)
            {
                message = "Unallowed libs: ";
                foreach (ushort lib in libs)
                {
                    message += lib.ToString() + ", ";
                }
                message.Remove(message.Length - 2, 2);
                message += "; ";
                return true;
            }
            else return false;
        }
        internal static bool TryCheckVarcharListAccess(RequestVarCharList request, out string message)
        {
            message = string.Empty;
            if (TryCheckVarcharListAccess(request, out List<ushort> libs) && libs.Count > 0)
            {
                message = "Unallowed libs: ";
                foreach (ushort lib in libs)
                {
                    message += lib.ToString() + ", ";
                }
                message.Remove(message.Length - 2, 2);
                message += "; ";
                return true;
            }
            else return false;
        }
        internal static bool TryCheckVarcharListAccess(RequestVarCharList request, out List<ushort> libs)
        {
            libs = new List<ushort>();
            Dictionary<ushort, List<string>> RequestSortedByLibs = new Dictionary<ushort, List<string>>();
            GetVarCharList gvcl = new GetVarCharList();
            foreach (string bigid in request.bigids)
            {
                ushort lib = IdParser.ParseBigId(bigid).lib;
                if (!RequestSortedByLibs.ContainsKey(lib))
                {
                    RequestSortedByLibs[lib] = new List<string>() { bigid };
                }
                else
                {
                    RequestSortedByLibs[lib].Add(bigid);
                }
            }
            foreach (var key in RequestSortedByLibs.Keys)
            {
                RequestVarCharList requestSatellite = new RequestVarCharList()
                {
                    permit = new UMKBRequests.Models.API.Codes.Permit()
                    {
                        key = request.permit.key
                    },
                    bigids = RequestSortedByLibs[key],
                    locale = "ru"

                };
                ResultVarCharList rvcl = gvcl.Get(requestSatellite);

                if (rvcl.alerts != null && rvcl.alerts.code.Equals(500))
                {
                    if (!libs.Contains(key)) libs.Add(key);
                }
            }
            return libs.Count != 0;
        }


        internal static bool TryCheckBooleanAccess(RequestSatellite request, out List<ushort> RequiredLibs)
        {
            RequiredLibs = new List<ushort>();
            try
            {

                Dictionary<ushort, List<string>> RequestSortedByLibs = new Dictionary<ushort, List<string>>();
                foreach (string bigid in request.bigids)
                {
                    ushort lib = IdParser.ParseBigId(bigid).lib;
                    if (!RequestSortedByLibs.ContainsKey(lib))
                    {
                        RequestSortedByLibs[lib] = new List<string>() { bigid };
                    }
                    else
                    {
                        RequestSortedByLibs[lib].Add(bigid);
                    }
                }
                foreach (var key in RequestSortedByLibs.Keys)
                {
                    RequestSatellite requestSatellite = new RequestSatellite()
                    {
                        permit = new UMKBRequests.Models.API.Codes.Permit()
                        {
                            key = request.permit.key
                        },
                        bigids = RequestSortedByLibs[key],
                        locale = "ru"

                    };
                    ResponseBoolean responseBoolean = UMKBRequests.GetBoolean.Get(requestSatellite);

                    if (responseBoolean.alerts!=null&& responseBoolean.alerts.Count>0&&responseBoolean.alerts[0].code.Equals("405"))
                    {
                        if (!RequiredLibs.Contains(key)) RequiredLibs.Add(key);
                    }

                }
                return RequiredLibs.Count != 0;
            }
            catch
            {
                return false;
            }
        }
        internal static bool TryCheckBooleanAccess(RequestSatellite request, out string message)
        {
            message = string.Empty;
            if (TryCheckBooleanAccess(request, out List<ushort> libs) && libs != null && libs.Count != 0)
            {
                message = "Unallowed libs: ";
                foreach (ushort lib in libs)
                {
                    message += lib.ToString() + ", ";
                }
                message.Remove(message.Length - 2, 2);
                message += "; ";
                return true;
            }
            else return false;
        }

        
    }
}