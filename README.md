# sympli-seo-checker
A web scraper that retrieves the data about references of a web address in the top 100 results of various search engines

## Implementation details:
### Backend
* .Net5 Rest WebApi.
* WebScraping (with paging support) is used to get the data.
* Regular expressions are used to parse the html (since we cannot use thirdParty libraries).
* MemoryCache is used to prevent subsequent requests to the same web address. It is wrapped into an interface using the Adapter pattern to allow easily switching to any other type of caching.
* Caching is applied to the main data retriever using the Decorator pattern.
* Google and Bing engines are implemented.
* The necessary abstractions are created to easily incorporate new SearchEngines.
* All configurable data such as cache duration is stored in appsettings.json.
* Cors allows only the specific web-addressed to access the api. It is separately configured for dev and prod environments.
* All the errors are caught in GlobalErrorHandlingMiddleware. The errors which depend on the input (BusinessException) are converted to BadRequest, all the others - to InternalServerError.

### Frontend
* Vue3 + Vuex.
* Store is extracted into a module so that it is separate from the rest of the application.
* Local storage is used to keep the previous input after the browser is closed.
* When opened for the first time it straight away gets the results for e-settlements and sympli.com.au.
* API address is separated for dev and prod environments by .env files.
* Subsequent Check button click cancels the current request.
* Quasar framework with SASS support is used for visual styling.

## Possible improvements:

### Backend
* Instead of regex use some third-party html parser like HtmlAgilityPack.
* Use Circuit breaker to prevent the cases when the search provider repeatedly returns an issue or is jut unaviable (or when the internet is disconnected).
* Use Distributed cache like Redis in order to prevent extra requests from different machines. Other than that, there is no shared data in the application, so it is horizontally scalable.
* Web scraping is not the best approach to get the remote data. It has a lot of downsides, such as web pages changing over time or encountering captcha. Instead, it would be better to use Google search API.
* If we need to continue scraping on a corporate level, we could leverage a distributed proxy like Crawlera or SmartProxy, or at least use a Captcha Solving service.
* Add end-to-end tests (for manual testing - with real httpClient, or for CI testing - with mocked httpClient (integration tests should not depend on third party services)).
* To add a new SearchEngine, we need to implement an IQueryProvider and an ISearchEngineResultsParser, register the httpClient and the necessary instances in Startup.cs.
* Logging is now setup only to output data to Console. It could be improved by logging into ELK or at least to a file.

### Frontend
* Finish the tests that are commented out because of Quasar library limitation with Vue3.
* Add End-to-End tests.
* If we need to publish the application and it itself should be searcheable in SearchEngines, then it would be a good idea to implement SSR to improve SEO capabilities.
* Add a storybook to list all the components.
* Improve error handling by displaying the reason of an error.

## Possible issues
* SearchEngine is protecting the requests with Captcha / bans the API - use official API/CaptchaSolverService/Distributed Proxy