using System;

namespace IW4ServerInfo
{
	public class NoInfoException : Exception
	{
		public NoInfoException ()
		{
			Console.WriteLine ("Error: Impossible to retrieve correct server informations!");
		}
	}
}

