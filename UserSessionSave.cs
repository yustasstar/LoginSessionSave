using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserSessionSave : PageTest
{
    [Test]
    public async Task UserSession()
    {
        // 1. відкривав https://courses.ultimateqa.com/enrollments,
        await Page.GotoAsync("https://courses.ultimateqa.com/enrollments");
        // 2.перевіряв що тепер ми на https://courses.ultimateqa.com/users/sign_in, 
        await Expect(Page).ToHaveURLAsync("https://courses.ultimateqa.com/users/sign_in");
        // 3.логінився зі створеним вручну логіном/ паролем
        await Page.GetByPlaceholder("Email").FillAsync("yustas.silaev@gmail.com");
        await Page.GetByPlaceholder("Password").FillAsync("~Test123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        // 4.клік на "View more courses" //*[@id="category-name"]/text()
        await Page.GetByRole(AriaRole.Link, new() { Name = "View more courses" }).ClickAsync();
        // 5.в полі Search вводив "Selenium" та натиск клавіші Enter
        await Page.GetByPlaceholder("Search").FillAsync("Selenium");
        await Page.GetByPlaceholder("Search").PressAsync("Enter");
        // 6.перевіряв що відобразились курси які мають в назві Selenium
        await Expect(Page.GetByText("Selenium", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Complete Selenium WebDriver with Java Bootcamp Course star 5.0 average rating (8 reviews) $" })).ToBeVisibleAsync();
        // 7.клік на "My Dashboard"
        await Page.GetByRole(AriaRole.Link, new() { Name = "My Dashboard" }).ClickAsync();
        // 8.перевірити що відображаться "Welcome back, <username>!"
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome back, Test123 T!" })).ToBeVisibleAsync();
        // 9.клік на ім'я користувача справа вверху
        await Page.GetByLabel("Toggle menu").ClickAsync();
        // 10.клік Sign out
        await Page.GetByRole(AriaRole.Link, new() { Name = "Sign Out" }).ClickAsync();
        // 11.перевірити що є посилання Sign In
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Sign In" })).ToBeVisibleAsync();
    }
}