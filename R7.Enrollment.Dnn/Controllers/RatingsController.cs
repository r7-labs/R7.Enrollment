using System.Linq;
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
            result.Campaigns = TandemRatingsDbManager.Instance.GetCampaigns (PortalSettings.PortalId)
                .Select (c => new CampaignViewModel (c)).ToList (); 
            return View (result);
        }
    }
}