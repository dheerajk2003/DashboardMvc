using ClosedXML.Excel;
using HomeProjectCore.AppData;
using HomeProjectCore.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc4.Models;
using mvc4.ViewModels;
using System.Data;

namespace HomeProjectCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(ClientModel client)
        {
            var clientModel = new ClientModel();
            try
            {
                clientModel.ClientName = client.ClientName;
                clientModel.ClientAddress = client.ClientAddress;
                clientModel.ClientContact = client.ClientContact;
                clientModel.ClientContactA = client.ClientContactA;
                clientModel.ClientEmail = client.ClientEmail;
                clientModel.ClientLogo = client.ClientLogo;
                clientModel.ClientPassword = Guid.NewGuid().ToString().Substring(0, 8);
                clientModel.ClientDate = System.DateTime.Now;
                clientModel.ClientActive = 1;


                _context.ClientModel.Add(clientModel);
                await _context.SaveChangesAsync();
                ModelState.Clear();
                TempData["success"] = "Candidate Registered Successfully";

            }
            catch (Exception ex) { TempData["Failed"] = ex.Message; }
            return RedirectToAction("FetchClient", "Home");
        }


        [HttpGet]
        public async Task<ActionResult<List<ClientModel>>> FetchClient()
        {
             return View( await _context.ClientModel.ToListAsync());
        }

        public IActionResult AddClient()
        {
            return View();
        }

        public IActionResult AddInvestor()
        {
            var Clients = _context.ClientModel.Select(u => new ClientModel { ClientId = u.ClientId, ClientName = u.ClientName }).ToList();
            var Funds = _context.FundTable.Select(u => new FundModel { FundId = u.FundId, FundName = u.FundName }).ToList();

            //List<List<object>> tList = new List<List<object>>();
            //tList.Add((List<object>)Clients.Cast<object>());
            //tList.Add((List<object>)Funds.Cast<object>());

            IdListModel idm = new IdListModel();
            idm.Fm = Funds;
            idm.Cm = Clients;

            return View(idm);
        }
        [HttpPost]
        public IActionResult AddInvestor(InvestorModel model, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string fname = Guid.NewGuid().ToString() + file.FileName;
                string fpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fname);
                using (var fileStream = new FileStream(fpath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                model.InvestorLogo = fname;
            }
            else
            {
                model.InvestorLogo = "photo.png";
            }
            model.InvestorActive = 1;
            _context.InvestorTable.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            // var id = Request.Form["applicantId"].ToString();
           // var t = ViewBag.applicationID;
            //var newId = fc["Id"];
            var st = await _context.ClientModel.SingleOrDefaultAsync(x => x.ClientId== id);
            if (st != null)
            {

                _context.ClientModel.Remove(st);
                _context.SaveChanges();
                TempData["warning"] = "Client Deleted Successfully";
                return RedirectToAction("FetchClient", "Home");
            }
            else
            {
                return RedirectToAction("FetchClient", "Home");
            }
            // return View(stu);
        }


        [HttpPost]
        private ActionResult ExportToExcel()
        {
            DataTable products = new DataTable();
            //products.Columns.Add(new DataColumn("ID"))
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "ProductID";
                worksheet.Cell(currentRow, 2).Value = "ProductName";
                worksheet.Cell(currentRow, 3).Value = "Price";
                worksheet.Cell(currentRow, 4).Value = "ProductDescription";

                for (int i = 0; i < 10; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = "Laptop"+i;
                        worksheet.Cell(currentRow, 2).Value = "KeyBoard" + i;
                        worksheet.Cell(currentRow, 3).Value = "5000" + i;
                        worksheet.Cell(currentRow, 4).Value = "LaptopKeyboard" + i;

                    }
                }
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=ProductDetails.xls");
                Response.ContentType = "application/xls";
                Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
            return RedirectToAction("FetchClient","Home");
        }

    }
}
