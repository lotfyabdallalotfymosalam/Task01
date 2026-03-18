using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; // مهم جداً
using Task01.Hubs;
using Task01.Models; // اتأكد إن عندك موديل اسمه Employee

namespace Task01.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHubContext<EmployeeHub> _hubContext;

        // بنعمل Injection للـ HubContext هنا
        public EmployeeController(IHubContext<EmployeeHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // صفحة عرض الموظفين (Display Page)
        public IActionResult Index()
        {
            return View();
        }

        // صفحة إضافة الموظف (Add Page)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            // 1. هنا المفروض كود الحفظ في قاعدة البيانات (DbContext)
            // _context.Employees.Add(emp);
            // await _context.SaveChangesAsync();

            // 2. السطر ده هو اللي بيعمل Broadcasting (إذاعة) لكل الناس
            // بنبعت كائن الموظف (emp) لكل الـ Clients المتصلين
            // في الـ JavaScript Client، هتستقبل البيانات دي في الـ "ReceiveNewEmployee" event
            await _hubContext.Clients.All.SendAsync("ReceiveNewEmployee", emp.Name, emp.Address, emp.Age);

            return RedirectToAction("Index");


        }
    }
}