using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserSessionSave
{

    public IPage? Page { get; private set; }
    private IBrowser browser;

    [SetUp]
    public async Task LoginSetup()
    {
        var playwrightDriver = await Playwright.CreateAsync();
        browser = await playwrightDriver.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        var storagePath = "../../../playwright/.auth/state.json";
        var fileInfo = new FileInfo(storagePath);

        if (!fileInfo.Exists)
        {
            Directory.CreateDirectory(fileInfo.Directory.FullName);
            using (FileStream fs = File.Create(storagePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("");
                fs.Write(info, 0, info.Length);
            }
            Console.WriteLine($"File '{storagePath}' created successfully.");
        }
        else
        {
            Console.WriteLine($"File '{storagePath}' already exists.");
        }

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1890, Height = 940 },
            StorageStatePath = storagePath
        });

        Page = await context.NewPageAsync();

        await Page.GotoAsync("https://courses.ultimateqa.com/enrollments");
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        if (Page.Url.EndsWith("users/sign_in"))
        {
            await Page.GetByPlaceholder("Email").FillAsync("yustas.mailfortest@gmail.com");
            await Page.GetByPlaceholder("Password").FillAsync("~Test123");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            await Assertions.Expect(Page.GetByRole(AriaRole.Link, new() { Name = "My Dashboard" })).ToBeVisibleAsync();
            await context.StorageStateAsync(new() { Path = storagePath });
        }
    }

    [Test]
    public async Task SeleniumSearch()
    {
        // 4.клік на "View more courses"
        await Page.GetByRole(AriaRole.Link, new() { Name = "View more courses" }).ClickAsync();
        // 5.в полі Search вводив "Selenium" та натиск клавіші Enter
        await Page.GetByPlaceholder("Search").FillAsync("Selenium");
        await Page.GetByPlaceholder("Search").PressAsync("Enter");
        // 6.перевіряв що відобразились курси які мають в назві Selenium
        await Assertions.Expect(Page.GetByText("Selenium", new() { Exact = true })).ToBeVisibleAsync();
        await Assertions.Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Complete Selenium WebDriver with Java Bootcamp Course star 5.0 average rating (8 reviews) $" })).ToBeVisibleAsync();
        // 7.клік на "My Dashboard"
        await Page.GetByRole(AriaRole.Link, new() { Name = "My Dashboard" }).ClickAsync();
        // 8.перевірити що відображаться "Welcome back, <username>!"
        await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome back, FirstName L!" })).ToBeVisibleAsync();
    }
}