UnitySocketIO
=============================
This project is a socket.io client for Unity 3D, written in C#.
It's based on socketio4net.Client (http://socketio4net.codeplex.com/). However 
socketio4net.Client only provides a .NET 4.0 C# client, and isn't compatible with Unity 3D. 
We've done a lot of work on supporting Unity 3D.

The project was initially designed for Unity client of [pomelo](https://github.com/NetEase/pomelo) 
framework, which is a powerful, scalable game server framework.

## How to use

It is very simple to use UnitySocketIO. Copy all the DLLs located in the directory /bin/Debug/ to your project.

Of course, you can download this project and compile it:

>git clone  https://github.com/NetEase/UnitySocketIO.git

## API

Create and initialize a new UnitySocketIO client.

```c#
Client client = new Client(url);

client.Opened += SocketOpened;
client.Message += SocketMessage;
client.SocketConnectionClosed += SocketConnectionClosed;
client.Error +=SocketError;

client.Connect();

private void SocketOpened(object sender, MessageEventArgs e) {
    //invoke when socket opened
}

```
Send message to server.

```c#

client.Send(messge);

```
Get message from server.

```c#
private void SocketMessage (object sender, MessageEventArgs e) {
    if ( e!= null && e.Message.Event == "message") {
        string msg = e.Message.MessageText;
       process(msg);
    }
}

```
Close connection.

```c#

client.Close();

```


##License
(The MIT License)

Copyright (c) 2012-2013 NetEase, Inc. and other contributors

Permission is hereby granted, free of charge, to any person obtaining a 
copy of this software and associated documentation files (the 'Software'), 
to deal in the Software without restriction, including without limitation
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

