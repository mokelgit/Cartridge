using Cartridge.Data;
using Cartridge.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cartridge.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyRepository _companyRepository;

        public CompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<IActionResult> Index()
        {
            var companies = await _companyRepository.GetAllCompanies();
            return View(companies);
        }

        [HttpGet("/companies/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyRepository.GetCompanyByID(id);
            if (company == null) return NotFound();
            return View(company);
        }

    }
}
