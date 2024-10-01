using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneField : IEquatable<SceneField>
{
    [SerializeField]
    private UnityEngine.Object m_SceneAsset;

    [SerializeField]
    private string m_SceneName = "";
    public string SceneName
    {
        get { return m_SceneName; }
    }

    // makes it work with the existing Unity methods (LoadLevel/LoadScene)
    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }

    //By default, List<T>.Contains checks for equality using the Equals method.
    //For user-defined types like SceneField, Equals compares object references unless overridden.
    //This means that .Contains will only return true if the exact same instance is present in the list.

    //If you want .Contains to check for equality based on the SceneName property (e.g., comparing the scene names), you need to override the Equals and GetHashCode methods in the SceneField class.

    // Implement IEquatable<SceneField>
    public bool Equals(SceneField other)
    {
        if (other == null)
            return false;

        return this.SceneName == other.SceneName;
    }

    public override bool Equals(object obj)
    {
        if (obj is SceneField)
        {
            return Equals((SceneField)obj);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return SceneName.GetHashCode();
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, GUIContent.none, _property);
        SerializedProperty sceneAsset = _property.FindPropertyRelative("m_SceneAsset");
        SerializedProperty sceneName = _property.FindPropertyRelative("m_SceneName");
        _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
        if (sceneAsset != null)
        {
            sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

            if (sceneAsset.objectReferenceValue != null)
            {
                sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
            }
        }
        EditorGUI.EndProperty();
    }
}
#endif
