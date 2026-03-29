using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
[CustomEditor(typeof(MonoBehaviour), true)]
public class GenericButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw normal inspector first
        DrawDefaultInspector();

        MonoBehaviour targetScript = (MonoBehaviour)target;
        MethodInfo[] methods = targetScript.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        bool hasButtons = false;

        foreach (MethodInfo method in methods)
        {
            // Only allow parameterless methods
            if (method.GetParameters().Length != 0)
                continue;

            // Skip Unity / inherited methods
            if (method.IsSpecialName)
                continue;

            // ✅ Only include methods with [Button]
            if (!Attribute.IsDefined(method, typeof(ButtonAttribute)))
                continue;

            if (!hasButtons)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
                hasButtons = true;
            }
            var attr = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
            string buttonLabel = string.IsNullOrEmpty(attr.Label) ? method.Name : attr.Label;

            if (GUILayout.Button(method.Name))
            {
                method.Invoke(targetScript, null);
            }
        }
    }
}
