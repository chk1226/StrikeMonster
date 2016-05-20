using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
    public class EnemyComponent : UnitComponent {

        public EnemyType Type;
        public HpPropertyComponent HPProperty;
        public GameObject WeakPointGroup;
        public GameObject WeakPointPrefab;

        private List<WeakPointComponent> m_WeakPointList = new List<WeakPointComponent>();
        public List<WeakPointComponent> WeakPointList
        {
            get{return m_WeakPointList;}
        }


        private List<BaseSkill> skills = new List<BaseSkill>();
        private System.Random rnd = new System.Random();

        public override void Initialize (UnitInfo baseInfo)
        {
            var enemyInfo = baseInfo as EnemyInfo;
            if (enemyInfo != null)
            {
                base.Initialize (baseInfo);

                Type = enemyInfo.Type;

                foreach(var skill in enemyInfo.EnemySkill)
                {
                    var prefab = Instantiate(Resources.Load<GameObject>(SKILL_PATH + skill.SkillName));

                    if(prefab)
                    {
                        prefab.transform.SetParent(SkillGroup.transform);
                        prefab.transform.localPosition = Vector3.zero;
                        prefab.transform.localScale = Vector3.one;


                        var skillCmp = prefab.GetComponent<BaseSkill>();
                        if(skillCmp)
                        {
                            skillCmp.Config(skill);
                            skillCmp.Targets = TeamComponent.Instance.Team;

                            skills.Add(skillCmp);
                        }

                    }
                }

                // weak point
                foreach(var weakPoint in enemyInfo.WeakPoint)
                {
                    var prefab = Instantiate(WeakPointPrefab);
                    if(prefab)
                    {
                        prefab.transform.SetParent(WeakPointGroup.transform);
                        prefab.transform.localPosition = new Vector3(weakPoint.x, weakPoint.y, 0);
                        prefab.transform.localScale = Vector3.one;
                        
                        var wpCmp = prefab.GetComponent<WeakPointComponent>();
                        if(wpCmp)
                        {
                            wpCmp.gameObject.SetActive(false);
                            wpCmp.Self = this;
                            m_WeakPointList.Add(wpCmp);
                        }
                    }

                }

                // hp 
                if(HPProperty)
                {
                    HPProperty.Initialize(enemyInfo.Hp, 0, enemyInfo.Hp);
                }

            }

        }

        public void RandomWeakPoint()
        {
            if(m_WeakPointList.Count > 0)
            {
                var index = rnd.Next() % m_WeakPointList.Count;
                for(int i = 0; i < m_WeakPointList.Count; i++)
                {
                    if(i == index)
                    {
                        m_WeakPointList[i].gameObject.SetActive(true); 
                    }
                    else
                    {

                        m_WeakPointList[i].gameObject.SetActive(false);
                    }
                }


            }
        }


        void OnCollisionEnter2D(Collision2D coll) {

            var hero = coll.gameObject.GetComponent<HeroComponent>();
            if (hero == TeamComponent.Instance.CurrentHero && GamePlaySettings.Instance.IsPlayerRound)
            {
                OnHurt(hero.Attack);
            }


        }

        public override void OnHurt (float damage)
        {  
            HPProperty.Value -= damage;
            base.OnHurt(damage);

            ComboComponent.Instance.EnableCombo(true);
            ComboComponent.Instance.Increment(1);
        }


        public bool LaunchEnemySkill()
        {
            bool hasSkillLaunch = false;
            foreach(var skill in skills)
            {
                skill.DoFire();

                if(skill.State == BaseSkill.SkillState.CurrentFire)
                {
                    hasSkillLaunch = true;
                }
            }

            return hasSkillLaunch;
        }


        public bool SkillsIsReady()
        {
            foreach(var skill in skills)
            {
                if(skill.State != BaseSkill.SkillState.Ready)
                {
                    return false;
                }
            }

            return true;
        }




    }

}