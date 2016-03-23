using UnityEngine;
using System.Collections;


namespace StrikeMonster
{

    public class MissileComponent : MonoBehaviour {

        public UnityEngine.UI.Image Image;

        private float m_RotationSpeed;
//        [SerializeField]
        private Vector2 m_Velocity;
        private Transform m_Target;
        private float m_LifeTime;

        public float LifeTime
        {
            get{ return m_LifeTime; }
        }


        float Direction{
            get { return Mathf.Atan2(m_Velocity.y, m_Velocity.x) * Mathf.Rad2Deg; }
        }


    	// Use this for initialization
    	void Start () {
    	
    	}
    	

        public void Initilize(Vector2 velocity, float rotSpeed, Transform target, float lifeTime)
        {
            m_Velocity = velocity;
            m_RotationSpeed = rotSpeed;
            m_Target = target;
            m_LifeTime = lifeTime;

            
        }


        public void SetVelocity(float dir, float value)
        {
            m_Velocity.x = Mathf.Cos(Mathf.Deg2Rad * dir) * value;
            m_Velocity.y = Mathf.Sin(Mathf.Deg2Rad * dir) * value;
           
        }


        void Update(){

            if (m_LifeTime > 0)
            {
                m_LifeTime -= GamePlaySettings.Instance.GetDeltaTime();
            }
            else
            {
                return;
            }

            if(m_Target)
            {

                float tAngle = Mathf.Atan2(m_Target.position.y - this.transform.position.y, m_Target.position.x - this.transform.position.x) * Mathf.Rad2Deg;
                float newDir = Direction;
                var deltaAngle = Mathf.DeltaAngle( newDir, tAngle);
                
                if(Mathf.Abs(deltaAngle) < m_RotationSpeed)
                {
                }
                else if(deltaAngle > 0)
                {
                    newDir += m_RotationSpeed;
                }
                else
                {
                    newDir -= m_RotationSpeed;
                }
                
                SetVelocity(newDir, m_Velocity.magnitude);

                var now_position = this.transform.position;
                now_position.x += m_Velocity.x;
                now_position.y += m_Velocity.y;
                
                this.transform.position = now_position;

                Image.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Direction - 90));
                
            }

        }


    }


}