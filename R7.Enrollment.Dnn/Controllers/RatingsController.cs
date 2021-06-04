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
            var result = new RatingsViewModel ();
            result.CampaignTitles = TandemDbManager.Instance.GetCampaignTitles (PortalSettings.PortalId); 
            return View (result);
        }
    }
}