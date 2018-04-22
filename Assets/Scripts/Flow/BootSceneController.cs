using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BootSceneController : MonoBehaviour
{
    public Text m_IPText = null;
    public Text m_JoinIPText = null;
    public List<GameObject> m_DestroyOnLoad = new List<GameObject>();

    public void Start()
    {
        m_IPText.text += NetworkManager.Instance().GetIPAddress();
    }

    public void OnCreateServerPressed()
    {
        LoadHomeScene( true );
    }

    public void OnJoinServerPressed()
    {
        Debug.Log( "Joining server " + m_JoinIPText.text );
        LoadHomeScene( false );
    }

    private void LoadHomeScene( bool server )
    {
        MultiTask tasks = new MultiTask();
        if( server == false )
        {
            tasks.AddTask( new JoinGameTask( m_JoinIPText.text ) );
        }
        tasks.AddTask( new AsyncLoadScene( "HomeScene" ) );
        tasks.AddTask( new DestroyObjectsOnComplete( m_DestroyOnLoad ) );
        NetworkManager.Instance().AddTask( tasks );
    }
}
