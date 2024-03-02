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

		public static async Task<bool> Login(string email, string pwd)
		{
			string route = "login?useCookies=true&useSessionCookies=true";
			var jsonString = JsonConvert.SerializeObject(new LoginUser
			{
				Email = email,
				Password = pwd
			});

			var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
			var response = await Client.PostAsync(route, httpContent);

			var cookies = cookieContainer.GetCookies(new Uri(baseAddress));
			Debug.WriteLine(cookies);

			return response.IsSuccessStatusCode ? true :
				throw new Exception(response.ReasonPhrase);
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



		//public static async Task<List<VolLightDto>> GetVolLights(DateTime dateJour)
		//{
		//	string route = $"api/vols/search/{dateJour.ToString("yyyy-MM-dd")}";
		//	var response = await Client.GetAsync(route);
		//	if (response.IsSuccessStatusCode)
		//	{
		//		string resultat = await response.Content.ReadAsStringAsync();
		//		return JsonConvert.DeserializeObject<List<VolLightDto>>(resultat)
		//		?? throw new FormatException($"Erreur Http : {route}");
		//	}
		//	throw new Exception(response.ReasonPhrase);
		//}

		//public static async Task<bool> PostClient(Client client)
		//{
		//	string route = $"api/clients";
		//	var jsonString = JsonConvert.SerializeObject(client);

		//	var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
		//	var response = await Client.PostAsync(route, httpContent);
		//	return response.IsSuccessStatusCode ? true :
		//		throw new Exception(response.ReasonPhrase);
		//}

	}

}
