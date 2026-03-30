using Microsoft.Playwright;
using Base;

public abstract class BaseTest
{
    protected IPlaywright Playwright;
    protected IBrowser Browser;
    protected IPage Page;

    [SetUp]
    public async Task BaseSetUp()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = Config.HeadlessMode,
            SlowMo = Config.SlowMo
        });
        var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        });
        Page = await context.NewPageAsync();
        Page.SetDefaultTimeout(Config.Timeout);
    }

    protected async Task TakeScreenshotAsync(string name)
    {
        var screenshotPath = $"screenshot_{name}_{DateTime.Now:yyyyMMddHHmmss}.png";
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = screenshotPath,
            FullPage = true
        });
        TestContext.AddTestAttachment(screenshotPath);
    }
}
