using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.Enrollment.Dnn.Data;
using R7.Enrollment.Queries;
using R7.Enrollment.Renderers;

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
                var db = TandemDbManager.Instance.GetCachedDb ();

                var competitionQuery = new CompetitionQuery ();
                var competitions =
                    competitionQuery.ByPersonalNumber (db.EntrantRatingEnvironment.Competitions, dto.EntrantId);

                var htmlRenderer = new TandemEntrantRatingHtmlRenderer ();
                var sb = new StringBuilder ();
                var html = XmlWriter.Create (sb, new XmlWriterSettings {ConformanceLevel = ConformanceLevel.Auto});
                foreach (var competition in competitions) {
                    htmlRenderer.RenderCompetition (competition, html);    
                }
                html.Close ();
                
                return Request.CreateResponse (HttpStatusCode.OK, sb.ToString ());
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
