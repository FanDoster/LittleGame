using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The point of this class is 
/// </summary>
public class DestroyObjectsOnComplete : BaseTask
{
    private List<GameObject> m_destoryList;

    public DestroyObjectsOnComplete( List<GameObject> objectsToDestroy )
    {
        m_destoryList = objectsToDestroy;
    }

    public override void Start( object startObject = null )
    {
        base.Start( startObject );
        for( int i = 0; i < m_destoryList.Count; ++i )
        {
            GameObject.Destroy( m_destoryList[i] );
        }
    }
}
