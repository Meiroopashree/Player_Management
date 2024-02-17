using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dotnetapp.Models;
using System.Collections.Generic;
using System.Linq;

namespace TestProject;


[TestFixture]
public class Tests
{
    private HttpClient _httpClient;
    private string _generatedToken;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://8080-bfdeeddcedfabcfacbdcbaeadbebabcdebdca.premiumproject.examly.io/"); 
    }

[Test, Order(1)]
    public async Task Backend_TestRegisterUser()
    {
        string uniqueId = Guid.NewGuid().ToString();
 
        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
 
        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Organizer\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
 
        Console.WriteLine(response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseString);
 
        // Assuming you get a 200 OK status for successful registration
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

 
    [Test, Order(2)]
    public async Task Backend_TestLoginUser()
    {
        string uniqueId = Guid.NewGuid().ToString();
 
        string uniqueusername = $"abcd_{uniqueId}";
        string uniquepassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
        string uniqueuserrole = "Organizer";
        string requestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"{uniqueuserrole}\" }}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
 
        string requestBody1 = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
        HttpResponseMessage response1 = await _httpClient.PostAsync("/api/login", new StringContent(requestBody1, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        string responseBody = await response1.Content.ReadAsStringAsync();
    }
 
    [Test, Order(3)]
    public async Task Backend_TestRegisterAdmin()
    {
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
 
        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
       
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
 
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        string responseBody = await response.Content.ReadAsStringAsync();
        // Add your assertions based on the response if needed
    }
 
    [Test, Order(4)]
    public async Task Backend_TestLoginAdmin()
    {
        string uniqueId = Guid.NewGuid().ToString();
 
        string uniqueusername = $"abcd_{uniqueId}";
        string uniquepassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";
        string uniqueuserrole = "Admin";
        string requestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"{uniqueuserrole}\" }}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
 
        string requestBody1 = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
        HttpResponseMessage response1 = await _httpClient.PostAsync("/api/login", new StringContent(requestBody1, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
        string responseBody = await response1.Content.ReadAsStringAsync();
    }

[Test]
public async Task Backend_TestAddEvent()
{
    // Generate unique identifiers
    string uniqueId = Guid.NewGuid().ToString();
    string uniqueUsername = $"abcd_{uniqueId}";
    string uniquePassword = $"abcdA{uniqueId}@123";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    // Register an organizer
    string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
    HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

    // Login the registered organizer
    string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
    string organizerAuthToken = loginResponseMap.token;

    // Use the obtained token in the request to add an event
    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

    var eventToAdd = new
    {
        EventName = "Sample Event",
        StartDate = DateTime.UtcNow.AddDays(1),
        EndDate = DateTime.UtcNow.AddDays(2),
        EventImageURL = "https://example.com/image.jpg",
        Description = "Event description"
    };

    string eventRequestBody = JsonConvert.SerializeObject(eventToAdd);
    HttpResponseMessage eventResponse = await _httpClient.PostAsync("api/event", new StringContent(eventRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, eventResponse.StatusCode);
}
        [Test]
        public async Task Backend_TestGetAllEvents()
        {
            // Generate unique identifiers
            string uniqueId = Guid.NewGuid().ToString();
            string uniqueusername = $"abcd_{uniqueId}";
            string uniquepassword = $"abcdA{uniqueId}@123";
            string uniqueEmail = $"abcd{uniqueId}@gmail.com";

            // Register a customer
            string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
            HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

            // Login the registered customer
            string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
            HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
            Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
            string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
            dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
            string customerAuthToken = loginResponseMap.token;

            // Use the obtained token in the request to get events
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

            // Make a request to get events
            HttpResponseMessage getEventsResponse = await _httpClient.GetAsync("api/event");
            Assert.AreEqual(HttpStatusCode.OK, getEventsResponse.StatusCode);

            // Validate the response content (assuming the response is a JSON array of events)
            string getEventsResponseBody = await getEventsResponse.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<Event>>(getEventsResponseBody);
            Assert.IsNotNull(events);
            Assert.IsTrue(events.Any());
        }

[Test]
public async Task Backend_TestGetEventById()
{
    // Generate unique identifiers
    string uniqueId = Guid.NewGuid().ToString();
    string uniqueusername = $"abcd_{uniqueId}";
    string uniquepassword = $"abcdA{uniqueId}@123";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    // Register a customer
    string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
    HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

    // Login the registered customer
    string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
    string organizerAuthToken = loginResponseMap.token;

    // Use the obtained token in the request to get the organizer's event
    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

    // Make a request to get the events
    HttpResponseMessage getEventsResponse = await _httpClient.GetAsync("api/event");
    Assert.AreEqual(HttpStatusCode.OK, getEventsResponse.StatusCode);

    // Validate the response content (assuming the response is a JSON array of events)
    string getEventsResponseBody = await getEventsResponse.Content.ReadAsStringAsync();
    var events = JsonConvert.DeserializeObject<List<Event>>(getEventsResponseBody);
    Assert.IsNotNull(events);
    Assert.IsTrue(events.Any());

    // Get the first event ID from the response
    int eventId = events.First().EventId;

    // Make a request to get a specific event by ID
    HttpResponseMessage getEventByIdResponse = await _httpClient.GetAsync($"api/event/{eventId}");
    Assert.AreEqual(HttpStatusCode.OK, getEventByIdResponse.StatusCode);

    // Validate the response content for the specific event
    string getEventByIdResponseBody = await getEventByIdResponse.Content.ReadAsStringAsync();
    var specificEvent = JsonConvert.DeserializeObject<Event>(getEventByIdResponseBody);
    Assert.IsNotNull(specificEvent);
    Assert.AreEqual(eventId, specificEvent.EventId);
}

[Test]
public async Task Backend_TestUpdateEvent()
{
    // Generate unique identifiers
    string uniqueId = Guid.NewGuid().ToString();
    string uniqueUsername = $"abcd_{uniqueId}";
    string uniquePassword = $"abcdA{uniqueId}@123";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    // Register an organizer
    string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
    HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

    // Login the registered organizer
    string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
    string organizerAuthToken = loginResponseMap.token;

    // Use the obtained token in the request to get the organizer's events
    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

    // Make a request to get the organizer's events
    HttpResponseMessage getEventsResponse = await _httpClient.GetAsync("api/event");
    Assert.AreEqual(HttpStatusCode.OK, getEventsResponse.StatusCode);

    // Validate the response content (assuming the response is a JSON array of events)
    string getEventsResponseBody = await getEventsResponse.Content.ReadAsStringAsync();
    var events = JsonConvert.DeserializeObject<List<Event>>(getEventsResponseBody);
    Assert.IsNotNull(events);
    Assert.IsTrue(events.Any());

    // Get the first event ID from the response
    int eventId= events.First().EventId;

    // Generate updated event data
    var updatedEvent = new Event
    {
        EventName = "Updated Event",
        StartDate = DateTime.UtcNow,
        EndDate = DateTime.UtcNow.AddDays(2),
        EventImageURL = "updated-image-url.jpg",
        Description = "Updated event description"
        // Add more properties as needed
    };

    // Make a request to update the event
    string updateEventRequestBody = JsonConvert.SerializeObject(updatedEvent);
    HttpResponseMessage updateEventResponse = await _httpClient.PutAsync($"api/event/{eventId}", new StringContent(updateEventRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, updateEventResponse.StatusCode);

    // Validate the response content
    string updateEventResponseBody = await updateEventResponse.Content.ReadAsStringAsync();
    dynamic updateEventResponseMap = JsonConvert.DeserializeObject(updateEventResponseBody);

    // Corrected assertion for the response message
    Assert.AreEqual("Event updated successfully", updateEventResponseMap.message.ToString());
}

[Test]
public async Task Backend_TestDeleteEvent()
{
    // Generate unique identifiers
    string uniqueId = Guid.NewGuid().ToString();
    string uniqueUsername = $"abcd_{uniqueId}";
    string uniquePassword = $"abcdA{uniqueId}@123";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    // Register an organizer
    string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
    HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

    // Login the registered organizer
    string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
    string organizerAuthToken = loginResponseMap.token;

    // Use the obtained token in the request to get the organizer's events
    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

    // Make a request to get the organizer's events
    HttpResponseMessage getEventsResponse = await _httpClient.GetAsync("api/event");
    Assert.AreEqual(HttpStatusCode.OK, getEventsResponse.StatusCode);

    // Validate the response content (assuming the response is a JSON array of events)
    string getEventsResponseBody = await getEventsResponse.Content.ReadAsStringAsync();
    var events = JsonConvert.DeserializeObject<List<Event>>(getEventsResponseBody);
    Assert.IsNotNull(events);
    Assert.IsTrue(events.Any());

    // Get the first event ID from the response
    int eventIdToDelete = events.First().EventId;

    // Make a request to delete the event
    HttpResponseMessage deleteEventResponse = await _httpClient.DeleteAsync($"api/event/{eventIdToDelete}");
    Assert.AreEqual(HttpStatusCode.OK, deleteEventResponse.StatusCode);

    // Validate the response content
    string deleteEventResponseBody = await deleteEventResponse.Content.ReadAsStringAsync();
    dynamic deleteEventResponseMap = JsonConvert.DeserializeObject(deleteEventResponseBody);
    
    // Assert that the event was deleted successfully
    Assert.AreEqual("Event deleted successfully", deleteEventResponseMap.message.ToString());
}

// [Test]
// public async Task Backend_TestAddPlayer()
// {
//     try
//     {
//         // Generate unique identifiers
//         string uniqueId = Guid.NewGuid().ToString();
//         string uniqueUsername = $"abcd_{uniqueId}";
//         string uniquePassword = $"abcdA{uniqueId}@123";
//         string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//         // Register an admin user
//         string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
//         HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//         // Login the registered admin user
//         string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
//         HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//         string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//         dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//         string adminAuthToken = loginResponseMap.token;

//         // Use the obtained token in the request to add a player
//         _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

//         // Add a player
//         var playerToAdd = new Player
//         {
//             PlayerName = "John Doe",
//             Age = 25,
//             Country = "United States",
//             BattingStyle = "Right-Handed",
//             BowlingStyle = "Medium Pace",
//             DateOfBirth = new DateTime(1997, 5, 15),
//             Role = "Batsman",
//             TotalMatchesPlayed = 50,
//             TotalRunsScored = 1500,
//             TotalWicketsTaken = 20,
//             TotalCatches = 10,
//             TeamId = 1,
//         };

//         string playerRequestBody = JsonConvert.SerializeObject(playerToAdd);
//         HttpResponseMessage playerResponse = await _httpClient.PostAsync("/api/player", new StringContent(playerRequestBody, Encoding.UTF8, "application/json"));

//         // Check for successful status code
//         Assert.AreEqual(HttpStatusCode.OK, playerResponse.StatusCode);

//         // Check for valid response content
//         string playerResponseBody = await playerResponse.Content.ReadAsStringAsync();
//         dynamic playerResponseMap = JsonConvert.DeserializeObject(playerResponseBody);

//         // Assuming the response contains a message indicating success
//         Assert.AreEqual("Player added successfully", playerResponseMap.message.ToString());
//     }
//     catch (HttpRequestException httpEx)
//     {
//         // Log HTTP exception details
//         Console.WriteLine($"HTTP Exception: {httpEx.Message}");
//         if (httpEx.InnerException != null)
//         {
//             Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
//         }

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
//     catch (Exception ex)
//     {
//         // Log general exception details
//         Console.WriteLine($"Exception: {ex.Message}");
//         Console.WriteLine($"StackTrace: {ex.StackTrace}");

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
// }

[Test]
public async Task Backend_TestAddReferee()
{
    try
    {
        // Generate unique identifiers
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register an admin user
        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered admin
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string adminAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to add a referee
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        var refereeToAdd = new
        {
            RefereeName = "John Doe",
            Country = "United States",
        };

        string refereeRequestBody = JsonConvert.SerializeObject(refereeToAdd);
        HttpResponseMessage refereeResponse = await _httpClient.PostAsync("api/referee", new StringContent(refereeRequestBody, Encoding.UTF8, "application/json"));

        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, refereeResponse.StatusCode);

        // Check for valid response content
        string refereeResponseBody = await refereeResponse.Content.ReadAsStringAsync();
        dynamic refereeResponseMap = JsonConvert.DeserializeObject(refereeResponseBody);

        Assert.AreEqual("Referee added successfully", refereeResponseMap.message.ToString());
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}

[Test]
public async Task Backend_TestGetAllReferees()
{
    try
    {
        // Generate unique identifiers
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register a user (assuming they have necessary permissions, e.g., Admin)
        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string userAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to get all referees
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userAuthToken);

        HttpResponseMessage getAllRefereesResponse = await _httpClient.GetAsync("api/referee");

        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, getAllRefereesResponse.StatusCode);

        // Check for valid response content
        string getAllRefereesResponseBody = await getAllRefereesResponse.Content.ReadAsStringAsync();
        var referees = JsonConvert.DeserializeObject<List<Referee>>(getAllRefereesResponseBody);

        // Assuming referees is a non-null list, you can perform further assertions as needed
        Assert.IsNotNull(referees);
        Assert.IsTrue(referees.Any());
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}


// [Test]
// public async Task Backend_TestGetRefereeById()
// {
//     try
//     {
//         // Generate unique identifiers
//         string uniqueId = Guid.NewGuid().ToString();
//         string uniqueUsername = $"abcd_{uniqueId}";
//         string uniquePassword = $"abcdA{uniqueId}@123";
//         string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//         // Register a user (assuming they have necessary permissions, e.g., Admin)
//         string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
//         HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//         // Login the registered user
//         string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
//         HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//         string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//         dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//         string userAuthToken = loginResponseMap.token;

//         // Use the obtained token in the request to get referee by ID
//         _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userAuthToken);

//         // Make a request to get referee by ID (assuming refereeId is a valid ID)
//         int refereeId = 1; // Replace with a valid referee ID
//         HttpResponseMessage getRefereeByIdResponse = await _httpClient.GetAsync($"api/referee/{refereeId}");

//         // Check for successful status code
//         Assert.AreEqual(HttpStatusCode.OK, getRefereeByIdResponse.StatusCode);

//         // Check for valid response content
//         string getRefereeByIdResponseBody = await getRefereeByIdResponse.Content.ReadAsStringAsync();
//         var referee = JsonConvert.DeserializeObject<Referee>(getRefereeByIdResponseBody);

//         // Assuming referee is not null, you can perform further assertions as needed
//         Assert.IsNotNull(referee);
//     }
//     catch (HttpRequestException httpEx)
//     {
//         // Log HTTP exception details
//         Console.WriteLine($"HTTP Exception: {httpEx.Message}");
//         if (httpEx.InnerException != null)
//         {
//             Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
//         }

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
//     catch (Exception ex)
//     {
//         // Log general exception details
//         Console.WriteLine($"Exception: {ex.Message}");
//         Console.WriteLine($"StackTrace: {ex.StackTrace}");

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
// }

// [Test]
// public async Task Backend_TestAddSchedule()
// {
//     try
//     {
//         // Generate unique identifiers
//         string uniqueId = Guid.NewGuid().ToString();
//         string uniqueUsername = $"abcd_{uniqueId}";
//         string uniquePassword = $"abcdA{uniqueId}@123";
//         string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//         // Register an organizer user
//         string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
//         HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//         // Login the registered organizer user
//         string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
//         HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//         Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//         string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//         dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//         string organizerAuthToken = loginResponseMap.token;

//         // Use the obtained token in the request to add a schedule
//         _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

//         // Add a schedule
//         var scheduleToAdd = new Schedule
//         {
//             MatchDateTime = DateTime.Now.AddHours(24), // Set match date and time 24 hours from now
//             EventId = 1, // Replace with a valid event ID
//             VenueId = 1, // Replace with a valid venue ID
//             RefereeId = 1, // Replace with a valid referee ID
//             Team1Id = 1, // Replace with a valid team ID
//             Team2Id = 2, // Replace with another valid team ID
//         };

//         string scheduleRequestBody = JsonConvert.SerializeObject(scheduleToAdd);
//         HttpResponseMessage scheduleResponse = await _httpClient.PostAsync("/api/schedule", new StringContent(scheduleRequestBody, Encoding.UTF8, "application/json"));

//         // Check for successful status code
//         Assert.AreEqual(HttpStatusCode.OK, scheduleResponse.StatusCode);

//         // Check for valid response content
//         string scheduleResponseBody = await scheduleResponse.Content.ReadAsStringAsync();
//         dynamic scheduleResponseMap = JsonConvert.DeserializeObject(scheduleResponseBody);

//         // Assuming the response contains a message indicating success
//         Assert.AreEqual("Schedule added successfully", scheduleResponseMap.message.ToString());
//     }
//     catch (HttpRequestException httpEx)
//     {
//         // Log HTTP exception details
//         Console.WriteLine($"HTTP Exception: {httpEx.Message}");
//         if (httpEx.InnerException != null)
//         {
//             Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
//         }

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
//     catch (Exception ex)
//     {
//         // Log general exception details
//         Console.WriteLine($"Exception: {ex.Message}");
//         Console.WriteLine($"StackTrace: {ex.StackTrace}");

//         // Re-throw the exception to mark the test as failed
//         throw;
//     }
// }

[Test]
public async Task Backend_TestGetAllSchedules()
{
    try
    {
        // Generate unique identifiers
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register an organizer user
        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Organizer\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered organizer user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string organizerAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to get all schedules
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", organizerAuthToken);

        // Get all schedules
        HttpResponseMessage getAllSchedulesResponse = await _httpClient.GetAsync("/api/schedule");

        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, getAllSchedulesResponse.StatusCode);

        // Check for valid response content (assuming it returns a list of schedules)
        string schedulesResponseBody = await getAllSchedulesResponse.Content.ReadAsStringAsync();
        List<Schedule> schedules = JsonConvert.DeserializeObject<List<Schedule>>(schedulesResponseBody);

        // Assert that the returned list is not null
        Assert.IsNotNull(schedules);

        // You can add more specific assertions based on your application logic
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}


[Test]
public async Task Backend_TestAddTeam()
{
    try
    {
        // Generate unique identifiers
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register an admin user
        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered admin user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string adminAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to add a team
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Add a team
        var teamToAdd = new Team
        {
            TeamName = "Sample Team",
            teamDescription = "This is a sample team for testing purposes.",
            // You can add other properties based on your actual Team model
        };

        string teamRequestBody = JsonConvert.SerializeObject(teamToAdd);
        HttpResponseMessage teamResponse = await _httpClient.PostAsync("/api/team", new StringContent(teamRequestBody, Encoding.UTF8, "application/json"));

        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, teamResponse.StatusCode);

        // Check for valid response content
        string teamResponseBody = await teamResponse.Content.ReadAsStringAsync();
        dynamic teamResponseMap = JsonConvert.DeserializeObject(teamResponseBody);

        // Assuming the response contains a message indicating success
        Assert.AreEqual("Team added successfully", teamResponseMap.message.ToString());
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}

[Test]
public async Task Backend_TestGetAllTeams()
{
    try
    {
        // Generate unique identifiers
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        // Register an admin user
        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered admin user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string adminAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to get all teams
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Get all teams
        HttpResponseMessage teamsResponse = await _httpClient.GetAsync("/api/team");
        
        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, teamsResponse.StatusCode);

        // Check for valid response content
        string teamsResponseBody = await teamsResponse.Content.ReadAsStringAsync();
        dynamic teamsResponseMap = JsonConvert.DeserializeObject(teamsResponseBody);

        // Assuming the response contains a list of teams
        Assert.IsNotNull(teamsResponseMap);
        Assert.IsInstanceOf<IEnumerable<dynamic>>(teamsResponseMap);
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}


[Test]
public async Task Backend_TestGetTeamById_ExistingTeam()
{
    try
    {
        // Register an admin user
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered admin user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string adminAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to get team by ID
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Assume there is an existing team with ID 1
        int teamId = 1;

        // Get team by ID
        HttpResponseMessage teamResponse = await _httpClient.GetAsync($"/api/team/{teamId}");

        // Check for successful status code
        Assert.AreEqual(HttpStatusCode.OK, teamResponse.StatusCode);

        // Check for valid response content
        string teamResponseBody = await teamResponse.Content.ReadAsStringAsync();
        dynamic teamResponseMap = JsonConvert.DeserializeObject<Team>(teamResponseBody);

        // Assuming the response contains team details
        Assert.IsNotNull(teamResponseMap);
        Assert.AreEqual(teamId, teamResponseMap.TeamId);
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}


[Test]
public async Task Backend_TestUpdateTeam_ExistingTeam()
{
    try
    {
        // Register an admin user
        string uniqueId = Guid.NewGuid().ToString();
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniquePassword = $"abcdA{uniqueId}@123";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string registerRequestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"{uniquePassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"UserRole\" : \"Admin\" }}";
        HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

        // Login the registered admin user
        string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquePassword}\"}}";
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
        string adminAuthToken = loginResponseMap.token;

        // Use the obtained token in the request to add a team (for existing team data)
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

        // Assume there is an existing team with ID 1
        int existingTeamId = 1;

        // Get the existing team details
        HttpResponseMessage getTeamResponse = await _httpClient.GetAsync($"/api/team/{existingTeamId}");
        Assert.AreEqual(HttpStatusCode.OK, getTeamResponse.StatusCode);
        string existingTeamResponseBody = await getTeamResponse.Content.ReadAsStringAsync();
        dynamic existingTeamResponseMap = JsonConvert.DeserializeObject<Team>(existingTeamResponseBody);
        Assert.IsNotNull(existingTeamResponseMap);

        // Modify some properties of the existing team
        existingTeamResponseMap.TeamName = "Updated Team Name";
        existingTeamResponseMap.teamDescription = "Updated Team Description";

        // Update the team
        HttpResponseMessage updateTeamResponse = await _httpClient.PutAsync($"/api/team/{existingTeamId}", new StringContent(JsonConvert.SerializeObject(existingTeamResponseMap), Encoding.UTF8, "application/json"));
        Assert.AreEqual(HttpStatusCode.OK, updateTeamResponse.StatusCode);

        // Check for valid response content
        string updateTeamResponseBody = await updateTeamResponse.Content.ReadAsStringAsync();
        dynamic updateTeamResponseMap = JsonConvert.DeserializeObject(updateTeamResponseBody);

        // Assuming the response contains a message indicating success
        Assert.IsNotNull(updateTeamResponseMap);
        Assert.AreEqual("Team updated successfully", updateTeamResponseMap.message.ToString());
    }
    catch (HttpRequestException httpEx)
    {
        // Log HTTP exception details
        Console.WriteLine($"HTTP Exception: {httpEx.Message}");
        if (httpEx.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {httpEx.InnerException.Message}");
        }

        // Re-throw the exception to mark the test as failed
        throw;
    }
    catch (Exception ex)
    {
        // Log general exception details
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        // Re-throw the exception to mark the test as failed
        throw;
    }
}



// [Test]
// public async Task Backend_TestGetAddonsAsCustomer()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to get addons
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Make a request to get addons
//     HttpResponseMessage getAddonsResponse = await _httpClient.GetAsync("api/getAddon");
//     Assert.AreEqual(HttpStatusCode.OK, getAddonsResponse.StatusCode);

//     // Validate the response content (assuming the response is a JSON array of addons)
//     string getAddonsResponseBody = await getAddonsResponse.Content.ReadAsStringAsync();
//     var addons = JsonConvert.DeserializeObject<List<Addon>>(getAddonsResponseBody);
//     Assert.IsNotNull(addons);
//     Assert.IsTrue(addons.Any());
// }

// [Test]
// public async Task Backend_TestUpdateAddon()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add an addon
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Add an addon
//     var addAddonReview = new
//     {
//         AddonValidity = "Test subject",
//         AddonDetails = "Test body",
//         AddonPrice = 5,
//         AddonName = "sample name"
//     };

//     string addonRequestBody = JsonConvert.SerializeObject(addAddonReview);
//     HttpResponseMessage addonResponse = await _httpClient.PostAsync("api/addAddon", new StringContent(addonRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, addonResponse.StatusCode);

//     // Get the list of addons
//     HttpResponseMessage getAddonsResponse = await _httpClient.GetAsync("api/getAddon");
//     Assert.AreEqual(HttpStatusCode.OK, getAddonsResponse.StatusCode);
//     string getAddonsResponseBody = await getAddonsResponse.Content.ReadAsStringAsync();
//     var addons = JsonConvert.DeserializeObject<List<Addon>>(getAddonsResponseBody);
//     Assert.IsNotNull(addons);
//     Assert.IsTrue(addons.Any());

//     // Update the newly added addon
//     var updatedAddon = new
//     {
//         AddonValidity = "Updated subject",
//         AddonDetails = "Updated body",
//         AddonPrice = 10,
//         AddonName = "updated name"
//     };

//     string updateAddonRequestBody = JsonConvert.SerializeObject(updatedAddon);

//     // Assuming AddonId property exists in Addon model
//     long addonIdToUpdate = addons.FirstOrDefault()?.AddonId ?? 1;
//     HttpResponseMessage updateAddonResponse = await _httpClient.PutAsync($"api/editAddon/{addonIdToUpdate}", new StringContent(updateAddonRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, updateAddonResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestDeleteAddon()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add an addon
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Add an addon
//     var addAddonReview = new
//     {
//         AddonValidity = "Test subject",
//         AddonDetails = "Test body",
//         AddonPrice = 5,
//         AddonName = "sample name"
//     };

//     string addonRequestBody = JsonConvert.SerializeObject(addAddonReview);
//     HttpResponseMessage addonResponse = await _httpClient.PostAsync("api/addAddon", new StringContent(addonRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, addonResponse.StatusCode);

//     // Get the list of addons
//     HttpResponseMessage getAddonsResponse = await _httpClient.GetAsync("api/getAddon");
//     Assert.AreEqual(HttpStatusCode.OK, getAddonsResponse.StatusCode);
//     string getAddonsResponseBody = await getAddonsResponse.Content.ReadAsStringAsync();
//     var addons = JsonConvert.DeserializeObject<List<Addon>>(getAddonsResponseBody);
//     Assert.IsNotNull(addons);
//     Assert.IsTrue(addons.Any());

//     // Delete the newly added addon
//     // Assuming AddonId property exists in Addon model
//     long addonIdToDelete = addons.FirstOrDefault()?.AddonId ?? 1;
//     HttpResponseMessage deleteAddonResponse = await _httpClient.DeleteAsync($"api/deleteAddon/{addonIdToDelete}");
//     Assert.AreEqual(HttpStatusCode.OK, deleteAddonResponse.StatusCode);

//     // Verify that the addon is deleted
//     HttpResponseMessage verifyDeleteResponse = await _httpClient.GetAsync($"api/getAddon/{addonIdToDelete}");
//     Assert.AreEqual(HttpStatusCode.NotFound, verifyDeleteResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestAddPlan()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add an addon
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     var planDetails = new
//     {
//         PlanName = "Test Plan",
//         PlanPrice = 10,
//         PlanDescription = "Test plan description",
//         PlanType = "prepaid",
//         PlanValidity = "30",
//         PlanOffer = "Test Offer"
//     };

//     string planRequestBody = JsonConvert.SerializeObject(planDetails);
//     HttpResponseMessage planResponse = await _httpClient.PostAsync("api/addPlan", new StringContent(planRequestBody, Encoding.UTF8, "application/json"));
    
//     // Assert that the plan is added successfully
//     Assert.AreEqual(HttpStatusCode.OK, planResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestGetPlans()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add an addon
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     var planDetails = new
//     {
//         PlanName = "Test Plan",
//         PlanPrice = 10,
//         PlanDescription = "Test plan description",
//         PlanType = "prepaid",
//         PlanValidity = "30",
//         PlanOffer = "Test Offer"
//     };

//     string planRequestBody = JsonConvert.SerializeObject(planDetails);
//     HttpResponseMessage planResponse = await _httpClient.PostAsync("api/addPlan", new StringContent(planRequestBody, Encoding.UTF8, "application/json"));
    
//     // Assert that the plan is added successfully
//     Assert.AreEqual(HttpStatusCode.OK, planResponse.StatusCode);

//     // Use the obtained token in the request to get plans
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Make a request to get all plans
//     HttpResponseMessage getPlansResponse = await _httpClient.GetAsync("api/getAllPlan");
//     Assert.AreEqual(HttpStatusCode.OK, getPlansResponse.StatusCode);

//     // Validate the response content (assuming the response is a JSON array of plans)
//     string getPlansResponseBody = await getPlansResponse.Content.ReadAsStringAsync();
//     var plans = JsonConvert.DeserializeObject<List<Plan>>(getPlansResponseBody);
//     Assert.IsNotNull(plans);
//     Assert.IsTrue(plans.Any());
// }


// [Test]
// public async Task Backend_TestGetAllReviews()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add an addon
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     string responseString = await loginResponse.Content.ReadAsStringAsync();
//         dynamic responseMap = JsonConvert.DeserializeObject(responseString);
//         string adminAuthToken = responseMap.token;
 
//         _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);
 
//         HttpResponseMessage getReviewsResponse = await _httpClient.GetAsync("/api");
       
//         Assert.AreEqual(HttpStatusCode.OK, getReviewsResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestPostReviews()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a review
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     var reviewDetails = new
//     {
//         ReviewId = 0,
//         UserId = 1,
//         Subject = "Test Subject",
//         Body = "Test Body",
//         Rating = 4,
//         DateCreated = DateTime.Now,
//         User = new
//         {
//             UserId = 0,
//             Email = "string",
//             Password = "string",
//             Username = "string",
//             MobileNumber = "string",
//             Role = "string"
//         }
//     };

//     string reviewRequestBody = JsonConvert.SerializeObject(reviewDetails);
//     HttpResponseMessage addReviewResponse = await _httpClient.PostAsync("/api", new StringContent(reviewRequestBody, Encoding.UTF8, "application/json"));

//     // Assert that the review is added successfully
//     if (addReviewResponse.StatusCode != HttpStatusCode.OK)
//     {
//         // Additional information about the response
//         string responseContent = await addReviewResponse.Content.ReadAsStringAsync();
//         Console.WriteLine($"Response Content: {responseContent}");
//     }

//     Assert.AreEqual(HttpStatusCode.OK, addReviewResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestAddRecharge()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a recharge
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     var rechargeDetails = new
//     {
//         RechargeId = 0,
//         Price = 20,
//         RechargeDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
//         ValidityDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
//         UserId = 1, // Assuming the user is registered with ID 1
//         PlanId = 1, // Assuming the plan is already registered with ID 1
//         User = new
//         {
//             UserId = 0,
//             Email = "string",
//             Password = "string",
//             Username = "string",
//             MobileNumber = "string",
//             Role = "string"
//         },
//         Plan = new
//         {
//             PlanId = 0,
//             PlanType = "string",
//             PlanName = "string",
//             PlanValidity = "string",
//             PlanOffer = "string",
//             PlanDescription = "string",
//             PlanPrice = 0
//         }
//     };


//     string rechargeRequestBody = JsonConvert.SerializeObject(rechargeDetails);
//     HttpResponseMessage rechargeResponse = await _httpClient.PostAsync("/api/addRecharge", new StringContent(rechargeRequestBody, Encoding.UTF8, "application/json"));

//     // Assert that the recharge is added successfully
//     Assert.AreEqual(HttpStatusCode.OK, rechargeResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestGetRechargeById()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a recharge
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Assuming you have a rechargeId from a previous recharge
//     long rechargeId = 1; // Replace with the actual rechargeId

//     HttpResponseMessage getRechargeResponse = await _httpClient.GetAsync($"/api/getRecharge/{rechargeId}");

//     // Assert that the response is successful
//     Assert.AreEqual(HttpStatusCode.OK, getRechargeResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestGetRechargesByUserId()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to get recharges by user ID
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);

//     // Assuming there is a user with ID 1
//     long userId = 1;

//     HttpResponseMessage getRechargesResponse = await _httpClient.GetAsync($"/api/getRechargesByUser/{userId}");

//     // Assert that the response is successful
//     Assert.AreEqual(HttpStatusCode.OK, getRechargesResponse.StatusCode);

//     // Additional assertions or validations based on your specific requirements
// }

// [Test]
// public async Task Backend_TestPutReviews()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a review
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    
//     // Add a review to update
//     var initialReviewDetails = new
//     {
//         UserId = 1,
//         Subject = "Initial Subject",
//         Body = "Initial Body",
//         Rating = 3,
//         DateCreated = DateTime.Now,
//         User = new
//         {
//             UserId = 0,
//             Email = "string",
//             Password = "string",
//             Username = "string",
//             MobileNumber = "string",
//             Role = "string"
//         }
//     };

//     string initialReviewRequestBody = JsonConvert.SerializeObject(initialReviewDetails);
//     HttpResponseMessage addReviewResponse = await _httpClient.PostAsync("/api", new StringContent(initialReviewRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, addReviewResponse.StatusCode);

//     // Get the added review details
//     string addReviewResponseBody = await addReviewResponse.Content.ReadAsStringAsync();
//     dynamic addReviewResponseMap = JsonConvert.DeserializeObject(addReviewResponseBody);

//     // Handle the potential null value for the review ID
//     int? reviewId = addReviewResponseMap?.reviewId;

//     if (reviewId.HasValue)
//     {
//         // Update the review with the correct reviewId
//         var updatedReviewDetails = new
//         {
//             ReviewId = reviewId,
//             UserId = 1,
//             Subject = "Updated Subject",
//             Body = "Updated Body",
//             Rating = 4,
//             DateCreated = DateTime.Now,
//             User = new
//             {
//                 UserId = 0,
//                 Email = "string",
//                 Password = "string",
//                 Username = "string",
//                 MobileNumber = "string",
//                 Role = "string"
//             }
//         };

//         string updateReviewRequestBody = JsonConvert.SerializeObject(updatedReviewDetails);
//         HttpResponseMessage updateReviewResponse = await _httpClient.PutAsync($"/api/{reviewId}", new StringContent(updateReviewRequestBody, Encoding.UTF8, "application/json"));

//         // Assert that the review is updated successfully
//         if (updateReviewResponse.StatusCode != HttpStatusCode.OK)
//         {
//             // Additional information about the response
//             string responseContent = await updateReviewResponse.Content.ReadAsStringAsync();
//             Console.WriteLine($"Response Content: {responseContent}");
//         }

//         Assert.AreEqual(HttpStatusCode.OK, updateReviewResponse.StatusCode);
//     }
//     else
//     {
//         // Log additional information for debugging
//         string responseContent = await addReviewResponse.Content.ReadAsStringAsync();
//         Console.WriteLine($"Add Review Response Content: {responseContent}");

//         Assert.Fail("Review ID is null or not found in the response.");
//     }
// }

// [Test]
// public async Task Backend_TestDeleteReview()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"customer\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string customerAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a review
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", customerAuthToken);
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    
//     // Add a review to delete
//     var initialReviewDetails = new
//     {
//         UserId = 1,
//         Subject = "Delete Subject",
//         Body = "Delete Body",
//         Rating = 5,
//         DateCreated = DateTime.Now,
//         User = new
//         {
//             UserId = 0,
//             Email = "string",
//             Password = "string",
//             Username = "string",
//             MobileNumber = "string",
//             Role = "string"
//         }
//     };

//     string initialReviewRequestBody = JsonConvert.SerializeObject(initialReviewDetails);
//     HttpResponseMessage addReviewResponse = await _httpClient.PostAsync("/api", new StringContent(initialReviewRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, addReviewResponse.StatusCode);

//     // Get the added review details
//     string addReviewResponseBody = await addReviewResponse.Content.ReadAsStringAsync();
//     dynamic addReviewResponseMap = JsonConvert.DeserializeObject(addReviewResponseBody);

//     // Extract the reviewId for deletion
//     int? reviewId = addReviewResponseMap?.reviewId;

//     if (reviewId.HasValue)
//     {
//         // Delete the review
//         HttpResponseMessage deleteReviewResponse = await _httpClient.DeleteAsync($"/api/{reviewId}");

//         // Assert that the review is deleted successfully
//         if (deleteReviewResponse.StatusCode != HttpStatusCode.OK)
//         {
//             // Additional information about the response
//             string responseContent = await deleteReviewResponse.Content.ReadAsStringAsync();
//             Console.WriteLine($"Response Content: {responseContent}");
//         }

//         Assert.AreEqual(HttpStatusCode.OK, deleteReviewResponse.StatusCode);
//     }
//     else
//     {
//         // Log additional information for debugging
//         string responseContent = await addReviewResponse.Content.ReadAsStringAsync();
//         Console.WriteLine($"Add Review Response Content: {responseContent}");

//         Assert.Fail("Review ID is null or not found in the response.");
//     }
// }

// [Test]
// public async Task Backend_TestPutPlan()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string adminAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a plan
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

//     var planDetails = new
//     {
//         PlanName = "Test Plan",
//         PlanPrice = 10,
//         PlanDescription = "Test plan description",
//         PlanType = "prepaid",
//         PlanValidity = "30",
//         PlanOffer = "Test Offer"
//     };

//     string planRequestBody = JsonConvert.SerializeObject(planDetails);
//     HttpResponseMessage planResponse = await _httpClient.PostAsync("api/addPlan", new StringContent(planRequestBody, Encoding.UTF8, "application/json"));
    
//     // Assert that the plan is added successfully
//     Assert.AreEqual(HttpStatusCode.OK, planResponse.StatusCode);

//     // Use the obtained token in the request to get plans
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

//     // Make a request to get all plans
//     HttpResponseMessage getPlansResponse = await _httpClient.GetAsync("api/getAllPlan");
//     Assert.AreEqual(HttpStatusCode.OK, getPlansResponse.StatusCode);

//     // Validate the response content (assuming the response is a JSON array of plans)
//     string getPlansResponseBody = await getPlansResponse.Content.ReadAsStringAsync();
//     var plans = JsonConvert.DeserializeObject<List<Plan>>(getPlansResponseBody);
//     Assert.IsNotNull(plans);
//     Assert.IsTrue(plans.Any());

//     // Update the first plan (assuming at least one plan exists)
//     long planIdToUpdate = plans.First().PlanId;

//     var updatedPlanDetails = new
//     {
//         PlanId = planIdToUpdate,
//         PlanType = "Updated Type",
//         PlanName = "Updated Name",
//         PlanValidity = "Updated Validity",
//         PlanOffer = "Updated Offer",
//         PlanDescription = "Updated Description",
//         PlanPrice = 20
//     };

//     string updatePlanRequestBody = JsonConvert.SerializeObject(updatedPlanDetails);
//     HttpResponseMessage updatePlanResponse = await _httpClient.PutAsync($"api/editPlan/{planIdToUpdate}", new StringContent(updatePlanRequestBody, Encoding.UTF8, "application/json"));

//     // Assert that the plan is updated successfully
//     if (updatePlanResponse.StatusCode != HttpStatusCode.OK)
//     {
//         // Additional information about the response
//         string responseContent = await updatePlanResponse.Content.ReadAsStringAsync();
//         Console.WriteLine($"Response Content: {responseContent}");

//         // Attempt to deserialize the response for further analysis
//         try
//         {
//             var deserializedResponse = JsonConvert.DeserializeObject(responseContent);
//             Console.WriteLine($"Deserialized Response: {deserializedResponse}");
//         }
//         catch (JsonReaderException jsonException)
//         {
//             Console.WriteLine($"JSON Deserialization Exception: {jsonException.Message}");
//         }
//     }

//     Assert.AreEqual(HttpStatusCode.OK, updatePlanResponse.StatusCode);
// }

// [Test]
// public async Task Backend_TestDeletePlan()
// {
//     // Generate unique identifiers
//     string uniqueId = Guid.NewGuid().ToString();
//     string uniqueusername = $"abcd_{uniqueId}";
//     string uniquepassword = $"abcdA{uniqueId}@123";
//     string uniqueEmail = $"abcd{uniqueId}@gmail.com";

//     // Register a customer
//     string registerRequestBody = $"{{\"Username\": \"{uniqueusername}\", \"Password\": \"{uniquepassword}\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\",\"Role\" : \"admin\" }}";
//     HttpResponseMessage registerResponse = await _httpClient.PostAsync("/api/register", new StringContent(registerRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, registerResponse.StatusCode);

//     // Login the registered customer
//     string loginRequestBody = $"{{\"email\": \"{uniqueEmail}\",\"password\": \"{uniquepassword}\"}}";
//     HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));
//     Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
//     string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//     dynamic loginResponseMap = JsonConvert.DeserializeObject(loginResponseBody);
//     string adminAuthToken = loginResponseMap.token;

//     // Use the obtained token in the request to add a plan
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

//     var planDetails = new
//     {
//         PlanName = "Test Plan",
//         PlanPrice = 10,
//         PlanDescription = "Test plan description",
//         PlanType = "prepaid",
//         PlanValidity = "30",
//         PlanOffer = "Test Offer"
//     };

//     string planRequestBody = JsonConvert.SerializeObject(planDetails);
//     HttpResponseMessage planResponse = await _httpClient.PostAsync("api/addPlan", new StringContent(planRequestBody, Encoding.UTF8, "application/json"));
    
//     // Assert that the plan is added successfully
//     Assert.AreEqual(HttpStatusCode.OK, planResponse.StatusCode);

//     // Use the obtained token in the request to get plans
//     _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminAuthToken);

//     // Make a request to get all plans
//     HttpResponseMessage getPlansResponse = await _httpClient.GetAsync("api/getAllPlan");
//     Assert.AreEqual(HttpStatusCode.OK, getPlansResponse.StatusCode);

//     // Validate the response content (assuming the response is a JSON array of plans)
//     string getPlansResponseBody = await getPlansResponse.Content.ReadAsStringAsync();
//     var plans = JsonConvert.DeserializeObject<List<Plan>>(getPlansResponseBody);
//     Assert.IsNotNull(plans);
//     Assert.IsTrue(plans.Any());

//     // Delete the first plan (assuming at least one plan exists)
//     long planIdToDelete = plans.First().PlanId;

//     HttpResponseMessage deletePlanResponse = await _httpClient.DeleteAsync($"api/deletePlan/{planIdToDelete}");

//     // Assert that the plan is deleted successfully
//     if (deletePlanResponse.StatusCode != HttpStatusCode.OK)
//     {
//         // Additional information about the response
//         string responseContent = await deletePlanResponse.Content.ReadAsStringAsync();
//         Console.WriteLine($"Response Content: {responseContent}");

//         // Attempt to deserialize the response for further analysis
//         try
//         {
//             var deserializedResponse = JsonConvert.DeserializeObject(responseContent);
//             Console.WriteLine($"Deserialized Response: {deserializedResponse}");
//         }
//         catch (JsonReaderException jsonException)
//         {
//             Console.WriteLine($"JSON Deserialization Exception: {jsonException.Message}");
//         }
//     }

//     Assert.AreEqual(HttpStatusCode.OK, deletePlanResponse.StatusCode);
// }

}
