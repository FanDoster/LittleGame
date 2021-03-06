using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvtDispatcher : Singleton<EvtDispatcher>
{
    public delegate void EVT_HANDLER (params object[] paramArr);

    private Dictionary<string, HashSet<EVT_HANDLER>> evtMap;

    public EvtDispatcher()
    {
        evtMap = new Dictionary<string, HashSet<EVT_HANDLER>>();
    }

    public void RegisterHandler(string evt, EVT_HANDLER handler)
    {

        HashSet<EVT_HANDLER> handlers;
        if (!evtMap.TryGetValue(evt, out handlers))
        {
            handlers = new HashSet<EVT_HANDLER>();
            evtMap.Add(evt, handlers);
        }

        if (handlers.Contains(handler))
        {
            Debug.LogError("Same handler register twice!!!");
            return;
        }

        handlers.Add(handler);
    }

    public void UnregisterHandler(string evt, EVT_HANDLER handler)
    {
        HashSet<EVT_HANDLER> handlers;
        if (!evtMap.TryGetValue(evt, out handlers))
            return;

        if (!handlers.Contains(handler))
            return;

        handlers.Remove(handler);
        if (handlers.Count == 0)
            evtMap.Remove(evt);
    }

    public void Dispatch(string evt, params object[] paramArr)
    {
        HashSet<EVT_HANDLER> handlers;
        if (!evtMap.TryGetValue(evt, out handlers))
            return;

        if (handlers.Count == 0)
            return;

        HashSet<EVT_HANDLER>.Enumerator iter = handlers.GetEnumerator();
        while (iter.MoveNext())
        {
            try
            {
                iter.Current.Invoke(paramArr);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(e.Message + "/n" + e.StackTrace);
#endif
            }
        }
    }

}
