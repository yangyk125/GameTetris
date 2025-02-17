using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTetris
{
    public class GameUtility
    {
       
        public static void CleanChildren(Transform root)
        {
            int childCount = root.childCount;
            for (int idx = childCount - 1; idx >= 0; idx--)
            {
                Transform child = root.GetChild(idx);
                child.parent = null;
                Object.Destroy(child.gameObject);
            }
        }

        public static void SetTransformIdentity(Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }

}
