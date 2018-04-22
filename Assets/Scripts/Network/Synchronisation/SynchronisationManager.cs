using System.Collections.Generic;
using Network;

public class SynchronisationManager : BaseTaskSystem<SynchronisationManager>
{
	private int m_PeerID = 0;
	private List<ISynchroniser> m_Synchronisers = new List<ISynchroniser>();
	private List<ISynchroniser> m_OwnedSynchronisers = new List<ISynchroniser>();

	public void Serialise( ByteArray byteArray )
	{
		byteArray.WriteInt( m_Synchronisers.Count );
		foreach( ISynchroniser i in m_Synchronisers )
		{
			// Check to see if this machine is the owner, if so then serialise.
			if( i.GetOwner() == m_PeerID )
			{
				byteArray.WriteInt( i.GetID() );
				i.Write( byteArray );
			}
		}
	}

	public void ReceiveSerialisation( ByteArray byteArray )
	{
		int serialisationCount = byteArray.ReadInt();
		for( int i = 0; i < serialisationCount; ++i )
		{
			int id = byteArray.ReadInt();
			for( int j = 0; j < m_Synchronisers.Count; ++j )
			{
				if( m_Synchronisers[j].GetID() == id )
				{
					m_Synchronisers[j].Read( byteArray );
					break;
				}
			}
		}
	}

	public virtual void LateUpdate()
	{
		if( m_PeerID == 0 )
		{
			NetworkManager networkManager = NetworkManager.Instance();
			ByteArray byteArray = networkManager.GetBufferToSend();
			byteArray.Reset();
			byteArray.WriteHead( (short)NetworkManager.PacketHeader.SynchroniserMessage );
			Serialise( byteArray );
			NetworkManager.Instance().SendBuffer();
		}
	}

	public void AddSynchroniser( ISynchroniser synchroniser )
	{
		m_Synchronisers.Add( synchroniser );
	}

	public void RemoveSynchroniser( ISynchroniser synchroniser )
	{
		m_Synchronisers.Remove( synchroniser );
	}

	public void SetPeerID( int id )
	{
		m_PeerID = id;
	}
}
