This project focuses on building a lightweight server-side app that handles the simple generation and redemption of discount codes. 
It uses WebSockets for real-time communication, stores codes in a SQLite database via Entity Framework, 
exposes its core methods through Swagger—just run the app and navigate to /swagger in your browser to test them. 
You can also monitor generated codes live from the index page whenever a generate request is made.


The design is straightforward. We separate the service and models and added WebSocketHandler.
