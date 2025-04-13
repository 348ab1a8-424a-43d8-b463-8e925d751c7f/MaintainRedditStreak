using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MaintainRedditStreak
{
    internal class MaintainRedditStreak
    {
        static void Main(string[] args)
        {
            UpvoteFirstPost();
        }

        private static void UpvoteFirstPost()
        {
            string userDir = string.Empty;

            try
            {
                string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
                userDir = File.ReadLines(_filePath).First();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Config.txt file not found in executable directory.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
            }

            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument($"{userDir}");

            IWebDriver driver = new ChromeDriver(options);

            try
            {
                driver.Navigate().GoToUrl("https://old.reddit.com/");

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

                string button = "div[class='arrow up login-required access-required']";
                wait.Until(ExpectedConditions.ElementExists(By.CssSelector($"{button}")));

                var upvoteButtons = driver.FindElements(By.CssSelector($"{button}"));

                if (upvoteButtons.Count > 0)
                {
                    upvoteButtons[0].Click();
                    Console.WriteLine("Succesfully upvoted the first post.");
                }
                else
                {
                    Console.WriteLine("Could not find any upvote buttons.");
                }

                Thread.Sleep(2000);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Web driver timed out waiting for elements to load.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
