using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UMKBRequests.Models.API.Semantic;


namespace IASK.InterviewerEngine.Models.Output
{
    /// <summary>
    /// Класс для передачи и хранения дополнительного содержимого структур в интерфейсе пользователя. 
    /// Лежит в основе поля <see cref="InterfaceUnit.Content"/>. <br/>
    /// Может содержать виды данных, описанные в <see cref="Content.ContentType"/>.
    /// </summary>
    public class Content
    {
        #region Fields& Properities
        /// <summary>
        /// Id элемента
        /// </summary>
        public readonly string Id;
        /// <summary>
        /// Id родителя для формирования древовидной структуры
        /// </summary>
        public readonly string ParentId;
        /// <summary>
        /// Основное поле для отправки дополнительного содержимого элемента на фронт.
        /// </summary>
        public readonly string Value;
        /// <summary>
        /// Дополнительное поле для отправки данных на фронт. Используется при отправке расчитанных вероятностей.
        /// </summary>
        public readonly string Value2;
        /// <summary>
        /// Тип отправляемого содержимого
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public readonly ContentType Type;

        #endregion

        #region enum
        /// <summary>
        /// Возможные типы контента.
        /// </summary>
        public enum ContentType
        {
            /// <summary>
            /// Тип не определн. Значение по умолчанию.
            /// </summary>
            undef,
            /// <summary>
            /// Вывод риска возникновения нозологии
            /// </summary>
            risk,
            /// <summary>
            /// Вывод шанса наличия нозологии
            /// </summary>
            symptom,
            /// <summary>
            /// Содержит регулярное выражение для проверки вводимых пользователем 
            /// данных.
            /// </summary>
            regex,
            /// <summary>
            /// Содержит маску ввода пользовательских данных.
            /// </summary>
            mask,
            /// <summary>
            /// Элемент древовидной структуры
            /// </summary>
            tree_element,
            /// <summary>
            /// Концепт в выпадающем списке концептов
            /// </summary>
            concept,
            /// <summary>
            /// Числовое значения для выпадающего списка с численными параметрами (рос, вес и т.д.)
            /// </summary>
            parameter,
            /// <summary>
            /// Результ поиска
            /// </summary>
            search_result,
            /// <summary>
            /// Раздел (lib).
            /// </summary>
            lib,
            /// <summary>
            /// Настройка для поиска. Раздел (lib), в котором проводить поиск.
            /// </summary>
            id,
            /// <summary>
            /// id объекта.
            /// </summary>
            level,
            /// <summary>
            /// Текст для вывода
            /// </summary>
            text,
            /// <summary>
            /// Текст со ссылкой для вывода
            /// </summary>
            hiperlink,
            first_name,
            name,
            last_name,
            second_name,
            email,
            snils,
            phone,
            passport,
            main_address,


            pergancy,
            weight,
            trimester,
            sex,

            disease,
            allergy,
            data,

            timestamp,
            doctor,
            mutable

        }

        #endregion

        #region Constructors

        public Content(string Id = null, string ParentId = null, string Value = null, string Value2 = null, ContentType Type = ContentType.undef)
        {
            this.Id = Id;
            this.ParentId = ParentId;
            this.Value = Value;
            this.Value2 = Value2;
            this.Type = Type;
        }
        #endregion

        #region methods
        internal static ContentType ConvertToContentType(InterviewerState.ProbabilityType probabilityType)
        {
            ContentType res;
            switch (probabilityType)
            {
                case InterviewerState.ProbabilityType.Nosology:
                    res = ContentType.symptom;
                    break;
                case InterviewerState.ProbabilityType.Risk:
                    res = ContentType.risk;
                    break;
                default:
                    res = ContentType.undef;
                    break;
            }
            return res;
        }

        internal static bool CheckStringValues(string val1, string val2)
        {
            if (val1 == null && val2 == null)
            {
                return true;
            }
            else if ((val1 != null && val2 == null) || (val1 == null && val2 != null))
            {
                return false;
            }
            else
            {
                return val1.Equals(val2);
            }
        }

        public static implicit operator Content(FieldsVarChar fd)
        {

            if (fd.field.Contains(Constants.InputMaskField)|| fd.field.Contains(Constants.InputMaskField2))
            {
                return new Content(Value: fd.value, Type: ContentType.mask);
            }
            else if (fd.field.Contains(Constants.RegexField))
            {
                return new Content(Value: fd.value, Type: ContentType.regex);
            }
            return new Content(Value: fd.value);
        }
        public override bool Equals(object obj)
        {
            Content _obj = obj as Content;
            if (_obj != null)
            {
                return CheckStringValues(_obj.Id, Id) && CheckStringValues(_obj.Value, Value) && CheckStringValues(_obj.ParentId, ParentId) && _obj.Type.Equals(Type);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (Id != null) hash ^= Id.GetHashCode();
            if (ParentId != null) hash^=ParentId.GetHashCode();
            if (Value != null) hash^= Value.GetHashCode();
            if (Value2 != null) hash^= Value2.GetHashCode();
            hash^= Type.GetHashCode();
            return hash;
        }
        #endregion
    }
}
