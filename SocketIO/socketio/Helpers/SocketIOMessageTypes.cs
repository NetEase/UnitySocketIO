using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient
{
    public enum SocketIOMessageTypes
    {
        Disconnect = 0, //Signals disconnection. If no endpoint is specified, disconnects the entire socket.
        Connect = 1,    // Only used for multiple sockets. Signals a connection to the endpoint. Once the server receives it, it's echoed back to the client.
        Heartbeat = 2,
        Message = 3, // A regular message
        JSONMessage = 4, // A JSON message
        Event = 5, // An event is like a JSON message, but has mandatory name and args fields.
        ACK = 6,  //An acknowledgment contains the message id as the message data. If a + sign follows the message id, it's treated as an event message packet.
        Error = 7, // Error
        Noop = 8 // No operation
    }
}
