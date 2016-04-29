using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

	public class DectionPullForce : MonoBehaviour {

        public GameObject Arrow;

//        [SerializeField]
//		private Rigidbody2D m_HeroRigidbody;			 
//		private RectTransform m_rectTransform;


		// Use this for initialization
		void Start () {
			
		}

		private bool m_canStrike = false;
		private Vector3 m_mousePosition = Vector3.zero;
		private Vector3 m_touchPosition = Vector3.zero;
		private const float DELTA_LENGTH_THRESHOLD = 20;
		void Update()
		{
		}
		
        public void OnPointerDown()
        {
            if(!GamePlaySettings.Instance.CanStrike)
            {
                m_canStrike = false;
                return;
            }

            m_touchPosition = Input.mousePosition;
            m_canStrike = true;
            
        }

        public void OnPointerUp()
        {

            if (Arrow.activeSelf && m_canStrike)
            {

                var currentHero = TeamComponent.Instance.CurrentHero;
                if (!currentHero)
                {
                    m_canStrike = false;
                    return;
                }

                var dir_v2 = new Vector2(m_touchPosition.x - m_mousePosition.x,
                                         m_touchPosition.y - m_mousePosition.y).normalized;
                
                currentHero.GiveForce(dir_v2);
                DisableArrow();
                
                
                GameFlowComponent.Instance.GameFlowFSM.SendEvent(GameFlowComponent.Instance.WaitHeroBattleEndEvent);
                m_canStrike = false;
            }

        }

        public void OnPointerMove()
        {
            if(m_canStrike)
            {
                m_mousePosition = Input.mousePosition;
                
                if(Vector3.Distance(m_touchPosition, m_mousePosition) > DELTA_LENGTH_THRESHOLD)
                {
                    var currentHero = TeamComponent.Instance.CurrentHero;
                    if (!currentHero)
                    {
                        m_canStrike = false;
                        return;
                    }

                    ShowArrow(currentHero.transform);
                }
                else
                {
                    DisableArrow();
                }
            }

        }

        private void ShowArrow(Transform heroTransform)
        {
            if (Arrow && heroTransform)
            {
                
                var touchPos = Camera.main.ScreenToWorldPoint(m_touchPosition);
                touchPos.z = 0;
                var arrowTransform = Arrow.GetComponent<RectTransform>();
                var deltaVector = (m_mousePosition - m_touchPosition).normalized;

//                Debug.Log(deltaVector);
                float euler_z = Mathf.Acos(Vector3.Dot(deltaVector,Vector3.right)) * Mathf.Rad2Deg;
                if(m_mousePosition.y < m_touchPosition.y)
                {
                    euler_z = 360 - euler_z;
                }

                arrowTransform.position = heroTransform.position;
                arrowTransform.rotation = Quaternion.Euler( 0, 0, euler_z);

                Arrow.SetActive(true);

            }
        }

        private void DisableArrow()
        {
            if(Arrow)
            {
                Arrow.SetActive(false);
            }
        }

	}
	


}

