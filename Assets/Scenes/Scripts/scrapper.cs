using UnityEngine;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SeleniumExample : MonoBehaviour
{
    private AddTextToTextMeshPro addTextToTextMeshPro;

    public static Boolean loginStatus {  get; private set; }


    public void TriggerScraping(string targetName)
    {
        // Call the ScrapeEventData method with the detected target name
        Dictionary<string, List<string>> eventData = ScrapeEventData(targetName);
        // Write the extracted event data to a text file
        string fileName = "schedule.txt";
        string filePath = Path.Combine(Application.dataPath, "Resources", fileName);
        WriteEventDataToFile(eventData, filePath);
    }
    private void Start()
    {
    }

    Dictionary<string, List<string>> ScrapeEventData(string textValue)
    {
        // Initialize dictionary to store event data
        Dictionary<string, List<string>> eventData = new Dictionary<string, List<string>>();

        // Set the path to ChromeDriver executable
        string chromeDriverPath = @"C:\Users\Kalden\Documents\GitHub\Uni-Vision\Assets\Packages\Selenium.WebDriver.ChromeDriver.122.0.6261.11100\driver\win32";

        // Set up Chrome WebDriver with the specified path
        ChromeOptions options = new ChromeOptions();
        //options.AddArgument("--headless"); // Optional: Run Chrome in headless mode

        // Initialize WebDriver outside using block for later disposal
        IWebDriver driver = new ChromeDriver(chromeDriverPath, options);
        try
        {
            // Navigate to the website
            driver.Navigate().GoToUrl("https://25live.collegenet.com/manhattan");

            // Find username and password elements
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IWebElement usernameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username")));
            IWebElement passwordElement = driver.FindElement(By.Id("password"));

            string userId = saveLoginInfo.UserId;
            string password = saveLoginInfo.Password;

            // Enter username and password
            usernameElement.SendKeys(userId);
            passwordElement.SendKeys(password);
            passwordElement.SendKeys(Keys.Return);

            Thread.Sleep(3000);
            IWebElement errorMessage = driver.FindElement(By.CssSelector("#error-msg"));
            if (errorMessage != null)
            {
                loginStatus = false;
                Debug.Log(loginStatus);
                return null;
            }
            else
            {
                Debug.Log(loginStatus);
                loginStatus = true;
            }


            // Wait for the trust button to appear and click it
            IWebElement trustButton = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("trust-browser-button")));
            trustButton.Click();

            // Navigate to the calendar page
            wait.Until(ExpectedConditions.UrlContains("https://25live.collegenet.com/pro/manhattan#!/home/search"));
            driver.Navigate().GoToUrl("https://25live.collegenet.com/pro/manhattan#!/home/search/location/calendar");

            // Enter the query in the search input field and press Enter
            IWebElement searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("textarea.searchInput")));
            searchInput.SendKeys(textValue);
            searchInput.SendKeys(Keys.Return);

            // Wait for the event items to load
            Thread.Sleep(5000);
            var tdElements1 = wait.Until(driver => driver.FindElements(By.CssSelector("td.ngTD.CalendarCell.ngTD.ngZmid.ng-scope.CalendarCellToday.ngMonthclass1")));
            var tdElements2 = wait.Until(driver => driver.FindElements(By.CssSelector("td.ngTD.CalendarCell.ngTD.ngZmid.ng-scope.CalendarCellToday.ngMonthclass2")));
            var tdElements = new List<IWebElement>(tdElements1);
            tdElements.AddRange(tdElements2);

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
        finally
        {
            // Dispose of WebDriver resources
            driver.Quit();
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
        if (File.Exists(filePath))
        {
            // Delete the file if it exists
            File.Delete(filePath);
            Debug.Log("Deleted existing file: " + filePath);
        }
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

