using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class RaySkill : BaseSkill {

        public GameObject ClusterRayPrefab;


        private List<ClusterRayComponent> m_ClusterRayList = new List<ClusterRayComponent>(); 

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);

        }

        private void SettingRay(ClusterRayComponent clusterRay)
        {
            float angle = 360 / clusterRay.ClusterRay.Count;
            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            if(clusterRay)
            {
                for( int i = 0; i < clusterRay.ClusterRay.Count; i++)
                {
                    
                    clusterRay.ClusterRay[i].transform.rotation = Quaternion.Euler(0, 0, i * angle + rotationDeg) * rotation;
                    clusterRay.ClusterRay[i].Szie = size;
                    clusterRay.ClusterRay[i].Color = normalColor;
                }
            }
        }

        private void CastSkill()
        {
            var clone = Instantiate<GameObject>(ClusterRayPrefab);
            if (clone)
            {
                var clusterRay = clone.GetComponent<ClusterRayComponent>();
                if(clusterRay)
                {
                    clusterRay.Initialize(4, hitIntervalTime, lifeTime, CollisionBehavior);
                    clusterRay.transform.SetParent(WaveComponent.Instance.SkillEffectLayer.transform);
                    clusterRay.transform.position = this.transform.position;
                    clusterRay.transform.localScale = Vector3.one;
                    SettingRay(clusterRay);
                    
                    clusterRay.CastRay(Targets);
                    m_ClusterRayList.Add(clusterRay);
                }

            }

        }


        protected override void AttackBehavior()
        {
            CastSkill();
        }

        protected override void RecoveryReady()
        {

            for(int i = 0; i < m_ClusterRayList.Count; i++)
            {

                if(m_ClusterRayList[i].DoDestory())
                {
                    m_ClusterRayList.RemoveAt(i);
                }
            }


            if(m_ClusterRayList.Count == 0)
            {
                base.RecoveryReady();
                
            }
        }



    	
    }


}