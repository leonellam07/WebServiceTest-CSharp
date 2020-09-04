using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebServiceTest.DataAccess;
using WebServiceTest.Utils;

namespace WebServiceTest.Controllers
{
    public class ArticuloController : ApiController
    {
        public IHttpActionResult Get([FromUri] string code)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                response = Request.CreateResponse(HttpStatusCode.Accepted);
                response.Content = new StringContent(JsonConvert.SerializeObject(
                        new ArticuloRepository().BuscarPorCodigo(code)
                    ));
            }
            catch(Exception ex)
            {
                Error error = new Error();
                error.Mensaje = ex.Message;
                error.MensajeInterno = ex.InnerException != null ? ex.InnerException.ToString() : null;

                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(error));
            }

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        public IHttpActionResult Lista([FromUri] string CodigoGrupo)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            try
            {
                response = Request.CreateResponse(HttpStatusCode.Accepted);
                response.Content = new StringContent(JsonConvert.SerializeObject(
                        new ArticuloRepository().Lista(CodigoGrupo)
                    ));
            }
            catch (Exception ex)
            {
                Error error = new Error();
                error.Mensaje = ex.Message;
                error.MensajeInterno = ex.InnerException != null ? ex.InnerException.ToString() : null;

                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(error));
            }

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return ResponseMessage(response);
        }
    }
}
