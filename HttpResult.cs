using System;
using System.Collections.Generic;
using System.Text;

namespace SecureRequest
{
    /// <summary>
    /// Ответ веб-сервиса
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// Адрес удаленного узла
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Тело запроса
        /// </summary>
        public string RequestBody { get; set; }
        /// <summary>
        /// Тело ответа
        /// </summary>
        public string ResponseBody { get; set; }

        public HttpResult() { }

        /// <summary>
        /// Создает копию HttpResult
        /// </summary>
        /// <returns></returns>
        public HttpResult Copy()
        {
            HttpResult result = new HttpResult()
            {
                Url = this.Url,
                RequestBody = this.RequestBody,
                ResponseBody = this.ResponseBody
            };
            return result;
        }
    }
}
