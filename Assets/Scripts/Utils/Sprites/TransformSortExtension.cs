using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public static class TransformSortExtension
    {
        public static int GetSortingOrder(this Transform transform, float yOffset = 0)
        {
            int pixelsPerUnit = -100;
            return (int)((transform.position.y + yOffset) * pixelsPerUnit);
        }
    }
