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
            if(!GamePlaySettings.Instance.CanStrike)
            {
                return;
            }


            var currentHero = TeamComponent.Instance.CurrentHero;
            if (!currentHero)
            {
                return;
            }

			//button down
			if(Input.GetMouseButtonDown(0))
			{
				m_canStrike = false;
				m_touchPosition = Input.mousePosition;
				

			}
					
			//button move
			if (Input.GetMouseButton(0) )
			{

				m_mousePosition = Input.mousePosition;

				if(Vector3.Distance(m_touchPosition, m_mousePosition) > DELTA_LENGTH_THRESHOLD)
				{
					m_canStrike = true;
                    ShowArrow();
				}
				else
				{
					m_canStrike = false;
                    DisableArrow();
				}
			
			}
		
			// button up
			if (Input.GetMouseButtonUp(0) &&
			    m_canStrike )
			{
                if(currentHero)
				{
					var dir_v2 = new Vector2(m_touchPosition.x - m_mousePosition.x,
					                         m_touchPosition.y - m_mousePosition.y).normalized;

                    currentHero.GiveForce(dir_v2);
                    DisableArrow();


                    GameFlowComponent.Instance.GameFlowFSM.SendEvent(GameFlowComponent.Instance.WaitHeroBattleEndEvent);
				}
			}
				
	
		}
		


        private void ShowArrow()
        {
            if (Arrow)
            {
                
                var touchPos = Camera.main.ScreenToWorldPoint(m_touchPosition);
                touchPos.z = 0;
                var arrowTransform = Arrow.GetComponent<RectTransform>();
                var deltaVector = (m_mousePosition - m_touchPosition).normalized;

                float euler_z = Mathf.Acos(Vector3.Dot(deltaVector,Vector3.right)) * Mathf.Rad2Deg;
                if(m_mousePosition.y < m_touchPosition.y)
                {
                    euler_z = 360 - euler_z;
                }

                arrowTransform.position = touchPos;
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

