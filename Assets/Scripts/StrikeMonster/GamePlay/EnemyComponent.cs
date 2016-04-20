using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{
    public class EnemyComponent : UnitComponent {

        public EnemyType Type;
        public HpPropertyComponent HPProperty;

        private List<BaseSkill> skills = new List<BaseSkill>();

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

                // hp 
                if(HPProperty)
                {
                    HPProperty.Initialize(enemyInfo.Hp, 0, enemyInfo.Hp);
                }

            }

        }


        void OnCollisionEnter2D(Collision2D coll) {

            var hero = coll.gameObject.GetComponent<HeroComponent>();
            if (hero == TeamComponent.Instance.CurrentHero)
            {
                OnHurt(hero.Attack);
            }


        }

        public override void OnHurt (float damage)
        {  
            HPProperty.Value -= damage;
            base.OnHurt(damage);
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