using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Kompleksinis.Tests
{
    public class LoginTests
    {
        IWebDriver driver;

        [SetUp]
        public void Initialize()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(@"C:\Users\Evaldas\Desktop", "chromedriver.exe");
            driver = new ChromeDriver(service);
        }

        [Test]
        public void TestGoodLogin()
        {
            driver.Navigate().GoToUrl("https://localhost:44345");
           driver.FindElement(By.Id("Email")).SendKeys("i@i.i");
            driver.FindElement(By.Id("Password")).SendKeys("evas");
            driver.FindElement(By.XPath("//input[@type=\"submit\"]")).Click();
            Assert.AreEqual("https://localhost:44345/", driver.Url);
        }
        [Test]
        public void TestBadLogin()
        {
            driver.Navigate().GoToUrl("https://localhost:44345");
            driver.FindElement(By.Id("Email")).SendKeys("gdfgdfg");
            driver.FindElement(By.Id("Password")).SendKeys("dfgdgfdg");
            driver.FindElement(By.XPath("//input[@type=\"submit\"]")).Click();
            Assert.AreEqual("https://localhost:44345/Account/Login", driver.Url);
        }
        [TearDown]
        public void EndTest()
        {
            driver.Close();
            driver.Quit();
        }
    }
}
