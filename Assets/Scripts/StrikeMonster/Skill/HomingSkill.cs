using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class HomingSkill : BaseSkill {

        public GameObject MissilePrefab;
        public float RotationSpeed;
        public float SpeedValue;

        private List<MissileComponent> m_missileParticle = new List<MissileComponent>();
        private System.Random rnd = new System.Random();
        private bool waitFire = false;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);


        }

        IEnumerator FrieMissile()
        {
            UnitComponent target;
            if (Targets != null && Targets.Count > 0)
            {

                if(waveNumber == CurrentWaveNumber)
                {
                    waitFire = true;
                }


                for(int i = 0; i < lineNumber; i++)
                {
                    target = Targets[rnd.Next() % Targets.Count]; 
                    var missileCmp = GameObject.Instantiate(MissilePrefab).GetComponent<MissileComponent>();

                    missileCmp.transform.SetParent(this.transform, false);
                    missileCmp.transform.position = this.transform.position;

                    var startVelocity = new Vector2(this.transform.position.x - target.transform.position.x, 
                                                    this.transform.position.y - target.transform.position.y);
                    

                    // offset angle

                    float offsetAngle = (rnd.Next() % 30 * ( (rnd.Next() % 2  == 1) ? 1 : -1)) * Mathf.Deg2Rad ;
                    float new_x = startVelocity.x * Mathf.Cos(offsetAngle) - startVelocity.y * Mathf.Sin(offsetAngle);
                    float new_y = startVelocity.x * Mathf.Sin(offsetAngle) + startVelocity.y * Mathf.Cos(offsetAngle);
                    startVelocity.x = new_x;
                    startVelocity.y = new_y;

                    startVelocity.Normalize();


                    missileCmp.Initilize(startVelocity * SpeedValue, RotationSpeed, target.transform, lifeTime);
                    m_missileParticle.Add(missileCmp);

                    yield return null;
                    
                }


                if(CurrentWaveNumber == 0)
                {
                    waitFire = false;
                }

            }

        }


        protected override void OnFixedUpdate()
        {
           
            int i = 0;
            while(i < m_missileParticle.Count)
            {
                var mis = m_missileParticle[i];
                if(mis.LifeTime <= 0)
                {
                    Destroy(mis.gameObject);
                    m_missileParticle.Remove(mis);
                }
                else
                {
                    i ++;
                }

            }

        }

        protected override void DectionParticleCollision2D()
        {

            int i = 0;
            while (i < m_missileParticle.Count)
            {

                var mis = m_missileParticle[i];
                var misCollider = mis.GetComponent<Collider2D>();
                
                if(!misCollider)
                {
                    i++;
                    continue;
                    
                }

                bool killMis = false;
                for(int j = 0; j < Targets.Count; j++)
                {
                    var tCollisider = Targets[j].GetComponent<Collider2D>();

                    if(tCollisider && tCollisider.bounds.Contains(misCollider.bounds.center))
                    {
//                        Debug.Log("Hit "+ this.transform.parent.gameObject.name);
                        CollisionBehavior(Targets[j]);

                        Destroy(mis.gameObject);
                        m_missileParticle.Remove(mis);
                        killMis = true;
                        break;

                    }
                }


                if(killMis)
                {

                }
                else
                {
                    i++;
                }

            }





        }



        protected override void AttackBehavior()
        {
            StartCoroutine(FrieMissile());
        }


        protected override void RecoveryReady()
        {

            if(waitFire)
            {
                return;
            }

            if(m_missileParticle.Count != 0)
            {
                return;
            }

            base.RecoveryReady();
        }





    }


}