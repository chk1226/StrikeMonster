using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class RayComponent : MonoBehaviour {

        public ParticleSystem BaseRay;

        private bool m_Emission = false;
        public bool Emission
        {
            get
            {
                return m_Emission;
            }
            set
            {
                if(!m_Emission && m_RayEmission == null)
                {
                    m_Emission = value;
                    RestHitTime();
                    m_RayEmission = StartCoroutine(EnableEmission());
                }
            }
        }


        public float Szie
        {
            get{return BaseRay.startSize;}
            set{BaseRay.startSize = value;}
        }

        public Color Color
        {
            get{return BaseRay.startColor;}
            set{BaseRay.startColor = value;}
        }

        public delegate void CollisionCallback(UnitComponent unit);
        public CollisionCallback CollisionEvent;


        private float m_HitIntervalTime;
        private List<UnitComponent> m_Targets;
        public List<UnitComponent> Target
        {
            get{return m_Targets;}
            set{m_Targets = value;}
        }
        private float m_LifeTime; 
        private List<float> m_HitTimeList = new List<float>();
        private Coroutine m_RayEmission;


        public void Initialize(float hitIntervalTime, float lifeTime)
        {
            m_HitIntervalTime = hitIntervalTime;
//            m_Targets = targets;  
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
            if(m_Targets == null || BaseRay.particleCount == 0)
            {
                return;
            }
            
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[BaseRay.particleCount];
            BaseRay.GetParticles(particles);
            Vector3 pSize = Vector3.zero;


            for(int j = 0; j < particles.Length; j++)
            {
                for(int index = 0; index < m_Targets.Count; index++)
                {
                    var target = m_Targets[index];
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
                            if(m_HitTimeList[index] <= 0)
                            {
                                if(CollisionEvent != null)
                                {

                                    CollisionEvent(target);
                                }
                                m_HitTimeList[index] = m_HitIntervalTime;
                                continue;
                                
                            }
                            
                            
                        }
                        
                        
                    }
                    
                }

            }
            SubHitTime(Time.fixedTime);
        }


        IEnumerator EnableEmission()
        {
            yield return null;
            
            BaseRay.enableEmission = true;

            yield return new WaitForSeconds(m_LifeTime);

            BaseRay.enableEmission = false;
            m_Emission = false;
            m_RayEmission = null;
        }


    }


}
    