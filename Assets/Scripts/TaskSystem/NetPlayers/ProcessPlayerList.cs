using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessPlayerList : BaseTask
{
    private List<NetPlayer> m_NewPlayers = null;

    public ProcessPlayerList( List<NetPlayer> newPlayerList )
    {
        m_NewPlayers = newPlayerList;
    }

    // Use this for initialization
    public override void Start( object initObject)
    {
        
    }

    public override void Update()
    {
        base.Update();

        List<NetPlayer> NetPlayers = NetPlayersSystem.Instance().GetAllPlayers();
        // Look for players which have been lost.
        for( int i = NetPlayers.Count - 1; i >= 0; --i )
        {
            int currentPlayerID = NetPlayers[i].PlayerID;
            bool bRemove = true;
            for( int j = 0; j < m_NewPlayers.Count; ++j )
            {
                if( m_NewPlayers[j].PlayerID == currentPlayerID )
                {
                    bRemove = false;
                    break;
                }
            }

            if( bRemove )
            {
                NetPlayers.RemoveAt( i );
            }
        }


        // See if there are any new players here.
        for( int j = 0; j < m_NewPlayers.Count; ++j )
        {
            bool bAdd = true;
            int currentPlayerID = m_NewPlayers[j].PlayerID;
            for( int i = 0; i < NetPlayers.Count; ++i )
            {
                if( NetPlayers[i].PlayerID == currentPlayerID )
                {
                    // Not new don't add.
                    bAdd = false;
                    break;
                }
            }

            if( bAdd )
            {
                NetPlayersSystem.Instance().AddPlayer( NetPlayers[j] );
            }
        }

        // Can't really fail this task.
        SetComplete( true );
    }
}
