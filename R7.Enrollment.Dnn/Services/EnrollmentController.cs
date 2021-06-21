using System;
using System.Collections.Generic;
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
    public class GetRatingListsArgs
    {
        public string CampaignTitle { get; set; }

        public string Snils { get; set; }

        public string PersonalNumber { get; set; }
    }

    public class GetRatingListsResult
    {
        public string Html { get; set; }
    }

    public class EnrollmentController: DnnApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize (AccessLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage GetRatingLists (GetRatingListsArgs args)
        {
            try {
                var db = TandemRatingsDbManager.Instance.GetDb (args.CampaignTitle, PortalSettings.PortalId);
                if (db == null) {
                    return Request.CreateResponse (HttpStatusCode.NotFound);
                }

                var competitionQuery = new CompetitionQuery ();
                var competitions =
                    competitionQuery.BySnilsOrPersonalNumber (db.EntrantRatingEnvironment.Competitions, args.Snils, args.PersonalNumber);

                var results = new List<GetRatingListsResult> ();
                var htmlRenderer = new TandemRatingsHtmlRenderer (
                    new TandemRatingsRendererSettings {
                        Depersonalize = true,
                        Snils = args.Snils,
                        PersonalNumber = args.PersonalNumber,
                        UseBasicCompetitionHeader = true
                    }
                );

                foreach (var competition in competitions) {
                    var sb = new StringBuilder ();
                    var html = XmlWriter.Create (sb, new XmlWriterSettings {ConformanceLevel = ConformanceLevel.Auto});
                    htmlRenderer.RenderCompetition (competition, html);
                    html.Close ();
                    results.Add (new GetRatingListsResult {
                        Html = sb.ToString ()
                    });
                }

                return Request.CreateResponse (HttpStatusCode.OK, results);
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
