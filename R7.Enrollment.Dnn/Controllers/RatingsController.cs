using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using R7.Enrollment.Dnn.Data;
using R7.Enrollment.Dnn.ViewModels;

namespace R7.Enrollment.Dnn.Controllers
{
    [DnnHandleError]
    public class RatingsController : DnnController
    {
        public ActionResult Index ()
        {
            var result = new RatingsModuleViewModel ();
            result.Campaigns = TandemRatingsDbManager.GetInstance (ActiveModule.ModuleID).GetDbs ()
                .Select (db => new CampaignViewModel (db.EntrantRatingEnvironment)).ToList ();
            result.Version = GetVersion ();

            return View (result);
        }

        string GetVersion ()
        {
            var assembly = Assembly.GetExecutingAssembly ();
            var informationalVersionAttr = assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute> ()
                .FirstOrDefault ();

            return informationalVersionAttr?.InformationalVersion ?? assembly.GetName ().Version.ToString (3);
        }
    }
}
