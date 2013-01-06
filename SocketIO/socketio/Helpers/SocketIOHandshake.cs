using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient
{
    public class SocketIOHandshake
    {
        public string SID { get; set; }
        public int HeartbeatTimeout { get; set; }
		public string ErrorMessage { get; set; }
		public bool HadError
		{
			get { return !string.IsNullOrEmpty(this.ErrorMessage); }

		}
		/// <summary>
		/// The HearbeatInterval will be approxamately 20% faster than the Socket.IO service indicated was required
		/// </summary>
        public TimeSpan HeartbeatInterval
        {
            get
            {
                return new TimeSpan(0, 0, HeartbeatTimeout);
            }
        }
        public int ConnectionTimeout { get; set; }
        public List<string> Transports = new List<string>();

		public SocketIOHandshake()
		{

		}
        public static SocketIOHandshake LoadFromString(string value)
        {
            SocketIOHandshake returnItem = new SocketIOHandshake();
            if (!string.IsNullOrEmpty(value))
            {
                string[] items = value.Split(new char[] { ':' });
                if (items.Count() == 4)
                {
                    int hb = 0;
                    int ct = 0;
                    returnItem.SID = items[0];

                    if (int.TryParse(items[1], out hb))
                    { 
                        var pct = (int)(hb * .75);  // setup client time to occure 25% faster than needed
                        returnItem.HeartbeatTimeout = pct;
                    }
                    if (int.TryParse(items[2], out ct))
                        returnItem.ConnectionTimeout = ct;
                    returnItem.Transports.AddRange(items[3].Split(new char[] { ',' }));
                    return returnItem;
                }
            }
            return null;
        }
    }
}
