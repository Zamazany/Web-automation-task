using Speeron.Pages;

namespace Base.BasePage.Tests
{
    [TestFixture]
    public class GuestPortalNavigationTests : BaseTest
    {

        private GuestPortalPage _guestPortalPage;

        [SetUp]
        /// <summary>
            /// Executes the 'SetupTest' action.
        /// </summary>
        public async Task SetupTest()
        {
            _guestPortalPage = new GuestPortalPage(Page, Config.Timeout);
            try
            {
                await _guestPortalPage.LoginAsync(Config.AuthToken);
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"[ERROR] Login failed: {ex.Message}");
                throw;
            }
        }
        
        [Test]
        /// <summary>
            /// Executes the 'Should_Navigate_Through_Guest_Portal_Sections' action.
        /// </summary>
        public async Task E2E_002_Should_Navigate_Through_Guest_Portal_Sections()
        {
            try
            {

                /// Navigation to Guest Portal
                await _guestPortalPage.NavigateToGuestPortalAsync();

                /// Click on "Guest Portal" - we assume it takes you to the Food and drinks section
                await _guestPortalPage.ClickMenuOptionByTextAsync("Guest Portal");
                await _guestPortalPage.WaitForUrlContainsAsync("guest-portal/food-and-drinks");

                /// Map of menu options and expected URL fragments
                var menuOptions = new List<(string TestId, string FallbackText, string ExpectedUrl)>
                {
                    ("food-and-drinks-submenu", "Food and drinks", "food-and-drinks"),
                    ("shops-submenu", "Shops", "shops"),
                    ("services-submenu", "Services", "services"),
                    ("templates-submenu", "Templates", "templates"),
                    ("static-pages-submenu", "Pages", "static-pages"),
                    ("menu-editor-submenu", "Menu", "menu-editor"),
                    ("orders-submenu", "Orders", "orders"),
                    ("statistics-submenu", "Statistics", "statistics"),
                    ("theme-editor-submenu", "Theme editor", "theme-editor")
                };

                /// Iterate through menu options
                foreach (var (testId, fallbackText, expectedUrl) in menuOptions)
                {
                    await _guestPortalPage.ClickMenuOptionByTestIdOrTextAsync(testId, fallbackText);
                    await _guestPortalPage.WaitForUrlContainsAsync(expectedUrl);
                    TestContext.WriteLine($"[PASS] Menu option '{fallbackText}' navigated to URL containing '{expectedUrl}'");
                }
            }
            catch (Exception ex)
            {
                await TakeScreenshotAsync("NavigationError");
                throw new Exception($"Navigation test failed: {ex.Message}");
            }
        }
    }
}
