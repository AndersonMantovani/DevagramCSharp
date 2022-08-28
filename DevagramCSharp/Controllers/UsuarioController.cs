using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Services;
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

				return Ok(new UsuariorespostaDto
				{
					Nome = usuario.Nome,
					Email = usuario.Email
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
		public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
		{
			try
			{
				if (usuariodto != null)
				{
					var erros = new List<string>();

					if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
					{
						erros.Add("Nome invalido");
					}
					if (string.IsNullOrEmpty(usuariodto.Email) || string.IsNullOrWhiteSpace(usuariodto.Email) || !usuariodto.Email.Contains("@"))
					{
						erros.Add("E-mail invalido");
					}
					if (string.IsNullOrEmpty(usuariodto.Senha) || string.IsNullOrWhiteSpace(usuariodto.Senha))
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

					CosmicService cosmicservice = new CosmicService();

					Usuario usuario = new Usuario()
					{
						    Email = usuariodto.Email,
							Senha = usuariodto.Senha,
							Nome = usuariodto.Nome,
							FotoPerfil = cosmicservice.EnviarImagem(new ImagemDto { Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ", "")})
          
					};


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
