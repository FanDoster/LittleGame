
using UnityEngine;

/// <summary>
/// Contains all of the information needed to connect to a player
/// on the network.
/// </summary>
public class NetPlayer
{
    public string IP;
    public int PlayerID;
    public GameObject PlayerObject;

    public NetPlayer( string ip, int playerid )
    {
        IP = ip;
        PlayerID = playerid;
        PlayerObject = null; // TODO.
    }
}
