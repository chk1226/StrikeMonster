using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class RaySkill : BaseSkill {

        private List<List<float>> hitTime;
        private bool waitFire = false;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;

            if(rayEmitter != null && rayEmitter.Count != 0)
            {
                hitTime = new List<List<float>>();

                float angle = 360 / rayEmitter.Count;
                Quaternion rotation = Quaternion.Euler(90, 0, 0);

                for( int i = 0; i < rayEmitter.Count; i++)
                {

                    rayEmitter[i].transform.rotation = Quaternion.Euler(0, 0, i * angle + rotationDeg) * rotation;
                    rayEmitter[i].startSize = size;
                }

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


        IEnumerator EnableEmission()
        {

            waitFire = true;

            foreach(var ray in rayEmitter)
            {

                ray.enableEmission = true;

            }


            yield return new WaitForSeconds(lifeTime);
            
            
            foreach(var ray in rayEmitter)
            {
                
                ray.enableEmission = false;
                
            }

            waitFire = false;
        }


        protected override void AttackBehavior()
        {
            RestHitTime();
            StartCoroutine(EnableEmission());

        }

        protected override void RecoveryReady()
        {

            if (waitFire)
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

            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[rayEmitter[1].particleCount];
            Vector3 pSize = Vector3.zero;

            for(int i = 0; i < rayEmitter.Count; i++)
            {

                if(rayEmitter[i].particleCount == 0)
                {
                    continue;
                }
            
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
                            if(IntersectsRectToRect(collider2D.bounds, pBounds))
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