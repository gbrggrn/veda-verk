using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using VedaVerk.Client.Services.Interfaces;
using VedaVerk.Shared.DTOs;

namespace VedaVerk.Client.Services.Implementations
{
	public class ProductService(HttpClient httpClient) : IProductService
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task<bool> CheckAvailability(int productId, int quantity)
		{
			return await _httpClient.GetFromJsonAsync<bool>($"/api/Products/availability/{productId}?quantity={quantity}");
		}

		[Authorize(Roles = "Admin")]
		public async Task<bool> Create(CreateProductDTO dto)
		{
			var response = await _httpClient.PostAsJsonAsync($"/api/Products", dto);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> Delete(int id)
		{
			var response = await _httpClient.DeleteAsync($"/api/Products/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<List<ResponseProductDTO>> GetAll()
		{
			var response = await _httpClient.GetAsync($"/api/Products");

			if (response.IsSuccessStatusCode)
			{
				var products = await response.Content.ReadFromJsonAsync<List<ResponseProductDTO>>();
				return products!;
			}
			else
			{
				// Return an empty list if the request fails
				return [];
			}
		}

		public async Task<ResponseProductDTO> GetById(int id)
		{
			var response = await _httpClient.GetAsync($"/api/Products/{id}");
			if (response.IsSuccessStatusCode)
			{
				var product = await response.Content.ReadFromJsonAsync<ResponseProductDTO>();
				return product!;
			}
			else
			{
				throw new Exception("Failed to fetch product.");
			}
		}

		public async Task<bool> Update(int id, EditProductDTO dto)
		{
			var response = await _httpClient.PutAsJsonAsync($"/api/Products/{id}", dto);
			return response.IsSuccessStatusCode;
		}
	}
}
