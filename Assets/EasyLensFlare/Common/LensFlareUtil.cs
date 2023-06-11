using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GR
{
    public static class LensFlareUtil
    {
        public static Vector2 GetElementPositionReferenceByOffsetWeight(Vector2 position, Vector2 center, float offsetWeight)
        {
            var distance = Vector2.Distance(position, center) * offsetWeight;
            var vector = (position - center).normalized;

            return center + vector * distance;
        }
    }
}
