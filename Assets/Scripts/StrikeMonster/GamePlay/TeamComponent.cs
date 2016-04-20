using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class TeamComponent : MonoBehaviour {

    	public GameObject HeroLayer;
        public GameObject HeroPrefab;
        public HpPropertyComponent HPProperty;

        private List<UnitComponent> m_Team = new List<UnitComponent>();
        public List<UnitComponent> Team
        {
            get{
                return m_Team;
            }
        }

        private static TeamComponent m_TeamComponent;
        public static TeamComponent Instance
        {
            get{
                return m_TeamComponent;
            }
        }


        private int m_CurrentHeroIndex = 0;
        public HeroComponent CurrentHero
        {
            get{
                if(m_Team.Count > m_CurrentHeroIndex)
                {
                    return m_Team[m_CurrentHeroIndex] as HeroComponent;
                }
                else
                {
                    return null;
                }
            }
        }

        
        public void NextHero()
        {
            m_CurrentHeroIndex = (m_CurrentHeroIndex + 1) % m_Team.Count;
        }


        void Awake()
        {
            m_TeamComponent = this;
        }

        // Use this for initialization
        void Start () {
    	}
    	
    	// Update is called once per frame
    	void Update () {
    	
    	}


    	public void Initialize()
    	{
    		if (m_Team.Count == 0 && HeroLayer)
            {

                float totalHP = 0;

                foreach(var heroInfo in PrototypeSystem.Instance.CastHeroInfoList())
                {
                    var hero_instance = Instantiate(HeroPrefab) as GameObject;
                    if (hero_instance)
                    {
                        var hero = hero_instance.GetComponent<HeroComponent>();
                        if(hero)
                        {
                            hero_instance.transform.SetParent(HeroLayer.transform);
                            hero_instance.transform.localPosition = Vector3.zero;
                            hero_instance.transform.localScale = Vector3.one;
                            
                            hero.Initialize(heroInfo);
                            m_Team.Add(hero);

                            totalHP += hero.HP;
                        }
                        else
                        {
                            Destroy(hero_instance);
                            Debug.LogWarning("No has HeroComponent");

                        }

                    }

                }

                if(HPProperty)
                {
                    HPProperty.Initialize(totalHP, 0 , totalHP);
                }


            }


    	}

        public bool IsGameOver()
        {
            if (HPProperty)
            {
                return HPProperty.Value == 0;
            }

            return true;
        }

        public bool HerosIsStillness()
        {
            foreach(var hero in Team)
            {
                var hero_cmp = hero as HeroComponent;
                if(hero_cmp)
                {
                    if( hero_cmp.Rigidbody_2D.velocity != Vector2.zero)
                    {
                        return false;
                    }

                }
            }

            return true;

        }


        public void HandleHerosTrigger(bool status)
        {
            foreach(var hero_obj in m_Team)
            {
                var hero = hero_obj as HeroComponent;

                if(hero != CurrentHero)
                {
                    if(hero.CircleCollider_2D)
                    {
                        hero.CircleCollider_2D.isTrigger = status;
                    }
                }

            }
        }



        public bool HerosFriendlySkillsReady()
        {
            foreach(var hero_obj in m_Team)
            {
                var hero = hero_obj as HeroComponent;
                
                if(!hero.FriendlySkillsIsReady())
                {
                    return false;
                }
                
            }

            return true;
        }



    }

}