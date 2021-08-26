using IASK.InterviewerEngine.Exceptions;
using IASK.InterviewerEngine.Models.Input;
using IASK.InterviewerEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using UMKBRequests;
[assembly: InternalsVisibleTo("IASK.Tests.InterviewerEngineTests")]

namespace IASK.InterviewerEngine
{
    /// <summary>
    /// Реализация вопросно-ответной системы. Принцип работы взят
    /// из реализации чекера в проекте https://gitlab.socmedica.dev/root/globaldev
    /// Потокобезопасен. Хранит только логику обработки информации, состояния хранятся в классе <see cref="InterviewerState"/>.
    /// Для взаимодействия с модулем расчета вероятностей используются поля класса <see cref="InterviewerState"/>: 
    /// <see cref="InterviewerState.TryGetProbability"/> и <see cref="InterviewerState.TryGetNosologies"/>. 
    /// </summary>
    public partial class Interviewer
    {
        #region Fields & properties
        internal readonly ulong Id;
        internal readonly Factory Creator;
        internal readonly bool OnePage = true;
        internal readonly InterviewerType Type = InterviewerType.Questioning;
        internal readonly ImmutableDictionary<ulong, Trigger> Triggers;
        internal readonly ImmutableList<ulong> sorting;
        internal readonly ImmutableDictionary<ulong, InterviewerItem> ChekerTriggerIDs;
        internal readonly ImmutableDictionary<ulong, ImmutableList<Content>> Contents;
        internal readonly ImmutableDictionary<ulong, ComplexDescription> Descriptions;
        internal readonly ImmutableDictionary<ulong, Name> Names;
        #endregion

