using DotNetNuke.Web.Api;

namespace R7.Enrollment.Dnn.Services
{
    public class EnrollmentRouteMapper: IServiceRouteMapper
    {
        public void RegisterRoutes (IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute ("R7.Enrollment", "EnrollmentMap1", "{controller}/{action}",
                new [] { "R7.Enrollment.Dnn.Services" });
        }
    }
}
