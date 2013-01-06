using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient
{
	class EndPointClient : IEndPointClient
	{
		public IClient Client { get; private set; }
		public string EndPoint { get; private set; }

		public EndPointClient(IClient client, string endPoint)
		{
			this.validateNameSpace(endPoint);
			this.Client = client;
			this.EndPoint = endPoint;
		}

		private void validateNameSpace(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("nameSpace", "Parameter cannot be null");
			if (name.Contains(':'))
				throw new ArgumentException("Parameter cannot contain ':' characters", "nameSpace");
		}
			
		public void On(string eventName, Action<Messages.IMessage> action)
		{
			this.Client.On(eventName, this.EndPoint, action);
		}

		public void Emit(string eventName, Object payload, Action<Object> callBack )
		{
			this.Client.Emit(eventName, payload, this.EndPoint, callBack);
		}

		public void Send(Messages.IMessage msg)
		{
			msg.Endpoint = this.EndPoint;
			this.Client.Send(msg);
		}
	}
}