        #region Constructor
        internal Interviewer(Builder builder, Factory factory)
        {
            this.Creator = factory;
            this.Id = builder.Id;
            this.Type = builder.Type;
            this.OnePage = builder.OnePageMode;
            var builder1 = ImmutableDictionary.CreateBuilder<ulong, Trigger>();
            foreach (ulong key in builder.Triggers.Keys)
            {
                builder1.Add(key, new Trigger(builder.Triggers[key]));
            }
            this.Triggers = builder1.ToImmutable();

            var builder2 = ImmutableDictionary.CreateBuilder<ulong, InterviewerItem>();
            foreach (ulong key in builder.ChekerTriggerIDs.Keys)
            {
                builder2.Add(key, new InterviewerItem(builder.ChekerTriggerIDs[key]));
            }
            this.ChekerTriggerIDs = builder2.ToImmutable();

            var builder3 = ImmutableDictionary.CreateBuilder<ulong, ImmutableList<Content>>();
            foreach (ulong key in builder.Contents.Keys)
            {
                builder3.Add(key, ImmutableList.CreateRange(builder.Contents[key]));
            }
            this.Contents = builder3.ToImmutable();

            this.sorting = ImmutableList.CreateRange(builder.ChekerTriggerIDs.Keys);
            var builder4 = ImmutableDictionary.CreateBuilder<ulong, ComplexDescription>();
            foreach (ulong key in builder.Descriptions.Keys)
            {
                builder4.Add(key, new ComplexDescription(builder.Descriptions[key]));
            }
            this.Descriptions = builder4.ToImmutable();

            var builder5 = ImmutableDictionary.CreateBuilder<ulong, Name>();
            foreach (ulong key in builder.Names.Keys)
            {
                builder5.Add(key, new Name(builder.Names[key]));
            }
            Names = builder5.ToImmutable();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Передать в вопросно-ответную систему ответ на вопрос. Может бросать <see cref="UncorrectAnsverException"/> 
        /// при несоответствии последовательности ответов последовательности вопросов или множественным ответам на сенсоры, 
        /// сгруппированные по принципу "и".
        /// </summary>
        /// <param name="state">Состояние вопросно-ответной системы</param>
        /// <param name="answer">Ответ на вопрос</param>
        public void SetAnswer(InterviewerState state, Answer answer = null)
        {
            if (answer == null)
            {
                if (this.ChekerTriggerIDs != null)
                    foreach (ulong id in sorting)
                        if (this.Triggers[id].Sensors.Count > 0 &&
                            this.TrySetCurrentQuestionTriggerID(id, state))
                            break;
            }
            else
                SetAnswer(answer.CheckerResponse, answer.TriggerId, state);
        }
        /// <summary>
        /// Получить следующую конфигурацию интерфейса пользователя
        /// </summary>
        /// <param name="state">Состояние вопросно-ответной системы</param>
        /// <returns></returns>
        public List<InterfaceUnit> GetResponse(InterviewerState state)
        {
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            if (state.CheckerEnded)
            {
                result.Add(new InterfaceUnit() { Type = InterfaceUnit.UnitType.THE_END });
                return result;
            }

            bool ContainButtons = false;

            HashSet<ulong> hashSetSensorsIDs = GetHashSetIDTrurnOnSensors(state);

            if (hashSetSensorsIDs.Count > 0)
            {
                if (TryGetHeader(state, out InterfaceUnit MainHeader))
                    result.Add(MainHeader);

                InterfaceUnit BodyWrapper = new InterfaceUnit();

                CreateUI(BodyWrapper, state);

                result.AddRange(BodyWrapper.Units);

                if ((!ContainButtons) || (state.CurrentQuestionTriggerID == Constants.IDTriggerTestBreath))
                {
                    InterfaceUnit buttons = new InterfaceUnit() { Idb = Constants.NextButtonIdb, Type = InterfaceUnit.UnitType.BUTTON, Label = Constants.NextButtonLabel, Id = state.CurrentQuestionTriggerID.ToString(), };

                    result.Add(buttons);
                }
                return result;
            }
            else
            {
                CalculateNextTrigger(state);
                return this.GetResponse(state);
            }
        }

        /// <summary>
        /// Получить все страницы в виде плоского списка.
        /// </summary>
        /// <param name="state">Состояние вопросно-ответной системы</param>
        /// <returns></returns>
        public List<InterfaceUnit> GetAllFields(string dialogType="doctor", string lang = "ru")
        {
            if (!this.OnePage) throw new MethodAccessException("Get all pages not allowed to this checker");
            List<InterfaceUnit> result = new List<InterfaceUnit>();
            InterviewerState state = new InterviewerState();

            if (Enum.TryParse(dialogType, out DialogType dialog) &&
                Enum.TryParse(lang, out Lang language))
            {
                state.DialogType = dialog;
                state.lang = language;
            }
            foreach (ulong id in this.sorting)
            {
                state.CurrentQuestionTriggerID = id;
                InterfaceUnit BodyWrapper = new InterfaceUnit
                {
                    Type = InterfaceUnit.UnitType.EMPTY
                };
                if (TryGetHeaderLabel(state, out string label))
                    BodyWrapper.Label = label;
                CreateUI(BodyWrapper, state,false);
                result.Add(BodyWrapper);
            }
            return result;
        }
        #endregion

        #region Creating structures for frontend

        private bool TryGetHeader(InterviewerState state, out InterfaceUnit result)
        {
            if (TryGetHeaderLabel(state, out string label))
            {
                result = new InterfaceUnit()
                {
                    Idb = Constants.HeaderIdb,
                    Type = InterfaceUnit.UnitType.HEADER,
                    Label =label,
                };
                return true;
            }
            result = null;
            return false;
        }

        private bool TryGetHeaderLabel(InterviewerState state, out string result)
        {
            if (this.Descriptions.ContainsKey(state.CurrentQuestionTriggerID))
            {
                result = GetText(state.CurrentQuestionTriggerID, state, state.DialogType, state.lang);
                return true;
            }
            else
            {
                result = string.Empty;
                return false;
            }
        }
        private List<Content> GetContents(InterviewerItem sensor, InterviewerState state)
        {
            ulong sensor_id = sensor.idb;
            List<Content> vcs = new List<Content>(); ;
            if (Contents.TryGetValue(sensor_id, out var content))
                vcs = content.ToList();

            string RecomendationText = GetText(sensor.id, state, DialogType.recommendations, state.lang);
            string ReferenceText = GetText(sensor.id, state, DialogType.references, state.lang);
            if (!String.IsNullOrEmpty(RecomendationText))
                vcs.Add(new Content(Type: Content.ContentType.text, Value: RecomendationText));
            if (!String.IsNullOrEmpty(ReferenceText))
                vcs.Add(new Content(Type: Content.ContentType.hiperlink, Value: ReferenceText));

            return vcs.Count == 0 ? null : vcs;
        }
        private List<Content> GetProbabilitiesContents(InterviewerState state, ulong sensor_id)
        {
            List<Content> contents = new List<Content>();
            if (state.TryGetNosologies != null && state.TryGetProbability != null
                && state.TryGetNosologies(out Dictionary<ulong, string> Nosologies))
            {
                foreach (ulong NosologyId in Nosologies.Keys)
                {
                    if (Enum.IsDefined(typeof(InterviewerState.ProbabilityType), sensor_id))
                    {
                        InterviewerState.ProbabilityType probabilityType = (InterviewerState.ProbabilityType)sensor_id;
                        if (state.TryGetProbability(probabilityType, NosologyId, out double prob))
                        {
                            if (prob >= Constants.ProbabilityTreshhold)
                            {
                                contents.Add(new Content(
                                    Value: Nosologies[NosologyId],
                                    Value2: ((int)Math.Truncate(prob * 100)).ToString(),
                                    Type: Content.ConvertToContentType(probabilityType)
                                ));
                            }
                        }
                    }
                }
            }
            return contents;
        }

        /// <summary>
        /// Создает основную страницу интерфейса для фронта.
        /// </summary>
        /// <param name="container">"Контейнер, в которы будут помещены создаваемые элементы</param>
        /// <param name="state">Состояние вопросно-ответной системы</param>
        /// <param name="CheckingEnabled">Переключение режима сборки интерфейса. 
        /// true - классический чекер/анкетирование, false - отправка всех элементов в одной странице. </param>
        private void CreateUI(InterfaceUnit container, InterviewerState state, bool CheckingEnabled = true)
        {
            List<InterfaceUnit> parents = new List<InterfaceUnit>();
            InterfaceUnit iu_virtual = new InterfaceUnit(-6, 1, null, null, null);//для обработки ситуации несгруппированных radiobutton
            parents.Add(iu_virtual);
            List<ulong> ReqirmentSensorsIds = new List<ulong>();
            foreach (ulong key in Triggers[state.CurrentQuestionTriggerID].SensorsSequence)
            {
                InterviewerItem sensor = Triggers[state.CurrentQuestionTriggerID].Sensors[key];
                if (!(!CheckingEnabled || CheckSensor(state, sensor)))//Пропускаем сенсор, если можно и нужно
                {
                    continue;
                }
                ulong id = sensor.id;

                ulong sensor_id = sensor.idb;
                int val = GetSensorCaseSwitch(sensor.id, state);
                TextGetterMode textGetterMode = val == 44 || val == 25 ? TextGetterMode.NamesOnly : TextGetterMode.Common;
                InterfaceUnit iu = new InterfaceUnit(val, sensor.value_b, sensor.value_c, sensor.value_d) 
                {
                    Idb = sensor.idb.ToString(),
                    Id = id.ToString(),
                    ParentId = sensor.parent_id,
                    Label = GetText(id, state, state.DialogType, state.lang, textGetterMode),
                };

                if (iu.Condition == InterfaceUnit.BrowseCondition.COLLAPSED_PAGE)
                {
                    if (Creator.TryGetInterviewer(sensor.idb, out Interviewer interviewer))
                    {
                        ulong CurrentTriggerId = state.CurrentQuestionTriggerID;
                        interviewer.SetAnswer(state);
                        iu.Units = interviewer.GetResponse(state);
                        state.CurrentQuestionTriggerID = CurrentTriggerId;
                    }
                }

                string ShortLabel = GetName(id, state.lang, NameType.Short) ?? GetName(sensor_id, state.lang, NameType.Short);
                if (!string.IsNullOrEmpty(ShortLabel))
                    iu.ShortLabel = ShortLabel;
                iu.SetTypeByFunction();
                parents.Add(iu);


                if (val == 15)
                {
                    iu.Content = GetProbabilitiesContents(state, sensor_id);
                }
                else
                    iu.Content = GetContents(sensor, state);

                if (val == 25)
                    iu.Value = GetText(id, state, state.DialogType, state.lang, TextGetterMode.DescriptionsOnly);
                if (iu.Condition == InterfaceUnit.BrowseCondition.GOTO)
                {
                    var parsed_sensor_id = IdParser.ParseNewBigId(sensor_id);
                    iu.Content = new List<Content>() 
                    {
                        new Content(null,null, parsed_sensor_id.lib.ToString(),null,Content.ContentType.lib),
                        new Content(null,null, parsed_sensor_id.id.ToString(), null,Content.ContentType.id) 
                    };
                }
                if ((string.IsNullOrEmpty(iu.Label)) && iu.Type.Equals(InterfaceUnit.UnitType.HEADER))
                {
                    iu.Type = InterfaceUnit.UnitType.EMPTY;
                }
                if (sensor.parent_id == null && !iu.Type.Equals(InterfaceUnit.UnitType.RADIOBUTTON))
                {
                    container.Units.Add(iu);
                }
                if (iu.Condition == InterfaceUnit.BrowseCondition.REQUIRED && checkSensorResultRequirment(val))
                    ReqirmentSensorsIds.Add(id);

                TryAddIsolationSensor(state, sensor, iu.Condition != InterfaceUnit.BrowseCondition.NOREQUIRED,Force: iu.Type == InterfaceUnit.UnitType.FORCE_SEND);
            }

            foreach (var ch in parents)
            {
                if (ch.ParentId != null)
                {
                    int index = parents.FindIndex(item => ((ulong)ch.ParentId).ToString().Equals(item.Id));
                    if (index >= 0)
                        parents[index].Units.Add(ch);
                }
            }

            #region обработка ситуации несгруппированных радиобаттонов, для обратной совместимости
            foreach (var ch in parents)//обработка ситуации несгруппированных radiobutton
            {
                if (ch.Type.Equals(InterfaceUnit.UnitType.RADIOBUTTON) && ch.ParentId == null)
                {
                    iu_virtual.Units.Add(ch);
                }
            }
            
            if (iu_virtual.Units.Count != 0)//обработка ситуации несгруппированных radiobutton
                container.Units.Add(iu_virtual);

            double? val_a = ChekerTriggerIDs[state.CurrentQuestionTriggerID].value_a;//Обработка возможности пропуска страницы
            if (val_a != null && Math.Round((double)val_a) == 1)
            {
                container.Units.Add(new InterfaceUnit() {Type=InterfaceUnit.UnitType.SKIP_PERMITTED });
            }
            #endregion

            state.UpdateLinks(state.CurrentQuestionTriggerID, ReqirmentSensorsIds);
        }

        private string GetText(ulong id, InterviewerState state, DialogType dialogType, Lang lang, TextGetterMode mode = TextGetterMode.Common)
        {
            if (this.Descriptions.ContainsKey(id)&&(mode==TextGetterMode.Common||mode==TextGetterMode.DescriptionsOnly)&&this.Descriptions[id].TryGetDescription(out Description desc1, dialogType, lang))
            {
                return desc1.text;
            }
            else if (this.Triggers[state.CurrentQuestionTriggerID].Sensors.ContainsKey(id))
            {
                var sensors = this.Triggers[state.CurrentQuestionTriggerID].Sensors;
                if (mode == TextGetterMode.Common || mode == TextGetterMode.DescriptionsOnly)
                {
                    if (this.Descriptions.ContainsKey(sensors[id].idb)&& this.Descriptions[sensors[id].idb].TryGetDescription(out Description desc, dialogType, lang))
                    {
                        return desc.text;
                    }
                }
                if (mode==TextGetterMode.Common || mode == TextGetterMode.NamesOnly)
                {
                    //string tex = sensors[id].name ?? sensors[id].name_idb??string.Empty;
                    string tex2 = GetName(id, state.lang)?? GetName(sensors[id].idb,state.lang) ?? string.Empty;

                    return tex2;
                
                }
                    
            }
            return string.Empty; 
        }


        private string GetName(ulong id, Lang lang, NameType nameType = NameType.Name)
        {
            return Names.TryGetValue(id, out Name name) && name.TryGetName(nameType, lang, out string GettedName) ? GettedName : null;
        }
        #endregion

        #region Next question calculation
        private bool checkSensorResultRequirment(int val)
        {
            return (val > 0 && val <= 8) || val == 18 || val == 48 || val == 19 || (val > 11 && val < 15);
        }

        private bool TryAddIsolationSensor(InterviewerState state, InterviewerItem sensor, bool required, bool Force = false)
        {
            int val = GetSensorCaseSwitch(sensor.id, state);

            if ((checkSensorResultRequirment(val) || Force) &&
                !state.DataToIsolationSensor.ContainsKey(sensor.idb))
            {
                IsolationSensor isolationSensor = new IsolationSensor(sensor.idb,
                    sensor.id, state.CurrentQuestionTriggerID, 0, sensor.name_idb, required) {sensor = sensor };
                isolationSensor.Forced = Force;
                state.DataToIsolationSensor.Add(sensor.idb, isolationSensor);
                return true;
            }
            return false;
        }

        private bool TrySetCurrentQuestionTriggerID(ulong id, InterviewerState state)
        {
            if (this.Triggers.ContainsKey(id))
            {
                state.CurrentQuestionTriggerID = id;
                return true;
            }

            return false;
        }

        private HashSet<ulong> GetHashSetIDTrurnOnSensors(InterviewerState state)
        {
            HashSet<ulong> temp = new HashSet<ulong>();

            if (!CheckSensor(state, ChekerTriggerIDs[state.CurrentQuestionTriggerID]))
            {
                return temp;
            }

            foreach (ulong id in this.Triggers[state.CurrentQuestionTriggerID].Sensors.Keys)
                if (!this.Triggers[state.CurrentQuestionTriggerID].PlexsToSensorGroups.ContainsKey(id))
                {
                    bool t = CheckSensor(state, this.Triggers[state.CurrentQuestionTriggerID].Sensors[id]);
                    if (t)
                    {
                        temp.Add(id);
                    }
                }


            return temp;
        }

        private bool CheckSensor(InterviewerState state, InterviewerItem eTItem)
        {
            if (eTItem.SensorTurnOn.Count > 0)
            {
                if (eTItem.LogicSensorTurnOn.Count > 0)
                {
                    HashSet<uint> hash = new HashSet<uint>();
                    foreach (uint idb in eTItem.LogicSensorTurnOn.Keys)
                        foreach (uint ida in eTItem.LogicSensorTurnOn[idb])
                            hash.Add(ida);

                    List<InterviewerItem> temp = new List<InterviewerItem>();

                    foreach (uint id in eTItem.SensorTurnOn.Keys)
                        if (!hash.Contains(id))
                            temp.Add(eTItem.SensorTurnOn[id]);

                    if (temp.Count > 0)
                        return CheckOrSensors(state, eTItem, temp);
                }

                return CheckOrSensors(state, eTItem, eTItem.SensorTurnOn.Values.ToList());
            }
            return true;
        }

        private bool CheckLogicSensorBySensorTurnOn(InterviewerState state, InterviewerItem eTItem, InterviewerItem PlexNewBigId, bool isRF, bool isNS)
        {
            if (PlexNewBigId.idb == Constants.ID_OR)
            {
                var tempOR = new List<InterviewerItem>();
                if (eTItem.LogicSensorTurnOn.ContainsKey(PlexNewBigId.id))
                    foreach (uint ida in eTItem.LogicSensorTurnOn[PlexNewBigId.id])
                        tempOR.Add(eTItem.SensorTurnOn[ida]);

                return CheckOrSensors(state, eTItem, tempOR);
            }
            else if (PlexNewBigId.idb == Constants.ID_AND)
            {
                var tempAND = new List<InterviewerItem>();
                if (eTItem.LogicSensorTurnOn.ContainsKey(PlexNewBigId.id))
                    foreach (uint ida in eTItem.LogicSensorTurnOn[PlexNewBigId.id])
                        tempAND.Add(eTItem.SensorTurnOn[ida]);

                return CheckAndSensors(state, eTItem, tempAND);
            }
            else if (PlexNewBigId.idb == Constants.IDDataToRiskFactor)
            {
                return true;
            }
            else if (PlexNewBigId.idb == Constants.IDDataToSymptoms)
            {
                return true;
            }
            else
            {
                bool? temp = IsTurnOn(state, PlexNewBigId, isRF, isNS);

                if (temp == null)
                    return true;

                return (bool)temp;
            }
        }

        private bool CheckOrSensors(InterviewerState state, InterviewerItem eTItem, List<InterviewerItem> list)
        {
            if (list.Count > 0)
            {
                bool isRF = false;
                bool isNS = false;

                foreach (InterviewerItem PlexNewBigId in list)
                {
                    if (PlexNewBigId.idb == Constants.IDDataToRiskFactor)
                        isRF = true;

                    if (PlexNewBigId.idb == Constants.IDDataToSymptoms)
                        isNS = true;
                }

                if (!isNS && !isRF)
                {
                    foreach (InterviewerItem PlexNewBigId in list)
                    {
                        if (CheckLogicSensorBySensorTurnOn(state, eTItem, eTItem.SensorTurnOn[PlexNewBigId.id], !isRF, !isNS))
                            return true;
                    }
                }
                else
                {
                    foreach (InterviewerItem PlexNewBigId in list)
                    {
                        if (CheckLogicSensorBySensorTurnOn(state, eTItem, eTItem.SensorTurnOn[PlexNewBigId.id], isRF, isNS))
                            return true;
                    }
                }
            }

            return false;
        }

        private bool CheckAndSensors(InterviewerState state, InterviewerItem eTItem, List<InterviewerItem> list)
        {
            if (list.Count > 0)
            {
                bool isRF = false;
                bool isNS = false;

                foreach (InterviewerItem PlexNewBigId in list)
                {
                    if (PlexNewBigId.idb == Constants.IDDataToRiskFactor)
                        isRF = true;

                    if (PlexNewBigId.idb == Constants.IDDataToSymptoms)
                        isNS = true;
                }

                if (!isNS && !isRF)
                {
                    foreach (InterviewerItem PlexNewBigId in list)
                    {
                        if (!CheckLogicSensorBySensorTurnOn(state, eTItem, eTItem.SensorTurnOn[PlexNewBigId.id], !isRF, !isNS))
                            return false;
                    }
                }
                else
                {
                    foreach (InterviewerItem PlexNewBigId in list)
                    {
                        if (!CheckLogicSensorBySensorTurnOn(state, eTItem, eTItem.SensorTurnOn[PlexNewBigId.id], isRF, isNS))
                            return false;
                    }
                }
            }

            return true;
        }

        private bool? IsTurnOn(InterviewerState state, InterviewerItem PlexNewBigId, bool isRS, bool isNS)
        {
            Dictionary<ulong, IsolationSensor> DataToIsolationSensor = state.DataToIsolationSensor;
            if (IdParser.ParseNewBigId(PlexNewBigId.idb).lib == 86)
            {
                List<IsolationSensor> answers = DataToIsolationSensor.Values.ToList();

                List<IsolationSensor> _answers = answers.FindAll(item => item.Id == PlexNewBigId.idb && item.TriggerId == PlexNewBigId.ida);
                if (_answers.Count > 0)//Проверка связи с сенсором
                {
                    foreach (IsolationSensor sensor in _answers)
                    {
                        if (!sensor.IsTurnOn && sensor.Required) return false;
                    }
                    return true;
                }


                if (Triggers.ContainsKey(PlexNewBigId.idb))//проверка триггера
                {
                    if (state.TriggerToSensorsLinks.ContainsKey(PlexNewBigId.idb))
                    {
                        _answers = answers.FindAll(item => item.IsTurnOn);
                        foreach (ulong id in state.TriggerToSensorsLinks[PlexNewBigId.idb])
                        {
                            if (_answers.FindIndex(item => item.Id == id) < 0) return false;
                        }
                        return true;
                    }
                    else return false;


                }


            }
            else if (PlexNewBigId.value_a == null)
                return null;
            int caseSwitch = GetCaseSwitchByValueA(PlexNewBigId.value_a);

            switch (caseSwitch)
            {
                case 1:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                    {
                        if (DataToIsolationSensor[PlexNewBigId.idb].IsTurnOn)
                            return true;
                        else
                            return false;
                    }


                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            return true;
                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            return true;
                    }
                    return false;

                case 2:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                    {
                        if (!DataToIsolationSensor[PlexNewBigId.idb].IsTurnOn)
                            return true;
                        else
                            return false;
                    }



                    if (state.TryGetProbability != null)
                    {
                        if (isRS && !state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            return true;
                        if (isNS && !state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            return true;
                    }

                    return false;

                case 5:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                        if (DataToIsolationSensor[PlexNewBigId.idb].ContainValueD)
                        {
                            if ((double)DataToIsolationSensor[PlexNewBigId.idb].ValueD >= PlexNewBigId.value_b - double.Epsilon &&
                               (double)DataToIsolationSensor[PlexNewBigId.idb].ValueD <= PlexNewBigId.value_c + double.Epsilon)
                                return true;
                            else
                                return false;
                        }
                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                        {
                            if (res1 >= PlexNewBigId.value_b - double.Epsilon &&
                                    res1 <= PlexNewBigId.value_c + double.Epsilon)
                                return true;
                        }
                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                        {
                            if (res2 >= PlexNewBigId.value_b - double.Epsilon &&
                                    res2 <= PlexNewBigId.value_c + double.Epsilon)
                                return true;
                        }

                    }

                    return false;

                case 6:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                        if (DataToIsolationSensor[PlexNewBigId.idb].ContainValueD)
                        {
                            if ((double)DataToIsolationSensor[PlexNewBigId.idb].ValueD <
                                PlexNewBigId.value_b - double.Epsilon)
                                return true;
                            else
                                return false;
                        }

                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            if (res1 < PlexNewBigId.value_b - double.Epsilon)
                                return true;

                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            if (res2 < PlexNewBigId.value_b - double.Epsilon)
                                return true;

                    }




                    return false;

                case 7:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                        if (DataToIsolationSensor[PlexNewBigId.idb].ContainValueD)
                        {
                            if ((double)DataToIsolationSensor[PlexNewBigId.idb].ValueD <=
                                PlexNewBigId.value_b + double.Epsilon)
                                return true;
                            else
                                return false;
                        }

                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            if (res1 <= PlexNewBigId.value_b + double.Epsilon)
                                return true;

                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            if (res2 <= PlexNewBigId.value_b + double.Epsilon)
                                return true;

                    }

                    return false;

                case 8:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                        if (DataToIsolationSensor[PlexNewBigId.idb].ContainValueD)
                        {
                            if ((double)DataToIsolationSensor[PlexNewBigId.idb].ValueD >=
                                PlexNewBigId.value_b - double.Epsilon)
                                return true;
                            else
                                return false;
                        }
                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            if (res1 >= PlexNewBigId.value_b - double.Epsilon)
                                return true;

                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            if (res2 >= PlexNewBigId.value_b - double.Epsilon)
                                return true;
                    }


