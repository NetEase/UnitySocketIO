using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SocketIOClient.Messages
{
	/// <summary>
	/// Interface for core Message class
	/// </summary>
    public interface IMessage
    {
		/// <summary>
		/// Enumeration of one of 9 basic messages provided by socket.io
		/// </summary>
		SocketIOMessageTypes MessageType { get; }

		/// <summary>
		/// <para>RawMessage includes the full socket.io message string</para>
		/// <para>[message type] ':' [message id ('+')] ':' [message endpoint] (':' [message data]) </para>
		/// </summary>
		string RawMessage { get; }

		/// <summary>
		/// Event 'name' of originating message
		/// </summary>
		string Event { get; }
		/// <summary>
		/// AckId represents unique id associated with a message callback
		/// </summary>
		int? AckId { get; }

		/// <summary>
		/// Each socket is identified by an endpoint (can be omitted).
		/// </summary>
		string Endpoint { get; set; }
		
		/// <summary>
		/// String version of message data
		/// </summary>
        string MessageText { get; }

		JsonEncodedEventMessage Json { get; }
		[ObsoleteAttribute(".JsonEncodedMessage has been deprecated. Please use .Json instead.")]
		JsonEncodedEventMessage JsonEncodedMessage { get; }
		
		/// <summary>
		/// <para>Socket.IO encoded message structure - represents the actual message string sent to Socket.IO </para>
		/// <para>[message type] ':' [message id ('+')] ':' [message endpoint] (':' [message data]) </para>
		/// </summary>
        string Encoded { get; }

    }
}