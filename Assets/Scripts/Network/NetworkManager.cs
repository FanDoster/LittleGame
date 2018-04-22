using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Network;

public class NetworkManager : BaseTaskSystem<NetworkManager>
{
    public enum PacketHeader : short
    {
        Test = 0,
        PlayersInfo,
        Echo,
		SynchroniserMessage,
    }

    private GenericVoid<NetworkEventType, int, int> m_OnConnectionEvent = null;

    private int m_ReliableChannelID = -1;
    private int m_SocketID = -1;
	private SynchronisationManager m_SynchronisationManager = null;

    protected ByteArray m_Buffer;

    private List<NetworkConnection> m_Connections = new List<NetworkConnection>();

    private class NetworkConnection
    {
        public string IP = "";
        public int ConnectionID = -1;
        public int Port = -1;

        public NetworkConnection( string ip, int port )
        {
            IP = ip;
            Port = port;
        }
        public override string ToString()
        {
            return IP + ":" + Port;
        }
    }

    public override void Awake()
    {
        base.Awake();

		// Get the synchronisation manager from the same component.
		m_SynchronisationManager = GetComponent<SynchronisationManager>();

		GlobalConfig gc = new GlobalConfig();
        gc.ReactorModel = ReactorModel.FixRateReactor;
        gc.ThreadAwakeTimeout = 10;

        NetworkTransport.Init( gc );
        m_Buffer = new ByteArray( Network.NetworkConst.MAX_PACKET_LEN );

        ConnectionConfig config = new ConnectionConfig();
        m_ReliableChannelID = config.AddChannel( QosType.Reliable );

        HostTopology topo = new HostTopology( config, 10 );
        m_SocketID = NetworkTransport.AddHost( topo, 8888 );


		Debug.Log( "Host established " + m_SocketID );
    }

    public void ConnectToAddress( string address )
    {
        NetworkConnection connection = new NetworkConnection( address, 8888 );
        Connect( connection );
    }

    private void Connect( NetworkConnection connection )
    {
        byte error;
        connection.ConnectionID = NetworkTransport.Connect( m_SocketID, connection.IP, connection.Port, 0, out error );

        if( error != (byte)NetworkError.Ok )
        {
            LogNetworkError( error );
        }else
        {
            // Sucessfully connected to target.
            m_Connections.Add( connection );
            Debug.Log( "Started connecting to " + connection.ToString() );
        }
    }

    private void LogNetworkError( byte error )
    {
        if( error != (byte)NetworkError.Ok )
        {
            NetworkError realError = (NetworkError)error;
            Debug.LogError( "Network error : " + realError.ToString() );
        }
    }

    public void SendTest()
    {
        m_Buffer.Reset();
        m_Buffer.WriteHead( (short)PacketHeader.Test );
        m_Buffer.WriteString( "Hello, Network" );
        byte error;
        for( int i = 0; i < m_Connections.Count; ++i )
        {
            NetworkConnection current = m_Connections[i];
            bool bOkay = NetworkTransport.Send( m_SocketID, current.ConnectionID, m_ReliableChannelID, m_Buffer.BufferArray, m_Buffer.DataSize, out error );
            Debug.Log( "Send test returned " + bOkay );
            LogNetworkError( error );
        }
    }

    public void SendBuffer()
    {
        byte error;
        for( int i = 0; i < m_Connections.Count; ++i )
        {
            NetworkConnection current = m_Connections[i];
            NetworkTransport.Send( m_SocketID, current.ConnectionID, m_ReliableChannelID, m_Buffer.BufferArray, m_Buffer.DataSize, out error );
            LogNetworkError( error );
        }

        // Done, resent after transmission.
        m_Buffer.Reset();
    }
    
    public string GetIPAddress()
    {
        return UnityEngine.Network.player.ipAddress;
    }

    public void RegisterForConnectionEvent( GenericVoid<NetworkEventType, int, int> callback )
    {
        m_OnConnectionEvent += callback;
    }

