using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class SkillInfo
    {
        public string SkillName;
        public int CDTime;
        public Vector2 CDLocPos;
        public bool AutoDestory;
        public float Speed;
        public int LineNumber;
        public int WaveNumber;
        public float LifeTime;
        public float IntervalTime;
        public Color NormalColor;
        public float Damage;
//        public bool EnableEmission;

        public SkillInfo(PrototypeSystem.PrototypeSkill skill)
        {
            SkillName = skill.SkillName;
            CDTime = skill.CDTime;
            AutoDestory = skill.AutoDestory;
            Speed = skill.Speed;
            LineNumber = skill.LineNumber;
            WaveNumber = skill.WaveNumber;
            IntervalTime = skill.IntervalTime;
            LifeTime = skill.LifeTime;
//            EnableEmission = skill.EnableEmission;
            NormalColor = new Color(skill.NormalColorR, skill.NormalColorG, skill.NormalColorB);
            CDLocPos = new Vector2(skill.CDLocPosX, skill.CDLocPosY);
            Damage = skill.Damage;
        }
    }


    public class UnitInfo
    {
        public float Hp;
        public Vector2 Position;
        public float Attack;
        public string ImageSource;


        public UnitInfo(PrototypeSystem.PrototypeInfo baseInfo)
        {
            Hp = baseInfo.Hp;
            Position = new Vector2(baseInfo.pos_x, baseInfo.pos_y);
            Attack = baseInfo.Attack;
            ImageSource = baseInfo.ImgSrc;

        }

    }


    public class HeroInfo : UnitInfo{
    
        public string Name;
        public float Speed; 
        public SkillInfo FriendlySkill;

        public HeroInfo(PrototypeSystem.PrototypeHeroInfo heroInfo) : base(heroInfo)
        {
            Name = heroInfo.Name;
            Speed = heroInfo.Speed;

            if (heroInfo.FriendlySkill != null)
            {
                FriendlySkill = new SkillInfo(heroInfo.FriendlySkill);
            }
        }
    
    }


    public enum EnemyType
    {
        Normal,
        Boss
    }


    public class EnemyInfo : UnitInfo {
   
        public EnemyType Type;
        public List<SkillInfo> EnemySkill;

        public EnemyInfo(PrototypeSystem.PrototypeEnemyInfo enemyInfo) : base(enemyInfo)
        {
            Type = (EnemyType)enemyInfo.EnemyType;

            EnemySkill = new List<SkillInfo>();
            foreach (var skill in enemyInfo.EnemySkill)
            {
                EnemySkill.Add(new SkillInfo(skill));
            }
        }

    }





}