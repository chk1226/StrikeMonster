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
    	
        public void Initialize(uint rayNum, float hitIntervalTime, float lifeTime, RayComponent.CollisionCallback collisionEvent)
        {
 
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
                        
                        rayClone.Initialize(hitIntervalTime, lifeTime);
                        rayClone.CollisionEvent = collisionEvent;
                        
                        ClusterRay.Add(rayClone);
                    }

                }

            }

        }


        public virtual void CastRay(List<UnitComponent> targets, List<GameObject> intersectObjs = null)
        {
            for(int i = 0; i < ClusterRay.Count; i ++)
            {
                ClusterRay[i].Target = targets;
                ClusterRay [i].IntersectTarget = intersectObjs;
                ClusterRay[i].Emission = true;
            }

            hasCast = true;
        }
            


    }


}