    public void UnregisterForConnectionEvent( GenericVoid<NetworkEventType, int, int> callback )
    {
        m_OnConnectionEvent -= callback;
    }

    public void SendEcho( string message )
    {
        m_Buffer.Reset();
        m_Buffer.WriteHead( (short)PacketHeader.Echo );
        m_Buffer.WriteString( message );
        SendBuffer();
    }

    public override void Update()
    {
        base.Update();
        int recHostID;
        int recConnectionID;
        int recChannelId;
        int bufferSize = Network.NetworkConst.MAX_PACKET_LEN;
        int dataSize;
        byte error;

        NetworkEventType networkEvent;
        do
        {
            m_Buffer.Reset();
            networkEvent = NetworkTransport.Receive( out recHostID, out recConnectionID, out recChannelId, m_Buffer.BufferArray, bufferSize, out dataSize, out error );
            if( m_OnConnectionEvent != null && networkEvent != NetworkEventType.Nothing )
            {
                m_OnConnectionEvent( networkEvent, recHostID, recConnectionID );
            }

            switch( networkEvent )
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log( "<color=blue>Connection Established</color>" );
                    OnConnectEvent( recHostID, recConnectionID, recChannelId );
                    break;
                case NetworkEventType.DisconnectEvent:
                    Debug.Log( "<color=red>Disconnect Event</color>" );
                    break;
                case NetworkEventType.DataEvent:
                    //Debug.Log( "<color=blue>Data Received</color>" );
                    m_Buffer.ReadHead();
                    switch( (PacketHeader)m_Buffer.PacketId )
                    {
                        case PacketHeader.Test:
                            Debug.Log( "<color=red> Test message '" + m_Buffer.ReadString() +"'</color>" );
                            break;

                        case PacketHeader.Echo:
                            Debug.Log( "<color=red> Echo " + m_Buffer.ReadString() + "</color>" );
                            break;

						case PacketHeader.SynchroniserMessage:
							SynchronisationManager.Instance().ReceiveSerialisation( m_Buffer );
							break;


						default:
                            throw new System.Exception( "UNIMPLEMENTED NETWORK HEADER" );
                    }
                    break;

                case NetworkEventType.BroadcastEvent:
                    Debug.Log( "<color=blue>Broadcast Event</color>" );

                    break;
                case NetworkEventType.Nothing:
                    // Nothing to do.
                    break;
                default:
                    Debug.LogError( "Unknown message type" + networkEvent.ToString());
                    break;
            }
        } while( networkEvent != NetworkEventType.Nothing );
    }
	
    private void OnConnectEvent( int recvHostID, int recvConnectionID, int recvChannelID )
    {
        bool bFound = false;
        for( int i = 0; i < m_Connections.Count; ++i )
        {
            if( m_Connections[i].ConnectionID == recvConnectionID )
            {
                bFound = true;
                Debug.Log( "<color=yellow>New Connection from " + m_Connections[i].IP + "</color>" );
            }
        }

        if( bFound == false )
        {
            Debug.Log( "<color=red>NEW CONNECTION</color>" );
			NetworkConnection connection = new NetworkConnection( "newconnection", 8888 );
			connection.ConnectionID = recvConnectionID;
			m_Connections.Add( connection );
        }
    }
    

    /// <summary>
    /// Get the static buffer that's used to send data.
    /// </summary>
    /// <returns></returns>
    public ByteArray GetBufferToSend()
    {
        return m_Buffer;
    }

    /// <summary>
    /// Called when we need to read the players info from the server.
    /// </summary>
    private void ProcessPlayersInfo()
    {
        int playersCount = m_Buffer.ReadInt();
        List<NetPlayer> playersInfo = new List<NetPlayer>( playersCount );
        for( int i = 0; i < playersCount; ++i )
        {
            playersInfo.Add( new NetPlayer( m_Buffer.ReadString(), m_Buffer.ReadInt() ) );
        }

        NetPlayersSystem.Instance().ReceiveNetPlayersList( playersInfo );
    }


	public SynchronisationManager GetSynchronisationManager()
	{
		return m_SynchronisationManager;
	}
}
