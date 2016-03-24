using UnityEngine;
using System.Collections;


namespace StrikeMonster
{

    public class ExplosionSkill : BaseSkill {

        public Detonator detonator;
        private bool waitTime = false;
        private bool firstSortLayer = true;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;

            if (detonator)
            {
                detonator.duration = lifeTime;
            }

        }


        IEnumerator WaitTime()
        {
            waitTime = true;


            if (firstSortLayer)
            {
                yield return null;
                
                var sortLayer = this.gameObject.GetComponent<SortingLayerComponent>();
                if(sortLayer)
                {
                    sortLayer.SetLayer();
                }

                firstSortLayer = false;
            }

            yield return new WaitForSeconds(lifeTime);
            
            waitTime = false;
        }


        protected override void AttackBehavior()
        {

            detonator.Explode();
            StartCoroutine(WaitTime());
        }


        protected override void RecoveryReady()
        {
            if (waitTime)
            {
                return;
            }

            base.RecoveryReady();



        }

    }

}