using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdCab { get; set; }
        public string CodCliente { get; set; }
        public string CodArticulo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
                 public decimal TotalCab { get; set; }


        public DetallePedido()
        {
            IdCab = 0;
            CodCliente = string.Empty;
            CodArticulo = string.Empty;
            Descripcion = string.Empty;
            Cantidad = 0;
            PrecioUnitario = 0;
            Total = 0;

        }
    } 

    public class ResponseDetalle : WebResponse.BaseResponse
    {
        public DetallePedido Detalle { get; set; }
        public ResponseDetalle()
        {
            Detalle = new DetallePedido();
        }
    }

    public class ResponseDetalleList : WebResponse.BaseResponse
    {
        public List<DetallePedido> ListDetalle { get; set; }
        public ResponseDetalleList()
        {
            ListDetalle = new List<DetallePedido>();
        }
    }

}
