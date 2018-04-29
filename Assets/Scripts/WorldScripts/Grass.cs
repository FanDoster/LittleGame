using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
	public virtual void Start()
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		if( meshRenderer != null )
		{
			WorldManager worldManager = WorldManager.Instance();
			Material[] grassMaterials = worldManager.GrassMaterials;
			Debug.Assert( grassMaterials != null && grassMaterials.Length > 1 );
			Material randomMaterial = grassMaterials[UnityEngine.Random.Range( 0, grassMaterials.Length )];
			meshRenderer.material = randomMaterial;
		}
	}
}
