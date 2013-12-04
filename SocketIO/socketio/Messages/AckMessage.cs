using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SocketIOClient.Messages
{
	public sealed class AckMessage : Message
	{
		private static Regex reAckId = new Regex(@"^(\d{1,})");
 		private static Regex reAckPayload = new Regex(@"(?:[\d\+]*)(?<data>.*)$");
		private static Regex reAckComplex = new Regex(@"^\[(?<payload>.*)\]$");

		private static object ackLock = new object();
		private static int _akid = 0;
		public static int NextAckID
		{
			get
			{
				lock (ackLock)
				{
					_akid++;
					if (_akid < 0)
						_akid = 0;
					return _akid;
				}
			}
		}

		public Action  Callback;

		public AckMessage()
			: base()
        {
            this.MessageType = SocketIOMessageTypes.ACK;
        }
		
		public static AckMessage Deserialize(string rawMessage)
        {
			AckMessage msg = new AckMessage();
			//  '6:::' [message id] '+' [data]
			//   6:::4
			//	 6:::4+["A","B"]
			msg.RawMessage = rawMessage;

            string[] args = rawMessage.Split(SPLITCHARS, 4);
            if (args.Length == 4)
            {
				msg.Endpoint = args[2];
                int id;
				string[] parts = args[3].Split(new char[] {'+'});
				if (parts.Length > 1)
				{
					if (int.TryParse(parts[0], out id))
					{
						msg.AckId = id;
						msg.MessageText = parts[1];
						Match payloadMatch = reAckComplex.Match(msg.MessageText);

						if (payloadMatch.Success)
						{
							msg.Json = new JsonEncodedEventMessage();
							msg.Json.args = new string[]  {payloadMatch.Groups["payload"].Value};
						}
					}
				}
            }
			return msg;
        }
		public override string Encoded
		{
			get
			{
				int msgId = (int)this.MessageType;
				if (this.AckId.HasValue)
				{
					if (this.Callback == null)
						return string.Format("{0}:{1}:{2}:{3}", msgId, this.AckId ?? -1, this.Endpoint, this.MessageText);
					else
						return string.Format("{0}:{1}+:{2}:{3}", msgId, this.AckId ?? -1, this.Endpoint, this.MessageText);
				}
				else
					return string.Format("{0}::{1}:{2}", msgId, this.Endpoint, this.MessageText);
			}
		}
	}
}
