using UnityEngine;
using System.Collections;


namespace StrikeMonster
{
    public class ExplosionSkill : BaseSkill {

        public Detonator detonator;
        public float Radius;
        
        private float hitTime;
        private bool waitTime = false;
        private bool firstSortLayer = true;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;
            hitIntervalTime = 10;

            hitTime = 0;

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
            detonator.size = Radius + 0.2f;
            detonator.Explode();
            StartCoroutine(WaitTime());

        }

        protected override void DectionParticleCollision2D()
        {
            if(Targets == null)
            {
                return;
            }

            if (hitTime <= 0)
            {

                for (int i = 0; i < Targets.Count; i++)
                {
                    var targetCollider = Targets [i].GetComponent<Collider2D>();
                    if (targetCollider)
                    {
                        Vector2 centerPos = new Vector2(this.transform.position.x, this.transform.position.y);
                        if (SMUtility.IntersectsCircleToRect(centerPos, Radius, targetCollider.bounds))
                        {
                            CollisionBehavior(Targets [i]);

                        }
                    }

                }

                hitTime = hitIntervalTime;
            } 
            else
            {
                hitTime -= GamePlaySettings.Instance.GetDeltaTime();
            }

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