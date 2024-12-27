using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventManager : Singleton<EventManager> {
    Dictionary<EventKey, Dictionary<object, List<MethodInfo>>> dic = new Dictionary<EventKey, Dictionary<object, List<MethodInfo>>>();

    public void Listen(EventKey key, object go, MethodInfo m)
    {
        if (dic.TryGetValue(key, out Dictionary<object, List<MethodInfo>> objectMethod))
        {
            if (objectMethod.TryGetValue(go, out List<MethodInfo> methods))
            {
                methods.Add(m);
            }
            else
            {
                objectMethod.Add(go, new List<MethodInfo>() { m });
            }
        }
        else
        {
            objectMethod = new Dictionary<object, List<MethodInfo>>();
            objectMethod.Add(go, new List<MethodInfo>() { m });
            dic.Add(key, objectMethod);
        }
    }

    public void Remove(EventKey key, object go, MethodInfo m)
    {
        if (dic.TryGetValue(key, out Dictionary<object, List<MethodInfo>> objectMethod))
        {
            if (objectMethod.TryGetValue(go, out List<MethodInfo> methods) && methods.Contains(m))
            {
                methods.Remove(m);
            }
        }
    }

    public void Push(EventKey key, params object[] datas)
    {
        if (dic.TryGetValue(key, out Dictionary<object, List<MethodInfo>> objectMethod))
        {
            foreach (var goMethod in objectMethod)
            {
                foreach (var m in goMethod.Value)
                {
                    m.Invoke(goMethod.Key, datas);
                }
            }
        }
    }
}