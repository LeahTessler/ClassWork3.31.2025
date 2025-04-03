using HomeWork3._31._2025.Data;
using HomeWork3._31._2025.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HomeWork3._31._2025.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Ads;Integrated Security=true;TrustServerCertificate=yes;";

        public IActionResult Index()



        {
            var am = new AdManager(_connectionString);
            var avm = new AdViewModel();
            var Ids = HttpContext.Session.Get<List<int>>("personsIds");
            if (Ids == null)
            {

                Ids = new List<int>();

            }

            avm.Ads = am.GetAds();
            avm.Ids = Ids;
            return View(avm);
        }


        public IActionResult NewAd()
        {
            return View();

        }

        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            var am = new AdManager(_connectionString);
            int id = am.AddAd(ad);

            List<int> Ids = HttpContext.Session.Get<List<int>>("personsIds");
            if (Ids == null)
            {

                Ids = new List<int>();
            
            }
            Ids.Add(id);
            HttpContext.Session.Set("personsIds", Ids);

            return Redirect("/home/index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var am = new AdManager(_connectionString);
            am.Delete(id);

            return Redirect("/home/index");

        }

       
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
