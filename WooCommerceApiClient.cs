using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

public class WooCommerceApiClient
{
    private readonly string _baseUrl;
    private readonly string _consumerKey;
    private readonly string _consumerSecret;

    public WooCommerceApiClient(string baseUrl, string consumerKey, string consumerSecret)
    {
        _baseUrl = baseUrl;
        _consumerKey = consumerKey;
        _consumerSecret = consumerSecret;
    }

    private RestClient GetClient()
    {
        var options = new RestClientOptions(_baseUrl)
        {
            Authenticator = new HttpBasicAuthenticator(_consumerKey, _consumerSecret)
        };
        return new RestClient(options);
    }

    public async Task<RestResponse> CreateProduct(string name, string description, decimal price)
    {
        var client = GetClient();
        var request = new RestRequest("wp-json/wc/v3/products", Method.Post);
        request.AddJsonBody(new
        {
            name = name,
            description = description,
            regular_price = price.ToString()
        });

        return await client.ExecuteAsync(request);
    }

    public async Task<RestResponse> UpdateProduct(int productId, string name, string description, decimal price)
    {
        var client = GetClient();
        var request = new RestRequest($"wp-json/wc/v3/products/{productId}", Method.Put);
        request.AddJsonBody(new
        {
            name = name,
            description = description,
            regular_price = price.ToString()
        });

        return await client.ExecuteAsync(request);
    }

    public async Task<RestResponse> DeleteProduct(int productId)
    {
        var client = GetClient();
        var request = new RestRequest($"wp-json/wc/v3/products/{productId}", Method.Delete);
        request.AddParameter("force", "true");

        return await client.ExecuteAsync(request);
    }
}