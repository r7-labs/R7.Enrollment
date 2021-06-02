using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DotNetNuke.Collections;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;

namespace R7.Enrollment.Dnn.Services
{
    public class EnrollmentController: DnnApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> GetRatingLists ()
        {
           try {
                var root = HttpContext.Current.Server.MapPath ("~/App_Data");
                var provider = new MultipartFormDataStreamProvider (root);
                await Request.Content.ReadAsMultipartAsync (provider);

                var campaign = provider.FormData.GetValue<string> ("campaign");
                var entrantId = provider.FormData.GetValue<int> ("entrantId");

                return Request.CreateResponse (HttpStatusCode.OK, $"campaign:{campaign}, entrantId:{entrantId}");
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
