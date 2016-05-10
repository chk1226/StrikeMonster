using UnityEngine;
using System.Collections;


namespace StrikeMonster
{
    public class SMUtility{
 
        public static bool IntersectsCircleToRect(Vector2 circlePos, float circleRadius, Bounds rect)
        {
            Vector2 distance = new Vector2(Mathf.Abs(circlePos.x - rect.center.x), Mathf.Abs(circlePos.y - rect.center.y));
            
            if (distance.x > (rect.extents.x + circleRadius))
                return false;
            if (distance.y > (rect.extents.y + circleRadius))
                return false;
            
            
            if (distance.x <= rect.extents.x)
                return true;
            if (distance.y <= rect.extents.y)
                return true;
            
            float cornerDistance = Mathf.Pow(distance.x - rect.extents.x, 2) + Mathf.Pow(distance.y - rect.extents.y, 2);
            
            return cornerDistance <= Mathf.Pow(circleRadius, 2);
            
        }


        public static bool IntersectsRectToRect(Bounds preRect, Bounds postRect)
        {
            Vector3 reflect = new Vector3(-1, 1, 1);
            
            if(preRect.Contains(postRect.max) || preRect.Contains(postRect.min) || 
               preRect.Contains(postRect.center + Vector3.Scale(postRect.extents, reflect)) ||
               preRect.Contains(postRect.center - Vector3.Scale(postRect.extents, reflect)))
            {
                return true;
            }
            
            
            
            if(postRect.Contains(preRect.max) || postRect.Contains(preRect.min) || 
               postRect.Contains(preRect.center + Vector3.Scale(preRect.extents, reflect)) ||
               postRect.Contains(preRect.center - Vector3.Scale(preRect.extents, reflect)))
            {
                return true;
            }
            
            
            return false;
        }

    }

}


