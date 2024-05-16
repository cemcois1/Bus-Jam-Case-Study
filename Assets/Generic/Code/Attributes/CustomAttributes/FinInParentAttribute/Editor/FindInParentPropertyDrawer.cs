#if UNITY_EDITOR
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using UnityEditor;
using UnityEngine;

namespace _SpesficCode.States.ObjectChangingState
{
    [CustomPropertyDrawer(typeof(FindInParentAttribute))]
    public class FindInParentPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect labelRect;
            if (property.objectReferenceValue == null)
            {
                labelRect = new Rect(position.x, position.y, position.width - 35, position.height);

            }
            else
            {
                labelRect = new Rect(position.x, position.y, position.width, position.height);
            }

            Rect buttonRect = new Rect(position.x + position.width - 35, position.y, 45, position.height);


            GUIStyle greenButtonStyle = new GUIStyle(EditorStyles.miniButton);
            greenButtonStyle.normal.textColor = Color.green;

            GUIStyle redLabelStyle = new GUIStyle(EditorStyles.label);
            redLabelStyle.normal.textColor = Color.red;

            if (property.objectReferenceValue == null)
            {
                GUI.color = Color.red;

                EditorGUI.PropertyField(labelRect, property, true);
                GUI.color = Color.white;
            }
            else
            {
                EditorGUI.PropertyField(labelRect, property, true);
            }

            if (property.objectReferenceValue == null)
            {
                if (GUI.Button(buttonRect, "Find", greenButtonStyle))
                {
                    var attribute = this.attribute as FindInParentAttribute;

                    var fieldType = fieldInfo.FieldType;
                    var script = property.serializedObject.targetObject as MonoBehaviour;

                    if (script.gameObject != null)
                    {
                        var childComponent = script.transform.FindComponentInParents(fieldType,attribute.containInName);
        

                        if (childComponent != null)
                        {
                            property.objectReferenceValue = childComponent;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                        else
                        {
                            Debug.LogError("ChildComponent is Null");
                        }
                    }
                }
            }

            EditorGUI.EndProperty();

        }

    }
}
#endif