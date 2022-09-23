using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelMapGenerator))]
public class LevelMapGeneratorEditor : Editor
{
    private LevelMapGenerator instance;
    public override VisualElement CreateInspectorGUI()
    {
        instance = (LevelMapGenerator)target; 
        return base.CreateInspectorGUI();
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ClearLevelMap"))
        {
            instance.ClearLevelMap();
        }
        if (GUILayout.Button("CreateLevelMap"))
        {
            instance.CreateLevelMap();
        }
    }
}
