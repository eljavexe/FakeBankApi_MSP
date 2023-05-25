﻿using FakeBankApi_MSP.Modelos;
using Microsoft.AspNetCore.Mvc;
using MusicProAPI;
using System.Threading;

namespace FakeBankApi_MSP.Controllers
{
	[ApiController]
	[Route("Cuenta")]
	public class CuentaController : Controller
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetCuentas")]
		public dynamic GetCuentas()
		{
			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay cuentas registradas"
				};
			}

			List<Cuenta> cuentas = new List<Cuenta>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				Cuenta cuenta = new Cuenta();

				cuenta.Rut_Persona = splitArr[0];
				cuenta.NumeroCuenta = splitArr[1];
				cuenta.FechaCreacion = splitArr[2];
				cuenta.Activa = Convert.ToBoolean(splitArr[3]);

				cuentas.Add(cuenta);
			}

			return cuentas;
		}

		[HttpGet]
		[Route("GetCuentasPersona")]
		public dynamic GetCuentasPersona(string Rut_Persona)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay cuentas registradas"
				};
			}

			string[] listPersonas = metods.getContentFile("Personas");

			bool userEncontrada = false;

			for (int i = 0; i < listPersonas.Count(); i++)
			{
				string[] splitArr = listPersonas[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					userEncontrada = true;
				}
			}

			if (!userEncontrada)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			List<Cuenta> cuentas = new List<Cuenta>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					Cuenta cuenta = new Cuenta();

					cuenta.Rut_Persona = splitArr[0];
					cuenta.NumeroCuenta = splitArr[1];
					cuenta.FechaCreacion = splitArr[2];
					cuenta.Activa = Convert.ToBoolean(splitArr[3]);

					cuentas.Add(cuenta);
				}
			}

			return cuentas;
		}

		[HttpGet]
		[Route("GetCuenta")]
		public dynamic GetCuenta(string Rut_Persona, string NumeroCuenta)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay cuentas registradas"
				};
			}

			string[] listUsuarios = metods.getContentFile("Personas");

			bool userEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					userEncontrada = true;
				}
			}

			if (!userEncontrada)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			Cuenta cuenta = new Cuenta();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{
					cuenta.Rut_Persona = splitArr[0];
					cuenta.NumeroCuenta = splitArr[1];
					cuenta.FechaCreacion = splitArr[2];
					cuenta.Activa = Convert.ToBoolean(splitArr[3]);

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			return cuenta;
		}
        [HttpGet]
        [Route("GetSaldoCuenta")]
        public dynamic GetSaldoCuenta(string Rut_Persona, string NumeroCuenta)
        {
            if (!metods.validarRut(Rut_Persona))
            {
                return new
                {
                    mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
                };
            }

            string[] listCuentas = metods.getContentFile("Cuentas");

            if (listCuentas.Count() == 0)
            {
                return new
                {
                    mesage = "No hay cuentas registradas"
                };
            }

            string[] listUsuarios = metods.getContentFile("Personas");

            bool userEncontrada = false;

            for (int i = 0; i < listUsuarios.Count(); i++)
            {
                string[] splitArr = listUsuarios[i].Split("||");

                if (splitArr[0] == Rut_Persona)
                {
                    userEncontrada = true;
                }
            }

            if (!userEncontrada)
            {
                return new
                {
                    message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
                };
            }

            bool cuentaEncontrado = false;

            for (int i = 0; i < listCuentas.Count(); i++)
            {
                string[] splitArr = listCuentas[i].Split("||");

                if (splitArr[0] == Rut_Persona && splitArr[1] == NumeroCuenta)
                {
                    cuentaEncontrado = true;
                    break;
                }
            }

            if (!cuentaEncontrado)
            {
                return new
                {
                    mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
                };
            }

            string[] list = metods.getContentFile("FondosCuentas");

            List<string> content = new List<string>();

			FondosCuenta fondos = new FondosCuenta();
			fondos.NumeroCuenta = NumeroCuenta;

            bool encontrado = false;

            for (int i = 0; i < list.Count(); i++)
            {
                string[] splitArr = list[i].Split("||");

                if (splitArr[0] == NumeroCuenta)
                {
                    fondos.FondosCuentaBancaria = splitArr[1];
                    encontrado = true;

                    break;
                }

                content.Add(list[i]);
            }

            if (!encontrado)
            {
                return new
                {
                    message = "No hay fondos en la cuenta"
                };
            }

            return fondos;
        }

        [HttpGet]
		[Route("GetHistorialTransaccionesCuenta")]
		public dynamic GetHistorialTransaccionesCuenta(string Rut_Persona, string NumeroCuenta)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] list = metods.getContentFile("Transacciones");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay transaccione registradas"
				};
			}

			string[] listPersona = metods.getContentFile("Personas");

			bool personaEncontrado = false;

			for (int i = 0; i < listPersona.Count(); i++)
			{
				string[] splitArr = listPersona[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					personaEncontrado = true;
				}
			}

			if (!personaEncontrado)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			string[] listCuentas = metods.getContentFile("Cuentas");
			bool encontrado = false;

			for (int i = 0; i < listCuentas.Count(); i++)
			{
				string[] splitArr = listCuentas[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			List<Transaccion> transacciones = new List<Transaccion>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					Transaccion transaccion = new Transaccion();

					transaccion.Rut_Persona = splitArr[0];
					transaccion.NumeroCuenta = splitArr[1];
					transaccion.NumeroTarjeta = splitArr[2];	
					transaccion.Monto = splitArr[3];
					transaccion.TipoTransaccion = splitArr[4];
					transaccion.FechaTransaccion = splitArr[5];

					transacciones.Add(transaccion);
				}
			}

			return new
			{
				message = "Historial de transacciones",
				result = transacciones
			};
		}

		[HttpPost]
		[Route("CrearCuenta")]
		public dynamic CrearCuenta(string Rut_Persona)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] listPersona = metods.getContentFile("Personas");

			bool encontrado = false;

			for (int i = 0; i < listPersona.Count(); i++)
			{
				string[] splitArr = listPersona[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			Cuenta cuenta = new Cuenta();
			cuenta.Rut_Persona = Rut_Persona;
			cuenta.NumeroCuenta = metods.GenerateCreditAccountNumber();
			cuenta.FechaCreacion = DateTime.Now.ToString("dd-MM-yyyy");

			metods.saveLineFile("Cuentas", String.Format("{0}||{1}||{2}||{3}", cuenta.Rut_Persona, cuenta.NumeroCuenta, cuenta.FechaCreacion, true));

			return new
			{
				message = "Cuenta registrada",
				result = cuenta
			};
		}

		[HttpPost]
		[Route("AumentarFondosCuenta")]
		public dynamic AumentarFondosCuenta(string Rut_Persona, string NumeroCuenta, string Monto)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] listPersona = metods.getContentFile("Personas");

			bool personaEncontrado = false;

			for (int i = 0; i < listPersona.Count(); i++)
			{
				string[] splitArr = listPersona[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					personaEncontrado = true;
				}
			}

			if (!personaEncontrado)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			string[] listCuentas = metods.getContentFile("Cuentas");
			bool cuentaEncontrado = false;
			for (int i = 0; i < listCuentas.Count(); i++)
			{
				string[] splitArr = listCuentas[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{
					if (!Convert.ToBoolean(splitArr[3]))
					{
						return new
						{
							mesage = "La cuenta '" + NumeroCuenta + "' se encuentra inhabilitada"
						};
					}

					cuentaEncontrado = true;
					break;
				}
			}

			if (!cuentaEncontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			FondosCuenta fondos = new FondosCuenta();
			fondos.NumeroCuenta = NumeroCuenta;
			
			string[] list = metods.getContentFile("FondosCuentas");

			List<string> content = new List<string>();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0] == NumeroCuenta)
				{
					fondos.FondosCuentaBancaria = (Convert.ToInt64(splitArr[1] == "" ? "0" : splitArr[1]) + Convert.ToInt64(Monto)).ToString();
					content.Add(String.Format("{0}||{1}", fondos.NumeroCuenta, fondos.FondosCuentaBancaria));
					encontrado = true;

					break;
				}

				content.Add(list[i]);
			}

			if (!encontrado || list.Count() == 0)
			{
				fondos.FondosCuentaBancaria = Monto;
				metods.saveLineFile("FondosCuentas", String.Format("{0}||{1}", fondos.NumeroCuenta, fondos.FondosCuentaBancaria));

                metods.saveLineFile("Transacciones", String.Format("{0}||{1}||{2}||{3}||{4}||{5}", Rut_Persona, NumeroCuenta, "000000000", Monto, "Abono", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")));
            }
            else
			{
				metods.updateLineFile("FondosCuentas", content);

                metods.saveLineFile("Transacciones", String.Format("{0}||{1}||{2}||{3}||{4}||{5}", Rut_Persona, NumeroCuenta, "000000000", Monto, "Abono", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")));
            }

            return new
			{
				message = "Se han aumentado los fondos",
				result = fondos
			};
		}

		[HttpPost]
		[Route("DisminuirFondosCuenta")]
		public dynamic DisminuirFondosCuenta(string Rut_Persona, string NumeroCuenta, string Monto)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] listPersona = metods.getContentFile("Personas");

			bool personaEncontrada = false;

			for (int i = 0; i < listPersona.Count(); i++)
			{
				string[] splitArr = listPersona[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					personaEncontrada = true;
				}
			}

			if (!personaEncontrada)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			string[] listCuentas = metods.getContentFile("Cuentas");
			bool CuentaEncontrada = false;

			for (int i = 0; i < listCuentas.Count(); i++)
			{
				string[] splitArr = listCuentas[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{
					if (!Convert.ToBoolean(splitArr[3]))
					{
						return new
						{
							mesage = "La cuenta '" + NumeroCuenta + "' se encuentra inhabilitada"
						};
					}

					CuentaEncontrada = true;
					break;
				}
			}

			if (!CuentaEncontrada)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			FondosCuenta fondos = new FondosCuenta();
			fondos.NumeroCuenta = NumeroCuenta;

			string[] list = metods.getContentFile("FondosCuentas");

			List<string> content = new List<string>();

			bool encontrado = false;
			
			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0] == NumeroCuenta)
				{
					if (Convert.ToInt32(splitArr[1] == "" ? "0" : splitArr[1]) == 0)
					{
						return new
						{
							message = "No hay fondos en la cuenta"
						};
					}
					else if (Convert.ToInt64(Monto) > Convert.ToInt64(splitArr[1] == "" ? "0" : splitArr[1]))
					{
						return new
						{
							message = "Saldo insuficinte en la cuenta"
						};
					}

					fondos.FondosCuentaBancaria = (Convert.ToInt64(splitArr[1] == "" ? "0" : splitArr[1]) - Convert.ToInt64(Monto)).ToString(); ;
					content.Add(String.Format("{0}||{1}", fondos.NumeroCuenta, fondos.FondosCuentaBancaria));
					encontrado = true;
					break;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					message = "No hay fondos en la cuenta"
				};
			}
			else
			{
				metods.updateLineFile("FondosCuentas", content);

                metods.saveLineFile("Transacciones", String.Format("{0}||{1}||{2}||{3}||{4}||{5}", Rut_Persona, NumeroCuenta, "000000000", Monto, "Cargo", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")));
            }

            return new
			{
				message = "Se han disminuido los fondos",
				result = fondos
			};
		}

		[HttpPut]
		[Route("ActivarCuenta")]
		public dynamic ActivarCuenta(string Rut_Persona, string NumeroCuenta)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay cuentas registradas"
				};
			}

			string[] listUsuarios = metods.getContentFile("Personas");

			bool personaEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					personaEncontrada = true;
				}
			}

			if (!personaEncontrada)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();
			Cuenta cuenta = new Cuenta();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{

					content.Add(String.Format("{0}||{1}||{2}||{3}", splitArr[0], splitArr[1], splitArr[2], true));

					encontrado = true;

					cuenta.Rut_Persona = Rut_Persona;
					cuenta.NumeroCuenta = NumeroCuenta;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Cuentas", content);

			return new
			{
				mesage = "Cuenta activada"
			};
		}

		[HttpPut]
		[Route("DesactivarCuenta")]
		public dynamic DesactivarCuenta(string Rut_Persona, string NumeroCuenta)
		{
			if (!metods.validarRut(Rut_Persona))
			{
				return new
				{
					mesage = "El formato de rut es invalido, formato requerido: 99999999-9"
				};
			}

			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay cuentas registradas"
				};
			}

			string[] listUsuarios = metods.getContentFile("Personas");

			bool personaEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (splitArr[0]== Rut_Persona)
				{
					personaEncontrada = true;
				}
			}

			if (!personaEncontrada)
			{
				return new
				{
					message = "La persona con rut '" + Rut_Persona + "' no existe en los registros",
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();
			Cuenta cuenta = new Cuenta();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[0]== Rut_Persona && splitArr[1] == NumeroCuenta)
				{

					content.Add(String.Format("{0}||{1}||{2}||{3}", splitArr[0], splitArr[1], splitArr[2], false));

					encontrado = true;

					cuenta.Rut_Persona = Rut_Persona;
					cuenta.NumeroCuenta = NumeroCuenta;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Cuentas", content);

			return new
			{
				mesage = "Cuenta desactivada"
			};
		}

		[HttpDelete]
		[Route("EliminarCuenta")]
		public dynamic EliminarCuenta(string NumeroCuenta)
		{
			string[] list = metods.getContentFile("Cuentas");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ceuntas registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[1] != NumeroCuenta)
				{
					content.Add(list[i]);
				}
				else
				{
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La cuenta '" + NumeroCuenta + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Cuentas", content);

			return new
			{
				mesage = "La cuenta '" + NumeroCuenta + "' fue eliminado exitosamente"
			};
		}
	}
}
