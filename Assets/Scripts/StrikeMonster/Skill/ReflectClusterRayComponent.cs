using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class ReflectClusterRayComponent : ClusterRayComponent {
        private int m_ReflectNum = 5;

        public override void Initialize(uint rayNum, float hitIntervalTime, float lifeTime, float size, Color color, RayComponent.CollisionCallback collisionEvent)
        {
            base.Initialize(rayNum, hitIntervalTime, lifeTime, size, color, collisionEvent);

            for(int i = 0; i < ClusterRay.Count; i ++)
            {
                ClusterRay[i].IntersectEventTrigger = ReflectionBehavior;
            }


        }


        private void InsertRay(Vector3 locPosition, Vector3 dir, List<GameObject> intersect)
        {
            var clone = Instantiate<GameObject>(RayPrefab);
            if(clone)
            {
                var rayClone = clone.GetComponent<RayComponent>();
                if (rayClone)
                {

                    rayClone.transform.SetParent(this.transform,false);
                    rayClone.transform.localPosition = locPosition;
                    rayClone.transform.localScale = Vector3.one;

                    float delta = Vector3.Dot(dir, Vector3.up);
                    delta = Mathf.Acos(delta) * Mathf.Rad2Deg;
                    if(dir.x > 0)
                    {
                        delta *= -1;
                    }
                    rayClone.transform.rotation = Quaternion.Euler(0, 0, delta);

                    rayClone.Initialize(m_HitIntervalTime, m_LifeTime);
                    rayClone.Color = m_Color;
                    rayClone.Size = m_Size;
                    rayClone.CollisionEvent = m_CollisionEvent;
                    rayClone.IntersectEventTrigger = ReflectionBehavior;


                    rayClone.Target = m_Target;
                    rayClone.IntersectTarget = intersect;
                    rayClone.Emission = true;

                    ClusterRay.Add(rayClone);



                }

            }
        }


        private void ReflectionBehavior(Vector2 point, Vector3 inputVec, Vector3 normal, GameObject Intersect)
        {
            var reflect = Vector3.Reflect(inputVec, normal).normalized;
            Debug.Log("reflect " + reflect.ToString());

            var locPos = this.transform.worldToLocalMatrix.MultiplyPoint(new Vector3(point.x, point.y, 0));

            m_ReflectNum -= 1;

            if (m_ReflectNum > 1)
            {
                var maskIntersect = new List<GameObject>(m_Intersect);
                maskIntersect.Remove(Intersect);

                InsertRay(locPos, reflect, maskIntersect);
            } else if(m_ReflectNum == 1)
            {
                InsertRay(locPos, reflect, null);
            }else if(m_ReflectNum < 1)
            {
                
            }
           

        }



    }


}