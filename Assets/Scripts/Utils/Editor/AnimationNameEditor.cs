using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AnimationName), true)]
public class AnimationNameEditor : PropertyDrawer
{
    private string[] availableAnimations;
    private int animIndex;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.IndentedRect(position);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        SerializedProperty nameproperty = property.FindPropertyRelative("Name");
        SerializedProperty indexProperty = property.FindPropertyRelative("CurrentIndex");
        SerializedProperty overrideAnimationProperty = property.FindPropertyRelative("CanBeCancelled");
        SerializedProperty maxBufferPercentageProperty = property.FindPropertyRelative("MaxBufferPercentage");
        SerializedProperty acceptableOverrideTimeProperty = property.FindPropertyRelative("AcceptableOverrideTime");

        // Set index to stored value to prevent name from being overwritten.
        animIndex = indexProperty.intValue;

        // Gets the component the AnimationName has been added to.
        Component comp = property.serializedObject.targetObject as Component;

        // Attempt to retreive animator from component.
        Animator anim = comp.GetComponent<Animator>();

        // If there is no editor then notify and abort.
        if (anim == null)
        {
            Debug.Log("GameObject does not contain an Animator! AnimationName will be set to null!");
            return;
        }

        // Retreive all animation clips being held by the animator.
        AnimationClip[] animClips = anim.runtimeAnimatorController.animationClips;

        // Create a string array the same length as the animClips array.
        availableAnimations = new string[animClips.Length];

        int i = 0;

        // Populate string array with the names of the animation clips
        // (can't use a list because Unity bad).
        foreach (AnimationClip clip in animClips)
        {
            availableAnimations[i] = clip.name;
            i++;
        }

        // Begin checking for GUI changes.
        EditorGUI.BeginChangeCheck();
        // Serialize the index as a dropdown of available animation names.
        animIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, position.height/2), animIndex, availableAnimations);

        EditorGUI.PropertyField(new Rect(position.x, position.y + 15, position.width, position.height / 2), overrideAnimationProperty);
        EditorGUI.PropertyField(new Rect(position.x, position.y + 30, position.width, position.height / 2), maxBufferPercentageProperty);
        EditorGUI.PropertyField(new Rect(position.x, position.y + 45, position.width, position.height / 2), acceptableOverrideTimeProperty);

        // Once check is complete set the name property to the selected animation name.
        if (EditorGUI.EndChangeCheck())
        {
            nameproperty.stringValue = availableAnimations[animIndex];
        }

        // If the selected index hasn't changed then there's nothing to save.
        if (indexProperty.intValue == animIndex)
        {
            return;
        }

        // Store the new index to save the selection.
        indexProperty.intValue = animIndex;

        // Mark the object the AnimationName has been added to as dirty.
        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float extraHeight = 90f;
        return base.GetPropertyHeight(property, label) + extraHeight;
    }
}