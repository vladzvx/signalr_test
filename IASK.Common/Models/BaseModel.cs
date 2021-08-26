using System;
using System.Net;
using UMKBRequests;
using UMKBRequests.Models.API.Codes;

namespace IASK.Common.Models
{
    /// <summary>
    /// Класс, в который вынесен общие поля и методы всех моделей
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Поле для хранения прав доступа
        /// </summary>
        public Permit Permit { get; set; }
        /// <summary>
        /// Ответ на запрос (HTTP код с дополнительным сообщением)
        /// </summary>
        public Alert Alert { get; set; } = new Alert();
        public virtual void SetAlert(System.Net.HttpStatusCode statusCode, bool sticky = false, string message = null)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    {
                        Alert.Ok();
                        break;
                    }
                case HttpStatusCode.BadRequest:
                    {
                        Alert.BadRequest();
                        Alert.message += message ?? string.Empty;
                        break;
                    }
                default:
                    {
                        Alert.code = ((int)statusCode).ToString();
                        Alert.message = message ?? statusCode.ToString();
                        break;
                    }
            }
            Alert.sticky = sticky;

        }

        public virtual void SetAlert(Exception ex)
        {
            Alert.message = ex.Message;
            Alert.code = "500";
            Alert.sticky = false;
        }

        public virtual void SetAlert(string code, bool sticky = false, string message = null)
        {

            Alert.code = code;
            Alert.message = message;
            Alert.sticky = sticky;

        }
        public virtual void SetAlert(string code, bool sticky = false, string message = null, string title =null, string level = null)
        {
            Alert.title = level;
            Alert.level = title;
            Alert.code = code;
            Alert.message = message;
            Alert.sticky = sticky;
        }
        public virtual void SetAlert(UMKBRequests.Models.API.Semantic.Alert alert)
        {
            this.Alert.code = alert.code.ToString();
            this.Alert.message = alert.message;
            this.Alert.sticky = alert.sticky;
        }
    }
}
