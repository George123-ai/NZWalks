using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
	public class RegionsController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public RegionsController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			List<RegionDto> response = new List<RegionDto>();

			try
			{
				var client = httpClientFactory.CreateClient();

				var httpResponseMessage = await client
					.GetAsync("https://localhost:7206/api/regions");

				httpResponseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResponseMessage.Content
					.ReadFromJsonAsync<IEnumerable<RegionDto>>());


			}
			catch (Exception)
			{

				throw;
			}

			//Get all regions form Web API 

			return View(response);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddRegionViewModel model)
		{

			var client = httpClientFactory.CreateClient();

			var httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7206/api/regions"),
				Content = new StringContent(JsonSerializer.Serialize(model),
				Encoding.UTF8, "application/json")
			}; 

			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			httpResponseMessage.EnsureSuccessStatusCode();

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

			if (response != null)
			{
				return RedirectToAction("Index", "Regions");
			}

			return View();
			
			
		}

		[HttpGet]

		public async Task<IActionResult> Edit(Guid id)
		{
			var client = httpClientFactory.CreateClient();

			var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7206/api/regions/{id.ToString()}");

			if (response != null)
			{
				return View(response);
			}

			return View(null);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RegionDto regionDto)
		{
			var client = httpClientFactory.CreateClient();

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7206/api/regions/{regionDto.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(regionDto),Encoding.UTF8,
				"application/json")
			};

			var httpResponseMessage = await client.SendAsync(request);
			httpResponseMessage.EnsureSuccessStatusCode();

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

			if (response != null)
			{
				return RedirectToAction("Edit", "Regions");
			}

			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Delete(RegionDto request)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7206/api/regions/{request.Id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index", "Regions");
			}
			catch (Exception)
			{
				// Console
			}

			return View("Edit");
		}
	}
}
