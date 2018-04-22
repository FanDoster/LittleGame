using Network;

public interface ISynchroniser
{
	int GetID();
	int GetOwner();
	void Write( ByteArray byteArray );
	void Read( ByteArray byteArray );
}
