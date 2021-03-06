﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{

    public class SkillInfo
    {
        public string SkillName;
        public int CDTime;
        public Vector2 CDLocPos;
        public float Speed;
        public int LineNumber;
        public int WaveNumber;
        public float LifeTime;
        public float IntervalTime;
        public Color NormalColor;
        public float Damage;
        public float Size;
        public float RotationDeg;
		public float Radius;

        public SkillInfo(PrototypeSystem.PrototypeSkill skill)
        {
            SkillName = skill.SkillName;
            CDTime = skill.CDTime;
            Speed = skill.Speed;
            LineNumber = skill.LineNumber;
            WaveNumber = skill.WaveNumber;
            IntervalTime = skill.IntervalTime;
            LifeTime = skill.LifeTime;
            NormalColor = new Color(skill.NormalColorR, skill.NormalColorG, skill.NormalColorB);
            CDLocPos = new Vector2(skill.CDLocPosX, skill.CDLocPosY);
            Damage = skill.Damage;
            Size = skill.Size;
            RotationDeg = skill.RotationDeg;
			Radius = skill.Radius;
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
//        public int ActiveSkillCD;
        public string SpineData;
        public SkillInfo ActiveSkill;

        public HeroInfo(PrototypeSystem.PrototypeHeroInfo heroInfo) : base(heroInfo)
        {
            Name = heroInfo.Name;
            Speed = heroInfo.Speed;
//            ActiveSkillCD = heroInfo.ActiveSkillCD;
            SpineData = heroInfo.SpineData;

            if (heroInfo.FriendlySkill != null)
            {
                FriendlySkill = new SkillInfo(heroInfo.FriendlySkill);
            }

            if (heroInfo.ActiveSkill != null)
            {
                ActiveSkill = new SkillInfo(heroInfo.ActiveSkill);
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
        public List<Vector2> WeakPoint;

        public EnemyInfo(PrototypeSystem.PrototypeEnemyInfo enemyInfo) : base(enemyInfo)
        {
            Type = (EnemyType)enemyInfo.EnemyType;

            EnemySkill = new List<SkillInfo>();
            foreach (var skill in enemyInfo.EnemySkill)
            {
                EnemySkill.Add(new SkillInfo(skill));
            }

            WeakPoint = new List<Vector2>();
            if(enemyInfo.WeakPoint != null)
            {
                foreach(var locPos in enemyInfo.WeakPoint)
                {
                    WeakPoint.Add(new Vector2(locPos.LocPosX, locPos.LocPosY));
                }

            }


        }

    }





}