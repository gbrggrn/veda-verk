using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared;

namespace VedaVerk.Controllers
{
	public class ProductsController(IRepository<Product> productsRepository) : Controller
	{
		private readonly IRepository<Product> _productsRepository = productsRepository;

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var products = await _productsRepository.GetAllAsync();
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await _productsRepository.GetByIdAsync(id);
			if (product == null)
				return NotFound();

			return Ok(product);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create(CreateProductDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (dto == null)
				return BadRequest("Product data is null.");

			var product = new Product
			{
				Name = dto.Name ?? string.Empty,
				Description = dto.Description ?? string.Empty,
				Price = dto.Price,
				Capacity = dto.Capacity,
				Type = dto.Type,
				ImageUrl = dto.ImageUrl ?? string.Empty
			};

			await _productsRepository.AddAsync(product);

			return Ok(product);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete (int id)
		{
			if (id <= 0)
				return BadRequest("Invalid product ID.");

			var product = await _productsRepository.GetByIdAsync(id);
			if (product == null)
				return NotFound();

			await _productsRepository.DeleteAsync(id);

			return Ok();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(Product product)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingProduct = await _productsRepository.GetByIdAsync(product.Id);
			if (existingProduct == null)
				return NotFound();

			existingProduct.Name = product.Name;
			existingProduct.Description = product.Description;
			existingProduct.Price = product.Price;
			existingProduct.Capacity = product.Capacity;
			existingProduct.Type = product.Type;
			existingProduct.ImageUrl = product.ImageUrl;

			await _productsRepository.UpdateAsync(existingProduct);

			return Ok(existingProduct);
		}
	}
}
