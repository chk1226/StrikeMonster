using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class RayComponent : MonoBehaviour {

		public UnityEngine.UI.Image BaseRay;
//        [HideInInspector]

        private bool m_Emission = false;
        public bool Emission
        {
            get
            {
                return m_Emission;
            }
            set
            {
                if(!m_Emission && value)
                {
                    RestHitTime();
                    m_Emission = value;
                }
                else if(m_Emission && !value)
                {
                    BaseRay.enabled = value;
                    m_Emission = value;
                }
            }
        }


        public float Size
        {
            get{return BaseRay.transform.localScale.x;}
            set{
                var scale = BaseRay.transform.localScale;
                scale.x = value;
                BaseRay.transform.localScale = scale;}
        }

        public Color Color
        {
            get{return BaseRay.color;}
            set{BaseRay.color = value;}
        }

        public delegate void CollisionCallback(UnitComponent unit);
        public CollisionCallback CollisionEvent;

        private float m_RaycastWidth;
		private RectTransform m_RayRect;
        private float m_HitIntervalTime;
        private List<UnitComponent> m_Targets;
        public List<UnitComponent> Target
        {
            get{return m_Targets;}
            set{m_Targets = value;}
        }
        private float m_LifeTime; 
        private List<float> m_HitTimeList = new List<float>();
        private List<RaycastHit2D[]> m_RaycastHitList = new List<RaycastHit2D[]>();

        public void Initialize(float hitIntervalTime, float lifeTime)
        {
            m_HitIntervalTime = hitIntervalTime;
            m_LifeTime = lifeTime;
        }


		void Start()
		{
			m_RayRect = BaseRay.GetComponent<RectTransform>();
            m_RaycastWidth = WaveComponent.Instance.SkillEffectLayer.transform.localToWorldMatrix.MultiplyVector(new Vector3(m_RayRect.sizeDelta.x, 0f, 0f)).x / Size;
		}

		private float m_CurrentLifeTime = 0;
		private float m_OffsetY = 100;
		void Update()
		{
			if(Emission)
			{
				if(m_CurrentLifeTime >= m_LifeTime)
				{
					Emission = false;
				}
				else
				{
					var sizeDelta = m_RayRect.sizeDelta;
					sizeDelta.y += m_OffsetY;
					m_RayRect.sizeDelta = sizeDelta;
                    
					m_CurrentLifeTime += GamePlaySettings.Instance.GetDeltaTime();
				}
			}


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
			if(m_Targets == null || !BaseRay.enabled)
            {
                return;
            }


            m_RaycastHitList.Clear();

            var dir = this.transform.localRotation * Vector3.up;
            var rayOriPos = new Vector2(m_RayRect.position.x, m_RayRect.position.y); 

            int size = (int)Size;
            for(int i = 1; i <= size; i++)
            {
                rayOriPos.x += i * m_RaycastWidth;
                m_RaycastHitList.Add( Physics2D.RaycastAll(rayOriPos, dir) );

                rayOriPos.x -= i * m_RaycastWidth * 2;
                m_RaycastHitList.Add( Physics2D.RaycastAll(rayOriPos, dir) );

            }
          
            for(int index = 0; index < m_Targets.Count; index++)
            {
                var target = m_Targets[index];

                if(m_HitTimeList[index] <= 0)
                {
                    
                    for(int i = 0; i < m_RaycastHitList.Count; i++)
                    {
                        if(FindTargetInHitRay(target, m_RaycastHitList[i]))
                        {
                            if(CollisionEvent != null)
                            {
                                CollisionEvent(target);
                            }
                            m_HitTimeList[index] = m_HitIntervalTime;
                            break;
                        }
                    }

                }
                    
            }

            
            SubHitTime(Time.fixedDeltaTime);
        }


        private bool FindTargetInHitRay(UnitComponent target, RaycastHit2D[] hits)
        {
         
            foreach(var hit in hits)
            {
                var hitUnit = hit.transform.gameObject.GetComponent<UnitComponent>();
                if(hitUnit && hitUnit == target)
                {
                    return true;
                }
            }
            return false;

        }


    }


}
    