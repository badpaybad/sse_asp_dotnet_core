# server send event with asp dotnet core
Pull code then all code sample in side folder SSE.web

- Diagram to show how it work
https://docs.google.com/drawings/d/1Xz7rCKZnwKbo3-JKyLDEjHLDoOY-FxLAROzqqyoOEfI/edit?usp=sharing

### You have to know about asp dotnet core framework version 3.1 

### ServerSendEventHub.cs
- Keep client connected group by channel
- Provide function to push data when other post to channel
- When channel got message, broadcast to all client

# check server side code SSEController function Listener
ServerSendEventHub.RegisterListenerClientContext 

# check server side code SSEController function SendMessage
ServerSendEventHub.Publish

# check javascript usage SharedWorker
SSE.web\Views\Home\Index.cshtml

### SSE 
SSE.web\wwwroot\js\webpushnotification\eventsourcereceiver.js

### SharedWorker
SSE.web\wwwroot\js\webpushnotification\notificationwebsharedworker.js
