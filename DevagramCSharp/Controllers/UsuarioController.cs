using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]

	public class UsuarioController : BaseController
	{

		public readonly ILogger<UsuarioController> _logger;

		public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
		{
			_logger = logger;
		}


		[HttpGet]
		public IActionResult ObterUsuario()
		{
			try
			{
				Usuario usuario = LerToken();

				return Ok( new UsuariorespostaDto{
					Nome = usuario.Nome,
					Email =usuario.Email
				});
			}
			catch (Exception e)
			{
				_logger.LogError("Ocorreu um erro ao abter o usuario");
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu o seguinte erro: " + e.Message,
					Status = StatusCodes.Status500InternalServerError
				});
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult SalvarUsuario([FromBody] Usuario usuario)
		{
			try
			{
				if (usuario != null)
				{
					var erros = new List<string>();

					if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome))
					{
						erros.Add("Nome invalido");
					}
					if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains("@"))
					{
						erros.Add("E-mail invalido");
					}
					if (string.IsNullOrEmpty(usuario.Senha) || string.IsNullOrWhiteSpace(usuario.Senha))
					{
						erros.Add("senha invalida");
					}
					if (erros.Count > 0)
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Status = StatusCodes.Status400BadRequest,
							Erros = erros
						});
					}

					usuario.Senha = Utils.MD5Utils.GerarHashMD5(usuario.Senha);
					usuario.Email = usuario.Email.ToLower();

					if (!_usuarioRepository.VerificarEmail(usuario.Email))
					{
						_usuarioRepository.Salvar(usuario);
					}

					else
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Status = StatusCodes.Status400BadRequest,
							Descricao = "Usuario já cadastrado!"
						});
					}
				}

				return Ok("Usuario foi salvo com sucesso");
			}
			catch (Exception e)
			{
				_logger.LogError("Ocorreu um erro ao salvar o usuario");
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu o seguinte erro: " + e.Message,
					Status = StatusCodes.Status500InternalServerError
				});
			}
		}
	}
}
