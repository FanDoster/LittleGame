using System.Collections.Generic;
using UnityEngine;

public class BaseTaskSystem<T> : MonoBehaviourSingleton<T> where T: Component
{
    private List<BaseTask> m_Tasks = new List<BaseTask>();

    public virtual void Update()
    {
        for( int i = 0; i < m_Tasks.Count; ++i )
        {
            BaseTask current = m_Tasks[i];
            if( current.IsDone() == false )
            {
                current.Update();
            }
        }

        for( int i = m_Tasks.Count - 1; i >= 0; --i )
        {
            BaseTask current = m_Tasks[i];
            if( current.IsDone() == true )
            {
                // Task complete, remove from list.
                Debug.Log( "Task " + current.GetType() + " complete, removing from task list." );
                m_Tasks.RemoveAt( i );
            }
        }
    }

    public virtual void AddTask( BaseTask task )
    {
        m_Tasks.Add( task );
        task.Start();
    }
}
