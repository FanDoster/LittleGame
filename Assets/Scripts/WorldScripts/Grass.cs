using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
	public virtual void Start()
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		WorldManager worldManager = WorldManager.Instance();

		Material[] grassMaterials = worldManager.GrassMaterials;
		Debug.Assert( grassMaterials != null && grassMaterials.Length > 1 );
		Material randomMaterial = grassMaterials[UnityEngine.Random.Range( 0, grassMaterials.Length )];
		meshRenderer.material = randomMaterial;



		// Create some trees on myself.
		float areaPerTree = 50.0f;
		MeshCollider meshCollider = GetComponent<MeshCollider>();
	    Bounds bounds = meshCollider.bounds;
		// XZ plane.
		Vector2 size = new Vector2( bounds.max.x - bounds.min.x, bounds.max.z - bounds.min.z );
		float area = size.x * size.y;
		float trees = area / areaPerTree;
		float localX = bounds.min.x;
		float localY = transform.position.y;
		float localZ = bounds.min.z;
		for( int i = 0; i < trees; ++i )
		{
			// Put a tree inside the area.
			float x = UnityEngine.Random.Range( 0, size.x ) + localX;
			float y = (4.76f*0.5f) + localY;
			float z = UnityEngine.Random.Range( 0, size.y ) + localZ;
			GameObject tree = Instantiate<GameObject>( worldManager.TreePrefab, new Vector3( x, y, z ), Quaternion.identity, transform );
			tree.transform.parent = transform;
			tree.transform.position = new Vector3( x, y, z );
		}
	}
}
