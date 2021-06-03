using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using R7.Enrollment.Dnn.Data;
using R7.Enrollment.Models;
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
                var competitions = FindCompetitions (db.EntrantRatingEnvironment, dto.EntrantId);
                
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

        IList<Competition> FindCompetitions (EntrantRatingEnvironment env, int entrantId)
        {
            var competitions = new List<Competition> ();
            foreach (var competition in env.Competitions) {
                var entrant = competition.Entrants.FirstOrDefault (entr => entr.PersonalNumber == entrantId.ToString ());
                if (entrant != null) {
                    competitions.Add (competition);
                }
            }

            return competitions;
        }
    }
}
