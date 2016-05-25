using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class ReflectionRaySkill : BaseSkill {

        public GameObject ClusterRayPrefab;

//        private int m_ReflectNum = 3;
        private List<ClusterRayComponent> m_ClusterRayList = new List<ClusterRayComponent>(); 

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
        }


        private void SettingRay(ClusterRayComponent clusterRay)
        {
            var vec = (Vector2)Parameter [0];
            var delta = Vector2.Dot(vec.normalized, Vector2.up);
            delta = Mathf.Acos(delta) * Mathf.Rad2Deg;
            if (vec.x > 0)
            {
                delta *= -1;
            }

            clusterRay.ClusterRay[0].transform.rotation = Quaternion.Euler(0, 0, delta);
        }


        private void CastSkill()
        {
            var clone = Instantiate<GameObject>(ClusterRayPrefab);
            if (clone)
            {
                var clusterRay = clone.GetComponent<ClusterRayComponent>();
                if(clusterRay)
                {
                    clusterRay.Initialize(1, hitIntervalTime, lifeTime, size, normalColor, CollisionBehavior);
                    clusterRay.transform.SetParent(WaveComponent.Instance.SkillEffectLayer.transform, false);
                    clusterRay.transform.position = this.transform.position;
                    clusterRay.transform.localScale = Vector3.one;
                    SettingRay(clusterRay);

                    clusterRay.CastRay(Targets, WallComponent.Instance.GetAllWall());
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