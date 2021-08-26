using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CheckerEngineTests")]

namespace IASK.InterviewerEngine.Models.Output
{
    /// <summary>
    /// Основной класс, описывающий интерфейс опросника.
    /// </summary>
    public class InterfaceUnit
    {
        #region Fields
        /// <summary>
        /// Свойство для корректного формирования вложенности элементов интерфейса пользователя.
        /// </summary>
        [JsonIgnore]

        internal ulong? ParentId { get; set; }
        /// <summary>
        /// Значение, хранящееся в элементе. Обычно используется фронтом в качестве значения по умолчанию.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// id сенсора. Отправляется обратно в вопросно-ответную систему в 
        /// <see cref="IASK.InterviewerEngine.Models.Input.InputStruct.Id"/>, если опрашиваемый вводит ответ.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// idb сенсора. Отправляется обратно в вопросно-ответную систему в 
        /// <see cref="IASK.InterviewerEngine.Models.Input.InputStruct.Idb"/>, если опрашиваемый вводит ответ.
        /// </summary>
        public string Idb { get; set; }
        /// <summary>
        /// Текст, отоброжаемый на элементе интерфейса
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Сокращенный, отображаемый на элементе интерфейса
        /// </summary>
        public string ShortLabel { get; set; }
        /// <summary>
        /// Дополнительный контент для отображения в интерфейсе пользователя.
        /// </summary>
        public List<Content> Content { get; set; }

        [JsonIgnore]
        int? Repeat { get; set; } = null;

        [JsonConverter(typeof(StringEnumConverter))]
        public UnitType Type { get; set; }

        [JsonIgnore]
        public BrowseCondition Condition { get; set; }
        [JsonProperty("Condition")]
        public string ConditionForSerilizer
        {
            get
            {
                if (Condition == BrowseCondition.CYCLE && Repeat != null)
                {
                    return Condition.ToString() + "_" + Repeat.ToString();
                }
                else
                {
                    return Condition.ToString();
                }
            }
        }

        public List<InterfaceUnit> Units { get; set; } = new List<InterfaceUnit>();

        #endregion

        #region constructors
        public InterfaceUnit()
        {

        }
        public InterfaceUnit(int value_a, double? value_b, double? value_c, double? value_d, List<Content> contents = null)
        {
            if (value_a == 44 || value_a == 45)
                value_a = 10;

            if (Enum.IsDefined(typeof(UnitType), value_a))
            {
                Type = ((UnitType)value_a);
            }
            else
            {
                Type = UnitType.EMPTY;
            }

            if (value_b != null)
            {
                int _value_b = (int)Math.Round((double)value_b);
                if (Enum.IsDefined(typeof(BrowseCondition), _value_b))
                {
                    Condition = ((BrowseCondition)_value_b);
                }
                else
                {
                    if (_value_b == 76 || Type == UnitType.HEADER)//Новый вариант - запись в правое плечо
                    {
                        Type = UnitType.FORCE_SEND;
                        Condition = BrowseCondition.NOREQUIRED;
                    }
                    else
                    {
                        Condition = BrowseCondition.REQUIRED;
                    }
                }
            }

            if (value_d != null)
            {
                Repeat = (int)Math.Round((double)value_d);
            }

            if (contents != null && contents.Count != 0)
            {
                this.Content = new List<Content>();
                foreach (var fvc in contents)
                    this.Content.Add(fvc);
            }
        }

        internal void SetTypeByFunction()
        {
            if (Type == UnitType.FUNCTION)
            {
                if (ulong.TryParse(Idb, out ulong idb)&&Constants.Functions.ContainsKey(idb))
                {
                    Type = Constants.Functions[idb];
                }
            }
        }

        #endregion

        #region enums
        public enum BrowseCondition
        {
            NOREQUIRED = 0,
            REQUIRED = 1,
            RIGTH_ELEMENT = 2,
            CYCLE = 77,
            GOTO = 81,
            COLLAPSED_PAGE = 80,
            undef = -1
        }

        public enum UnitType
        {
            SKIP_PERMITTED = -3,
            THE_END = -1000,
            HEADER = -1,
            FORCE_SEND = -2,
            EMPTY = 0,
            YES_NO_SKIP = 1,
            YES_NO = 2,
            NUMBER = 3,
            SELECT_PARAMETER = 4,
            CHECKBOX = 5,
            RADIOBUTTON = 6,
            RADIOBUTTON_GROUP = -6,
            INPUT = 7,
            SEARCH = 8,
            OK = 9,
            GROUP_HEADER = 10,
            TEXT = 11,
            TREE_SELECT = 12,
            SELECT_CONCEPT = 13,
            SPECIAL_SENSOR = 14,
            RISK_OUTPUT = 15,
            SEARCH_ADD = 18,
            MASKED_INPUT = 19,
            BUTTON = 20,
            ACCORDION = 21,
            ACCORDION_COLLAPSED = 22,
            NEW_WINDOW_BUTTON = 23,
            LINK_BUTTON = 24,
            TEXT_AREA = 25,
            PHOTO_UPLOAD = 48,
            FUNCTION = 60,
            PARAMETER = 90,
            CALENDAR = 856,
            TIME_INTERVAL = 859,
            SCHEDULE = 839,
            TABLE = -1001,
            HEADERS = -1002,
            ROW = -1003,
            CELL = -1004,
            LIST = -1005,
            PATIENT = -2000,
            PATIENT_COMMON = -2001,
            PATIENT_PARAMETERS = -2002,
            PATIENT_DISEASES = -2003,
            PATIENT_ALLERGIES = -2004,
            HEAD_PROTOCOL = -2005,
            PROTOCOL = -2006,
            PROTOCOL_DATA = -2007,
        }
        #endregion
    }
}
