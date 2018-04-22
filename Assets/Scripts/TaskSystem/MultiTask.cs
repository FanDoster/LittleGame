using System.Collections.Generic;

/// <summary>
/// A multi-stage task that can be added to whilst being initailsied with .AddTask()./
/// </summary>
public class MultiTask : BaseTask
{
    private List<BaseTask> m_TaskList = new List<BaseTask>();
    private BaseTask m_CurrentTask = null;
    private int m_TaskIndex = 0;
    public virtual void AddTask( BaseTask task )
    {
        m_TaskList.Add( task );
    }

    public override void Start( object startObject = null )
    {
        base.Start();
        // Start at the start of the task list.
        m_TaskIndex = 0;
        m_CurrentTask = m_TaskList[m_TaskIndex];
        m_CurrentTask.Start( startObject );
    }

    public void NextTask()
    {
        m_TaskIndex++;
        if( m_TaskIndex == m_TaskList.Count )
        {
            SetComplete( true );
        }
        else
        {
            // Start the new task.
            m_CurrentTask = m_TaskList[m_TaskIndex];
            m_CurrentTask.Start();
        }
    }

    public override void Update()
    {
        base.Update();
        
        if( m_CurrentTask.IsDone() == false )
        {
            m_CurrentTask.Update();
        }
        else
        {
            if( m_CurrentTask.Success() )
            {
                NextTask();
            }
            else
            {
                // One of the tasks failed so fail ourselves.
                SetComplete( false );
            }
        }
    }
}