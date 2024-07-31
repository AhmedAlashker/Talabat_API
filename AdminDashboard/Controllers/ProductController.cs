using AdminDashboard.Helper;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.Product;

namespace AdminDashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            // GET ALL Products
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();

            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);

            return View(mappedProducts);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image != null)
                    productViewModel.PictureUrl = DocumentSettings.UploadFile(productViewModel.Image, "products");
                else
                    productViewModel.PictureUrl = "images/products/hot-caramel-macchiato.png";

                var mappedProduct = _mapper.Map<Product>(productViewModel);

                _unitOfWork.Repository<Product>().Add(mappedProduct);

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");
            }
            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

			if (product is null) return NotFound(new ApiResponse(404, "There isn't any product with this Id"));

			var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid) 
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        DocumentSettings.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = DocumentSettings.UploadFile(model.Image, "products");
                    }
                    else
                        model.PictureUrl = DocumentSettings.UploadFile(model.Image, "products");

                    var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);

                    _unitOfWork.Repository<Product>().Update(mappedProduct);

                    var result = await _unitOfWork.CompleteAsync();

                    if (result > 0)
                        return RedirectToAction("Index");
                }

            }
            return View(model);

        }


        public async  Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            if (product is null) return NotFound(new ApiResponse(404, "There isn't any product with this Id"));

            var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);

            return View(mappedProduct);

        }
        [HttpPost]
		public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
			if (id != model.Id)
				return NotFound();

            try
            {
				var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

                if (product.PictureUrl != null)
					DocumentSettings.DeleteFile(product.PictureUrl, "products");

                _unitOfWork.Repository<Product>().Delete(product);

                await _unitOfWork.CompleteAsync();

                return RedirectToAction("Index");

			}
            catch (Exception)
            {
                return View(model);
            }

		}

	}
}
