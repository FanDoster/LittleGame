using UnityEngine;

public class WorldObject : MonoBehaviour
{
	private Transform MainCameraTransform = null;
	protected Transform myTransform = null;

	public virtual void Start()
	{
		WorldManager.Instance().AddWorldObject( this );
	}

	public virtual void OnDestroy()
	{
		WorldManager.Instance().RemoveWorldObject( this );
	}

	public virtual void Init( WorldManager.WorldObjectInitCache initCache )
	{
		myTransform = transform;
	}

	public virtual void update( float deltaTime )
	{

	}

	public virtual void lateUpdate( float deltaTime )
	{

	}
}
