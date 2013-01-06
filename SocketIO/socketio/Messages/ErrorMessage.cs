using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient.Messages
{
    public class ErrorMessage : Message
    {

		public string Reason { get; set; }
		public string Advice { get; set; }

		public override string Event
		{
			get { return "error"; }
		}

		public ErrorMessage()
        {
            this.MessageType = SocketIOMessageTypes.Error;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rawMessage">'7::' [endpoint] ':' [reason] '+' [advice]</param>
		/// <returns>ErrorMessage</returns>
		public static ErrorMessage Deserialize(string rawMessage)
		{
			ErrorMessage errMsg = new ErrorMessage();
			string[] args = rawMessage.Split(':');
			if (args.Length == 4)
			{
				errMsg.Endpoint = args[2];
				errMsg.MessageText = args[3];
				string[] complex = args[3].Split(new char[] { '+' });
				if (complex.Length > 1)
				{
					errMsg.Advice = complex[1];
					errMsg.Reason = complex[0];
				}
			}
			return errMsg;
		}
    }
}
