using Network;
using UnityEngine;

public abstract class BaseSynchroniser : MonoBehaviour, ISynchroniser
{
	public virtual void Awake()
	{
		NetworkManager.Instance().GetSynchronisationManager().AddSynchroniser( this );
	}

	public virtual void OnDestroy()
	{
		NetworkManager.Instance().GetSynchronisationManager().RemoveSynchroniser( this );
	}

	public int GetID()
	{
		return 0;
	}

	public int GetOwner()
	{
		return 0;
	}

	public abstract void Read( ByteArray byteArray );

	public abstract void Write( ByteArray byteArray );


	// Update is called once per frame
	void Update()
	{

	}
}
