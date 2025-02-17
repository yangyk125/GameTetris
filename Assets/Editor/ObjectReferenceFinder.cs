using UnityEditor;
using UnityEngine;

namespace UnityEditor
{
    public class ObjectReferenceFinder
    {
        [MenuItem("Assets/Find Selection Referring Objects", true)]
        private static bool ValidateSelectionReferringObjects()
        {
            return Selection.activeObject != null;
        }

        [MenuItem("Assets/Find Selection Referring Objects")]
        private static void FindSelectionReferringObjects()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                Debug.LogError("No object selected.");
                return;
            }

            ObjectReferenceWindow.ShowWindow(ObjectReferenceSearchType.SelectionReferObject, selectedObject);
        }


        [MenuItem("Assets/Find Objects Referring Selection", true)]
        private static bool ValidateObjectsReferringSelection()
        {
            return Selection.activeObject != null;
        }

        [MenuItem("Assets/Find Objects Referring Selection")]
        private static void FindObjectsReferringSelection()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
            {
                Debug.LogError("No object selected.");
                return;
            }

            ObjectReferenceWindow.ShowWindow(ObjectReferenceSearchType.ObjectReferSelection, selectedObject);
        }
    }
}
