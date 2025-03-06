using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;
using System.Threading.Tasks;

namespace CarRentalWebApp.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        // GET: /Branch/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _branchRepository.GetBranches();
            return View(branches);
        }

        // GET: /Branch/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Branch/Create
        [HttpPost]
        public async Task<IActionResult> Create(BranchViewModel branchVM)
        {
            if (ModelState.IsValid)
            {
                var branch = new Branch
                {
                    Name = branchVM.Name,
                    City = branchVM.City
                };
                await _branchRepository.CreateBranch(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branchVM);
        }

        // GET: /Branch/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _branchRepository.GetBranch(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // DELETE: /Branch/Delete/{id}
        [HttpDelete, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _branchRepository.DeleteBranch(id);
            return RedirectToAction(nameof(Index));
        }
    }
}