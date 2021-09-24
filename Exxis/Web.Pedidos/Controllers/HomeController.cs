using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Web.Pedidos.Utils;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using DataModel;

namespace Web.Pedidos.Controllers
{
    public class HomeController : Controller
    {

        #region  internal
        internal string pintarRow(DataModel.ResponseCabeceraList Listadatos)
        {
            var retorno = string.Empty; 

            foreach(var row in Listadatos.ListCabecera)
            {
                string redirect = Url.Action("AgregarDetalle", "Home", new { id = row.Id });


            retorno += "<tr>"+
                     " <td>" + row.Id + "</td>" +
                           " <td>" +row.CodCliente+"</td>" +
                           " <td>" + row.NombreCliente + "</td>" +
                           " <td>" + row.FechaPedido.ToShortDateString() + "</td>" +
                            " <td>" + row.FechaEntrega.ToShortDateString() + "</td>" +
                           " <td>" + row.Vendedor+ "</td>" +
                           " <td>" + row.Moneda + "</td>" +
                            " <td>" +  row.Total.ToString("n0", new System.Globalization.CultureInfo("es-ES"))  + "</td>" +
                            "<td>  <a href=\" "+ redirect + "\" class=\"btn btn-default\"><i class=\"fa fa-pencil\" ></i>  Agregar Detalle </a></td>" +
                       " </tr>";
            }
            return retorno;
        }

        internal DataModel.CabeceraPedido ObtenerCabByID(int id)
        {
            string CodCli = id.ToString();
            var retorno = new DataModel.ResponseCabecera();
            var Lista = ObtenerListaCab();
            var registro = Lista.ListCabecera.Where(x => x.Id == id);
            if(registro.Count() > 0)
            {
                retorno.Cabecera = registro.FirstOrDefault();
            }
            return retorno.Cabecera;
        }

        internal DataModel.ResponseCabeceraList ObtenerListaCab()
        {
            var retorno = new DataModel.ResponseCabeceraList();
            var postTask = APIConector.httpClient.PostAsJsonAsync("cabecerapedido/obtenercabeceraall", new DataModel.CabeceraPedido());
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var listaCab = result.Content.ReadAsStringAsync().Result;
                retorno = JsonConvert.DeserializeObject<DataModel.ResponseCabeceraList>(listaCab);

            }
            return retorno;
        }

        internal DataModel.ResponseDetalleList ObtenerListaDet()
        {
            var retorno = new DataModel.ResponseDetalleList();
            var postTask = APIConector.httpClient.PostAsJsonAsync("detallepedido/obtenerdatosdetalleall", new DataModel.CabeceraPedido());
            postTask.Wait();
            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var listaCab = result.Content.ReadAsStringAsync().Result;
                retorno = JsonConvert.DeserializeObject<DataModel.ResponseDetalleList>(listaCab);

            }
            return retorno;
        }





        #endregion 

        public ActionResult Index()
        {
            var retorno = ObtenerListaCab();
            
            if(retorno.ListCabecera.Count()> 0)
            {
                ViewBag.RowGrid = pintarRow(retorno);
                ViewBag.showTbl = 1;
            }
            else
            {
                ViewBag.RowGrid = "";
             }

            return View();
        }

        public ActionResult AgregarCabecera()
        {
           
            return View();
        }

        public ActionResult AgregarDetalle(int? id) 
        { 
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var datosCab = ObtenerCabByID(id.Value);
            ViewBag.DatosCliente =Convert.ToString( "COD.CLIENTE: " + datosCab.CodCliente + " NOMBRE CLIENTE: " + datosCab.NombreCliente + " TOTAL PEDIDO :" + datosCab.Total.ToString("n0", new System.Globalization.CultureInfo("es-ES"))).ToUpper();
           
            ViewBag.IDCAB = datosCab.Id;
            ViewBag.CODCLI = datosCab.CodCliente;
            ViewBag.CANTIDAD = datosCab.Total; 
            string fila = string.Empty;
            var DatosDet = ObtenerListaDet();
            var ListDet = DatosDet.ListDetalle.Where(x => x.IdCab == id.Value); 
            if (ListDet.Count() > 0)
            {
                foreach(var row in ListDet)
                {
                    fila += "<tr>" + 
                           " <td>" + row.CodArticulo + "</td>" +
                           " <td>" + row.Descripcion + "</td>" +
                           " <td>" + row.Cantidad + "</td>" +
                            " <td>" + row.PrecioUnitario.ToString("n0", new System.Globalization.CultureInfo("es-ES")) + "</td>" +
                           " <td>" + row.Total.ToString("n0", new System.Globalization.CultureInfo("es-ES")) + "</td>" +                         
                                                      
                       " </tr>";
                }
                ViewBag.RowData = fila;
            }
            else
            {
                ViewBag.RowData = fila;
            }  
            return View();
        }

        [HttpPost]
        public ActionResult AgregarDetalle(DataModel.DetallePedido  detalle)
        {
            var retorno = new DataModel.ResponseDetalle();

            try
            {
                var postTask = APIConector.httpClient.PostAsJsonAsync("detallepedido/guardardatoscdetalle", detalle);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var DetResponse = result.Content.ReadAsStringAsync().Result;
                    retorno = JsonConvert.DeserializeObject<DataModel.ResponseDetalle>(DetResponse);                     
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.Msg = ex.Message;

            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AgregarCabecera(DataModel.CabeceraPedido cabeceraPedido)
        {
            var retorno = new DataModel.ResponseCabecera();
            try
            {
                var postTask = APIConector.httpClient.PostAsJsonAsync("cabecerapedido/guardardatoscabecera", cabeceraPedido);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var CabResponse = result.Content.ReadAsStringAsync().Result;
                    retorno = JsonConvert.DeserializeObject<DataModel.ResponseCabecera>(CabResponse);
                    TempData["SuccessMessage"] = "OK";                 
                }
            }
            catch (Exception ex)
            {
                retorno.HasError = true;
                retorno.Msg = ex.Message;

            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
 



    }
}