1. start signalR.Backplane
2. start 2 instances of signalR.ServerCore.console
3. start 2 instances of signalRclient
4. open 2 clients on each instances of signalRclient
5. push a notification on any instance of signalR.ServerCore.console. All clients should receive the notification 