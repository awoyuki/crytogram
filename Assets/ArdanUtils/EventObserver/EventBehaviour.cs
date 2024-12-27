using UnityEngine;
using System.Reflection;

public abstract class EventBehaviour : MonoBehaviour {

    protected virtual void OnEnable()
    {
        var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            var attributes = m.GetCustomAttributes(typeof(EventAttribute), true);
            foreach (var a in attributes)
            {
                var attribute = a as EventAttribute;
                EventManager.Instance.Listen(attribute.key, this, m);
            }
        }
    }

    protected virtual void OnDisable()
    {
        var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            var attributes = m.GetCustomAttributes(typeof(EventAttribute), true);
            foreach (var a in attributes)
            {
                var attribute = a as EventAttribute;
                EventManager.Instance.Remove(attribute.key, this, m);
            }
        }
    }
}

public abstract class EventClass<T> where T : new() {

    public EventClass()
    {
        var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            var attributes = m.GetCustomAttributes(typeof(EventAttribute), true);
            foreach (var a in attributes)
            {
                var attribute = a as EventAttribute;
                EventManager.Instance.Listen(attribute.key, this, m);
            }
        }
    }

    ~EventClass()
    {
        var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            var attributes = m.GetCustomAttributes(typeof(EventAttribute), true);
            foreach (var a in attributes)
            {
                var attribute = a as EventAttribute;
                EventManager.Instance.Remove(attribute.key, this, m);
            }
        }
    }
}