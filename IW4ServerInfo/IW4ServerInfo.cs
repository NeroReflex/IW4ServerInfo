﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace IW4ServerInfo
{
	public class IW4ServerInfo
	{

		private String infoMsg = "";

		public IW4ServerInfo (String serverHostPort)
		{
			String[] splitHostPort = serverHostPort.Split (':');

			String hostaddress = splitHostPort[0];
			int port = ((splitHostPort.Length > 1) && (int.TryParse(splitHostPort[1], out port))) ? port : 28960;

			//generate the get status message for the server
			byte[] bytCommand = System.Text.Encoding.ASCII.GetBytes("xxxxxgetstatus");
			bytCommand[0] = byte.Parse("255");
			bytCommand[1] = byte.Parse("255");
			bytCommand[2] = byte.Parse("255");
			bytCommand[3] = byte.Parse("255");
			bytCommand[4] = byte.Parse("02");

			//connect to the server and do actions
			try
			{
				//connect to the server
				System.Net.Sockets.Socket client = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
				client.ReceiveTimeout = 999;
				client.SendTimeout = 999;
				client.Connect(IPAddress.Parse(hostaddress), port);

				//send the getstatus message to the server
				client.Send(bytCommand, System.Net.Sockets.SocketFlags.None).ToString();

				//store the returned buffer and create a string out of it
				byte[] bufferRec = new byte[65001];
				client.Receive(bufferRec);
				this.infoMsg = System.Text.Encoding.ASCII.GetString(bufferRec);
			}
			catch
			{
				this.infoMsg = "";
				throw new ServerConnectionException ();
			}
		}

		public string getAllData()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			return this.infoMsg;
		}

		public string getGameName()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\gamename\\";
			String gameName = "";

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				gameName = this.infoMsg.Substring(startingPoint, endPoint - startingPoint);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return gameName;
		}

		public String getHostName()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\sv_hostname\\";
			String hostName = "";

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				hostName = this.infoMsg.Substring(startingPoint, endPoint - startingPoint);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return hostName;
		}

		public String getMapName()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\mapname\\";
			String mapName = "";

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				mapName = this.infoMsg.Substring(startingPoint, endPoint - startingPoint);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return mapName;
		}

		public String getCommonMapName()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String commonMapName = "";
			commonMapName = checkRealMapName(getMapName());

			return commonMapName;
		}

		private string checkRealMapName(string mapName)
		{
			if (getGameName().Equals("IW4"))
			{
				if (mapName.Equals ("mp_checkpoint"))
					mapName = "Karachi";
				else if (mapName.Equals ("mp_boneyard"))
					mapName = "Scrapyard";
				else if (mapName.Equals ("mp_nightshift"))
					mapName = "Skidrow";
				else if (mapName.Equals ("mp_subbase"))
					mapName = "Sub Base";
				else if (mapName.Equals ("mp_brecourt"))
					mapName = "Wasteland";
				else if (mapName.Equals ("mp_complex"))
					mapName = "Bailout";
				else if (mapName.Equals ("mp_compact"))
					mapName = "Salvage";
				else if (mapName.Equals ("mp_abandon"))
					mapName = "Carnival";
				else if (mapName.Equals ("mp_fuel2"))
					mapName = "Fuel";
				else if (mapName.Equals ("mp_trailerpark"))
					mapName = "Trailer Park";
				else if (mapName.Equals ("mp_nuked"))
					mapName = "Nuketown";
				else if (mapName.Equals ("oilrig"))
					mapName = "Oilrig";
				else if (mapName.Equals ("invasion"))
					mapName = "BurgerTown";
				else if (mapName.Equals ("iw4_credits"))
					mapName = "It's basicly A Cube";
				else if (mapName.Equals ("gulag"))
					mapName = "Gulag";
				else if (mapName.Equals ("contingency"))
					mapName = "Contingency";
				else if (mapName.Equals ("so_ghillies"))
					mapName = "Pripyat";
				else
					mapName = mapName.Substring (3, 1).ToUpper () + mapName.Substring (4, mapName.Length - 4);
			}
			
			return mapName;
		}

		public int getMaxClients()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\sv_maxclients\\";
			int max_player = 0;

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				int.TryParse(this.infoMsg.Substring(startingPoint, endPoint - startingPoint), out max_player);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return max_player;
		}

		public int getMaxPing()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\sv_maxPing\\";
			int max_ping = 0;

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				int.TryParse(this.infoMsg.Substring(startingPoint, endPoint - startingPoint), out max_ping);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return max_ping;
		}

		public int getMinPing()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\sv_minPing\\";
			int min_ping = 0;

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length;
				int endPoint = this.infoMsg.IndexOf("\\", startingPoint);

				int.TryParse(this.infoMsg.Substring(startingPoint, endPoint - startingPoint), out min_ping);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return min_ping;
		}

		public int getNumberPlayers()
		{
			string num_players = getAllPlayersListData ();

			int count = 0, i = 0;

			while ((i = num_players.IndexOf('"', i)) != -1)
			{
				count++;
				// Increment the index.
				i++;
			}

			return count / 2;
		}

		public string getCurrentPlayersList()
		{
			string list_players = getAllPlayersListData ();
			int num_players = getNumberPlayers ();

			if (num_players == 0)
				return "No active players";

			string[] array_nomi = new string[num_players];
			int a = 0, i = 0, z = 0;
			string temp = "";
			string lista = "";
			int marker = 1;

			while ((i = list_players.IndexOf('"', i)) != -1)
			{
				if (marker == 1)
					marker = 0;
				else
					marker = 1;

				if (marker != 0)
				{	
					temp = list_players.Substring(a + 1, i - a - 1);
					array_nomi [z++] = temp;
				}
				a = i;

				// Increment the index.
				i++;
			}

			for (int k = 0; k < num_players; k++)
				if (k == 0)
					lista = array_nomi[k];
				else
					lista = lista + "\n" + array_nomi[k];

			return lista;
		}

		private string getAllPlayersListData()
		{
			if (this.infoMsg.Length <= 50) throw new NoInfoException();

			String searchStr = "\\sv_tournament\\";
			string allplayersdata = "";

			try
			{
				int startingPoint = this.infoMsg.IndexOf(searchStr) + searchStr.Length + 1;
				int endPoint = this.infoMsg.Length;

				allplayersdata = this.infoMsg.Substring(startingPoint, endPoint - startingPoint);
			}
			catch
			{
				throw new NoInfoException ();
			}

			return allplayersdata;
		}



	}
}