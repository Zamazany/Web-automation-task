using Speeron.Pages;

namespace Speeron.Tests
{
    [TestFixture]
    public class MenuEditorTests : BaseTest
    {
        private MenuEditorPage _menuEditorPage;
        private ThemeEditorPage _themeEditorPage;
        private const string Label = "test_001";
        private const string ImagePath = "Images/kot.jpg";

        [SetUp]
        /// <summary>
            /// Executes the 'SetupTest' action.
        /// </summary>
        public async Task SetupTest()
        {
            _menuEditorPage = new MenuEditorPage(Page, Config.Timeout);
            _themeEditorPage = new ThemeEditorPage(Page, Config.Timeout);

            await _menuEditorPage.LoginAsync(Config.AuthToken);
        }

        [TestCase(Label,"pl", ImagePath)]
        /// <summary>
            /// Executes the 'E2E_001_Should_Display_Added_Menu_Item_In_Tv_And_Mobile_Preview' action.
        /// </summary>
        public async Task E2E_001_Should_Display_Added_Menu_Item_In_Tv_And_Mobile_Preview(string label, string language, string imagePath)
        {
            try
            {
                /// 1. Add an item in the Menu Editor
                await _menuEditorPage.NavigateToMenuEditorAsync();
                await _menuEditorPage.OpenNewMenuItemDialogAsync();
                await _menuEditorPage.FillMenuItemDialogAsync(label, "en", imagePath);
                await _menuEditorPage.SwitchLanguageTabAsync(language);
                await _menuEditorPage.FillMenuItemDialogAsync(label, language, imagePath);
                await _menuEditorPage.SubmitAddMenuItemDialogAsync();

                /// 2. Check if the element was added correctly
                await _menuEditorPage.AssertMenuItemExistsAsync(label);


                /// 3. Go to Theme Editor and verify if title exist in TV View
                await _themeEditorPage.NavigateToThemeEditorAsync();
                await _themeEditorPage.AssertMenuTileExistsInTvViewAsync(label);


                /// 3. Go to Theme Editor and verify if title exist in Mobile view
                await _themeEditorPage.SwitchToMobileViewAsync();
                await _themeEditorPage.AssertMenuTileExistsInMobileViewAsync(label);

            }
            catch (System.Exception ex)
            {
                await TakeScreenshotAsync("ThemeEditorTestError");
                throw new System.Exception($"Test failed: {ex.Message}");
            }
            finally
            {
                try
                {
                    TestContext.WriteLine("[CLEANUP] Attempting to delete test menu item...");

                    await _menuEditorPage.NavigateToMenuEditorAsync();
                    await _menuEditorPage.DeleteMenuItemAsync(label);
                }
                catch (Exception cleanupEx)
                {
                    TestContext.WriteLine($"[CLEANUP WARNING] Failed to delete test item: {cleanupEx.Message}");
                }
            }
        }
    }
}