using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Task01.Data;
using Task01.Hubs;
using Task01.Models;

namespace Task01.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHubContext<EmployeeHub> _hubContext;
        private readonly AppDbContext _context;

        public EmployeeController(IHubContext<EmployeeHub> hubContext, AppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        // صفحة عرض الموظفين (Display Page)
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        // صفحة إضافة الموظف (Add Page)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            // 1. حفظ في قاعدة البيانات
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();

            // 2. بث البيانات لكل الـ Clients المتصلين
            await _hubContext.Clients.All.SendAsync("ReceiveNewEmployee", emp.Name, emp.Address, emp.Age);

            return Json(new { success = true });
        }
    }
}
