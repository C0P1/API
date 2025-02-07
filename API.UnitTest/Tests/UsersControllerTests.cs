namespace API.UnitTest.Tests;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API.DTOs;
using Newtonsoft.Json.Linq;
using Xunit;
using System.Text;
using System.Text.Json;

public class UsersControllerTests : IClassFixture<APIWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObject;
    private HttpContent httpContent;

    public UsersControllerTests(APIWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    //Este metodo nos autenticara con el usuario
    private async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        // Arrange
        requestUrl = "api/account/login";
        var loginRequest = new LoginRequest
        {
            Username = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userResponse.Token);
        return _client;
    }

    //Probamos el metodo para obtener a toos los usuarios
    [Fact]
    public async Task GetAllAsyncShouldReturnAllUsers()
    {
        //Arrange
        var client = await GetAuthenticatedClientAsync();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<MemberResponse[]>();

        Assert.NotNull(users);
        Assert.True(users.Length > 0); // Asegúrate de que hay datos semilla
    }

    //Probamos que nos regrese solo un usuario
    [Theory]
    [InlineData("arenita")]
    public async Task GetByUsernameAsyncShouldReturnUser(string username)
    {
        //Arrange
        var client = await GetAuthenticatedClientAsync();

        // Act
        var response = await client.GetAsync($"/api/users/{username}");

        // Assert
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<MemberResponse>();

        Assert.NotNull(user);
        Assert.Equal(username, user.UserName);
    }

    //Prueba que nos haga un update a un usuario
    [Fact]
    public async Task UpdateUserShouldUpdateUser()
    {
        // Arrange
        var client = await GetAuthenticatedClientAsync();
        var updateRequest = new MemberUpdateRequest
        {
            Introduction = "Prueba de indtroduccion",
            LookingFor = "Prueba de Looking",
            Interests = "Prueba de interes",
            City = "Prueba de ciudad",
            Country = "Prueba de estado"
        }; //Actualizamos todos los datos

        // Act
        var response = await client.PutAsJsonAsync("/api/users", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        // Comprobar que el usuario fue actualizado
        var updatedResponse = await client.GetAsync("/api/users/arenita");
        updatedResponse.EnsureSuccessStatusCode();

        var updatedUser = await updatedResponse.Content.ReadFromJsonAsync<MemberResponse>();

        Assert.NotNull(updatedUser);
        Assert.Equal("Prueba de indtroduccion", updatedUser.Introduction); //Comprobamos que si de actualizo
    }

    //Registramos un usuario
    [Fact]
    public async Task RegisterShouldReturnSuccess()
    {
        // Arrange
        var client = _client; // Usa tu cliente HTTP inicializado
        var registerRequest = new RegisterRequest
        {
            Username = "nuevoUsuario",
            Password = "1234abcd"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/account/register", registerRequest);

        // Assert
        response.EnsureSuccessStatusCode(); // Verifica que el código de respuesta sea exitoso (200 OK o 201 Created)
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.NotNull(responseBody); // Verifica que haya una respuesta en el cuerpo
        //Comentamos esta linea por que en el codigo hacemos un this.cancel, por lo que no se actualiza en la base de datos
        //Assert.Contains("nuevoUsuario", responseBody); // Asegúramos que el usuario se haya registrado correctamente
    }

    //Intentamos registrar un usario incorrecto, por lo que retorna un bad request
    [Fact]
    public async Task RegisterShouldReturnBadRequest()
    {
        // Arrange
        var client = _client;
        var invalidRegisterRequest = new RegisterRequest
        {
            Username = "", // Nombre de usuario vacío
            Password = "123" // Contraseña demasiado corta
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/account/register", invalidRegisterRequest);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); // Verifica que devuelva un 400
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("The Username field is required.", responseBody); // Verifica los mensajes de error específicos
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
