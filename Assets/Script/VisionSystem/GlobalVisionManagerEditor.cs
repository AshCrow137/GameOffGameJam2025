//using UnityEditor;
//using UnityEngine;

///// <summary>
///// Custom editor for GlobalVisionManager that adds a button to refresh visibility in the Inspector
///// </summary>
//[CustomEditor(typeof(GlobalVisionManager))]
//public class GlobalVisionManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        // Draw the default inspector
//        DrawDefaultInspector();

//        // Add some space
//        EditorGUILayout.Space(10);

//        // Get reference to the target component
//        GlobalVisionManager visionManager = (GlobalVisionManager)target;

//        // Add a button to refresh visibility
//        if (GUILayout.Button("Refresh All Tiles Visibility", GUILayout.Height(30)))
//        {
//            if (Application.isPlaying)
//            {
//                visionManager.RefreshVisibilityForEntireScene();
//                Debug.Log("Refreshed visibility for entire scene!");
//            }
//            else
//            {
//                Debug.LogWarning("This button only works in Play Mode!");
//            }
//        }

//        // Add helpful info text
//        EditorGUILayout.HelpBox(
//            "Click the button above to refresh fog of war visibility for all tiles in the scene. " +
//            "This will update all entities based on the current fog state.\n\n" +
//            "Note: This button only works during Play Mode.",
//            MessageType.Info
//        );
//    }
//}

