using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class DirectionRaySkill : BaseSkill {

        public GameObject ClusterRayPrefab;

        private System.Random rnd = new System.Random();
        private List<ClusterRayComponent> m_ClusterRayList = new List<ClusterRayComponent>(); 

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
        }

        private void SettingRay(ClusterRayComponent clusterRay)
        {

            if(Targets != null && Targets.Count > 0)
            {

                if(clusterRay)
                {

                    var ray = clusterRay.ClusterRay [0];
  

                    var target = Targets[rnd.Next() % Targets.Count];
                    float delta = Mathf.Acos(Vector3.Dot(Vector3.Normalize(target.transform.position - clusterRay.transform.position), Vector3.up)) * Mathf.Rad2Deg;

                    if(target.transform.position.x > clusterRay.transform.position.x)
                    {
                        delta *= -1;
                    }

                    ray.transform.rotation = Quaternion.Euler(0, 0, delta);
//                    Debug.Log(delta);
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
                    clusterRay.Initialize(1, hitIntervalTime, lifeTime, size, normalColor, CollisionBehavior);
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