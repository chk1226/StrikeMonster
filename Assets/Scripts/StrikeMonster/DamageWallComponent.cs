using UnityEngine;
using System.Collections;

namespace StrikeMonster
{

    public class DamageWallComponent : MonoBehaviour, IReduceCD {

        private int m_turn;
        private float m_damage;
        private float m_animationTime;
        protected const string FLAME_PATH = "Prefabs/Effect/flame";

        private static GameObject m_flamePrefab;
        
        public void Initialize(int turn, float damage, float animationTime)
        {
            m_turn = turn;
            m_damage = damage;
            m_animationTime = animationTime;

            StartCoroutine(GeneratorFlame());
        }

        public void Reset(int turn)
        {
            m_turn = turn;
            
        }


        void Awake()
        {
            if (!m_flamePrefab)
            {
                m_flamePrefab = Resources.Load<GameObject>(FLAME_PATH);
            }
        }

    	// Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}

        void OnCollisionEnter2D(Collision2D coll) 
        {
            var hero = coll.gameObject.GetComponent<HeroComponent>();
            if (hero)
            {
                hero.OnHurt(m_damage);
            }
        }

        public void ReduceCD ()
        {
            if (m_turn > 0)
            {
                m_turn -= 1;
                
            } 
            else
            {
                
                foreach(Transform child in this.transform)
                {
                    Destroy(child.gameObject);
                }
                
                
                Destroy(this);
            }
        }

        IEnumerator GeneratorFlame()
        {

            var wallCollider = gameObject.GetComponent<BoxCollider2D>();            
            if (wallCollider)
            {
                Bounds wallBounds = wallCollider.bounds;                
                float currentTime = m_animationTime;
                GameObject particle;
                
                while(currentTime >= 0)
                {
                    
                    float rate = currentTime / m_animationTime;
                    var currentPos = Vector3.Lerp(wallBounds.max, wallBounds.min, rate);

                        
                    particle = GameObject.Instantiate(m_flamePrefab);
                    particle.transform.SetParent(this.transform, false);
                    particle.transform.position = currentPos;


                    yield return null;
                    yield return null;
                    
                    
                    
                    currentTime -= (GamePlaySettings.Instance.GetDeltaTime())*2;
                    
                }

                particle = GameObject.Instantiate(m_flamePrefab);
                particle.transform.SetParent(this.transform, false);
                particle.transform.position = wallBounds.max;

                yield return null;

            }
            
            
            
        }

    }

}