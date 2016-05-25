using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class ClusterRayComponent : MonoBehaviour {

        public GameObject RayPrefab;
        [HideInInspector]
        public List<RayComponent> ClusterRay = new List<RayComponent>();

        protected bool hasCast = false;
        protected float m_HitIntervalTime;
        protected float m_LifeTime;
        protected float m_Size;
        protected Color m_Color;
        protected RayComponent.CollisionCallback m_CollisionEvent;
        protected List<UnitComponent> m_Target;
        protected List<GameObject> m_Intersect;

        public bool DoDestory()
        {
            if(hasCast && ClusterRay.Count > 0)
            {
                
                for(int i = 0; i < ClusterRay.Count; i++)
                {
                    
                    if(ClusterRay[i].Emission)
                    {
                        return false; 
                    }
                }
                
                
                Destroy(this.gameObject);
                return true;
            }

            return false;
        }
    	
        public virtual void Initialize(uint rayNum, float hitIntervalTime, float lifeTime, float size, Color color, RayComponent.CollisionCallback collisionEvent)
        {
            m_HitIntervalTime = hitIntervalTime;
            m_LifeTime = lifeTime;
            m_CollisionEvent = collisionEvent;
            m_Color = color;
            m_Size = size;
            m_Color = color;

            for (int i = 0; i < rayNum; i ++)
            {

                var clone = Instantiate<GameObject>(RayPrefab);
                if(clone)
                {
                    var rayClone = clone.GetComponent<RayComponent>();
                    if (rayClone)
                    {
                        
                        rayClone.transform.SetParent(this.transform);
                        rayClone.transform.localPosition = Vector3.zero;
                        rayClone.transform.localScale = Vector3.one;
                        
                        rayClone.Initialize(m_HitIntervalTime, m_LifeTime);
                        rayClone.Size = m_Size;
                        rayClone.Color = m_Color;
                        rayClone.CollisionEvent = m_CollisionEvent;
                        
                        ClusterRay.Add(rayClone);
                    }

                }

            }

        }


        public void CastRay(List<UnitComponent> targets, List<GameObject> intersectObjs = null)
        {
            m_Target = targets;
            m_Intersect = intersectObjs;
            for(int i = 0; i < ClusterRay.Count; i ++)
            {
                ClusterRay[i].Target = m_Target;
                ClusterRay [i].IntersectTarget = m_Intersect;
                ClusterRay[i].Emission = true;
            }

            hasCast = true;
        }
            


    }


}