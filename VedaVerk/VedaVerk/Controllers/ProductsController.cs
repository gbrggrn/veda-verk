using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VedaVerk.Models.Enitites;
using VedaVerk.Repositiories.Interfaces;
using VedaVerk.Shared;
using VedaVerk.Shared.DTOs;

namespace VedaVerk.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController(IRepository<Product> productsRepository) : Controller
	{
		private readonly IRepository<Product> _productsRepository = productsRepository;

		[HttpGet("availability/{productId}")]
		[AllowAnonymous]
		public async Task<IActionResult> CheckAvailability(int productId, [FromQuery] int quantity)
		{
			var product = await _productsRepository.GetByIdAsync(productId);
			if (product == null)
				return NotFound("Product not found.");

			if (quantity <= 0)
				return BadRequest("Quantity must be greater than zero.");

			if (quantity > product.Capacity)
				return Ok();
			else
				return BadRequest("Requested quantity exceeds available capacity.");
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAll()
		{
			var products = await _productsRepository.GetAllAsync();
			
			var dtos = products.Select(p => new ResponseProductDTO {
			 Id = p.Id,
			 Name = p.Name,
			 Description = p.Description,
			 Price = p.Price,
			 Capacity = p.Capacity,
			 Type = p.Type,
			 ImageUrl = p.ImageUrl,
			 IsActive = p.IsActive,
			 OpenTime = p.OpenTime,
			 CloseTime = p.CloseTime,
			 IntervalMinutes = p.IntervalMinutes,
			 CapacityPerSlot = p.CapacityPerSlot,
			 Created = p.Created,
			 LastUpdated = p.LastUpdated,
			 ActiveFrom = p.ActiveFrom,
			 ActiveTo = p.ActiveTo
			}).ToList();

			return Ok(dtos);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await _productsRepository.GetByIdAsync(id);
			if (product == null)
				return NotFound();

			var dtos = new ResponseProductDTO {
			 Id = product.Id,
			 Name = product.Name,
			 Description = product.Description,
			 Price = product.Price,
			 Capacity = product.Capacity,
			 Type = product.Type,
			 ImageUrl = product.ImageUrl,
			 IsActive = product.IsActive,
			 OpenTime = product.OpenTime,
			 CloseTime = product.CloseTime,
			 IntervalMinutes = product.IntervalMinutes,
			 CapacityPerSlot = product.CapacityPerSlot,
			 Created = product.Created,
			 LastUpdated = product.LastUpdated,
			 ActiveFrom = product.ActiveFrom,
			 ActiveTo =  product.ActiveTo
			};

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
				ImageUrl = dto.ImageUrl ?? string.Empty,
				IsActive = dto.IsActive,
				OpenTime = dto.OpenTime,
				CloseTime = dto.CloseTime,
				IntervalMinutes = dto.IntervalMinutes,
				CapacityPerSlot = dto.CapacityPerSlot,
				Created = DateTime.UtcNow,
				LastUpdated = DateTime.UtcNow,
				ActiveFrom = dto.ActiveFrom,
				ActiveTo = dto.ActiveTo
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

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(int id, EditProductDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existingProduct = await _productsRepository.GetByIdAsync(id);
			if (existingProduct == null)
				return NotFound();

			existingProduct.Name = dto.Name;
			existingProduct.Description = dto.Description;
			existingProduct.Price = dto.Price;
			existingProduct.Capacity = dto.Capacity;
			existingProduct.Type = dto.Type;
			existingProduct.ImageUrl = dto.ImageUrl;
			existingProduct.IsActive = dto.IsActive;
			existingProduct.OpenTime = dto.OpenTime;
			existingProduct.CloseTime = dto.CloseTime;
			existingProduct.IntervalMinutes = dto.IntervalMinutes;
			existingProduct.CapacityPerSlot = dto.CapacityPerSlot;
			existingProduct.ActiveFrom = dto.ActiveFrom;
			existingProduct.ActiveTo = dto.ActiveTo;
			existingProduct.LastUpdated = DateTime.UtcNow;
			existingProduct.Created = dto.Created;

			await _productsRepository.UpdateAsync(existingProduct);

			return Ok("Updated successfully.");
		}
	}
}
