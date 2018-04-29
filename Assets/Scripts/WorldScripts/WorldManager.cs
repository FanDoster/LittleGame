using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviourSingleton<WorldManager>
{
	public Material[] GrassMaterials;
	public Material[] TreeMaterials;
	public GameObject TreePrefab = null;
	private List<WorldObject> WorldObjects = new List<WorldObject>();
	private WorldObjectInitCache WorldObjectInit = new WorldObjectInitCache();

	/// <summary>
	/// This is a class that holds anything that a worldobject might want when it's created.
	/// This is just a big cache really.
	/// </summary>
	public class WorldObjectInitCache
	{
		public Camera MainCamera = null;
		public WorldManager Manager = null;
		public GameObject LocalPlayer = null;
	}

	public void AddWorldObject( WorldObject worldObject )
	{
		WorldObjects.Add( worldObject );

		worldObject.Init( WorldObjectInit );

	}

	public void RemoveWorldObject( WorldObject worldObject )
	{
		WorldObjects.Remove( worldObject );
	}

	public override void Awake()
	{
		base.Awake();

		// Find the main camera via the control script.
		CameraControl cameraControl = FindObjectOfType<CameraControl>();
		Debug.Assert( cameraControl != null );

		PlayerControls playerControls = FindObjectOfType<PlayerControls>();
		Debug.Assert( playerControls != null );

		WorldObjectInit.Manager = this;
		WorldObjectInit.MainCamera = cameraControl.gameObject.GetComponent<Camera>();
		WorldObjectInit.LocalPlayer = playerControls.gameObject;
	}

	public virtual void Update()
	{
		float deltaTime = Time.deltaTime;
		// Update all available world objects.
		for( int i = 0; i < WorldObjects.Count; ++i )
		{
			WorldObjects[i].update( deltaTime );
		}
	}

	public virtual void LateUpdate()
	{
		float deltaTime = Time.deltaTime;
		// Update all available world objects.
		for( int i = 0; i < WorldObjects.Count; ++i )
		{
			WorldObjects[i].lateUpdate( deltaTime );
		}
	}
}
