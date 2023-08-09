using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace PreventApp
{
    public class Scraper : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var url = "https://www.facebook.com/traficozmg";
                List<string> accidents = new();

                ChromeOptions options = new();
                options.AddArgument("--headless");

                using (var driver = new ChromeDriver(options))
                {
                    driver.Navigate().GoToUrl(url);

                    Thread.Sleep(1000);
                    var btn = driver.FindElement(By.XPath("/html/body/div[1]/div/div[1]/div/div[5]/div/div/div[1]/div/div[2]/div/div/div/div[1]"));
                    btn.Click();

                    IWebElement footer = driver.FindElement(By.TagName("body"));
                    new Actions(driver)
                        .ScrollByAmount(0, 2500)
                        .Perform();

                    Thread.Sleep(2000);
                    for (int i = 1; i <= 4; i++)
                    {
                        var accident = driver.FindElement(By.XPath($"/html/body/div[1]/div/div[1]/div/div[3]/div/div/div/div[1]/div[1]/div/div/div[4]/div[2]/div/div[2]/div/div[{i}]/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div[3]/div[1]/div/div/div/span/div/div"));
                        accidents.Add(accident.Text);
                    }
                }

                // Send to Api
                accidents.Clear();
                await Task.Delay(600000, stoppingToken);
            }
        }
    }
}
