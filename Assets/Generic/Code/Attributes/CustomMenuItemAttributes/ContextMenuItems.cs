using UnityEngine;
#if UNITY_EDITOR
using TMPro;
using UnityEditor;

public static class ContextMenuItems
{
    [MenuItem("CONTEXT/Component/Change Name to Component Name", priority = 1)]
    private static void PrintSelectedObjectInfo(MenuCommand command)
    {
        Component selectedComponent = (Component)command.context;
        
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedObject, "Print Object Info");
            
            selectedObject.name = PrintComponentName(selectedComponent);

        }
        else
        {
            Debug.Log("No GameObject selected.");
        }
    }

    [MenuItem("CONTEXT/CanvasGroup/Open Canvas", priority = 2)]
    private static void OpenCanvas(MenuCommand command)
    {
        CanvasGroup selectedComponent = (CanvasGroup)command.context;

        if (selectedComponent != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedComponent, "Open Canvas");
            selectedComponent.gameObject.SetActive(true);

            selectedComponent.alpha = 1;
            selectedComponent.interactable = true;
            selectedComponent.blocksRaycasts = true;
        }
        else
        {
            Debug.Log("No GameObject selected.");
        }
    }

    [MenuItem("CONTEXT/CanvasGroup/Close Canvas", priority = 3)]
    private static void CloseCanvas(MenuCommand command)
    {
        CanvasGroup selectedComponent = (CanvasGroup)command.context;

        if (selectedComponent != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedComponent, "Close Canvas");
            selectedComponent.gameObject.SetActive(false);
            selectedComponent.alpha = 0;
            selectedComponent.interactable = false;
            selectedComponent.blocksRaycasts = false;
        }
        else
        {
            Debug.Log("No GameObject selected.");
        }
    }

    [MenuItem("CONTEXT/Transform/LocalReset/LocalPosition", priority = 2)]
    private static void ResetPosition(MenuCommand command)
    {
        Transform selectedTransform = (Transform)command.context;

        if (selectedTransform != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedTransform, "Reset Position");

            // Konumu sıfırla
            selectedTransform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log("No Transform selected.");
        }
    }

    [MenuItem("CONTEXT/Transform/LocalReset/LocalRotation", priority = 3)]
    private static void ResetRotation(MenuCommand command)
    {
        Transform selectedTransform = (Transform)command.context;

        if (selectedTransform != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedTransform, "Reset Rotation");

            // Döndürmeyi sıfırla
            selectedTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Debug.Log("No Transform selected.");
        }
    }
    [MenuItem("CONTEXT/Transform/LocalReset/LocalScale", priority = 4)]
    private static void ResetScale(MenuCommand command)
    {
        Transform selectedTransform = (Transform)command.context;

        if (selectedTransform != null)
        {
            // Başlangıç durumunu kaydet
            Undo.RegisterCompleteObjectUndo(selectedTransform, "Reset Scale");

            // Ölçeği sıfırla
            selectedTransform.localScale = Vector3.one;
        }
        else
        {
            Debug.Log("No Transform selected.");
        }
    }

    private static void PrintObjectName(GameObject obj)
    {
        Debug.Log("Selected GameObject: " + obj.name);
    }

    private static string PrintComponentName(Component targetComponent)
    {
        if (targetComponent != null)
        {
            Debug.Log("Selected Component: " + targetComponent.GetType().Name);
            if (targetComponent.GetType()==typeof(TextMeshProUGUI))
            {
                TextMeshProUGUI textMeshProUGUI = (TextMeshProUGUI) targetComponent;
                return textMeshProUGUI.text+" Text";
                
            }
            return targetComponent.GetType().Name;
        }
        else
        {
            Debug.Log("No Component found on the selected GameObject.");
        }

        return "";
    }
}
#endif