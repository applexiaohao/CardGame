// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
//using UnityEngine.Networking;

namespace AssemblyCSharp
{
	public class LO_GameServer:MonoBehaviour
	{
		private LO_GameServer ()
		{
		}

		private static GameObject s_LO_GameServer_object;
		private static LO_GameServer s_LO_GameServer = null;
		private static NetworkView s_LO_NetworkView = null;

		public static LO_GameServer DefaultServer
		{
			get{
				if (s_LO_GameServer == null) 
				{
					s_LO_GameServer_object = new GameObject("DefaultServer");
					s_LO_GameServer = s_LO_GameServer_object.AddComponent<LO_GameServer>();
					s_LO_NetworkView = s_LO_GameServer_object.AddComponent<NetworkView>();
				}

				return s_LO_GameServer;
			}
		}

		private static NetworkView DefalutNetworkView
		{
			get{
				return s_LO_NetworkView;
			}
		}


		/// <summary>
		/// init server...
		/// </summary>
		/// <param name="ip">Ip.</param>
		/// <param name="port">Port.</param>
		public bool InitServer(string ip,int port)
		{
			//set property
			MasterServer.ipAddress = ip;
			MasterServer.port = port;

			return true;
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <returns><c>true</c>, if server was started, <c>false</c> otherwise.</returns>
		public bool StartServer(string _name, int _port)
		{
			//start...
			Network.InitializeServer(10,_port,!Network.HavePublicAddress());

			//register a game
			MasterServer.RegisterHost("Card",_name);

			return true;
		}

        public void EndServer()
        {
            Network.Disconnect();
            MasterServer.UnregisterHost();
        }

		public delegate void RequestRoomComplete(HostData[] list);
		private RequestRoomComplete complete_block = null;
		public RequestRoomComplete CompleteBlock{
			set{
				complete_block = value;
			}
			get{
				return complete_block;
			}
		}

		public void StartRequestRoom(RequestRoomComplete block)
		{
			LO_GameServer.DefaultServer.CompleteBlock = block;

			MasterServer.RequestHostList("Card");
		}


		public delegate void JoinHostRoomDelegate(int state);

		private JoinHostRoomDelegate join_delegate = null;
		public void JoinHostRoom(HostData room,JoinHostRoomDelegate block)
		{
			this.join_delegate = block;

			NetworkConnectionError error = Network.Connect(room.ip[0],room.port);

			Debug.Log(error);
		}

        //执行RPC方法...
		public void SendGameMessage(string message)
		{
			LO_GameServer.DefalutNetworkView.RPC("RemoteReceiveMessage",RPCMode.All,message);
		}

        //RPC方法...
		[RPC]
		public void RemoteReceiveMessage(string message)
		{
			Debug.Log(message);
		}

		#region Behaviour Actions
		/// <summary>
		/// some event notification from master server
		/// </summary>
		/// <param name="ev">Ev.</param>
		public void OnMasterServerEvent(MasterServerEvent ev)
		{
			switch (ev) {
			case MasterServerEvent.RegistrationSucceeded:
			{
				break;
			}
				
			case MasterServerEvent.RegistrationFailedNoServer:
			{
				break;
			}
			case MasterServerEvent.RegistrationFailedGameType:
			{
				break;
			}
			case MasterServerEvent.RegistrationFailedGameName:
			{
				break;
			}
			case MasterServerEvent.HostListReceived:
			{
				CompleteBlock(MasterServer.PollHostList());
				break;
			}
			default:
				break;
			}

		}


		public void OnPlayerConnected(NetworkPlayer player)
		{
            //Debug.Log(Network.connections.Length);
            if (Network.connections.Length > 1)
            {
                DefalutNetworkView.RPC("JoinRoom", RPCMode.All, null);
            }
    
		}

		public void OnConnectedToServer()
		{
			this.join_delegate(0);
			Debug.Log("Connected to server success!");
		}

		#endregion

        [RPC]
        void JoinRoom()
        {
            Application.LoadLevel("Main");
        }

        void OnGUI()
        {
            switch (Network.peerType)
            {
                case NetworkPeerType.Client:
                    GUILayout.Label("Client..");
                    break;
                case NetworkPeerType.Connecting:
                    GUILayout.Label("Connecting..");
                    break;
                case NetworkPeerType.Disconnected:
                    GUILayout.Label("disconnected..");
                    break;
                case NetworkPeerType.Server:
                    {
                        GUILayout.Label("server..");
                        for (int i = 0; i < Network.connections.Length; i ++ )
                        {
                            GUILayout.Label("IP=" + Network.connections[i].ipAddress);
                        }
                        break;
                    }

                default:
                    break;
            }
        }
	}
}






