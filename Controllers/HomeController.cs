using LDap.Data;
using LDap.Helpers;
using LDap.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly DbClass _dbContext;
    private readonly LdapAuthenticationService _ldapService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(DbClass dbContext, LdapAuthenticationService ldapService, ILogger<HomeController> logger)
    {
        _dbContext = dbContext;
        _ldapService = ldapService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<Model> modelList = _dbContext.EkDers.ToList();
        return View(modelList);
    }

    [HttpGet]
    public IActionResult Admin()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Dosya seçilmedi.";
            return View("Admin");
        }

        if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Lütfen geçerli bir Excel dosyasý yükleyin.";
            return View("Admin");
        }

        try
        {
            List<Model> models = ExcelHelper.ProcessExcel(file);

            foreach (var model in models)
            {
                _dbContext.EkDers.Add(model);
            }
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Excel dosyasý baþarýyla yüklendi ve veritabanýna aktarýldý.";
        }
        catch (Exception ex)
        {
            _logger.LogError($"Dosya yüklenirken bir hata oluþtu: {ex.Message}");
            TempData["ErrorMessage"] = $"Dosya yüklenirken bir hata oluþtu: {ex.Message}";
        }

        return View("Admin");
    }
}
