using UnityEngine.Networking;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public virtual void Update()
    {
        if( Input.GetKeyDown( KeyCode.E ) )
        {
            NetworkManager.Instance().SendEcho( "Test echo" );
        }
    }
}
