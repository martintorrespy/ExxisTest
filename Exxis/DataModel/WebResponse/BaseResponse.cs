using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.WebResponse
{ 
    public class BaseResponse
    {
        public bool HasError { get; set; }
        public int CodRet { get; set; }
        public string Msg { get; set; }

        public BaseResponse()
        {
            Msg = string.Empty;
            CodRet = 0;
            HasError = false;
        }
    } 
}
