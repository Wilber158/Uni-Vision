using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using SeleniumExtras.WaitHelpers;

class Program
{
    static void Main()
    {
        string url = "https://25live.collegenet.com/manhattan";
        string username = "kdorji01";
        string password = "KaldenDorji12!";
        string query = "rlc 102";

        Dictionary<string, List<string>> eventData = EnterUsernameWithoutRedirection(url, username, password, query);

        // Print the extracted event data
        using (StreamWriter writer = new StreamWriter("schedule.txt"))
        {
            foreach (var kvp in eventData)
            {
                writer.WriteLine("Class: " + kvp.Key);
                // Join the list of event times into a single string
                string eventTimes = string.Join(", ", kvp.Value);
                writer.WriteLine("Event Time: " + eventTimes);
                writer.WriteLine();
            }
        }
    }

    static Dictionary<string, List<string>> EnterUsernameWithoutRedirection(string url, string username, string password, string query)
    {
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();

        // Initialize Chrome WebDriver
        using (IWebDriver driver = new ChromeDriver())
        {
            try
            {
                driver.Navigate().GoToUrl(url);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement usernameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username")));
                IWebElement passwordElement = driver.FindElement(By.Id("password"));

                // Enter the provided username and password
                usernameElement.SendKeys(username);
                passwordElement.SendKeys(password);
                passwordElement.SendKeys(Keys.Return);

                // Wait for the trust button to appear and click it
                Thread.Sleep(15000);
                IWebElement trustButton = null;
                var trustButtonExists = wait.Until(driver =>
                {
                    trustButton = driver.FindElement(By.Id("trust-browser-button"));
                    return trustButton != null;
                });

                // Click the trust button if it exists
                if (trustButtonExists)
                {
                    trustButton.Click();
                }
                // Wait for redirections to complete
                string previousUrl = driver.Url;
                for (int i = 0; i < 10; i++) // Max 10 iterations to prevent infinite loop
                {
                    Thread.Sleep(500); // Wait for 1 second before checking again
                    string currentUrl = driver.Url;
                    if (currentUrl != previousUrl)
                        break;
                }

                // Navigate to the calendar page
                driver.Navigate().GoToUrl("https://25live.collegenet.com/pro/manhattan#!/home/search/location/calendar");

                // Enter the query in the search input field and press Enter
                IWebElement textarea = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("textarea.searchInput")));
                textarea.SendKeys(query);
                textarea.SendKeys(Keys.Return);

                Thread.Sleep(3000);
                // Find the current date elements in the calendar
                var tdElements = driver.FindElements(By.CssSelector("td.ngTD.CalendarCell.ngTD.ngZmid.ng-scope.CalendarCellToday.ngMonthclass1"));

                // Iterate through each current date element
                foreach (var tdElement in tdElements)
                {
                    // Find the event items within the current date element
                    Thread.Sleep(3000);
                    var calendarDayEventItems = tdElement.FindElements(By.CssSelector("div.ngCalendarDayEventItem.CalendarDayEventItem.ng-scope"));
                    Thread.Sleep(3000);
                    // Iterate through each event item
                    foreach (var item in calendarDayEventItems)
                    {
                        // Get the startDt, endDt, and s25-item-name elements
                        Thread.Sleep(10);
                        var startDtElement = item.FindElement(By.CssSelector("span.startDt"));
                        var endDtElement = item.FindElement(By.CssSelector("span.endDt"));
                        var itemNameElement = item.FindElement(By.CssSelector("div.s25-item-name"));

                        // Extract the text content of each element
                        string startDt = startDtElement.Text;
                        string endDt = endDtElement.Text;
                        string itemName = itemNameElement.Text;

                        // Format the dictionary key (item name stripped after the second space)
                        string key = GetDictionaryKey(itemName);

                        // Format the dictionary value (start time + hyphen + end time)
                        string value = $"{startDt} - {endDt}";

                        // Add the key-value pair to the dictionary
                        if (!eventData.ContainsKey(key))
                        {
                            eventData[key] = new List<string>();
                        }
                        eventData[key].Add(value);
                    }
                }
            }
            finally
            {
                driver.Quit();
            }
        }

        return eventData;
    }
    static string GetDictionaryKey(string itemName)
    {
        // Split the item name by space
        string[] parts = itemName.Split(' ');

        // Take the first two parts and concatenate them
        if (parts.Length >= 2)
        {
            return $"{parts[0]} {parts[1]}";
        }
        else
        {
            return itemName; // Return the original item name if less than two parts
        }
    }
}
