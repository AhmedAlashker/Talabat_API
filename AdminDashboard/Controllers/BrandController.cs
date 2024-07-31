using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities.Product;

namespace AdminDashboard.Controllers
{
	public class BrandController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public BrandController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<IActionResult> Index()
		{
			var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

			return View(brands);
		}

		public async Task<IActionResult> Create(ProductBrand brand)
		{
			try
			{
				_unitOfWork.Repository<ProductBrand>().Add(brand);
				await _unitOfWork.CompleteAsync();
				return RedirectToAction("Index");
			}
			catch (Exception)
			{
				ModelState.AddModelError("Name", "Please Enter New Brand");
				return View("Index", await _unitOfWork.Repository<ProductBrand>().GetAllAsync());
			}
		}

        public async Task<IActionResult> Delete(int id)
		{
			var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

            _unitOfWork.Repository<ProductBrand>().Delete(brand);

			await _unitOfWork.CompleteAsync();

			return RedirectToAction("Index");

        }

    }
}
