﻿using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly ILogger<LoginController> _logger;

		public LoginController(ILogger<LoginController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		[AllowAnonymous]

		public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto loginrequisicao)
		{

			try
			{
				if (!String.IsNullOrEmpty(loginrequisicao.Senha) && !String.IsNullOrEmpty(loginrequisicao.Email)
					&& !String.IsNullOrWhiteSpace(loginrequisicao.Senha) && !String.IsNullOrWhiteSpace(loginrequisicao.Email))
				{
					string email = "mantovani@mantovani.com.br";
					string senha = "Senha123";

					if (loginrequisicao.Email == email && loginrequisicao.Senha == senha)
					{
						Usuario usuario = new Usuario()
						{
							Email = loginrequisicao.Email,
							Id = 12,
							Nome = "Anderson Mantovani"
						};

						return Ok(new LoginRespostaDto()
						{

							Email = usuario.Email,
							Nome = usuario.Nome,
							Token = TokenService.CriarToken(usuario)

						});

					}
					else
					{
						return BadRequest(new ErrorRespostaDto()
						{
							Descricao = "Email ou senha inválido!",
							Status = StatusCodes.Status400BadRequest
						});
					}

				}
				else
				{
					return BadRequest(new ErrorRespostaDto()
					{
						Descricao = "Usuario não preencheu os campos de login corretamente",
						Status = StatusCodes.Status400BadRequest
					});

				}
			}

			catch (Exception ex)
			{

				_logger.LogError("Ocorreu um erro no login:" + ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRespostaDto()
				{
					Descricao = "Ocorreu um erro ao fazer o login",
					Status = StatusCodes.Status500InternalServerError
				});

			}
		}

	}
}
