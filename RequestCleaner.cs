using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecureRequest
{
    /// <summary>
    /// Выполняет зачистку данных пользователя в HttpResult
    /// </summary>
    public class RequestCleaner : ISecureRequest
    {
        private string[] GET_SECURE_PARAMS = new string[] {"user","pass"};
        private string[] REST_SECURE_PARAMS = new string[] { "users"};
        
        /// <summary>
        /// Символ, на который заменяются данные пользователя
        /// </summary>
        public char ReplacingChar { get; private set; }
        /// <summary>
        /// Параметры для поиска и замены в GET-запросах
        /// </summary>
        public string[] SearchPatternGET
        {
            get { return GET_SECURE_PARAMS; }
        }
        /// <summary>
        /// Параметры для поиска и замены в REST-запросах
        /// </summary>
        public string[] SearchPatternREST
        {
            get { return REST_SECURE_PARAMS; }
        }

        /// <summary>
        /// Создает новый объект для зачистки данных пользователей
        /// </summary>
        public RequestCleaner() 
        {
            ReplacingChar = 'X';
        }

        /// <summary>
        /// Создает новый объект для зачистки данных пользователей
        /// </summary>
        /// <param name="repChar">Символ для замены (по-умолчанию X)</param>
        /// <param name="GETSecureParams">Параметры для замены в GET-запросах</param>
        /// <param name="RESTSecureParams">Параметры для замены в REST-запросах</param>
        public RequestCleaner(char repChar, IEnumerable<string> GETSecureParams, IEnumerable<string> RESTSecureParams) 
        {
            ReplacingChar = repChar;
            if (GETSecureParams != null)
                GET_SECURE_PARAMS = GETSecureParams.ToArray();
            if (RESTSecureParams != null)
                REST_SECURE_PARAMS = RESTSecureParams.ToArray();
        }

        /// <summary>
        /// Зачищает данные пользователя в HttpResult
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public HttpResult SecureData(HttpResult source)
        {
            HttpResult result = new HttpResult();
            result.Url = SecureString(source.Url);
            result.RequestBody = SecureString(source.RequestBody);
            result.ResponseBody = SecureString(source.ResponseBody);
            return result;
        }

        /// <summary>
        /// Зачищает данные пользователя в строке
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public string SecureString(string original)
        {
            Uri uri = null;
            if (!Uri.TryCreate(original, UriKind.RelativeOrAbsolute, out uri))
                throw new ArgumentException($"Не удается создать Uri из строки {original}");
            UriBuilder ub = new UriBuilder(uri);
            ub.Path = SecurePath(ub.Path);
            ub.Query = SecureQuery(ub.Query);
            string result = ub.Uri.AbsoluteUri;
            //свойство Path возвращает хотя бы косую черту / даже если Path пустой
            if (ub.Uri.AbsolutePath == "/")
                result = result.Replace("/?", "?");
            return result;
        }

        /// <summary>
        /// Выполняет очистку данных пользователя в Uri.Path
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        private string SecurePath(string sourcePath)
        {
            List<string> pathArgs = new List<string>();
            pathArgs.AddRange(sourcePath.ToLower().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            foreach (string searchPattern in SearchPatternREST)
            {
                int usersIndex = pathArgs.IndexOf(searchPattern.ToLower());
                if (usersIndex != -1 && usersIndex != pathArgs.Count - 1)
                {
                    pathArgs[usersIndex + 1] = new string(ReplacingChar, pathArgs[usersIndex + 1].Length);
                }
            }
            StringBuilder sb = new StringBuilder();
            foreach (string item in pathArgs)
            {
                sb.Append($"/{item}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Выполняет очистку данных пользователя в Uri.Query
        /// </summary>
        /// <param name="sourceQuery"></param>
        /// <returns></returns>
        private string SecureQuery(string sourceQuery)
        {
            Dictionary<string, string> queryArgs = new Dictionary<string, string>();
            string[] argsArray = sourceQuery.TrimStart('?').Split('&');
            foreach (string argItem in argsArray)
            {
                string[] nameValue = argItem.Split('=');
                queryArgs.Add(nameValue[0].ToLower(), nameValue[1]);
            }
            foreach (string searchPattern in SearchPatternGET)
            {
                string sp = searchPattern.ToLower();
                if (queryArgs.ContainsKey(sp))
                {
                    queryArgs[sp] = new string(ReplacingChar, queryArgs[sp].Length);
                }
            }
            StringBuilder result = new StringBuilder();
            foreach (var item in queryArgs)
            {
                result.Append($"{item.Key}={item.Value}&");
            }
            return result.ToString().TrimEnd('&');
        }

    }
}
