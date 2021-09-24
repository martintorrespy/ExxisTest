using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Pedidos.Utils
{
    public static class Utils
    {
        public static string CacheClave = "CabeceraPedido";
        public static string CacheClaveDet = "DetallePedido";
        public static bool IsNumeric(object Expression)
        {
            decimal retNum;

            bool isNum = Decimal.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}