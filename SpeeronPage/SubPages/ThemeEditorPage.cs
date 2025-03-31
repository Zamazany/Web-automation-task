using Microsoft.Playwright;
using Speeron.SpeeronPage;


namespace Speeron.Pages
{
    public partial class ThemeEditorPage(IPage page, int timeout) : SpeeronBasePage(page, timeout)
    {
        /// <summary>
            /// Executes the 'NavigateToThemeEditorAsync' action.
        /// </summary>
        public async Task NavigateToThemeEditorAsync()
        {
            string url = $"{Config.BaseUrl}/guest-portal/theme-editor";
            TestContext.WriteLine($"[INFO] Navigating to: {url}");

            await _page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = _timeout
            });

            await CompleteLanguageSelectionViaIframeAsync();

            await WaitForUrlContainsAsync("theme-editor");
        }

        /// <summary>
            /// Executes the 'AssertMenuTileExistsInTvViewAsync' action.
        /// </summary>
        //TODO: Add validation of background image
        public async Task AssertMenuTileExistsInTvViewAsync(string label)
        {
            TestContext.WriteLine($"[INFO] Verifying tile '{label}' inside TV preview iframe.");

            var frame = _page.FrameLocator("#tv-preview-iframe");
            var allTiles = frame.Locator("app-tv-menu-tile-entry");

            int tileCount = await allTiles.CountAsync();
            TestContext.WriteLine($"[DEBUG] Found {tileCount} tile(s) in TV preview.");

            for (int i = 0; i < tileCount; i++)
            {
                var tile = allTiles.Nth(i);
                string innerText = await tile.InnerTextAsync();

                if (innerText.Contains(label))
                {
                    TestContext.WriteLine($"[PASS] Found tile with label '{label}' at index {i}.");

                    string styleAttr = await tile.GetAttributeAsync("style") ?? "";
                    var match = Regex.Match(styleAttr, @"--tile-image:\s*url\(([^)]+)\)", RegexOptions.IgnoreCase);

                    if (!match.Success || string.IsNullOrWhiteSpace(match.Groups[1].Value))
                    {
                        throw new AssertionException($"[FAIL] Tile '{label}' found, but it does not contain a valid image (style: '{styleAttr}').");
                    }

                    TestContext.WriteLine($"[PASS] Tile '{label}' has an uploaded image: {match.Groups[1].Value}");
                    return;
                }
            }

            throw new AssertionException($"❌ Tile with label '{label}' was NOT found in TV preview after checking {tileCount} tiles.");
        }


        /// <summary>
            /// Executes the 'SwitchToMobileViewAsync' action.
        /// </summary>
        public async Task SwitchToMobileViewAsync()
        {
            TestContext.WriteLine("[INFO] Switching to Mobile view");

            /// Click the "Mobile" button
            var mobileToggle = _page.Locator("button.btn.btn-outline-secondary.preview-toggle:has-text('Mobile')");
            await mobileToggle.ClickAsync();

            /// Wait for the mobile iframe to load
            var mobileFrame = _page.FrameLocator("#mobile-preview-iframe");

            /// Wait until some tile element appears inside the iframe
            var tileEntry = mobileFrame.Locator("app-mobile-menu-tile-entry");
            await tileEntry.First.WaitForAsync(new LocatorWaitForOptions
            {
                Timeout = _timeout,
                State = WaitForSelectorState.Visible
            });

            TestContext.WriteLine("[INFO] Mobile preview is ready.");
        }


        /// <summary>
            /// Executes the 'AssertMenuTileExistsInMobileViewAsync' action.
        /// </summary>
        public async Task AssertMenuTileExistsInMobileViewAsync(string label)
        {
            TestContext.WriteLine($"[INFO] Verifying tile '{label}' inside Mobile preview iframe.");

            var frame = _page.FrameLocator("#mobile-preview-iframe");
            var tile = frame.Locator($"app-mobile-menu-tile-entry:has-text('{label}')");

            try
            {
                await tile.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = _timeout
                });

                bool isVisible = await tile.IsVisibleAsync();
                Assert.That(isVisible, Is.True, $"Mobile tile '{label}' is not visible.");

                /// Checking for image presence using regex
                string style = await tile.GetAttributeAsync("style") ?? "";
                var match = ImageUrlRegex().Match(style);

                if (!match.Success || string.IsNullOrWhiteSpace(match.Groups[1].Value))
                {
                    throw new AssertionException($"[FAIL] Mobile tile '{label}' does not contain an uploaded image (style: '{style}').");
                }

                TestContext.WriteLine($"[PASS] Mobile tile '{label}' is visible and has an image in the Mobile preview: {match.Groups[1].Value}");
            }
            catch (TimeoutException)
            {
                string screenshotPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"mobile_tile_not_found_{label}_{DateTime.Now:yyyyMMddHHmmss}.png");

                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                TestContext.AddTestAttachment(screenshotPath);

                TestContext.WriteLine($"[FAIL] Mobile tile '{label}' was NOT found in Mobile preview.");
                throw new AssertionException($"[FAIL] Mobile tile '{label}' was NOT found in Mobile preview within {_timeout / 1000} seconds.");
            }
        }


        /// <summary>
            /// Executes the 'CompleteLanguageSelectionViaIframeAsync' action.
        /// </summary>
        public async Task CompleteLanguageSelectionViaIframeAsync()
        {
            TestContext.WriteLine("[INFO] Starting language selection sequence inside iframe.");
            await Task.Delay(1000);
            /// 1. Go to iframe
            var frameLocator = _page.FrameLocator("#tv-preview-iframe");

            /// 2. Press "Next" button
            var firstNextButton = frameLocator.Locator("[data-test-id=\"first-next-button\"]");
            await firstNextButton.ClickAsync();
            await Task.Delay(200);

            /// 3. Down Arrow + Enter (language selection)
            await firstNextButton.PressAsync("ArrowDown");
            await Task.Delay(200);
            await firstNextButton.PressAsync("Enter");
            await Task.Delay(500);

            /// 4. Welcome screen confirmation - second Enter
            var summaryBody = frameLocator.Locator("body");
            await summaryBody.PressAsync("Enter");

            TestContext.WriteLine("[INFO] Language selection completed via iframe sequence.");
        }

        [GeneratedRegex(@"--tile-image:\s*url\(([^)]+)\)", RegexOptions.IgnoreCase, "pl-PL")]
        private static partial Regex ImageUrlRegex();
    }
}