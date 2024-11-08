# Samples-TablesAutomation
## Description
This repository contains free framework built with <a href="https://playwright.dev/dotnet/">Playwright</span>, <a href="https://docs.nunit.org/">nUnit</a> and <a href="https://specflow.org/">Specflow</a> in C#. 

## Target
Tests are running against <a href = "https://material.angular.io/"> Angular Material </a> site, showcasing some examples of interactions with tables:
![image](https://github.com/user-attachments/assets/2fa6e0c7-827d-4ef0-a2e7-867d5f2077d2)


## Logic Layers
- Page Objects (Lowest logic level) - contains particular page elements and the simplest operations.
- Step Definitions - Business Level Abstraction layer. Contain page operations, which are more sophisticated than simple operations in the page.
- Test Scenarios - Easily Readable test scenarios
- Features - Combinations on Test Scenarios (can also be considered as Test Suites)

## Features
- Framework has parallel test runs (organized by test features). 
- Headful/headless browser mode (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
- Screenshots capturing (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
- Traces and videos capturing (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
