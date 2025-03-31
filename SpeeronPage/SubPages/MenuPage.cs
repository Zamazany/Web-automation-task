using Microsoft.Playwright;
using Speeron.SpeeronPage;

namespace Speeron.Pages
{
    public class MenuEditorPage(IPage page, int timeout) : SpeeronBasePage(page, timeout)
    {
        
        /// <summary>
            /// Executes the 'NavigateToMenuEditorAsync' action.
        /// </summary>
        public async Task NavigateToMenuEditorAsync()
        {
            string url = $"{Config.BaseUrl}/guest-portal/menu-editor";
            TestContext.WriteLine($"[INFO] Navigating to: {url}");
            await _page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = _timeout
            });
            await WaitForUrlContainsAsync("menu-editor");
        }

        /// <summary>
            /// Opens the "New menu item" dialog using the given selector
        /// </summary>
        public async Task OpenNewMenuItemDialogAsync()
        {
            TestContext.WriteLine("[INFO] Clicking 'Add menu item' button");
            var addButton = _page.Locator("[data-test-id='action-panel-add-button']");
            await addButton.ClickAsync();
            await _page.WaitForSelectorAsync("#name-input", new() { Timeout = _timeout });
        }

        /// <summary>
            /// Fills the Label field and selects the first Theme color
        /// </summary>
        public async Task FillMenuItemDialogAsync(string label, string language, string imagePath)
        {

            /// Specify the input index: for "en" it is 0, for "pl" it is 1
            int index = GetLanguageInputIndex(language);

            /// Get input by index
            var input = await GetNameInputByIndexAsync(index);

            await FillAndVerifyAsync(input, label);

            if (language == "en")
            {
                await SelectThemeColorAsync();
                await UploadImageBySelectorAsync(imagePath, "#tile-image-selector");
                await UploadImageBySelectorAsync(imagePath, "#background-image-selector");
            }
        }


        /// <summary>
             /// Returns the index of the input based on the language.
        /// </summary>
        private static int GetLanguageInputIndex(string language)
        {
            return language == "pl" ? 1 : 0;
        }


        /// <summary>
            /// Gets a specific input with ID #name-input based on index.
        /// </summary>
        private async Task<ILocator> GetNameInputByIndexAsync(int index)
        {
            var inputs = _page.Locator("#name-input");
            int count = await inputs.CountAsync();

            if (count <= index)
                throw new Exception($"Expected at least {index + 1} #name-input elements, but found {count}");

            return inputs.Nth(index);
        }

        /// <summary>
            /// Enters text into input and checks if it is set correctly.
        /// </summary>
        private static async Task FillAndVerifyAsync(ILocator input, string expectedValue)
        {
            await input.FillAsync(expectedValue);

            var currentValue = await input.InputValueAsync();
            if (currentValue != expectedValue)
            {
                TestContext.WriteLine($"[WARN] Input not updated. Retrying...");
                await input.FillAsync(expectedValue);
            }
        }


        /// <param name="colorIndex">Color index (0-based). Defaults to 1 = second color.</param>
        /// <summary>
            /// Opens the theme dropdown and selects a color by index (0-based).
        /// </summary>
        public async Task SelectThemeColorAsync(int colorIndex = 1)
        {
            TestContext.WriteLine("[INFO] Clicking #menu-theme");
            await _page.ClickAsync("#menu-theme");

            var allOptions = _page.Locator("div[data-test-id='theme-color-option']");
            int count = await allOptions.CountAsync();

            if (colorIndex >= count)
                throw new Exception($"Theme color index {colorIndex} is out of range. Only {count} options found.");

            TestContext.WriteLine($"[INFO] Selecting Theme color option at index {colorIndex}");
            await allOptions.Nth(colorIndex).ClickAsync();
        }

        /// <param name="langCode">Kod języka (np. "pl")</param>
        /// <summary>
            /// Toggles the language tab in the modal based on the language code (e.g. "pl", "en").
        /// </summary>
        public async Task SwitchLanguageTabAsync(string langCode)
        {
            TestContext.WriteLine($"[INFO] Switching language tab to '{langCode}'");

            string langTabSelector = $"div[data-test-id='language-tab'][data-lang-code='{langCode}']";
            var langTab = _page.Locator(langTabSelector);

            /// // Click on the language tab
            await langTab.ClickAsync();


            /// Wait until the next #name-input field is visible
            await _page.WaitForFunctionAsync(@"(langCode) => {
        const inputs = Array.from(document.querySelectorAll('#name-input'));
        return inputs.length > 1 && inputs.some(i => i.offsetParent !== null);
    }", langCode);

            TestContext.WriteLine($"[INFO] Language tab '{langCode}' is now active.");
        }



        /// <summary>
            /// Clicking the "Add" button in the dialog box
        /// </summary>
        public async Task SubmitAddMenuItemDialogAsync()
        {
            TestContext.WriteLine("[INFO] Submitting the new menu item");

            var addButton = _page.Locator("[data-test-id='form-add-or-update-button']");

            await addButton.ClickAsync();

            /// We wait until the dialog box disappears
            await _page.WaitForSelectorAsync("modal-container", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = _timeout
            });

            TestContext.WriteLine("[INFO] Add button clicked and modal closed successfully.");
        }

        /// <summary>
            /// Executes the 'AssertMenuItemExistsAsync' action.
        /// </summary>
        public async Task AssertMenuItemExistsAsync(string label)
        {
            TestContext.WriteLine($"[INFO] Verifying if menu item '{label}' exists in the list...");

            var itemLocator = _page.Locator($"div.list-group.list-group-fluid >> text={label}");

            try
            {
                await itemLocator.WaitForAsync(new LocatorWaitForOptions
                {
                    Timeout = _timeout,
                    State = WaitForSelectorState.Visible
                });

                bool isVisible = await itemLocator.IsVisibleAsync();
                Assert.That(isVisible, Is.True);

                TestContext.WriteLine($"[PASS] Menu item '{label}' found in the list.");
            }
            catch (TimeoutException)
            {
                string screenshotPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"menu_item_not_found_{label}_{DateTime.Now:yyyyMMddHHmmss}.png");
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
                TestContext.AddTestAttachment(screenshotPath);
                throw new AssertionException($"[FAIL] Menu item '{label}' was not found in the menu list.");
            }
        }


        /// <summary>
            /// Executes the 'DeleteMenuItemAsync' action.
        /// </summary>
        public async Task DeleteMenuItemAsync(string label)
        {
            TestContext.WriteLine($"[INFO] Attempting to delete menu item: {label}");

            /// Find the container with the list
            var listContainer = _page.Locator("div.list-group.list-group-fluid");

            /// Find the element containing the given label
            var item = listContainer.Locator($"text={label}").First;

            /// Make sure the item is visible
            await item.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = _timeout
            });

            /// Go to parent row item that contains buttons
            var row = item.Locator("xpath=ancestor::div[contains(@class, 'list-group-item')]");

            /// Locate the trash bin in this line
            var deleteButton = row.Locator("app-button-icon:nth-child(2) button");

            TestContext.WriteLine("[INFO] Clicking delete icon...");
            await deleteButton.ClickAsync();

            /// Confirm deletion in modal window
            var confirmDeleteButton = _page.Locator("modal-container button.btn.btn-danger");
            await confirmDeleteButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = _timeout
            });

            TestContext.WriteLine("[INFO] Confirming deletion...");
            await confirmDeleteButton.ClickAsync();

            /// Wait until the item disappears from the list
            await item.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = _timeout
            });

            TestContext.WriteLine($"[PASS] Menu item '{label}' was successfully deleted.");
        }

        /// <summary>
            /// Executes the 'UploadImageBySelectorAsync' action.
        /// </summary>
        public async Task UploadImageBySelectorAsync(string imagePath, string fileInputSelector)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Image not found at path: {imagePath}");
            }

            string type = fileInputSelector.Contains("background") ? "Background image" : "Tile image";
            TestContext.WriteLine($"[INFO] Uploading {type} from: {imagePath}");

            var fileInput = _page.Locator($"input[type='file']{fileInputSelector}");
            await fileInput.SetInputFilesAsync(imagePath);

            /// We select the "Upload" button in the modal
            var uploadButton = _page.Locator("button[data-test-id='form-upload-button']");

            await uploadButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = _timeout
            });

            TestContext.WriteLine("[INFO] Clicking Upload in image modal");
            await uploadButton.ClickAsync();

            await uploadButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = _timeout
            });

            TestContext.WriteLine($"[INFO] {type} uploaded successfully.");
        }

    }
}