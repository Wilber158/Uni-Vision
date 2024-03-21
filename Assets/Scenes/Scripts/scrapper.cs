using UnityEngine;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using SeleniumExtras.WaitHelpers;
using System;

public class SeleniumExample : MonoBehaviour
{
    void Start()
    {
        // Call the method to scrape the data
        Dictionary<string, List<string>> eventData = ScrapeEventData();

        // Write the extracted event data to a text file
        string filePath = Path.Combine(Application.persistentDataPath, "schedule.txt");
        WriteEventDataToFile(eventData, filePath);
    }

    Dictionary<string, List<string>> ScrapeEventData()
    {
        // Initialize dictionary to store event data
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();

        // Set the path to ChromeDriver executable
        string chromeDriverPath = @"C:\Users\Kalden\Documents\GitHub\Uni-Vision\Assets\Packages\Selenium.WebDriver.ChromeDriver.122.0.6261.11100\driver\win32";

        // Set up Chrome WebDriver with the specified path
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--headless"); // Optional: Run Chrome in headless mode
        using (IWebDriver driver = new ChromeDriver(chromeDriverPath, options))
        {
            // Navigate to the website
            driver.Navigate().GoToUrl("https://25live.collegenet.com/manhattan");

            // Find username and password elements
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            IWebElement usernameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username")));
            IWebElement passwordElement = driver.FindElement(By.Id("password"));

            // Enter username and password
            usernameElement.SendKeys("kdorji01");
            passwordElement.SendKeys("KaldenDorji12!");
            passwordElement.SendKeys(Keys.Return);

            // Wait for the trust button to appear and click it
            System.Threading.Thread.Sleep(1000); // Not recommended, but Unity doesn't have async support by default
            IWebElement trustButton = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("trust-browser-button")));
            trustButton.Click();

            // Navigate to the calendar page
            System.Threading.Thread.Sleep(15000); // Not recommended, but Unity doesn't have async support by default
            driver.Navigate().GoToUrl("https://25live.collegenet.com/pro/manhattan#!/home/search/location/calendar");

            // Enter the query in the search input field and press Enter
            System.Threading.Thread.Sleep(15000); // Not recommended, but Unity doesn't have async support by default
            IWebElement searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("textarea.searchInput")));
            searchInput.SendKeys("rlc 102");
            searchInput.SendKeys(Keys.Return);

            // Wait for the event items to load
            System.Threading.Thread.Sleep(10000); // Adjust timing as needed
            var tdElements = driver.FindElements(By.CssSelector("td.ngTD.CalendarCell.ngTD.ngZmid.ng-scope.CalendarCellToday.ngMonthclass1"));

            // Ensure only one <td> element is found for today
            if (tdElements.Count == 1)
            {
                var tdElement = tdElements[0];

                // Find the event items within the current date element
                var calendarDayEventItems = tdElement.FindElements(By.CssSelector("div.ngCalendarDayEventItem.CalendarDayEventItem.ng-scope"));

                // Iterate through each event item
                foreach (var item in calendarDayEventItems)
                {
                    // Get the startDt, endDt, and s25-item-name elements
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

        return eventData;
    }

    string GetDictionaryKey(string itemName)
    {
        string[] parts = itemName.Split(' ');
        if (parts.Length >= 2)
            return $"{parts[0]} {parts[1]}";
        else
            return itemName;
    }

    void WriteEventDataToFile(Dictionary<string, List<string>> eventData, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (KeyValuePair<string, List<string>> kvp in eventData)
            {
                writer.WriteLine("Class: " + kvp.Key);
                string eventTimes = string.Join(", ", kvp.Value);
                writer.WriteLine("Event Time: " + eventTimes);
                writer.WriteLine();
            }
            Debug.Log("Event data count: " + eventData.Count); // Output the count of event data
        }
    }
}
