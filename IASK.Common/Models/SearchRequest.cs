using System;
using System.Collections.Generic;

namespace IASK.Common.Models
{
    /// <summary>
    /// Модель запроса к сфинксу
    /// </summary>
    public class SearchRequest : BaseModel
    {
        /// <summary>
        /// Текст который ищется
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Либы в которых осуществляется поиск
        /// </summary>
        public List<string> Libs { get; set; }
        /// <summary>
        /// Левелы, по которым осуществляется фильтрация результатов
        /// </summary>
        public List<string> Levels { get; set; }
        public bool TryGetLibsString(out string LibsString)
        {
            bool IsRequestCorrect = !String.IsNullOrEmpty(Text);
            LibsString = string.Empty;
            if (IsRequestCorrect && Libs.Count == 1)
            {
                LibsString = Libs[0];
                return true;
            }
            else if (IsRequestCorrect && Libs.Count > 1)
            {
                for (int i = 0; i < Libs.Count; i++)
                {
                    if (!uint.TryParse(Libs[i], out uint re)) throw new ArgumentException(string.Format("Cannot cast lib value {0} to uint", Libs[i]));
                    LibsString += Libs[i].ToString();
                    if (i + 1 != Libs.Count) LibsString += ",";
                }
                return true;
            }
            SetAlert(System.Net.HttpStatusCode.BadRequest, message: "No data for finding");
            return false;
        }
    }
}
