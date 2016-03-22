using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
    public class HeroComponent : UnitComponent {

        public string Name;
        public Rigidbody2D Rigidbody_2D;
        public CircleCollider2D CircleCollider_2D;
        public bool CanFriendlySkill;

        private BaseSkill m_friendlySkill;
        private float m_Speed;

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
                            skillCmp.CDProperty.DisableCDText();
                            m_friendlySkill = skillCmp;                        
                        }
                        
                    }

                }

            }

        }


        public void GiveVelocity(Vector2 velocity)
        {
            if (Rigidbody_2D)
            {
                Rigidbody_2D.velocity = velocity * m_Speed;
            }
        }

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


    	void OnCollisionEnter2D(Collision2D coll) {

            Debug.Log("[OnCollisionEnter2D] hit!" + coll.gameObject.name);

    	}

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (TeamComponent.Instance.CurrentHero != this && CanFriendlySkill)
            {

                if(WaveComponent.Instance.CurrentEnemy.Count == 0)
                {
                    m_friendlySkill.Targets = null;
                }
                else
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


            }

//            Debug.Log("[OnTriggerEnter2D] Trigger!" + coll.gameObject.name);
            
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


    }


}