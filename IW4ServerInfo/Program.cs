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
			IW4ServerInfo server = new IW4ServerInfo ("101.50.106.67:28961");

			Console.WriteLine (server.getHostName() + " -> " + server.getMapName() + " " + server.getNumberPlayers() + "/" + server.getMaxClients() + " PLAYERS\nCURRENT PLAYERS LIST:\n" + server.getCurrentPlayersList());
		}
	}
}
