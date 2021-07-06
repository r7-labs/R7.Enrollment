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
using R7.Enrollment.Models;
using R7.Enrollment.Queries;
using R7.Enrollment.Renderers;

namespace R7.Enrollment.Dnn.Services
{
    public class GetRatingListsByEntrantArgs
    {
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
        public HttpResponseMessage GetRatingListsByEntrant (GetRatingListsByEntrantArgs args)
        {
            try {
                var competitionQuery = new CompetitionQuery ();
                var competitions = new List<Competition> ();
                foreach (var db in TandemRatingsDbManager.GetInstance (ActiveModule.ModuleID).GetDbs ()) {
                    competitions.AddRange (competitionQuery.BySnilsOrPersonalNumber (db, args.Snils, args.PersonalNumber));
                }

                var results = new List<GetRatingListsResult> ();
                var htmlRenderer = new RatingsHtmlRenderer (
                    new RatingsRendererSettings {
                        Snils = args.Snils,
                        PersonalNumber = args.PersonalNumber
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

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetRatingListsByCampaign (int moduleId, string campaignToken)
        {
            try {
                var db = TandemRatingsDbManager.GetInstance (moduleId).GetDb (campaignToken);
                if (db == null) {
                    return Request.CreateResponse (HttpStatusCode.NotFound);
                }

                var htmlRenderer = new RatingsHtmlRenderer ();

                var sb = new StringBuilder ();
                var html = XmlWriter.Create (sb, new XmlWriterSettings ());
                htmlRenderer.RenderStandalone (db.EntrantRatingEnvironment, html);
                html.Close ();

                return new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent (sb.ToString (), Encoding.Unicode, "text/html")
                };
            }
            catch (Exception ex) {
                Exceptions.LogException (ex);
                return Request.CreateErrorResponse (HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
