using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient.Messages
{
	/// <summary>
	/// Signals a connection to the endpoint. Once the server receives it, it's echoed back to the client
	/// </summary>
	/// <remarks>If the client is trying to connect to the endpoint /test, a message like this will be delivered:
	///		'1::' [path] [query]
	/// </remarks>
	public class ConnectMessage : Message
	{
		public string Query { get; private set; }

		public override string Event
		{
			get { return "connect"; }
		}

		public ConnectMessage() : base()
		{
			this.MessageType = SocketIOMessageTypes.Connect;
		}
		public ConnectMessage(string endPoint) : this()
		{
			this.Endpoint = endPoint;
		}
		public static ConnectMessage Deserialize(string rawMessage)
		{
			ConnectMessage msg = new ConnectMessage();
			//  1:: [path] [query]
			//  1::/test?my=param
			msg.RawMessage = rawMessage;

			string[] args = rawMessage.Split(SPLITCHARS, 3);
			if (args.Length == 3)
			{
				string[] pq = args[2].Split(new char[] { '?' });

				if (pq.Length > 0)
					msg.Endpoint = pq[0];
				
				if (pq.Length > 1)
					msg.Query = pq[1];
			}
			return msg;
		}
		public override string Encoded
		{
			get
			{
				return string.Format("1::{0}{1}", this.Endpoint, string.IsNullOrEmpty(this.Query) ? string.Empty: string.Format("?{0}",this.Query));
			}
		}
	}
}
