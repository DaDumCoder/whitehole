#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Unparent
{
    [MenuItem("GameObject/Unparent", priority = 0, validate = false)]
    public static void UnparentObject()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
            Selection.gameObjects[i].transform.SetParent(null);        
    }
}
#endif