using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class LevelComponent : MonoBehaviour {


//        public GameObject EnemyLayer;


        [SerializeField]
        private List<WaveInfo> m_WaveInfo = new List<WaveInfo>();
        
        
        
        // Use this for initialization
    	void Start () {
    	
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}



        public void Initialize()
        {
//            if (m_Team.Count == 0 && HeroLayer)
//            {
//                foreach(var proto_hero in PrototypeSystem.Instance.Config.HeroInfoList)
//                {
//                    var hero_instance = Instantiate(m_HeroPrefab) as GameObject;
//                    if (hero_instance)
//                    {
//                        var hero = hero_instance.GetComponent<HeroComponent>();
//                        if(hero)
//                        {
//                            hero_instance.transform.SetParent(HeroLayer.transform);
//                            hero_instance.transform.localPosition = Vector3.zero;
//                            hero_instance.transform.localScale = Vector3.one;
//                            
//                            hero.Initialize(proto_hero);
//                            m_Team.Add(hero);
//                            
//                        }
//                        else
//                        {
//                            Destroy(hero_instance);
//                            Debug.LogWarning("No has HeroComponent");
//                            
//                        }
//                        
//                        
//                        
//                    }
//                    
//                }
//                
//                
//            }
            
            
        }




    }

   

}