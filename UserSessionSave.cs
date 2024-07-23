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
        // 1. �������� https://courses.ultimateqa.com/enrollments,
        await Page.GotoAsync("https://courses.ultimateqa.com/enrollments");
        // 2.�������� �� ����� �� �� https://courses.ultimateqa.com/users/sign_in, 
        await Expect(Page).ToHaveURLAsync("https://courses.ultimateqa.com/users/sign_in");
        // 3.�������� � ��������� ������ ������/ �������
        await Page.GetByPlaceholder("Email").FillAsync("yustas.silaev@gmail.com");
        await Page.GetByPlaceholder("Password").FillAsync("~Test123");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        // 4.��� �� "View more courses" //*[@id="category-name"]/text()
        await Page.GetByRole(AriaRole.Link, new() { Name = "View more courses" }).ClickAsync();
        // 5.� ��� Search ������ "Selenium" �� ������ ������ Enter
        await Page.GetByPlaceholder("Search").FillAsync("Selenium");
        await Page.GetByPlaceholder("Search").PressAsync("Enter");
        // 6.�������� �� ������������ ����� �� ����� � ���� Selenium
        await Expect(Page.GetByText("Selenium", new() { Exact = true })).ToBeVisibleAsync();
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Complete Selenium WebDriver with Java Bootcamp Course star 5.0 average rating (8 reviews) $" })).ToBeVisibleAsync();
        // 7.��� �� "My Dashboard"
        await Page.GetByRole(AriaRole.Link, new() { Name = "My Dashboard" }).ClickAsync();
        // 8.��������� �� ������������ "Welcome back, <username>!"
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Welcome back, Test123 T!" })).ToBeVisibleAsync();
        // 9.��� �� ��'� ����������� ������ ������
        await Page.GetByLabel("Toggle menu").ClickAsync();
        // 10.��� Sign out
        await Page.GetByRole(AriaRole.Link, new() { Name = "Sign Out" }).ClickAsync();
        // 11.��������� �� � ��������� Sign In
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Sign In" })).ToBeVisibleAsync();
    }
}