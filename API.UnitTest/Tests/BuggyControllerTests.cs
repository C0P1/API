namespace API.UnitTest.Tests;

using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTest.Helpers;
using Newtonsoft.Json.Linq;

public class BuggyControllerTests
{
    private readonly string apiRoute = "api/buggy";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObject;
    private HttpContent httpContent;

    public BuggyControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    //Probamos que nos de acceso al usuario
    [Theory]
    [InlineData("OK", "arenita", "123456")]
    public async Task GetSecretShouldOK(string statusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"{apiRoute}/auth";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    //Probamos que nos marque error por que la contraseña/usario es incorrecto y por nombre vacio
    [Theory]
    [InlineData("Unauthorized", "arenita", "12345")]
    [InlineData("Unauthorized", "", "123456")]
    public async Task LoginShouldReturnUnauthorized(string statusCode, string username, string password)
    {
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        // Act
        httpResponse = await _client.PostAsync(requestUrl, httpContent);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    //Nos debe de dar el error notFound en la ruta de la api
    [Theory]
    [InlineData("NotFound")]
    public async Task GetNotFoundShouldNotFound(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/not-found";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    //Nos debe dar el error internal server
    [Theory]
    [InlineData("InternalServerError")]
    public async Task GetServerErrorShouldNotInternalServerError(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/server-error";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    //Nos debe de dar el error Bad request
    [Theory]
    [InlineData("BadRequest")]
    public async Task GetBadRequestShouldBadRequest(string statusCode)
    {
        // Arrange
        requestUrl = $"{apiRoute}/bad-request";

        // Act
        httpResponse = await _client.GetAsync(requestUrl);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion
}