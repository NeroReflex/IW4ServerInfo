using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace IW4ServerInfo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IW4ServerInfo server = new IW4ServerInfo ("52.90.193.240:28960");

			Console.WriteLine (server.getHostName() + " -> " + server.getMapName() + " MAX " + server.getMaxClients() + " Players");
		}
	}
}
