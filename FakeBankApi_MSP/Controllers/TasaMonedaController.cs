﻿using Microsoft.AspNetCore.Mvc;
using MusicProAPI;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http;
using System.Text.Json;
using FakeBankApi_MSP.Modelos;
using Microsoft.OpenApi.Any;
using static FakeBankApi_MSP.Modelos.TasaMoneda;

namespace FakeBankApi_MSP.Controllers
{
	[ApiController]
	[Route("TasaMoneda")]
	public class TasaMonedaController : Controller
	{
		GlobalMetods metods = new GlobalMetods();

		ObtenerTazaMoneda tasa = new ObtenerTazaMoneda();
		public string ApiKey = "f4b20e88262ba1da76d52a0c05475e26276151a5";

		[HttpGet]
		[Route("GetTasa")]
		public async Task<dynamic> GetTasa()
		{
			var resultDolar = await tasa.GetTasaDolar();
            var resultEuro = await tasa.GetTasaEuro();
            var resultUf = await tasa.GetTasaUF();

            var deserializeDolar = JsonConvert.DeserializeObject<TasaMoneda.ListDolar>(resultDolar);
            var deserializeEuro = JsonConvert.DeserializeObject<TasaMoneda.ListEuro>(resultEuro);
            var deserializeUf = JsonConvert.DeserializeObject<TasaMoneda.ListUf>(resultUf);

            return new
            {
                message = "Valores dfe moneda",
				ResultsDolar = deserializeDolar,
				ResultEuro = deserializeEuro,
				resultUf = deserializeUf
            };
        }
	}

	
}
