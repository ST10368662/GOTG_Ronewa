using Microsoft.Playwright.MSTest;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting; // Required for [TestClass] and [TestMethod]
using System.Threading.Tasks;

// FIX: Correctly placing the class within the specified namespace
namespace GOTG_Ronewa.Tests
{
    // Playwright uses PageTest as a base class to automatically manage the browser instance (Page).
    [TestClass]
    public class IncidentReportUITests : PageTest
    {
        // Define the base URL for the running application
        // IMPORTANT: Ensure your application is running before executing these tests.
        private const string BaseUrl = "http://localhost:5000";

        [TestMethod]
        public async Task SubmitIncidentReport_ShouldSucceedAndShowConfirmation()
        {
            // ARRANGE: Navigate to the Incident Report page
            await Page.GotoAsync($"{BaseUrl}/IncidentReports/Create");

            // ACT 1: Fill the form fields (using assumed IDs/names from your HTML)
            await Page.FillAsync("#Title", "Suspicious Activity Report");
            await Page.FillAsync("#Location", "Old Water Tower, Zone B");
            await Page.FillAsync("#Description", "Saw unauthorized personnel near the asset.");
            await Page.FillAsync("#Phone", "083-456-7890");

            // ACT 2: Click the submit button
            await Page.ClickAsync("text=Submit Report"); // Clicks button with this text

            // ASSERT 1: Verify the URL changed, indicating a successful form submission (e.g., redirect to Index)
            await Page.WaitForURLAsync($"{BaseUrl}/IncidentReports");

            // ASSERT 2: Verify a success message is displayed (Adjust the text/selector to match your actual application)
            var confirmationMessageVisible = await Page.Locator("text='Report submitted successfully'").IsVisibleAsync();

            Assert.IsTrue(confirmationMessageVisible, "The confirmation message was not visible after submission.");

            // Optional Check: Verify the new entry appears in the list
            var newEntryTitleVisible = await Page.Locator("text=Suspicious Activity Report").First.IsVisibleAsync();
            Assert.IsTrue(newEntryTitleVisible, "The newly created incident report title was not found on the list page.");
        }

        [TestMethod]
        public async Task SubmitIncidentReport_MissingRequiredFields_ShowsValidationErrors()
        {
            // ARRANGE: Navigate to the Incident Report page
            await Page.GotoAsync($"{BaseUrl}/IncidentReports/Create");

            // ACT 1: Fill fields, but deliberately leave the required Title field blank
            await Page.FillAsync("#Description", "Partial data entry for validation test.");
            await Page.FillAsync("#Phone", "0800-111-222");
            // Leave #Title blank

            // ACT 2: Click the submit button
            await Page.ClickAsync("text=Submit Report");

            // ASSERT 1: Verify the URL did NOT change (it stayed on the Create page)
            StringAssert.Contains(Page.Url, "/Create", "The page redirected unexpectedly, validation failed.");

            // ASSERT 2: Verify the ASP.NET Core validation error message for the missing field is visible
            // NOTE: You MUST inspect your HTML to ensure this selector (e.g., #Title-error) is correct
            var validationErrorVisible = await Page.Locator("text='The Title field is required.'").IsVisibleAsync();
            Assert.IsTrue(validationErrorVisible, "Validation error for the required Title field was not displayed.");
        }
    }
}