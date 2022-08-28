using System.Net.Mime;
using DevagramCSharp.Dtos;
using System.Net.Http.Headers;

namespace DevagramCSharp.Services
{
	public class CosmicService
	{
		public string EnviarImagem(ImagemDto imagemdto)
		{
			Stream imagem;
			imagem = imagemdto.Imagem.OpenReadStream();

			var client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "71TeXRrMqfSCrwlne3cxUf3sgqPVeduevdORSETljN5rdgTl8M");
			 
			 var request = new HttpRequestMessage(HttpMethod.Post, "file");
			 var conteudo = new MultipartFormDataContent
			 {
                 {new StreamContent(imagem), "media", imagemdto.Nome}
			 };

			 request.Content = conteudo;
			 var retornoreq = client.PostAsync("https://upload.cosmicjs.com/v2/buckets/devagramaula-devagramrede/media", request.Content).Result;

			 var urlretorno = retornoreq.Content.ReadFromJsonAsync<CosmicRespostaDto>();

			return urlretorno.Result.media.url;

		}
															
	}
}
