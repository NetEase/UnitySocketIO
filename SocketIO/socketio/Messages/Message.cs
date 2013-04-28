using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace SocketIOClient.Messages
{
	/// <summary>
	/// All Socket.IO messages have to be encoded before they're sent, and decoded when they're received.
	/// They all have the format of: [message type] ':' [message id ('+')] ':' [message endpoint] (':' [message data])
	/// </summary>
    public abstract class Message : IMessage
    {
        private static Regex re = new Regex(@"\d:\d?:\w?:");
        public  static char[] SPLITCHARS = new char[] { ':' };

		public string RawMessage { get; protected set; }

		/// <summary>
		/// The message type represents a single digit integer [0-8].
		/// </summary>
        public SocketIOMessageTypes MessageType { get; protected set; }

		/// <summary>
		/// The message id is an incremental integer, required for ACKs (can be ommitted). 
		/// If the message id is followed by a +, the ACK is not handled by socket.io, but by the user instead.
		/// </summary>
		public int? AckId { get; set; }

		/// <summary>
		/// Socket.IO has built-in support for multiple channels of communication (which we call "multiple sockets"). 
		/// Each socket is identified by an endpoint (can be omitted).
		/// </summary>
		public string Endpoint { get; set; }

		/// <summary>
		/// String value of the message
		/// </summary>
        public string MessageText { get; set; }

		private JsonEncodedEventMessage _json;
		[ObsoleteAttribute(".JsonEncodedMessage has been deprecated. Please use .Json instead.")]
		public JsonEncodedEventMessage JsonEncodedMessage
		{
			get { return this.Json; }
			set { this._json = value; }
		}
		
		public JsonEncodedEventMessage Json
		{
			get
			{
				if (_json == null)
				{
					if (!string.IsNullOrEmpty(this.MessageText) &&
						this.MessageText.Contains("name") &&
						this.MessageText.Contains("args"))
					{
						this._json = JsonEncodedEventMessage.Deserialize(this.MessageText);
					}
					else
						this._json = new JsonEncodedEventMessage();
				}
				return _json;

			}
			set { this._json = value; }
		}
		
		/// <summary>
		/// String value of the Event
		/// </summary>
        public virtual string Event { get; set; }
        
		/// <summary>
		/// <para>Messages have to be encoded before they're sent. The structure of a message is as follows:</para>
		/// <para>[message type] ':' [message id ('+')] ':' [message endpoint] (':' [message data])</para>
		/// <para>All message payloads are sent as strings</para>
		/// </summary>
        public virtual string Encoded 
        {
            get
            {
                int msgId = (int)this.MessageType;
                if (this.AckId.HasValue)
                    return string.Format("{0}:{1}:{2}:{3}", msgId, this.AckId ?? -1, this.Endpoint, this.MessageText);
                else
                    return string.Format("{0}::{1}:{2}", msgId, this.Endpoint, this.MessageText);
            }
        }
       

        public Message() 
        {
            this.MessageType = SocketIOMessageTypes.Message;
        }

        public Message(string rawMessage)
            : this()
        {

            this.RawMessage = rawMessage;

            string[] args = rawMessage.Split(SPLITCHARS, 4);
            if (args.Length == 4)
            {
                int id;
                if (int.TryParse(args[1], out id))
                    this.AckId = id;
                this.Endpoint = args[2];
                this.MessageText = args[3];
            }
        }

		//public static Regex reMessageType = new Regex("^[0-8]{1}:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		public static Regex reMessageType = new Regex("^[0-8]{1}:", RegexOptions.IgnoreCase);
		public static IMessage Factory(string rawMessage)
		{
			
			if (reMessageType.IsMatch(rawMessage))
			{
				char id = rawMessage.First(); 
				switch (id)
				{
					case '0':
						return DisconnectMessage.Deserialize(rawMessage);
					case '1':
						return ConnectMessage.Deserialize(rawMessage);
					case '2':
						return new Heartbeat();
					case '3':
						return TextMessage.Deserialize(rawMessage);
					case '4':
						return JSONMessage.Deserialize(rawMessage);
					case '5':
						return EventMessage.Deserialize(rawMessage);
					case '6':
						return AckMessage.Deserialize(rawMessage);
					case '7':
						return ErrorMessage.Deserialize(rawMessage);
					case '8':
						return new NoopMessage();
					default:
						Trace.WriteLine(string.Format("Message.Factory undetermined message: {0}", rawMessage));
						return new TextMessage();
				}
			}
			else
			{
				Trace.WriteLine(string.Format("Message.Factory did not find matching message type: {0}", rawMessage));
				return new NoopMessage();
			}
		}
    }
}
