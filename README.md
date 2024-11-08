# Samples-TablesAutomation
## Description
This repository contains free framework built with <a href="https://playwright.dev/dotnet/">Playwright</span>, <a href="https://docs.nunit.org/">nUnit</a> and <a href="https://specflow.org/">Specflow</a> in C#. 

## Target
Tests are running against <a href = "https://material.angular.io/"> Angular Material </a> site, showcasing some examples of interactions with tables.

## Logic Layers
- Page Objects (lowest logic level) - contains particular page elements and the simplest operations.
- Step Definitions (business logic abstraction layer) - contains page operations, which are more sophisticated than simple operations in the page.
- Test Scenarios - easily readable test scenarios, consisting of step definitions.
- Features - combinations on Test Scenarios (can also be considered as Test Suites)

## Features
- Framework has parallel test runs (organized by test features). 
- Headful/headless browser mode (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
- Screenshots capturing (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
- Traces and videos capturing (Controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file)
