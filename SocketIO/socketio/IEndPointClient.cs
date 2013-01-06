using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient
{
	public interface IEndPointClient
	{
		void On(string eventName, Action<SocketIOClient.Messages.IMessage> action);
		void Emit(string eventName, Object payload, Action<Object>  callBack );

		void Send(SocketIOClient.Messages.IMessage msg);
	}
}
