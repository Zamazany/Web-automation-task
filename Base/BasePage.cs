using Microsoft.Playwright;

namespace Base.BasePage
{
    public abstract class BasePage(IPage page, int timeout)
    {
        protected readonly IPage _page = page;
        protected readonly int _timeout = timeout;

        /// <summary>
            /// Executes the 'LoginAsync' action.
        /// </summary>
        public async Task LoginAsync(string token)
        {
            string loginUrl = $"{Config.BaseUrl}?token={token}";
            TestContext.WriteLine($"[INFO] Logging in via URL: {loginUrl}");
            await _page.GotoAsync(loginUrl, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = _timeout
            });
        }

        /// <summary>
            /// Executes the 'ClickOptionByTextAsync' action.
        /// </summary>
        public async Task ClickOptionByTextAsync(string text)
        {
            TestContext.WriteLine($"[INFO] Clicking option by text: {text}");
            await _page.GetByText(text).ClickAsync();
        }

        /// <summary>
            /// Executes the 'ClickOptionByLocatorAsync' action.
        /// </summary>
        public async Task ClickOptionByLocatorAsync(string locator)
        {
            TestContext.WriteLine($"[INFO] Clicking option by locator: {locator}");
            await _page.Locator(locator).ClickAsync();
        }

        /// <summary>
            /// Retry mechanism - tries up to maxRetry times waiting for the expected URL.
        /// </summary>
        public async Task WaitForUrlContainsAsync(string expectedPart, int maxRetry = 3)
        {
            int attempts = 0;
            Exception? lastEx = null;

            while (attempts < maxRetry)
            {
                try
                {
                    await _page.WaitForURLAsync(url => url.Contains(expectedPart), new PageWaitForURLOptions { Timeout = _timeout });
                    string currentUrl = _page.Url;
                    TestContext.WriteLine($"[INFO] Current URL: {currentUrl}");
                    if (!currentUrl.Contains(expectedPart))
                        throw new Exception($"Current URL does not contain expected part: {expectedPart}");
                    return;
                }
                catch (Exception ex)
                {
                    attempts++;
                    lastEx = ex;
                    TestContext.WriteLine($"[WARN] Attempt {attempts}/{maxRetry} failed waiting for URL containing '{expectedPart}'. Exception: {ex.Message}");
                    await Task.Delay(1000);
                }
            }
            throw new Exception($"WaitForUrlContainsAsync: URL did not contain '{expectedPart}' after {maxRetry} attempts. Last error: {lastEx?.Message}");
        }
    }
}
