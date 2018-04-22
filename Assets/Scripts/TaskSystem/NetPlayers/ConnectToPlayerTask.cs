using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Connects to a player.
/// </summary>
public class ConnectToPlayerTask : BaseTask
{
    private NetPlayer m_ConnectToPlayer = null;

    public ConnectToPlayerTask( NetPlayer player )
    {
        m_ConnectToPlayer = player;
    }

    public override void Start( object startObject = null )
    {
        base.Start( startObject );
        NetworkManager.Instance().RegisterForConnectionEvent( OnConnectEvent );
    }

    protected override void SetComplete( bool bSuccess )
    {
        base.SetComplete( bSuccess );
        NetworkManager.Instance().UnregisterForConnectionEvent( OnConnectEvent );
    }

    public override void Update()
    {
        base.Update();

        NetworkManager.Instance().ConnectToAddress( m_ConnectToPlayer.IP );
    }

    private void OnConnectEvent( NetworkEventType networkEvent, int hostID, int connectionID )
    {
        if( networkEvent == NetworkEventType.ConnectEvent )
        {
            int port;
            ulong network;
            ushort dstNode;
            byte error;
            string address;
            
            // Find out if that is who we were wanting to connect to
            address = NetworkTransport.GetConnectionInfo( hostID, connectionID, out port, out network, out dstNode, out error );
            if( error != (byte)NetworkError.Ok )
            {
                SetComplete( false );
                Debug.LogError( (NetworkError)error );
            }
            else
            {
                Debug.Log( address );
                SetComplete( true );
            }
        }
    }
}
