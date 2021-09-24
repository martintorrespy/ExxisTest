using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Afiliados.Utils;
using Web.Pedidos.Utils;

namespace ApiPedidos.Controllers
{


    [RoutePrefix("api/detallepedido")]
    public class DetallePedidoController : ApiController
    {

        [HttpPost]
        [Route("guardardatoscdetalle")]
        public IHttpActionResult GuardarDatosDetalle(DataModel.DetallePedido DetallePedido)
        {
            var retorno = GuardarDetalle(DetallePedido);
            return Ok(retorno);
        }


        [HttpPost]
        [Route("obtenerdatosdetalleall")]
        public IHttpActionResult ObtenerDetalleAll(DataModel.CabeceraPedido cabeceraPedido)
        {
            var retorno = new DataModel.ResponseDetalleList();
            try
            {
                CacheManager DatosCache = new CacheManager();
                if (DatosCache.Cache.IsSet(Utils.CacheClave))
                {
                    var ListDetalle1 = ((List<DataModel.DetallePedido>)DatosCache.Cache.Get(Utils.CacheClaveDet));                  
                    retorno.ListDetalle.AddRange(ListDetalle1);
                }
                if (retorno.ListDetalle.Count > 0)
                {
                    retorno.CodRet = 0;
                    retorno.HasError = false;
                    retorno.Msg = "Correcto";
                }
                else
                {
                    retorno.CodRet = 1;
                    retorno.HasError = false;
                    retorno.Msg = "Lista Vacia";
                }
            }
            catch (Exception)
            {

                retorno.CodRet = -1;
                retorno.HasError = true;
                retorno.Msg = "Error al guardar datos de cabecera";
            }
            return Ok(retorno);
        }

        #region Internals
        internal DataModel.ResponseDetalle GuardarDetalle(DataModel.DetallePedido DetallePedido)
        {
            var retorno = new DataModel.ResponseDetalle();
            try
            {
                if (string.IsNullOrEmpty(DetallePedido.CodArticulo))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El CodArticulo es Obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(DetallePedido.Descripcion))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = " Descripcion es Obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(DetallePedido.Cantidad.ToString()))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "Cantidad es obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(DetallePedido.Total.ToString()))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Total es Obligatorio";
                    return retorno;
                }

                if (!Utils.IsNumeric(DetallePedido.PrecioUnitario))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El PrecioUnitario debe ser numerico";
                    return retorno;
                }
                if (!Utils.IsNumeric(DetallePedido.Cantidad))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "Cantidad debe ser numerico";
                    return retorno;
                }
                List<DataModel.DetallePedido> ListaDet = new List<DataModel.DetallePedido>();
                CacheManager DatosCache = new CacheManager();
                if (!DatosCache.Cache.IsSet(Utils.CacheClaveDet))
                {
                    if(DetallePedido.Total > DetallePedido.TotalCab)
                    {
                        retorno.CodRet = -1;
                        retorno.HasError = true;
                        retorno.Msg = "total detalle supera a configuracion de total cab";
                        return retorno;
                    }
                    DetallePedido.Id = 1;
                    ListaDet.Add(DetallePedido);
                    DatosCache.Cache.Set(Utils.CacheClaveDet, ListaDet, 1440);
                }
                else
                {
                    ListaDet = ((List<DataModel.DetallePedido>)DatosCache.Cache.Get(Utils.CacheClaveDet));
                    var sumaTotalDet = ListaDet.Sum(X => X.Total) + DetallePedido.Total;
                    if (sumaTotalDet > DetallePedido.TotalCab)
                    {
                        retorno.CodRet = -1;
                        retorno.HasError = true;
                        retorno.Msg = "total detalle supera a configuracion de total cab";
                        return retorno;
                    }
                    DetallePedido.Id = ListaDet.Count() + 1;
                    ListaDet.Add(DetallePedido);
                    DatosCache.Cache.Set(Utils.CacheClaveDet, ListaDet, 1440);
                }
                retorno.CodRet = 0;
                retorno.HasError = false;
                retorno.Msg = "El registro se agrego correctamente";
            }
            catch (Exception ex)
            {
                retorno.CodRet = -1;
                retorno.HasError = true;
                retorno.Msg = "Error al guardar datos de detalle";
                // hay que loggearla variable ex 
            }
            return retorno;
        }
        #endregion

    }
}
