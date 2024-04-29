import asyncio
import re
from bs4 import BeautifulSoup
from pyppeteer import launch
import sqlite3

async def login(username, password):
    login_url = 'https://25live.collegenet.com/pro/manhattan'
    
    # Launch headless browser
    browser = await launch(headless=False, executablePath=r'C:\Users\Kalden\AppData\Local\Chromium\Application\chrome.exe')
    page = await browser.newPage()
    
    # Wait for 2 seconds
    await asyncio.sleep(2)
    
    # Retrieve the login page to extract CSRF tokens or other necessary data
    await page.goto(login_url)
    
    # Check if redirected, get the redirected URL
    redirected_url = page.url
    if redirected_url != login_url:
        login_url = redirected_url
    
    # Retrieve the login page to extract CSRF tokens or other necessary data
    await page.goto(login_url)
    
    # Wait for 5 seconds to ensure page loads completely
    await asyncio.sleep(5)
    
    # Find the username and password fields and fill them
    await page.type('#username', username)
    await page.type('#password', password)
    
    # Submit the login form
    await page.keyboard.press('Enter')
    
    # Wait for 2 seconds for the page to load after login
    await asyncio.sleep(2)
    
    # Navigate to the desired page after login
    await page.goto('https://25live.collegenet.com/pro/manhattan#!/home/search/location/list')
    
    # Wait for 2 seconds for the page to load
    await asyncio.sleep(2)
    
    return browser, page

