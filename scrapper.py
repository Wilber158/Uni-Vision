from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from bs4 import BeautifulSoup
import requests
from time import sleep


def enter_username_without_redirection(url, username, password, query):
    # Initialize the WebDriver (assuming Chrome WebDriver here)
    driver = webdriver.Chrome()

    html_content = """
    <tr s25-ng-dnd-sortable-item="" tabindex="0" draggable="false" role="listitem" aria-roledescription="Draggable item. Press enter to grab or drop.">
        <!-- td elements -->
    </tr>
    <tr s25-ng-dnd-sortable-item="" tabindex="0" draggable="false" role="listitem" aria-roledescription="Draggable item. Press enter to grab or drop.">
        <!-- td elements -->
    </tr>
    <!-- More tr elements -->
    """

    try:
        # Open the URL
        driver.get(url)

        # Wait for the page to load (optional)
        WebDriverWait(driver, 10).until(EC.presence_of_element_located((By.ID, 'username')))

        # Find the input elements for username and password by ID
        username_element = driver.find_element(By.ID, 'username')
        password_element = driver.find_element(By.ID, 'password')

        # Enter the provided username and password
        username_element.send_keys(username)
        password_element.send_keys(password)

        # Submit the form by pressing Enter (assuming Enter key submits the form)
        password_element.send_keys(Keys.RETURN)

        # Wait for the prompt to appear
        wait = WebDriverWait(driver, 30)
        trust_button = wait.until(EC.presence_of_element_located((By.ID, 'trust-browser-button')))
        trust_button.click()

        # Wait for redirections to complete
        previous_url = driver.current_url
        for _ in range(10):  # Max 10 iterations to prevent infinite loop
            sleep(1)  # Wait for 1 second before checking again
            current_url = driver.current_url
            if current_url != previous_url:
                break
        else:
            print("Redirections did not complete within the specified time.")

        textarea = wait.until(EC.presence_of_element_located((By.CSS_SELECTOR, 'textarea.searchInput')))
        textarea.send_keys(query)
        textarea.send_keys(Keys.RETURN)

        sleep(3)  # Give some time for the results to load

        wrapper_element = WebDriverWait(driver, 10).until(EC.presence_of_element_located((By.CSS_SELECTOR, '.ng-scope.ng-isolate-scope')))

        # Get the HTML content of the wrapper element
        html_content = wrapper_element.get_attribute('outerHTML')

        # Parse the HTML content using BeautifulSoup
        soup = BeautifulSoup(html_content, 'html.parser')

        with open('scraped_data.txt', 'w', encoding='utf-8') as file:
            file.write(str(soup))

    finally:
        # Close the WebDriver session
        driver.quit()


# Example usage:
url = "https://25live.collegenet.com/manhattan"  # Replace with the actual URL
username = "your username"  # Replace with the actual username
password = "your password"
query = "102"
enter_username_without_redirection(url, username, password, query)
