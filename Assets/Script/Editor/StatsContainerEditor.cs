//#if UNITY_EDITOR
//using System;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(StatsModifierEffectData))]
//public class StatsContainerEditor : Editor
//{
//    private SerializedProperty _statsProp;

//    private void OnEnable()
//    {
//        _statsProp = serializedObject.FindProperty("StatsToModify");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        // Рисуем список
//        EditorGUILayout.PropertyField(_statsProp, includeChildren: true);

//        EditorGUILayout.Space(8);

//        using (new EditorGUILayout.HorizontalScope())
//        {
//            if (GUILayout.Button("Add Stat", GUILayout.Height(24)))
//                ShowAddMenu();

//            if (GUILayout.Button("Clear", GUILayout.Height(24)))
//            {
//                if (EditorUtility.DisplayDialog("Clear stats", "Remove all stats?", "Yes", "No"))
//                {
//                    _statsProp.ClearArray();
//                }
//            }
//        }

//        serializedObject.ApplyModifiedProperties();
//    }

//    private void ShowAddMenu()
//    {
//        var menu = new GenericMenu();

//        var baseType = typeof(StatsToModifyStruct);

//        // Находим все НЕ абстрактные типы, наследники StatBase
//        var statTypes = AppDomain.CurrentDomain.GetAssemblies()
//            .SelectMany(a =>
//            {
//                try { return a.GetTypes(); }
//                catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
//            })
//            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType)
//            .OrderBy(t => t.Name)
//            .ToArray();

//        if (statTypes.Length == 0)
//        {
//            menu.AddDisabledItem(new GUIContent("No StatBase implementations found"));
//        }
//        else
//        {
//            foreach (var t in statTypes)
//            {
//                menu.AddItem(new GUIContent(t.Name), false, () => AddStat(t));
//            }
//        }

//        menu.ShowAsContext();
//    }

//    private void AddStat(Type type)
//    {
//        serializedObject.Update();

//        int index = _statsProp.arraySize;
//        _statsProp.InsertArrayElementAtIndex(index);

//        var element = _statsProp.GetArrayElementAtIndex(index);

//        // Создаём экземпляр конкретного типа и кладём в managedReferenceValue
//        element.managedReferenceValue = Activator.CreateInstance(type);

//        serializedObject.ApplyModifiedProperties();
//    }
//}
//#endif


