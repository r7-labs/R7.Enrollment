using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.FileSystem;
using R7.Enrollment.Data;

namespace R7.Enrollment.Dnn.Data
{
    public class TandemDbManager
    {
        private static readonly Lazy<TandemDbManager> _instance = new Lazy<TandemDbManager> ();

        public static TandemDbManager Instance => _instance.Value;

        private IList<TandemEntrantRatingDb> Databases = new List<TandemEntrantRatingDb> ();
        
        public TandemDbManager ()
        {
            var db = new TandemEntrantRatingDb ();
            var file = FileManager.Instance.GetFile (0, "enrollment/enr_rating_1696453372720271613_2.xml");
            db.Load (file.PhysicalPath);
            Databases.Add (db);
        }

        public TandemEntrantRatingDb GetDb (string campaignTitle)
        {
            return Databases.FirstOrDefault (db =>
                db.EntrantRatingEnvironment.EnrollmentCampaignTitle == campaignTitle);
        }
    }
}