using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
    public class RayComponent : MonoBehaviour {

		public UnityEngine.UI.Image BaseRay;

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


        public void Initialize(float hitIntervalTime, float lifeTime)
        {
            m_HitIntervalTime = hitIntervalTime;
            m_LifeTime = lifeTime;
        }


		void Start()
		{
			m_RayRect = BaseRay.GetComponent<RectTransform>();
		}

		private float m_CurrentLifeTime = 0;
		private float m_OffsetY = 10;
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

			var oriPos = new Vector2(m_RayRect.position.x, m_RayRect.position.y); 
//			var gg = GameFlowComponent.Instance.BattleCanvasScaler;
//			var GG = GameFlowComponent.Instance.BattleCanvasScaler.GetComponent<RectTransform>();
////			Debug.Log(GameFlowComponent.Instance.BattleCanvasScaler.scaleFactor);
//			Debug.Log(GameFlowComponent.Instance.BattleCanvasScaler.transform.localScale);


			var hitAll = Physics2D.BoxCastAll(oriPos, m_RayRect.sizeDelta, m_RayRect.rotation.eulerAngles.z, Vector2.up, m_RayRect.sizeDelta.y);

            for(int index = 0; index < m_Targets.Count; index++)
            {
                var target = m_Targets[index];

				for(int i = 0; i < hitAll.Length; i++)
				{
					var hitUnit = hitAll[i].transform.gameObject.GetComponent<UnitComponent>();
					if(hitUnit && hitUnit == target)
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

//                
            }

            
            SubHitTime(Time.fixedTime);
        }


    }


}
    