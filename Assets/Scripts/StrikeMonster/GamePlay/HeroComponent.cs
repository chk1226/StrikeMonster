using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
    public class HeroComponent : UnitComponent, IRestFriendlySkill {

        public Rigidbody2D Rigidbody_2D;
        public CircleCollider2D CircleCollider_2D;
        public ThrustComponent ThrustComponent;

        [HideInInspector]
        public string Name;
        [HideInInspector]
        public bool CanFriendlySkill;

        private BaseSkill m_friendlySkill;
        private BaseSkill m_activeSkill;
        public BaseSkill ActiveSkill
        {
            get
            {
                return m_activeSkill;
            }
        }

        private float m_Speed;

        private string m_SpineData;
        public string SpineData
        {
            get{return m_SpineData;}
        }


       public override void Initialize (UnitInfo baseInfo)
        {
            var heroInfo = baseInfo as HeroInfo;
            if (heroInfo != null)
            {
                base.Initialize (baseInfo);

                // base attribute
                m_Speed = heroInfo.Speed;
                Name = heroInfo.Name;
                this.gameObject.name = Name;
                m_SpineData = heroInfo.SpineData;

                // friendly skill
                if(heroInfo.FriendlySkill != null)
                {
                    var prefab = Instantiate(Resources.Load<GameObject>(SKILL_PATH + heroInfo.FriendlySkill.SkillName));
                    if(prefab)
                    {
                        prefab.transform.SetParent(SkillGroup.transform);
                        prefab.transform.localPosition = Vector3.zero;
                        prefab.transform.localScale = Vector3.one;
                        
                        var skillCmp = prefab.GetComponent<BaseSkill>();
                        if(skillCmp)
                        {
                            skillCmp.Config(heroInfo.FriendlySkill);
                            skillCmp.CDProperty.EnableCDText(false);
                            m_friendlySkill = skillCmp;                        
                        }
                        
                    }

                }

                // active skill
                if(heroInfo.ActiveSkill != null)
                {
                    var prefab = Instantiate(Resources.Load<GameObject>(SKILL_PATH + heroInfo.ActiveSkill.SkillName));
                    if(prefab)
                    {
                        prefab.transform.SetParent(SkillGroup.transform);
                        prefab.transform.localPosition = Vector3.zero;
                        prefab.transform.localScale = Vector3.one;
                        
                        var skillCmp = prefab.GetComponent<BaseSkill>();
                        if(skillCmp)
                        {
                            skillCmp.Config(heroInfo.ActiveSkill);
                            skillCmp.CDProperty.EnableCDText(false);
                            m_activeSkill = skillCmp;                        
                        }
                        
                    }
                    
                }



                // thrust component set up
                if(ThrustComponent)
                {
                    ThrustComponent.ThrustEvent += delegate(Collider2D coll) {

                        if (TeamComponent.Instance.CurrentHero != this && CanFriendlySkill
                            && GamePlaySettings.Instance.IsActionStrike)
                        {
                            
                            if(WaveComponent.Instance.CurrentEnemy.Count == 0)
                            {
                                m_friendlySkill.Targets = null;
                            }
                            else if(coll.GetComponent<HeroComponent>())
                            {
                                CastFriendSkill();
                            }

                        }

                   };
                }



            }

        }


//        public void GiveVelocity(Vector2 velocity)
//        {
//            if (Rigidbody_2D)
//            {
//                Rigidbody_2D.velocity = velocity * m_Speed;
//            }
//        }

        public void GiveForce(Vector2 f)
        {
            if (Rigidbody_2D)
            {
                Rigidbody_2D.AddForce(f* m_Speed);
            }
        }

        public override void OnHurt(float damage)
        {
            if(TeamComponent.Instance.HPProperty)
            {
                TeamComponent.Instance.HPProperty.Value -= damage;

            }

            base.OnHurt(damage);
        }


        public override void HandlePhysics()
        {
            if(Rigidbody_2D)
            {
                if(GamePlaySettings.Instance.IsPlayerRound && Rigidbody_2D.velocity.magnitude < GamePlaySettings.Instance.ThresholdVelocity)
                {
                    Rigidbody_2D.velocity = Vector2.zero;
                }

            }
        }


//    	void OnCollisionEnter2D(Collision2D coll) {
//
//            Debug.Log("[OnCollisionEnter2D] hit!" + coll.gameObject.name);
//
//    	}

//        void OnTriggerEnter2D(Collider2D coll)
//        {
//            Debug.Log("[OnTriggerEnter2D] Trigger!" + coll.gameObject.name);
            
//        }


        private void CastFriendSkill()
        {
            if(m_friendlySkill != null)
            {
                var enemy_list = new List<UnitComponent>();
                
                foreach(var e in WaveComponent.Instance.CurrentEnemy)
                {
                    enemy_list.Add( e as UnitComponent );
                }
                
                m_friendlySkill.Targets = enemy_list;
                if(m_friendlySkill.DoFire())
                {
                    CanFriendlySkill = false;
                }
                
            }
        }

        public void CastActiveSkill()
        {
            if(m_friendlySkill != null)
            {
                var enemy_list = new List<UnitComponent>();
                
                foreach(var e in WaveComponent.Instance.CurrentEnemy)
                {
                    enemy_list.Add( e as UnitComponent );
                }
                
                m_activeSkill.Targets = enemy_list;
                if(m_activeSkill.DoFire())
                {
//                    CanFriendlySkill = false;
                }
            }


        }

        public bool ActiveSkillsIsReady()
        {
            if (m_activeSkill)
            {
                return m_activeSkill.State == BaseSkill.SkillState.Ready;
                
            } else
            {
                return true;
            }
            
            
        }

        public bool FriendlySkillsIsReady()
        {
            if (m_friendlySkill)
            {
                return m_friendlySkill.State == BaseSkill.SkillState.Ready;

            } else
            {
                return true;
            }

        }

        public void RestFriendlySkill()
        {
            CanFriendlySkill = true;
        }

    }


}