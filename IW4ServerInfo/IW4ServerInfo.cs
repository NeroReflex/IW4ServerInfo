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

	}
}

