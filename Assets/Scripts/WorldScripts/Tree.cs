using UnityEngine;

public class Tree : WorldObject
{
	// Use this for initialization
	public override void Init( WorldManager.WorldObjectInitCache initCache )
	{
		base.Init( initCache );
	
		Material[] treeMaterials = initCache.Manager.TreeMaterials;
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		Debug.Assert( treeMaterials != null && treeMaterials.Length > 0 );
		Material randomMaterial = treeMaterials[UnityEngine.Random.Range( 0, treeMaterials.Length )];
		meshRenderer.material = randomMaterial;
	}
}
