using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class RayComponent : MonoBehaviour {

        public ParticleSystem BaseRay;
        public bool Emission
        {
            get;
            set
            {
                if(value && m_RayEmission != null)
                {

                    m_RayEmission = StartCoroutine(EnableEmission());
                }
            }
        }

        public delegate void CollisionCallback();
        public event CollisionCallback CollisionEvent;


        private float m_HitIntervalTime;
        private List<UnitComponent> m_Targets;
        private float m_LifeTime; 
        private List<float> m_HitTimeList = new List<float>();
        private Coroutine m_RayEmission;


        public void Initialize(float hitIntervalTime, List<UnitComponent> targets, float lifeTime)
        {
            m_HitIntervalTime = hitIntervalTime;
            m_Targets = targets;
            m_LifeTime = lifeTime;
        }

        void FixedUpdate(){
            if(Emission)
            {
                DectionParticleCollision2D();
            }

        }



        private void RestHitTime()
        {
            m_HitTimeList.Clear();
            
            if (m_Targets != null)
            {
                for(int j = 0; j < m_Targets.Count; j++)
                {
                    m_HitTimeList.Add(0);
                }

            }
            
        }
        
        private void SubHitTime(float f)
        {

            for(int j = 0; j < m_HitTimeList.Count; j++)
            {
                m_HitTimeList[j] -= f;
            }

        }


        private void DectionParticleCollision2D()
        {
            if(m_Targets == null)
            {
                return;
            }
            
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[BaseRay.particleCount];
            Vector3 pSize = Vector3.zero;





//                
//                rayEmitter[i].GetParticles(particles);
//                
//                for(int j = 0; j < particles.Length; j++)
//                {
//                    for(int index = 0; index < Targets.Count; index++)
//                    {
//                        var target = Targets[index];
//                        var collider2D = target.gameObject.GetComponent<Collider2D>();
//                        if(collider2D)
//                        {
//                            pSize.x = particles[j].size;
//                            pSize.y = particles[j].size;
//                            var pPos = particles[j].position;
//                            pPos.z = 0;
//                            Bounds pBounds = new Bounds(pPos, pSize);
//                            if(IntersectsRectToRect(collider2D.bounds, pBounds))
//                            {
//                                
//                                if(hitTime[i][index] <= 0)
//                                {
//                                    CollisionBehavior(target);
//                                    hitTime[i][index] = hitIntervalTime;
//                                    continue;
//                                    
//                                }
//                                
//                                
//                            }
//                            
//                            
//                        }
//                        
//                    }
//                }
//                
//                
//            }
//            
//            SubHitTime(Time.fixedTime);
        }


        IEnumerator EnableEmission()
        {

            BaseRay.enableEmission = true;

            yield return new WaitForSeconds(m_LifeTime);

            BaseRay.enableEmission = false;
            Emission = false;
            m_RayEmission = null;
        }


    }


}
    