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
			if (args.Length > 0) {

				IW4ServerInfo server = new IW4ServerInfo (args[0]);

				Console.WriteLine ("This server is running: " + server.getGameName() + "\n" + server.getHostName () + " -> " + server.getMapName () + " ("+ server.getCommonMapName() + ") " + server.getNumberPlayers () + "/" + server.getMaxClients () + " PLAYERS\nCURRENT PLAYERS LIST:\n" + server.getCurrentPlayersList ());
				//Console.WriteLine (server.getAllData());
			} else {
				Console.WriteLine ("usage: IW4ServerInfo [ip address]:[port]");
			}
		}
	}
}
