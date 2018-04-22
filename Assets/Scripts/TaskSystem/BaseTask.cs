/// <summary>
/// Base class for a base task.
/// </summary>
public class BaseTask
{
    private bool m_bDone = false;
    private bool m_bSuccess = false;
    private bool m_bStarted = false;

    public virtual void Start( object startObject = null )
    {
        m_bStarted = true;
    }

    public virtual bool IsDone()
    {
        return m_bDone;
    }

    protected virtual void SetComplete( bool bSuccess )
    {
        m_bDone = true;
        m_bSuccess = bSuccess;
    }

    public virtual bool HasStarted()
    {
        return m_bStarted;
    }

    public virtual void Update()
    {

    }

    public virtual bool Success()
    {
        return m_bSuccess;
    }
}
