#!/usr/bin/python3
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
import time
from pyvirtualdisplay import Display
import requests

display = Display(visible=0, size=(1920, 1080))
display.start()

chrome_options = Options()
chrome_options.add_argument("--headless")
chrome_options.add_argument("--window-size=1920x1080")

chrome_driver = '/usr/lib/chromium-browser/chromedriver'

driver = webdriver.Chrome(chrome_options=chrome_options, executable_path=chrome_driver)

driver.get('https://www.worldpadeltour.com/jugadores/?ranking=todos')
time.sleep(20)

for i in range(1,25):
    driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
    time.sleep(2)

time.sleep(10)
driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
time.sleep(10)
driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
time.sleep(20)
html_source = driver.page_source
data = html_source.encode('utf-8')
file = open("/home/pi/Websites/padel-info-world-padel-tour-html.txt","wb")
file.write(data)
file.close()
driver.quit()
display.stop()
files = {
    'file': ('padel-info-world-padel-tour-html.txt', open('/home/pi/Websites/padel-info-world-padel-tour-html.txt', 'rb')),
}
response = requests.post('http://localhost:38000/api/UpdateWorldPadelTourRankingFromFileUpload', files=files)
print (response)