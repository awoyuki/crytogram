#region namespace
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
#endregion

[CustomEditor(typeof(MonoBehaviour), true)]
public class CuonglhButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mTarget = (MonoBehaviour) target;
        var methodInfors = mTarget.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var methodInfor in methodInfors)
        {
            var buttonEditor = methodInfor.GetCustomAttribute(typeof(ButtonEditor), true);
            if (buttonEditor != null)
            {
                if (GUILayout.Button(methodInfor.Name))
                {
                    methodInfor.Invoke((object)mTarget, null);
                }
            }
        }
    }
}
