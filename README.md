**SocketSim** is personal WPF project  

Status: Functions are working. There are some minor adjustments of the UI missing.

The app is meant to be used to test TCP/UDP sockets

Personal note: 
This semester (CS studies) I learned about socket programming in C# and decided to create an application with the new knowledge and combine it with a personal learning goal, to extentd my horizon and learn a bit about desktop development. To be fair, I want to mention that I was inspired by the app SocketTest (https://sourceforge.net/projects/sockettest/) and wanted to create something similar using my main programming language C#.

Learning goals:
- Create a WPF application	
- use different UI elements and controls (DockPanel, Grid, StackPanel, GroupBox)
- create and use custom styles and templates 
- get more understanding of TcpClient and UdpClient 
- use asynchronous programming with TcpClient and UdpClient
- OBJECTIVE UPDATE: LOGGING !!! (here: log4net)

**Create a TCP server**  
Enter the IP address and the port your server should listen on and start listening. Optionally, the server can run as echo server.  

![image](https://user-images.githubusercontent.com/70850868/152645087-9b9528ef-80da-4325-888b-fc84cd6ee263.png)

**Create a TCP client**  
Enter the IP address and the port of the server you want to conntect to and click Connect.  

![image](https://user-images.githubusercontent.com/70850868/152645098-b561052b-e882-470b-b69e-f6ffb1fc0560.png)

**Send/receive via UDP**  
You can either listen to incoming messages on the specified IP address and port.  
Or you can simply send a message.  

![image](https://user-images.githubusercontent.com/70850868/152645103-46b49178-1df5-4811-90a1-a67e2f79e577.png)


