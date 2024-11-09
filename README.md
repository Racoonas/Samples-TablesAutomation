# Samples-TablesAutomation
## Description
This repository contains free framework built with <a href="https://playwright.dev/dotnet/">Playwright</span>, <a href="https://docs.nunit.org/">nUnit</a> and <a href="https://specflow.org/">Specflow</a> in C#. 

## Target
Tests are running against <a href = "https://material.angular.io/"> Angular Material </a> site, showcasing some examples of interactions with tables.

## Projects
- <b>TablesAutomation.E2EFeaturedTests</b> - Tests themselves. SpecFlow featured test scenarios, step definitions and hooks.
- <b>TablesAutomation.E2EFramework</b> - Spinning gears under the hood. Page objects, utilities, models, configuration management classes.
- <b>TablesAutomation.E2EUnitTests</b> - Unit tests. Custom written Utils and Helpers should be covered as much as possible here.

## Logic Layers
- Page Objects (lowest logic level) - contains particular page elements and the simplest operations.
- Step Definitions (business logic abstraction layer) - contains page operations, which are more sophisticated than simple operations in the page.
- Test Scenarios - easily readable test scenarios, consisting of step definitions.
- Features - combinations on Test Scenarios (can also be considered as Test Suites)

## Features
- Framework has parallelizable test runs (organized by test features).
- Following features controlled by <a href = "https://github.com/Racoonas/Samples-TablesAutomation/blob/main/TablesAutomation.E2E.FeaturedTests/appsettings.json">appsettings </a> configuration file:
  - Multiple Browsers support
  - Headful/headless browser mode
  - Screenshots capturing
  - Traces and videos capturing
