using Microsoft.AspNetCore.Mvc;

namespace ProjectApp.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard/Index
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Set your ViewData or ViewBag properties for dashboard data
            ViewBag.TotalIncome = 1000;
            ViewBag.TotalExpense = 500;
            ViewBag.Balance = 500;

            // Your charts or additional data
            ViewBag.DoughnutChartData = new object(); // Replace with actual data
            ViewBag.SplineChartData = new object(); // Replace with actual data

            return View();
        }
    }
}
