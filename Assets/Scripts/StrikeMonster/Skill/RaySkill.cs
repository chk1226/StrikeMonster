using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class RaySkill : BaseSkill {


        public float[] hitTime;

        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);
            waveNumber = 1;

            if(rayEmitter != null && rayEmitter.Count != 0)
            {
                hitTime = new float[rayEmitter.Count];

                float angle = 360 / rayEmitter.Count;
                Quaternion rotation = Quaternion.Euler(90, 0, 0);

                for( int i = 0; i < rayEmitter.Count; i++)
                {

                    rayEmitter[i].transform.rotation = Quaternion.Euler(0, 0, i * angle) * rotation;
                }

            }


            
        }

        private void RestHitTime()
        {
            for(int i = 0; i < hitTime.Length; i++)
            {
                hitTime[i] = 0;
            }
        }


        IEnumerator EnableEmission()
        {
            foreach(var ray in rayEmitter)
            {

                ray.enableEmission = true;

            }


            yield return new WaitForSeconds(lifeTime);
            

            foreach(var ray in rayEmitter)
            {
                
                ray.enableEmission = false;
                
            }
        }


        protected override void AttackBehavior()
        {
            StartCoroutine(EnableEmission());

        }

        protected override void RecoveryReady()
        {
            foreach(var ray in rayEmitter)
            {
                if(ray.particleCount != 0)
                {
                    return;
                }
            }

            RestHitTime();
            base.RecoveryReady();
        }

        protected override void DectionParticleCollision2D()
        {
            if(Targets == null)
            {
                return;
            }
//
            for(int i = 0; i < rayEmitter.Count; i++)
            {

                if(rayEmitter[i].particleCount == 0)
                {
                    continue;
                }
                

                if(hitTime[i] <= 0)
                {
                    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[rayEmitter[i].particleCount];
                    rayEmitter[i].GetParticles(particles);
                    
                    for(int j = 0; j < particles.Length; j++)
                    {
                        foreach(var target in Targets)
                        {
                            var collider2D = target.gameObject.GetComponent<Collider2D>();
                            if(collider2D != null)
                            {
                                var pos = particles[j].position;
                                pos.z = 0;

                                if(collider2D.bounds.Contains(pos))
                                {

                                    CollisionBehavior(target);
                                    
                                    hitTime[i] = hitIntervalTime;
                                    break;
                                }


                            }
                            
                        }

                        if(hitTime[i] > 0)
                        {
                            break;
                        }
                        
                    }

                }else
                {
                    hitTime[i] -= GamePlaySettings.Instance.GetDeltaTime();
                }


            }


        }

    	
    }


}