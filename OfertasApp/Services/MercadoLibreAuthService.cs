public class MercadoLibreAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MercadoLibreAuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        var client = _httpClientFactory.CreateClient();

        var values = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", "4830097416227933" },
            { "client_secret", "sc0c7MhCByc0VuFOyJyg2ll97BK7TbXk" },
            { "refresh_token", refreshToken }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://api.mercadolibre.com/oauth/token", content);
        var responseString = await response.Content.ReadAsStringAsync();

        return responseString; // acá recibís el nuevo access_token y refresh_token
    }
}
