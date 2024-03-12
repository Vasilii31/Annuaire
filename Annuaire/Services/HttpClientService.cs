using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Annuaire.Model;
using AnnuaireLib.DTO;
using AnnuaireLib.DAO;

namespace Annuaire.Services
{
	public static class HttpClientService
	{
		private const string baseAddress = "https://localhost:7183/";
		private static HttpClient? client = null;
		private static CookieContainer cookieContainer = new();


		private static HttpClient Client
		{
			get
			{
				if (client == null)
				{
					var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
					client = new(handler) { BaseAddress = new Uri(baseAddress) };
				}
				return client;
			}
		}

		public static async Task<bool> Login(string pwd)
		{
			string route = "login?useCookies=true&useSessionCookies=true";
			var jsonString = JsonConvert.SerializeObject(new LoginUser
			{
				Email = "ADMIN@admin.fr",
				Password = pwd
			});

			var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
			var response = await Client.PostAsync(route, httpContent);

			var cookies = cookieContainer.GetCookies(new Uri(baseAddress));
			Debug.WriteLine(cookies);

			return response.IsSuccessStatusCode;
		}

		public static async Task<IEnumerable<SalarieLight>> GetSalaries()
		{
			string route = $"api/Salaries/";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<SalarieLight>>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			throw new Exception(response.ReasonPhrase);
		}

		public static async Task<Salarie?> GetSalarieDetails(int id)
		{
			string route = $"api/Salaries/{id}";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Salarie>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			return null;
		}

		public static async Task<IEnumerable<Site>> GetSites()
		{
			string route = $"api/Sites/";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<Site>>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			throw new Exception(response.ReasonPhrase);
		}

		public static async Task<bool> ModifySite(Site siteDAO, int id)
		{
			var produitJson = JsonConvert.SerializeObject(siteDAO);
			string route = $"api/Sites/{id}";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PutAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		public static async Task<Site> CreateNewSite(Site newsite)
		{
			var produitJson = JsonConvert.SerializeObject(newsite);
			string route = $"api/Sites/";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PostAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Site>(resultat)
					?? throw new FormatException($"Erreur lors de la désérialisation de la réponse HTTP : {route}");
			}
			else
			{
				string errorMessage = $"Erreur HTTP : {response.StatusCode} - {response.ReasonPhrase}";
				throw new HttpRequestException(errorMessage);
			}
		}

		internal static async Task<IEnumerable<SalarieLight>> GetSalariesFromSite(int idSite)
		{
			string route = $"api/Salaries/PerSite/{idSite}";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<SalarieLight>>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			throw new Exception(response.ReasonPhrase);
		}

		internal static async Task<bool> DeleteSite(int id)
		{
			string route = $"api/Sites/{id}";
			var response = await Client.DeleteAsync(route);
			return response.IsSuccessStatusCode;
		}

		internal static async Task<Service> CreateNewService(Service newService)
		{
			var produitJson = JsonConvert.SerializeObject(newService);
			string route = $"api/Services/";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PostAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Service>(resultat)
					?? throw new FormatException($"Erreur lors de la désérialisation de la réponse HTTP : {route}");
			}
			else
			{
				string errorMessage = $"Erreur HTTP : {response.StatusCode} - {response.ReasonPhrase}";
				throw new HttpRequestException(errorMessage);
			}
		}

		internal static async Task<IEnumerable<SalarieLight>> GetSalariesFromService(int id)
		{
			string route = $"api/Salaries/PerServices/{id}";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<SalarieLight>>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			throw new Exception(response.ReasonPhrase);
		}

		internal static async Task<bool> DeleteService(int id)
		{
			string route = $"api/Services/{id}";
			var response = await Client.DeleteAsync(route);
			return response.IsSuccessStatusCode;
		}

		internal static async Task<bool> ModifyService(Service service, int id)
		{
			var produitJson = JsonConvert.SerializeObject(service);
			string route = $"api/Services/{id}";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PutAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		internal static async Task<IEnumerable<Service>> GetServices()
		{
			string route = $"api/Services/";
			var response = await Client.GetAsync(route);
			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<IEnumerable<Service>>(resultat)
				?? throw new FormatException($"Erreur Http : {route}");
			}
			throw new Exception(response.ReasonPhrase);
		}

		internal static async Task<bool> DeleteSalarie(int id)
		{
			string route = $"api/Salaries/{id}";
			var response = await Client.DeleteAsync(route);
			return response.IsSuccessStatusCode;
		}

		internal static async Task<Salarie> CreateNewSalarie(Salarie newSal)
		{
			var produitJson = JsonConvert.SerializeObject(newSal);
			string route = $"api/Salaries/";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PostAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Salarie>(resultat)
					?? throw new FormatException($"Erreur lors de la désérialisation de la réponse HTTP : {route}");
			}
			else
			{
				string errorMessage = $"Erreur HTTP : {response.StatusCode} - {response.ReasonPhrase}";
				throw new HttpRequestException(errorMessage);
			}
		}

		internal static async Task<bool> ModifySalarie(Salarie currentSalarie, int id)
		{
			var produitJson = JsonConvert.SerializeObject(currentSalarie);
			string route = $"api/Salaries/{id}";

			var content = new StringContent(produitJson, Encoding.UTF8, "application/json");

			var response = await Client.PutAsync(route, content);

			if (response.IsSuccessStatusCode)
			{
				string resultat = await response.Content.ReadAsStringAsync();
				return true;
			}
			else
			{
				return false;
			}
		}
	}

}
