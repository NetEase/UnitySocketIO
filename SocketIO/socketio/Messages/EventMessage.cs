using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
 

namespace SocketIOClient.Messages
{
    public class EventMessage : Message
    {
		private static object ackLock = new object();
		private static int _akid = 0;
		private static int NextAckID
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

		public Action<Object>  Callback;

        public EventMessage()
        {
            this.MessageType = SocketIOMessageTypes.Event;
        }

		public EventMessage(string eventName, object jsonObject, string endpoint  , Action<Object>  callBack  )
			: this()
        {
			this.Callback = callBack;
			this.Endpoint = endpoint;

			if (callBack != null)
				this.AckId = EventMessage.NextAckID;

			this.JsonEncodedMessage = new JsonEncodedEventMessage(eventName, jsonObject);
			this.MessageText = this.Json.ToJsonString();
        }

        public static EventMessage Deserialize(string rawMessage)
        {
			EventMessage evtMsg = new EventMessage();
            //  '5:' [message id ('+')] ':' [message endpoint] ':' [json encoded event]
            //   5:1::{"a":"b"}
			evtMsg.RawMessage = rawMessage;
			try
			{
				string[] args = rawMessage.Split(SPLITCHARS, 4); // limit the number of pieces
				if (args.Length == 4)
				{
					int id;
					if (int.TryParse(args[1], out id))
						evtMsg.AckId = id;
					evtMsg.Endpoint = args[2];
					evtMsg.MessageText = args[3];

					if (!string.IsNullOrEmpty(evtMsg.MessageText) &&
						evtMsg.MessageText.Contains("name") &&
						evtMsg.MessageText.Contains("args"))
					{
						evtMsg.Json = JsonEncodedEventMessage.Deserialize(evtMsg.MessageText);
						evtMsg.Event = evtMsg.Json.name;
					}
					else
						evtMsg.Json = new JsonEncodedEventMessage();
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
			}
			return evtMsg;
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