                    return false;

                case 9:
                    if (DataToIsolationSensor.ContainsKey(PlexNewBigId.idb))
                        if (DataToIsolationSensor[PlexNewBigId.idb].ContainValueD)
                        {
                            if ((double)DataToIsolationSensor[PlexNewBigId.idb].ValueD >
                                PlexNewBigId.value_b + double.Epsilon)
                                return true;
                            else
                                return false;
                        }

                    if (state.TryGetProbability != null)
                    {
                        if (isRS && state.TryGetProbability(InterviewerState.ProbabilityType.Risk, PlexNewBigId.idb, out double res1))
                            if (res1 > PlexNewBigId.value_b + double.Epsilon)
                                return true;

                        if (isNS && state.TryGetProbability(InterviewerState.ProbabilityType.Nosology, PlexNewBigId.idb, out double res2))
                            if (res2 > PlexNewBigId.value_b + double.Epsilon)
                                return true;
                    }

                    return false;

                default:
                    return null;
            }
        }

        private void CalculateNextTrigger(InterviewerState state)
        {
            ulong OldCurrent = state.CurrentQuestionTriggerID;
            bool next = false;
            ulong? nextId = new ulong?();

            bool jump = false;
            if (this.Triggers[state.CurrentQuestionTriggerID].Collectors.Count > 0)
            {
                var eTItems = this.Triggers[state.CurrentQuestionTriggerID].Collectors.Values.ToList();
                foreach (var item in eTItems)
                {
                    if (CheckSensor(state, item) && this.TrySetCurrentQuestionTriggerID(item.idb, state))
                    {
                        nextId = item.idb;
                        jump = true;
                        break;
                    }
                }
            }

            if (!jump)
                foreach (uint id in sorting)
                {
                    if (next)
                    {
                        if (this.Triggers[id].Sensors.Count > 0 && this.TrySetCurrentQuestionTriggerID(id, state))
                        {
                            nextId = id;
                            break;
                        }
                    }

                    if (id == state.CurrentQuestionTriggerID)
                        next = true;
                }

            if (OldCurrent == state.CurrentQuestionTriggerID)
            {
                state.CheckerEnded = true;
            }
        }
        #endregion

        #region Support methods
        internal bool CheckSkipability(ulong triggerId)
        {
            double? val_a = ChekerTriggerIDs[triggerId].value_a;//Обработка возможности пропуска страницы
            return val_a != null && Math.Round((double)val_a) == 1;
        }
        internal bool CheckSkipability(string triggerId)
        {
            if (ulong.TryParse(triggerId, out ulong TriggerId))
            {
                return CheckSkipability(TriggerId);
            }
            else throw new InvalidCastException("Cannot parse Trigger id: " + triggerId);
        }
        internal void CheckAnsversIntegrity(InterviewerState state)
        {
            if (state.DataToIsolationSensor != null && state.DataToIsolationSensor.Count > 0)
            {
                List<IsolationSensor> isolationSensors = state.DataToIsolationSensor.Values.ToList();
                List<IsolationSensor> FailedAnswers = isolationSensors.FindAll(item => item.Required && !item.IsTurnOn && !CheckSkipability(item.TriggerId));
                if (FailedAnswers.Count > 0)
                {
                    string message = "Даны ответы не на все обязательные вопросы! Пропущенные: ";
                    foreach (IsolationSensor ans in FailedAnswers)
                    {
                            message += string.Format("Name: {0}; id: {1}; idb: {2}; \n", ans.Name, ans.Id, ans.Idb);
                    }
                    throw new UncorrectAnsverException(message);

                }
            }
        }
        internal static int GetCaseSwitchByValueA(double? valueA)
        {
            if (valueA != null)
                return Convert.ToInt32(Math.Round((double)valueA));
            return 0;
        }
        private int GetSensorCaseSwitch(ulong id, InterviewerState state)
        {
            if (this.Triggers[state.CurrentQuestionTriggerID].Sensors.ContainsKey(id))
            {
                if (this.Triggers[state.CurrentQuestionTriggerID].Sensors[id].idb == Constants.ID_OR)
                    return -6;

                if (this.Triggers[state.CurrentQuestionTriggerID].Sensors[id].value_a != null)
                {
                    return ConvertValueToCase((double)this.Triggers[state.CurrentQuestionTriggerID].Sensors[id].value_a);
                }
                else if (this.Triggers[state.CurrentQuestionTriggerID].Sensors[id].idb == Constants.ID_GROUP)
                {
                    return -6;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        internal static int ConvertValueToCase(double value)
        {
            try
            {
                return Convert.ToInt32(Math.Round(value));
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Set answers
        private void CheckAnswer(InputStruct[] inputStructs,InterviewerState state, string _triggerId)
        {
            if (ulong.TryParse(_triggerId, out ulong TriggerId))
            {
                if (TriggerId != state.CurrentQuestionTriggerID)
                    throw new UncorrectAnsverException("Несоответствие переданного ID триггера ожидаемому! Возможно нарушение последовательности ответов.");
                if (!Triggers.ContainsKey(state.CurrentQuestionTriggerID))
                    throw new UncorrectAnsverException("TriggerId " + TriggerId + "отсутствует в чекере");

                HashSet<ulong> RequirementSensors = new HashSet<ulong>();
                foreach (InterviewerItem sensor in Triggers[TriggerId].Sensors.Values)
                {
                    if (sensor.value_b != null)
                    {
                        int val = (int)Math.Round((double)sensor.value_b);
                        if (val == Constants.RequiredId)
                        {
                            RequirementSensors.Add(sensor.id);
                        }
                    }
                }
                HashSet<ulong> ExistingGroups = new HashSet<ulong>();
                if (inputStructs != null)
                {
                    foreach (InputStruct inputStruct in inputStructs)
                    {
                        if (ulong.TryParse(inputStruct.Id, out ulong Id))
                        {
                            if (ulong.TryParse(inputStruct.Idb, out ulong Idb))
                            {
                                RequirementSensors.Remove(Id);
                                if (Triggers[TriggerId].OrGroupsHeads != null && Triggers[TriggerId].OrGroupsHeads.Count > 0)
                                {
                                    if (Triggers[TriggerId].Sensors.ContainsKey(Id))
                                    {
                                        if (Triggers[TriggerId].Sensors[Id].idb != Idb)
                                            throw new Exceptions.UncorrectAnsverException("Uncorrect answer idb: " + inputStruct.Idb);
                                        else
                                        {
                                            if (Triggers[TriggerId].OrGroupsHeads != null)
                                            {
                                                foreach (InterviewerItem GroupHead in Triggers[TriggerId].OrGroupsHeads)
                                                {
                                                    if (GroupHead.OrGroupedItems != null)
                                                    {
                                                        if (GroupHead.OrGroupedItems.ContainsKey(Id))
                                                        {
                                                            if (ExistingGroups.Contains(GroupHead.id))
                                                            {
                                                                throw new Exceptions.UncorrectAnsverException("Multiple answers in OR group!");
                                                            }
                                                            else
                                                            {
                                                                ExistingGroups.Add(GroupHead.id);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (ulong groupId in ExistingGroups)
                    RequirementSensors.Remove(groupId);

                if (RequirementSensors.Count!=0&&ChekerTriggerIDs[TriggerId].value_a!=null&& Math.Round((double)ChekerTriggerIDs[TriggerId].value_a)==2)
                {
                    throw new Exceptions.UncorrectAnsverException("Not all requirment sensors answered!");
                }
            }
            else throw new InvalidCastException("Cannot parse Trigger id: " + TriggerId);
        }
        private void SetAnswer(InputStruct[] inputStructs, string TriggerId, InterviewerState state)
        {
            CheckAnswer(inputStructs, state, TriggerId);
            SetDataFromAnsver(inputStructs, state);
            CalculateNextTrigger(state);            
        }
        private void SetDataFromAnsver(InputStruct[] inputStructs, InterviewerState state)
        {
            try
            {
                if (inputStructs!=null)
                for (int i = 0; i < inputStructs.Length; i++)
                {
                    InputStruct inputStruct = inputStructs[i];
                        if (ulong.TryParse(inputStruct.Idb, out ulong idb) && ulong.TryParse(inputStruct.Id, out ulong id))
                        {
                            if (state.DataToIsolationSensor.ContainsKey(idb))
                            {
                                IsolationSensor sensor = state.DataToIsolationSensor[idb];
                                sensor.Id = id;
                                sensor.TriggerId = state.CurrentQuestionTriggerID;
                                int data_id = GetSensorCaseSwitch(sensor.sensor.id, state);
                                if (data_id == 5 || data_id == 6)
                                {
                                    if (bool.TryParse(inputStruct.Value, out bool ansver))
                                    {
                                        sensor.ValueB = ansver;
                                    }
                                    else
                                    {
                                        sensor.ValueB = true;
                                    }
                                }
                                else if (data_id == 3 || data_id == 4 || (data_id > 11 && data_id < 15))
                                {
                                    if (double.TryParse(inputStruct.Value, out double ansver))
                                    {
                                        sensor.ValueD = ansver;
                                    }
                                }
                                else
                                {
                                    if (inputStruct.Value != null && !inputStruct.Value.Equals(string.Empty))
                                        if (inputStruct.Value.Length < 1000) sensor.ValueS = inputStruct.Value;
                                        else sensor.ValueB = true;
                                }
                            }
                        }
                    }
            }
            catch 
            {

            }
        }
        #endregion
    }

}