async def scrape(page, target):
    await page.goto('https://25live.collegenet.com/pro/manhattan#!/home/search/location/list')
    await page.reload()

    try:
        search_input_selector = 'textarea[placeholder="Search Locations"]'
        search_input_element = await page.querySelector(search_input_selector)
        await search_input_element.type(target)
        await page.keyboard.press('Enter')
        await page.waitForXPath('//button[contains(@class, "btn btn-default ngCompview") and contains(text(), "Calendar")]')
        calendar_button = await page.xpath('//button[contains(@class, "btn btn-default ngCompview") and contains(text(), "Calendar")]')
        await calendar_button[0].click()
    except Exception as e:
        print("An error occurred:", e)

    await page.waitForXPath('//td[@class="ngTD CalendarCell ngTD ngZmid ng-scope ngMonthclass1"]')

    # Extract all child elements of the table cell
    td_elements = await page.xpath('//td[@class="ngTD CalendarCell ngTD ngZmid ng-scope ngMonthclass1"]')
    data = []
    days = ['Monday', 'Tuesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    day_index = 0
    for td_element in td_elements:
        start_dt_texts = []
        end_dt_texts = []
        s25_item_name_texts = []
        span_start_dts = await td_element.xpath('.//span[@class="startDt ng-binding"]')
        for span_start_dt in span_start_dts:
            start_dt_text = await page.evaluate('(element) => element.textContent', span_start_dt)
            start_dt_texts.append(start_dt_text)

        span_end_dts = await td_element.xpath('.//span[@class="endDt ng-binding"]')
        for span_end_dt in span_end_dts:
            end_dt_text = await page.evaluate('(element) => element.textContent', span_end_dt)
            end_dt_texts.append(end_dt_text)

        div_s25_item_names = await td_element.xpath('.//div[@class="s25-item-name "]')
        for div_s25_item_name in div_s25_item_names:
            s25_item_name_text = await page.evaluate('(element) => element.textContent', div_s25_item_name)
            # Check if s25_item_name_text contains non-alphabetic characters
            if not s25_item_name_text.isalpha():
                words = s25_item_name_text.split()
                s25_item_name_text = ' '.join(words[:2])
            s25_item_name_texts.append(s25_item_name_text)

        # Add all extracted texts to the data list
        for start_dt_text, end_dt_text, s25_item_name_text in zip(start_dt_texts, end_dt_texts, s25_item_name_texts):
            data.append({
                'day': days[day_index],
                'start_dt': start_dt_text,
                'end_dt': end_dt_text,
                's25_item_name': s25_item_name_text
            })
        day_index = (day_index + 1) % 7  # Wrap around to Monday after Sunday
    td_element_today = await page.xpath('//td[@class="ngTD CalendarCell ngTD ngZmid ng-scope CalendarCellToday ngMonthclass1"]')
    for td_element in td_element_today:
        start_dt_texts = []
        end_dt_texts = []
        s25_item_name_texts = []
        span_start_dts = await td_element.xpath('.//span[@class="startDt ng-binding"]')
        for span_start_dt in span_start_dts:
            start_dt_text = await page.evaluate('(element) => element.textContent', span_start_dt)
            start_dt_texts.append(start_dt_text)

        span_end_dts = await td_element.xpath('.//span[@class="endDt ng-binding"]')
        for span_end_dt in span_end_dts:
            end_dt_text = await page.evaluate('(element) => element.textContent', span_end_dt)
            end_dt_texts.append(end_dt_text)

        div_s25_item_names = await td_element.xpath('.//div[@class="s25-item-name "]')
        for div_s25_item_name in div_s25_item_names:
            s25_item_name_text = await page.evaluate('(element) => element.textContent', div_s25_item_name)
            # Check if s25_item_name_text contains non-alphabetic characters
            if not s25_item_name_text.isalpha():
                words = s25_item_name_text.split()
                s25_item_name_text = ' '.join(words[:2])
            s25_item_name_texts.append(s25_item_name_text)

        # Add all extracted texts to the data list
        for start_dt_text, end_dt_text, s25_item_name_text in zip(start_dt_texts, end_dt_texts, s25_item_name_texts):
            data.append({
                'day': 'Wednesday',
                'start_dt': start_dt_text,
                'end_dt': end_dt_text,
                's25_item_name': s25_item_name_text
            })

    return data

# Now, when you call the scrape function, you need to pass the page object obtained from the login function.
# For example:
# browser, page = await login(username, password)
# scraped_data = await scrape(page, 'your_target')
# print(scraped_data)

# Function to insert data into the database


def create_day_table(day,target):
    # Connect to SQLite database (create it if it doesn't exist)
    conn = sqlite3.connect(target+'.db')
    c = conn.cursor()

    # Create a table for the specific day
    c.execute('''CREATE TABLE IF NOT EXISTS {} (
                 start_date TEXT,
                 end_date TEXT,
                 class TEXT
                 )'''.format(day))

    # Commit changes and close connection
    conn.commit()
    conn.close()

def insert_data(day, start_date, end_date, class_name,target):
    # Connect to SQLite database
    conn = sqlite3.connect(target+'.db')
    c = conn.cursor()

    # Insert data into the table
    c.execute('''INSERT INTO {} (start_date, end_date, class)
                 VALUES (?, ?, ?)'''.format(day), (start_date, end_date, class_name))

    # Commit changes and close connection
    conn.commit()
    conn.close()


# Example usage
days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']

# Sample data
# Sample data

async def main():
    # Example credentials
    this    = [
    "LEO232", "LEO233", "LEO234", "LEO235", "LEO236", "LEO237", "LEO238", "LEO241", "LEO239",
    "LEO246", "LEO242", "LEO243", "LEO244", "LEO245", "LEO247", "LEO248", "LEO249", "LEO250", "LEO251",
    "LEO252", "LEO253", "LEO254", "LEO255", "LEO256", "LEO259", "LEO257", "LEO258", "LEO217", "LEO218",
    "LEO221", "LEO226", "LEO227", "LEO228", "LEO230"
]

    username = 'kdorji01'
    password = 'KaldenDorji12!'
    
    # Log in and get the browser and page objects
    browser, page = await login(username, password)

    for classes in this:
        current = classes[:3] + " " + classes[3:]
        current1 = classes

        try:
            # Scrape data for the current class
            data = await scrape(page, current)
            
            for item in data:
                day = item['day']
                create_day_table(day, current1)
                insert_data(day, item['start_dt'], item['end_dt'], item['s25_item_name'], current1)

        except Exception as e:
            print(f"An error occurred for class {current1}: {e}")

    # Don't forget to close the browser when you're done
    await browser.close()

# Run the main function
asyncio.get_event_loop().run_until_complete(main())
        
if __name__ == "__main__":
    main()
