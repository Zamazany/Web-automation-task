using Microsoft.Playwright;
using Speeron.SpeeronPage;

namespace Speeron.Pages
{
    public class GuestPortalPage(IPage page, int timeout) : SpeeronBasePage(page, timeout)
    {
        /// <summary>
            /// Executes the 'NavigateToGuestPortalAsync' action.
        /// </summary>
        public async Task NavigateToGuestPortalAsync()
        {
            string url = $"{Config.BaseUrl}/guest-portal";
            TestContext.WriteLine($"[INFO] Navigating to: {url}");
            await _page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = _timeout
            });
            await WaitForUrlContainsAsync("guest-portal");
        }

        /// <summary>
            /// We add methods that map names used in tests to methods from the base class.
        /// </summary>
        public async Task ClickMenuOptionByTextAsync(string text)
        {
            await ClickOptionByTextAsync(text);
        }

        /// <summary>
            /// Executes the 'ClickMenuOptionByLocatorAsync' action.
        /// </summary>
        public async Task ClickMenuOptionByLocatorAsync(string locator)
        {
            await ClickOptionByLocatorAsync(locator);
        }

        /// <summary>
        /// Executes the 'ClickMenuOptionByTestIdOrTextAsync' action.
        /// </summary>
        public async Task ClickMenuOptionByTestIdOrTextAsync(string testId, string fallbackText)
        {
            var testIdLocator = $"[data-test-id='{testId}']";
            var locator = _page.Locator(testIdLocator);
            if (await locator.CountAsync() > 0)
            {
                TestContext.WriteLine($"[INFO] Clicking by data-test-id: {testId}");
                await locator.First.ClickAsync();
            }
            else
            {
                TestContext.WriteLine($"[WARN] data-test-id '{testId}' not found. Falling back to text: {fallbackText}");
                await _page.GetByText(fallbackText).ClickAsync();
            }
        }

    }
}