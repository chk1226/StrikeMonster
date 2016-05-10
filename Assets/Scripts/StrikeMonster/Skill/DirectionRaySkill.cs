using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class DirectionRaySkill : BaseSkill {



        private List<List<float>> hitTime;
        private System.Random rnd = new System.Random();
        private bool waitFire = false;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);

            if(rayEmitter != null && rayEmitter.Count != 0)
            {
                hitTime = new List<List<float>>();
              
                Quaternion rotation = Quaternion.Euler(90, 0, 0);
                rayEmitter[0].transform.rotation = rotation;
                rayEmitter[0].startSize = size;

                for( int i = 0; i < waveNumber - 1; i++)
                {
                    GameObject clone = GameObject.Instantiate(rayEmitter[0].gameObject);
                    clone.transform.SetParent(this.transform, false);

                    var particle = clone.GetComponent<ParticleSystem>();
                    rayEmitter.Add(particle);
                }


                
            }


        }


        IEnumerator EnableEmission()
        {

            if(waveNumber == CurrentWaveNumber)
            {
                waitFire = true;
            }

            var ray = rayEmitter [CurrentWaveNumber - 1];

            if(Targets != null && Targets.Count > 0)
            {
                var t = Targets[rnd.Next() % Targets.Count];
                float delta = Mathf.Acos(Vector3.Dot(Vector3.Normalize(t.transform.position - this.transform.position), ray.transform.forward)) * Mathf.Rad2Deg;
                ray.transform.rotation = Quaternion.Euler(0, 0, delta) * ray.transform.rotation;

            }

            ray.enableEmission = true;
            
            yield return new WaitForSeconds(lifeTime);
            ray.enableEmission = false;

            if(CurrentWaveNumber == 0)
            {
                waitFire = false;
            }
        }



        private void RestHitTime()
        {
            hitTime.Clear();
            
            if (Targets != null)
            {
                for(int i = 0; i < rayEmitter.Count; i++)
                {
                    hitTime.Add(new List<float>());
                    for(int j = 0; j < Targets.Count; j++)
                    {
                        hitTime[i].Add(0);
                    }
                }
                
            }
            
        }

        private void SubHitTime(float f)
        {
            for (int i = 0; i < hitTime.Count; i++)
            {
                var list = hitTime[i];
                for(int j = 0; j < list.Count; j++)
                {
                    list[j] -= f;
                }
                
                
            }
        }

        protected override void AttackBehavior()
        {


            if (CurrentWaveNumber == waveNumber)
            {
                RestHitTime();
            }


            StartCoroutine(EnableEmission());
        }


        protected override void RecoveryReady()
        {

            if(waitFire)
            {
                return;
            }

            for(int i = 0; i < rayEmitter.Count; i++)
            {
                var ray = rayEmitter[i];
                if(ray.particleCount != 0)
                {
                    return;
                }
            }

            base.RecoveryReady();
        }


        protected override void DectionParticleCollision2D()
        {
            if(Targets == null)
            {
                return;
            }
            
            ParticleSystem.Particle[] particles;
            Vector3 pSize = Vector3.zero;
            
            for(int i = 0; i < rayEmitter.Count; i++)
            {

                if(rayEmitter[i].particleCount == 0)
                {
                    continue;
                }

                particles = new ParticleSystem.Particle[rayEmitter[i].particleCount];
                rayEmitter[i].GetParticles(particles);
                
                for(int j = 0; j < particles.Length; j++)
                {
                    for(int index = 0; index < Targets.Count; index++)
                    {
                        var target = Targets[index];
                        var collider2D = target.gameObject.GetComponent<Collider2D>();
                        if(collider2D)
                        {
                            pSize.x = particles[j].size;
                            pSize.y = particles[j].size;
                            var pPos = particles[j].position;
                            pPos.z = 0;
                            Bounds pBounds = new Bounds(pPos, pSize);
                            if(SMUtility.IntersectsRectToRect(collider2D.bounds, pBounds))
                            {
                                
                                if(hitTime[i][index] <= 0)
                                {
                                    CollisionBehavior(target);
                                    hitTime[i][index] = hitIntervalTime;
                                    continue;
                                    
                                }
                                
                                
                            }
                            
                            
                        }
                        
                    }
                }
                
                
            }
            
            SubHitTime(Time.fixedTime);

        
        }
    }


}