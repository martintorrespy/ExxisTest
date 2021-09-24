using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class CabeceraPedido
    {
        public int Id { get; set; }
        public string CodCliente { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Vendedor { get; set; }
        public string Moneda { get; set; }
        public int Total { get; set; } 
        public CabeceraPedido()
        {
            Total = 0;
            Moneda = "Guaranies";
            Vendedor = string.Empty;
            FechaEntrega = DateTime.Now;
            FechaPedido = DateTime.Now;
            NombreCliente = string.Empty;
            CodCliente = string.Empty;
            Id = 0;
        }  
    }

   
    public class ResponseCabecera : WebResponse.BaseResponse
    {
        public CabeceraPedido Cabecera { get; set; }
        public ResponseCabecera()
        {
            Cabecera = new CabeceraPedido();
        }
    }

    public class ResponseCabeceraList : WebResponse.BaseResponse
    {
        public List<CabeceraPedido> ListCabecera { get; set; }
        public ResponseCabeceraList()
        {
            ListCabecera = new List<CabeceraPedido>();
        }
    }

}
