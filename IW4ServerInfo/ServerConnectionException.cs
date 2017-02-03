using System;

namespace IW4ServerInfo
{
	public class ServerConnectionException : Exception
	{
		public ServerConnectionException()
		{
			Console.WriteLine ("Error: Selected server appears to be offline!");
		}
	}
}

