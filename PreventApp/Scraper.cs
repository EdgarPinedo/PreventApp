using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using PreventApp.DTOs;

namespace PreventApp
{
    public class Scraper : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var url = "https://www.facebook.com/traficozmg";
            /*var apiUrl = "";*/
            var email = "preventapp.udg@gmail.com";
            var pass = "PreventApp1234";

            List<string> actualAccidents = new();
            List<string> oldAccidents = new();
            List<ScraperDataDTO> data = new();

            /*FirefoxOptions options = new();
            options.AddArgument("--headless");*/

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var driver = new FirefoxDriver())
                {
                    driver.Navigate().GoToUrl(url);

                    Thread.Sleep(1000);
                    try
                    {
                        var emailInput = driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div/div[5]/div/div/div[1]/div/div[2]/div/div/div/div/form/div/div[3]/div/label/div/div/input"));
                        var passwordInput = driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div/div[5]/div/div/div[1]/div/div[2]/div/div/div/div/form/div/div[4]/div/label/div/div/input"));
                        var btn = driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div/div[5]/div/div/div[1]/div/div[2]/div/div/div/div/form/div/div[5]/div"));
                        emailInput.SendKeys(email);
                        passwordInput.SendKeys(pass);
                        btn.Click();

                        Thread.Sleep(3000);
                        IWebElement footer = driver.FindElement(By.TagName("body"));
                        new Actions(driver)
                            .ScrollByAmount(0, 2500)
                            .Perform();

                        Thread.Sleep(3000);
                        for (int i = 1; i <= 4; i++)
                        {
                            var accident = driver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div/div/div[4]/div[2]/div/div[2]/div/div[{i}]/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div[3]/div[1]/div/div/div/span/div/div"));
                            actualAccidents.Add(accident.Text);
                        }
                    }
                    catch (Exception ex) { }
                }

                if(oldAccidents.Count == 0)
                {
                    foreach (var accident in actualAccidents)
                    {
                        var date = DateTime.UtcNow.AddHours(-6);
                        ScraperDataDTO acc = new(accident, date);
                        data.Add(acc);
                        oldAccidents.Add(accident);
                    }
                }
                else
                {
                    foreach (var accident in actualAccidents)
                    {
                        if (!oldAccidents.Contains(accident))
                        {
                            var date = DateTime.UtcNow.AddHours(-6);
                            ScraperDataDTO acc = new(accident, date);
                            data.Add(acc);
                        }
                    }
                    oldAccidents = actualAccidents;
                }
                actualAccidents.Clear();

                /*using (var httpClient = new HttpClient())
                {
                    await httpClient.PostAsJsonAsync(apiUrl, data, stoppingToken);
                }*/

                data.Clear();
                /*await Task.Delay(600000, stoppingToken);*/
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
