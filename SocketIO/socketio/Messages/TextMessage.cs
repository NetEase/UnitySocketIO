using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketIOClient;
using System.Text.RegularExpressions;


namespace SocketIOClient.Messages
{
    public class TextMessage : Message
    {
		private string eventName = "message";
		public override string Event
		{
			get	{ return eventName;	}
		}

        public TextMessage()
        {
            this.MessageType = SocketIOMessageTypes.Message;
        }
		public TextMessage(string textMessage) : this()
		{
			this.MessageText = textMessage;
		}

        public static TextMessage Deserialize(string rawMessage)
        {
			TextMessage msg = new TextMessage();
            //  '3:' [message id ('+')] ':' [message endpoint] ':' [data]
            //   3:1::blabla
			msg.RawMessage = rawMessage;

            string[] args = rawMessage.Split(SPLITCHARS, 4);
			if (args.Length == 4)
			{
				int id;
				if (int.TryParse(args[1], out id))
					msg.AckId = id;
				msg.Endpoint = args[2];
				msg.MessageText = args[3];
			}
			else
				msg.MessageText = rawMessage;
			
			return msg;
        }
    }
}
