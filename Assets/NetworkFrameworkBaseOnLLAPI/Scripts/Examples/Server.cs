using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class EvtTestClass
{
    public EvtTestClass()
    {
        EvtDispatcher.Instance.RegisterHandler("TEST_EVT", TestHandler);
    }

    public void TestHandler(params object[] paramArr)
    {
        Debug.Log("Test Handler Called, Object instance: " + this.GetHashCode());
    }

}

public class Server2 : MonoBehaviour {
	// Use this for initialization
	void Start () {
        ServerTerminal.Instance.Init();
        PacketDispatcher.Instance.RegisterHandler(Network.Packets.PacketID.PACKET_TEST_PACKET, Handle_PACKET_TEST_PACKET);
        EvtDispatcher.Instance.RegisterHandler(NetworkEvt.EVT_REMOTE_TERMINAL_REGISTERED, OnRemoteTerminalRegistered);
	}
	
	// Update is called once per frame
	void Update () {
        ServerTerminal.Instance.BufferProcess();
	}

    private void Handle_PACKET_TEST_PACKET(IPacket packet)
    {
        Network.Packets.PACKET_TEST_PACKET msg = (Network.Packets.PACKET_TEST_PACKET)packet;
        Debug.Log("MsgReceive, ID: " + msg.GetPacketID());
    }

    private void OnRemoteTerminalRegistered(params object[] paramArr)
    {
        Debug.Log("Remote Terminal Registered, sending response");
        RemoteTerminalInfo info = (RemoteTerminalInfo)paramArr[0];
        Network.Packets.PACKET_CLIENT_CONN_RESPONSE response = new Network.Packets.PACKET_CLIENT_CONN_RESPONSE();
        ServerTerminal.Instance.SendPacketToClientReliably(response, info.ConnID);
    }
}
