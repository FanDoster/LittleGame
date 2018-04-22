using UnityEngine.SceneManagement;
using UnityEngine;

public class AsyncLoadScene : BaseTask
{
    private string m_sceneName;
    private AsyncOperation m_async = null;

    public AsyncLoadScene( string sceneName )
    {
        m_sceneName = sceneName;
    }

    public override void Start( object startObject = null )
    {
        base.Start( startObject );
        m_async = SceneManager.LoadSceneAsync( m_sceneName, LoadSceneMode.Additive );
    }

    public override void Update()
    {
        base.Update();
        if( m_async.isDone )
        {
            SetComplete( true );
        }
    }
}
