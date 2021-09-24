using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Afiliados.Utils;
using Web.Pedidos.Utils;

namespace ApiPedidos.Controllers
{
    [RoutePrefix("api/cabecerapedido")]
    public class CabeceraPedidoController : ApiController
    {
      
        [HttpPost]
        [Route("guardardatoscabecera")]
        public IHttpActionResult GuardarDatosCabecera(DataModel.CabeceraPedido cabeceraPedido)
        {
            var retorno = GuardarCacheCAb(cabeceraPedido);
            return Ok(retorno);
        }


        [HttpPost]
        [Route("obtenercabeceraall")]
        public IHttpActionResult ObtenerCabeceraAll(DataModel.CabeceraPedido cabeceraPedido)
        {
            var retorno = new DataModel.ResponseCabeceraList();
            try
            {
                CacheManager DatosCache = new CacheManager();
                if (DatosCache.Cache.IsSet(Utils.CacheClave))
                {
                    retorno.ListCabecera = ((List<DataModel.CabeceraPedido>)DatosCache.Cache.Get(Utils.CacheClave));                  
                    
                }
                if(retorno.ListCabecera.Count > 0)
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


        #region Internnals
        internal DataModel.ResponseCabecera GuardarCacheCAb(DataModel.CabeceraPedido cabeceraPedido)
        {
            var retorno = new DataModel.ResponseCabecera();
            try
            {
                if(string.IsNullOrEmpty(cabeceraPedido.CodCliente))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Codigo Cliente es Obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(cabeceraPedido.NombreCliente))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El NombreCliente es Obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(cabeceraPedido.Vendedor))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El dato de Vendedor es obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(cabeceraPedido.Total.ToString()))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Total es Obligatorio";
                    return retorno;
                }
                if (string.IsNullOrEmpty(cabeceraPedido.Total.ToString()))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Total es Obligatorio";
                    return retorno;
                }
                if(!Utils.IsNumeric(cabeceraPedido.Total))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Total es debe ser numerico";
                    return retorno;
                }
                if(cabeceraPedido.FechaPedido.Date < DateTime.Now.Date  )
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "La fecha pedido no puede ser menor a hoy";
                    return retorno;
                }
                if (cabeceraPedido.FechaEntrega.Date < cabeceraPedido.FechaPedido.Date)
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "La fecha de entrega de puede ser menor a la fecha de pedido";
                    return retorno;
                }
                if (!Utils.IsNumeric(cabeceraPedido.CodCliente))
                {
                    retorno.CodRet = -1;
                    retorno.HasError = true;
                    retorno.Msg = "El Cod Cliente debe ser numerico";
                    return retorno;
                }
                List<DataModel.CabeceraPedido> CapPed = new List<DataModel.CabeceraPedido>();
                CacheManager DatosCache = new CacheManager();
                if (!DatosCache.Cache.IsSet(Utils.CacheClave))
                {
                    cabeceraPedido.Id = 1;
                    CapPed.Add(cabeceraPedido);
                    DatosCache.Cache.Set(Utils.CacheClave, CapPed, 1440);
                }
                else
                {
                    CapPed =((List<DataModel.CabeceraPedido>)DatosCache.Cache.Get(Utils.CacheClave));
                    cabeceraPedido .Id= CapPed.Count() + 1;
                    CapPed.Add(cabeceraPedido);
                    DatosCache.Cache.Set(Utils.CacheClave, CapPed, 1440);
                }
                retorno.CodRet = 0;
                retorno.HasError = false;
                retorno.Msg = "El registro se agrego correctamente";
            }
            catch (Exception ex) 
            {
                retorno.CodRet = -1;
                retorno.HasError = true;
                retorno.Msg = "Error al guardar datos de cabecera";
                // hay que loggearla variable ex

            }
           
           
            return retorno;

        }
        #endregion
    }
}
