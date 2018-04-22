using System;

public class Singleton<T> where T : class, new()
{
    private static T m_Instance = null;
    private static bool m_CallingStaticMethod = false;

    public Singleton()
    {
        if (!m_CallingStaticMethod)
        {
            throw new Exception("This is a Singleton!");
        }
    }

    public static T GetInstance()
    {
        if (m_Instance == null)
        {
            m_CallingStaticMethod = true;
            m_Instance = new T();
            m_CallingStaticMethod = false;
        }

        return m_Instance;
    }

    public static T2 GetInstance<T2>() where T2: class, T, new()
    {
        if (m_Instance == null)
        {
            m_CallingStaticMethod = true;
            m_Instance = new T2();
            m_CallingStaticMethod = false;
        }

        return (T2)m_Instance;
    }

    public static T Instance
    {
        get
        {
            return GetInstance();
        }
    }

}
