using System;
using System.Collections.Generic;
using System.Text;

namespace SecureRequest
{
    public interface ISecureRequest
    {
        public HttpResult SecureData(HttpResult source);
    }
}
