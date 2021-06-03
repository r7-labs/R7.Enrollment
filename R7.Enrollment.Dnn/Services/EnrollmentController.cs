using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;

namespace R7.Enrollment.Dnn.Services
{
    public class GetRatingListsDTO
    {
        public string Campaign { get; set; }
        
        public int EntrantId { get; set; }
    }
    
    public class EnrollmentController: DnnApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize (AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetRatingLists (GetRatingListsDTO dto)
        {
            try {
                return Request.CreateResponse (HttpStatusCode.OK, dto);
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
