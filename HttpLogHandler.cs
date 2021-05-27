using System;
using System.Collections.Generic;
using System.Text;

namespace SecureRequest
{
    public class HttpLogHandler
    {
        HttpResult _currentLog;
        public HttpResult CurrentLog 
        { 
            get { return _currentLog; } 
        }

        public string Process(string url, string body, string response,ISecureRequest cleaner)
        {
            var httpResult = new HttpResult()
            {
                Url = url,
                RequestBody = body,
                ResponseBody = response
            };
            if (cleaner == null)
                throw new ArgumentException("Объект для зачистки данных пользователя не инициализирован");
            Log(cleaner.SecureData(httpResult));
            return response;
        }

        protected void Log(HttpResult res)
        {
            _currentLog = res.Copy();
        }
    }
}
