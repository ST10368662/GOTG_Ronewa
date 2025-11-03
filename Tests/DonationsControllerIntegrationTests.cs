using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
// CRITICAL FIX 3: Add the correct using statement for WebApplicationFactory
using Microsoft.AspNetCore.Mvc.Testing;
using GOTG.Ronewa.Web.Models;
using System.Text;
using System.Text.Json;
using System.Linq;

// CRITICAL FIX 4: Use the correct class for the IClassFixture
// If you added the TestingProgram.cs file, use that name instead of Program.
public class DonationsControllerIntegrationTests : IClassFixture<WebApplicationFactory<GOTG.Ronewa.Web.Program>>
{
    private readonly WebApplicationFactory<GOTG.Ronewa.Web.Program> _factory;
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public DonationsControllerIntegrationTests(WebApplicationFactory<GOTG.Ronewa.Web.Program> factory)
    {
        _factory = factory;
        // The factory should be configured here if you need a clean DB for each test, 
        // but for now, we use the default client.
        _client = _factory.CreateClient();
    }

    // --- Integration Test Scenarios ---

    [Fact]
    public async Task Post_Donation_ReturnsCreatedAndCreatesRecord()
    {
        // ARRANGE
        var newDonation = new Donation
        {
            DonorName = "Integration Test Donor",
            ResourceType = "Medical Supplies",
            Quantity = 50,
            DonatedAt = System.DateTime.Now
        };
        var json = JsonSerializer.Serialize(newDonation);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // ACT: Assumed endpoint is /Donations/Create
        var response = await _client.PostAsync("/Donations/Create", content);

        // ASSERT 1: Status Code
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode); // NOTE: Changed to OK/200 if your controller returns View/IActionResult(model)

        // ASSERT 2: Check returned data (if the controller redirects, you might skip this)
        var responseString = await response.Content.ReadAsStringAsync();
        // CRITICAL FIX 5: Handling the nullable response string with '!' is safe here.
        var createdDonation = JsonSerializer.Deserialize<Donation>(responseString!, _jsonOptions);

        Assert.NotNull(createdDonation);
        Assert.True(createdDonation.Id > 0);
    }

    [Fact]
    public async Task Get_AllDonations_ReturnsOkAndList()
    {
        // ARRANGE: Post a donation first to ensure the DB is not empty.
        var setupDonation = new Donation { DonorName = "Setup", ResourceType = "Fuel", Quantity = 1, DonatedAt = System.DateTime.Now };
        var setupContent = new StringContent(JsonSerializer.Serialize(setupDonation), Encoding.UTF8, "application/json");
        await _client.PostAsync("/Donations/Create", setupContent);

        // ACT: Send the GET request to the index endpoint.
        var response = await _client.GetAsync("/Donations");

        // ASSERT 1
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // ASSERT 2: Check the returned content.
        var responseString = await response.Content.ReadAsStringAsync();
        // CRITICAL FIX 6: Handling the nullable response string.
        var donations = JsonSerializer.Deserialize<Donation[]>(responseString!, _jsonOptions);

        Assert.NotNull(donations);
        Assert.True(donations.Length >= 1);
        // CRITICAL FIX 7: Use Assert.Contains with Linq for collection checking.
        Assert.Contains(donations, d => d.DonorName == "Setup");
    }

    [Fact]
    public async Task Post_InvalidDonation_ReturnsBadRequest()
    {
        // ARRANGE: Donation with missing required fields (e.g., ResourceType is not set in model)
        var invalidDonation = new Donation
        {
            DonorName = "Fails Validation",
            // ResourceType and Phone are required strings but are often non-nullable in C#
            // To test validation failure, we assume the API model state check will catch it.
            Quantity = 100,
            DonatedAt = System.DateTime.Now
        };

        // Serialize and send the object, which is missing a required property if the controller uses [FromBody]
        var content = new StringContent(JsonSerializer.Serialize(invalidDonation), Encoding.UTF8, "application/json");

        // ACT
        var response = await _client.PostAsync("/Donations/Create", content);

        // ASSERT: Check that the API correctly handles the invalid model state.
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}