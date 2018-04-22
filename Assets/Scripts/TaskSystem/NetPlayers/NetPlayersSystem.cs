using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using UnityEngine.Networking;

public class NetPlayersSystem : BaseTaskSystem<NetPlayersSystem>
{
    private List<NetPlayer> m_NetPlayers = new List<NetPlayer>();
	private NetworkManager m_NetworkManager = null;

	public virtual void Start()
	{
		m_NetworkManager = NetworkManager.Instance();

		m_NetworkManager.RegisterForConnectionEvent( OnConnectionEvent );
	}

	public virtual void OnDestroy()
	{
		m_NetworkManager.UnregisterForConnectionEvent( OnConnectionEvent );
	}

	public void ReceiveNetPlayersList( List<NetPlayer> newPlayers )
    {
        AddTask( new ProcessPlayerList( newPlayers ) );
    }

    public void AddPlayer( NetPlayer newPlayer )
    {
        Debug.Log( "Adding player " + newPlayer.IP + " : " + newPlayer.PlayerID );
    }

	private void OnConnectionEvent( NetworkEventType networkEvent, int hostID, int connectionID )
	{

	}

    public List<NetPlayer> GetAllPlayers()
    {
        return m_NetPlayers;
    }

    public void SendPlayersInfo()
    {
        ByteArray buffer = m_NetworkManager.GetBufferToSend();

        buffer.Reset();
        buffer.WriteHead( (int)NetworkManager.PacketHeader.PlayersInfo );
        buffer.WriteInt( m_NetPlayers.Count );

        for( int i = 0; i < m_NetPlayers.Count; ++i )
        {
            buffer.WriteString( m_NetPlayers[i].IP );
            buffer.WriteInt( m_NetPlayers[i].PlayerID );
        }

		m_NetworkManager.SendBuffer();
    }
}
