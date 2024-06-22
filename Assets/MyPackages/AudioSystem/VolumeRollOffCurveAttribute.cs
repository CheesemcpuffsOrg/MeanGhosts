using UnityEngine;

// These compiler flags let you put the script anywhere in your project
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Forces an AnimationCurve to keep its keys within a certain window. The default is a range of (x0, y0) = (0, 0) to (x1, y1) = (1, 1).
/// </summary>
public class VolumeRollOffCurveAttribute : PropertyAttribute
{

    public AudioRolloffMode rolloffMode;

    public VolumeRollOffCurveAttribute(AudioRolloffMode mode)
    {
        rolloffMode = mode;
    }
    /*public Rect bbox = new Rect(0f, 0f, 1f, 1f);
    public Color color = Color.green;

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute() { }

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute(Rect bbox)
    {
        this.bbox = bbox;
    }

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute(float xmin, float xmax, float ymin, float ymax)
    {
        bbox = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
    }

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute(Color color)
    {
        bbox = new Rect(0f, 0f, 1f, 1f);
        this.color = color;
    }

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute(float xmin, float xmax, float ymin, float ymax, Color color)
    {
        bbox = new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        this.color = color;
    }

    public AudioScriptableObjectVolumeRollOffAnimationCurveAttribute(Rect bbox, Color color)
    {
        this.bbox = bbox;
        this.color = color;
    }*/
}



#if UNITY_EDITOR
/*[CustomPropertyDrawer(typeof(AnimationCurveSimpleAttribute))]
public class AnimationCurveSimpleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AnimationCurveSimpleAttribute curve = attribute as AnimationCurveSimpleAttribute;

        // This attribute should only be used with AnimationCurves
        if (property.propertyType != SerializedPropertyType.AnimationCurve)
            throw new System.Exception("AnimationCurveSimple attribute can only be used with AnimationCurve properties, not " + property.propertyType);

        // Check if the property has an assigned AnimationCurve. Create one if it doesn't.
        // If it does, then make sure it has at least 2 keys to avoid empty curves in the inspector.
        if (property.animationCurveValue == null ? true : property.animationCurveValue.keys.Length <= 1)
            property.animationCurveValue = new AnimationCurve(new Keyframe(curve.bbox.xMin, curve.bbox.center.y), new Keyframe(curve.bbox.xMax, curve.bbox.center.y));

        // Draw the curve in the inspector (this is not the popup editor window)
        EditorGUI.CurveField(position, property, curve.color, curve.bbox, label);
    }
}*/

[CustomPropertyDrawer(typeof(VolumeRollOffCurveAttribute))]
public class VolumeRollOffCurveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VolumeRollOffCurveAttribute curveAttr = attribute as VolumeRollOffCurveAttribute;
        var component = property.serializedObject.targetObject as AudioScriptableObject;

        // Get the enum value from the serialized property
        if (component.volumeRollOffMode != curveAttr.rolloffMode)
            return; // Exit if enum value doesn't match

        // Ensure this attribute is only used with AnimationCurves
        if (property.propertyType != SerializedPropertyType.AnimationCurve)
        {
            EditorGUI.LabelField(position, label.text, "Use [VolumeRollOffCurve] with AnimationCurve.");
            return;
        }

        if (component == null)
        {
            EditorGUI.LabelField(position, label.text, "Component not found.");
            return;
        }

        // Get bbox from the component
        Rect bbox = new Rect(component.minDistance, 0, component.maxDistance - component.minDistance, 1);

        // Ensure the property has an assigned AnimationCurve with at least 2 keys
        if (property.animationCurveValue == null || property.animationCurveValue.keys.Length <= 1)
        {
            property.animationCurveValue = new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(component.maxDistance, component.minDistance)
            );
        }

        // Draw the curve field
        property.animationCurveValue = EditorGUI.CurveField(position, label, property.animationCurveValue, Color.green, bbox);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2.5f; // Adjust the height to fit the label and the curve field
    }
}
#endif
