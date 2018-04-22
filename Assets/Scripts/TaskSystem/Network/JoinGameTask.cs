using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

public class JoinGameTask : BaseTask
{
    private string m_GameAddress;
    private NetworkManager m_NetworkManager = null;
    public JoinGameTask( string address )
    {
        m_GameAddress = address;
    }

    public override void Start( object startObject = null )
    {
        base.Start( startObject );
		SynchronisationManager.Instance().SetPeerID( 1 );
        m_NetworkManager = NetworkManager.Instance();
        m_NetworkManager.RegisterForConnectionEvent( OnConnectionEvent );
        m_NetworkManager.ConnectToAddress( m_GameAddress );
    }

    private void OnConnectionEvent( NetworkEventType networkEvent, int hostID, int connectionID )
    {
        if( networkEvent == NetworkEventType.ConnectEvent )
        {
            SetComplete( true );
        }
        else if( networkEvent == NetworkEventType.DisconnectEvent )
        {
            SetComplete( false );
        }
    }

    protected override void SetComplete( bool bSuccess )
    {
        base.SetComplete( bSuccess );
        Debug.Log( "Join game task success? " + bSuccess );
        m_NetworkManager.UnregisterForConnectionEvent( OnConnectionEvent );
    }
}
