using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectApp.Models;
using ProjectApp.Data;
namespace ProjectApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public IActionResult Index()
{
    // Ensure the user is logged in
    if (HttpContext.Session.GetString("Username") == null)
    {
        return RedirectToAction("Login", "Account");
    }

    // Fetch the list of transactions
    var transactions = _context.Transactions.ToList();

    // Calculate total income, total expense, and balance
    var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
    var totalExpense = transactions.Where(t => t.Amount < 0).Sum(t => t.Amount);
    var balance = totalIncome + totalExpense;

    // Calculate data for the doughnut chart (expenses by category)
    var expenseCategories = transactions
        .Where(t => t.Amount < 0)
        .GroupBy(t => t.CategoryTitleWithIcon)
        .Select(g => new
        {
            CategoryTitleWithIcon = g.Key,
            Amount = g.Sum(t => t.Amount)
        })
        .ToList();

    // Prepare data for the spline chart (income vs expense by date)
    var incomeVsExpense = transactions
        .GroupBy(t => t.Date.Date) // Group by date
        .Select(g => new
        {
            Day = g.Key,
            Income = g.Where(t => t.Amount > 0).Sum(t => t.Amount),
            Expense = g.Where(t => t.Amount < 0).Sum(t => t.Amount)
        })
        .OrderBy(g => g.Day) // Ensure the data is sorted by date
        .ToList();

    // Pass the data to the view
    ViewBag.TotalIncome = totalIncome;
    ViewBag.TotalExpense = totalExpense;
    ViewBag.Balance = balance;
    ViewBag.DoughnutChartData = expenseCategories;
    ViewBag.SplineChartData = incomeVsExpense;

    // Return the view with the transaction data
    return View(transactions);
}

    }
}
