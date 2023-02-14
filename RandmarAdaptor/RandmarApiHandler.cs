using IdentityModel.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RandmarAdaptor
{
  public class RandmarApiHandler
  {
    private static string _apiToken = string.Empty;

    private static async Task<string> AuthenticateAppAsync()
    {
      string loginUrl = "https://auth.randmar.io";

      var client = new HttpClient();
      var discoveryResponse = await client.GetDiscoveryDocumentAsync(loginUrl);

      if (discoveryResponse.IsError) throw new ApplicationException();

      var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
      {
        Address = discoveryResponse.TokenEndpoint,
        ClientId = "<Your API Name>",
        ClientSecret = "<Your API Key>",
        Scope = "api"
      });

      if (tokenResponse.IsError) throw new ApplicationException();

      return tokenResponse.AccessToken;
    }

    public static string ApiToken
    {
      get
      {
        if (string.IsNullOrEmpty(_apiToken))
        {
          Task<string> task = AuthenticateAppAsync();

          task.Wait();

          _apiToken = task.Result;
        }

        if (string.IsNullOrEmpty(_apiToken)) throw new ApplicationException();

        return _apiToken;
      }
    }

    public static async Task<T> Get<T>(string apiPath)
    {
      return await Execute<T>(Method.Get, apiPath);
    }

    public static async Task<T> Post<T>(string apiPath, dynamic body = null)
    {
      return await Execute<T>(Method.Post, apiPath, body);
    }

    public static async Task<T> Put<T>(string apiPath, dynamic body = null)
    {
      return await Execute<T>(Method.Put, apiPath, body);
    }

    public static async Task<T> Delete<T>(string apiPath)
    {
      return await Execute<T>(Method.Delete, apiPath);
    }

    private static async Task<T> Execute<T>(Method method, string apiPath, dynamic body = null)
    {
      var restClient = new RestClient(@"https://api.randmar.io/V4");
      var request = new RestRequest(apiPath, method);

      request.AddParameter("Authorization", string.Format("Bearer " + ApiToken), ParameterType.HttpHeader);
      request.Timeout = 0;

      if (body != null) request.AddBody((object)body, "application/json");

      var response = await restClient.ExecuteAsync(request);
      if ((int)response.StatusCode < 200 || (int)response.StatusCode > 300)
        throw new ArgumentNullException(response.Content);

      if (typeof(T) == typeof(string)) return (T)(object)response.Content;
      else if (typeof(T) == typeof(byte[])) return (T)(object)response.RawBytes;

      return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public static T GetNonAsync<T>(string apiPath)
    {
      return Execute<T>(Method.Get, apiPath).GetAwaiter().GetResult();
    }

    internal static string ResellerId
    {
      get
      {
        var handler = new JwtSecurityTokenHandler();
        var accessToken = handler.ReadJwtToken(ApiToken);

        return accessToken.Claims.Where(c => c.Type == "ApplicationId").First().Value;
      }
    }
  }
}