using Network;
using UnityEngine;

public class PositionSynchroniser : BaseSynchroniser
{
	public override void Write( ByteArray byteArray )
	{
		byteArray.WriteFloat( transform.position.x );
		byteArray.WriteFloat( transform.position.y );
		byteArray.WriteFloat( transform.position.z );
	}

	public override void Read( ByteArray byteArray )
	{
		transform.position = new Vector3( byteArray.ReadFloat(), byteArray.ReadFloat(), byteArray.ReadFloat() );
	}
}
