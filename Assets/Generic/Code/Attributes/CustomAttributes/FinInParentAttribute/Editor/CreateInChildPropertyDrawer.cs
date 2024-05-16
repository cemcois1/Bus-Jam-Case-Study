#if UNITY_EDITOR
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using UnityEditor;
using UnityEngine;

namespace _SpesficCode.States.ObjectChangingState
{
    [CustomPropertyDrawer(typeof(CreateInChildAttribute))]
    public class CreateInChildPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect labelRect;
            var buttonWidth = 65;
            if (property.objectReferenceValue == null)
            {
                labelRect = new Rect(position.x, position.y, position.width - buttonWidth, position.height);

            }
            else
            {
                labelRect = new Rect(position.x, position.y, position.width, position.height);
            }

            Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);


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
                if (GUI.Button(buttonRect, "Create", greenButtonStyle))
                {
                    var attribute = this.attribute as CreateInChildAttribute;
                    var fieldType = fieldInfo.FieldType;
                    var script = property.serializedObject.targetObject as MonoBehaviour;

                    if (script.gameObject != null)
                    {

                        // Yeni bir transform oluştur
                        GameObject newGameObject = new GameObject((property.displayName));
                        newGameObject.transform.parent = script.transform;
                        // eğer component yoksa Transform bileşenini ekle
                        if (newGameObject.GetComponent(fieldType) == null)
                        {
                            Component newcomponent = newGameObject.AddComponent(fieldType);
                            // Yeni bileşeni referans olarak güncelle
                            property.objectReferenceValue = newcomponent;
                        }
                        else
                        {
                            // Eğer component varsa sadece referansı güncelle
                            property.objectReferenceValue = newGameObject.GetComponent(fieldType);
                        }
                        property.serializedObject.ApplyModifiedProperties();
                        
                    }
                }
            }

            EditorGUI.EndProperty();

        }
        
    }
}
#endif