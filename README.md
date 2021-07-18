# sympli-seo-checker
A web scraper that retrieves the data about metnionings of a web address in the top 100 results of various search engines

Possible improvements:
Instead of regex use some third-party html parser like HtmlAgilityPack
Use Circuit breaker to prevent the cases when the search provider repeatedly returns an issue or is jut unaviable (or when the internet is disconnected)