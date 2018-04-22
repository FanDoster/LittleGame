using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    private static T ms_instance;

    public virtual void Awake()
    {
        ms_instance = this as T;
        if( ms_instance == null )
        {
            throw new System.Exception( "Unable to create singleton for " + this.GetType() );
        }
    }

    public static T Instance()
    {
        return ms_instance;
    }
}
