using DevagramCSharp.Dtos;

namespace DevagramCSharp.Services
{
	public class CosmicService
	{
		public string EnviarImagem(ImagemDto imagemdto)
		{
            Stream imagem = imagemdto.Imagem.OpenReadStream();	

			return "";

		}
															
	}
}
