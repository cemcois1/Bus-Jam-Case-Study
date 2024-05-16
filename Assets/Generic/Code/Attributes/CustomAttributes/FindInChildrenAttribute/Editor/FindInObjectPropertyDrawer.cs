#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _GenericPackageStart.Core.CustomAttributes
{
    [CustomPropertyDrawer(typeof(FindInObjectAttribute))]
    public class FindInObjectPropertyDrawer : PropertyDrawer
    {
        //değer olarak component tipini almalıyım
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
                    var attribute = this.attribute as FindInObjectAttribute;

                    var fieldType = fieldInfo.FieldType;
                    var script = property.serializedObject.targetObject as MonoBehaviour;

                    if (script != null && script.gameObject != null)
                    {

                        var component = script.transform.GetComponent(fieldType);
                        if (component != null)
                        {
                            property.objectReferenceValue = component;
                            property.serializedObject.ApplyModifiedProperties();
                        }
                    }
                    else
                    {
                        //cast to gameobject
                        var objectTarget = property.serializedObject.targetObject as Component;
                        //get component in children
                        if (objectTarget != null)
                        {
                            var component = objectTarget.GetComponent(fieldType);
                            if (component != null)
                            {
                                property.objectReferenceValue = component;
                                property.serializedObject.ApplyModifiedProperties();
                            }
                            else
                            {
                                Debug.LogError("Component Not Found".Red());
                            }
                        }
  

                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif