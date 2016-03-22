﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class WaveComponent : MonoBehaviour {

        public GameObject EnemyLayer;
        public int CurrentWaveIndex;

        public GameObject MonsterPrefab;
        public GameObject BossPrefab;
//        [SerializeField]
        private List<WaveInfo> m_AllWave;


        public List<EnemyComponent> CurrentEnemy
        {
            get{
                return m_CurrentEnemy;
            }
        }

        private List<EnemyComponent> m_CurrentEnemy = new List<EnemyComponent>();
        

        private float enemyAttackIntervalTime = 0.8f;
        private static WaveComponent m_WaveComponent;
        public static WaveComponent Instance
        {
            get{
                return m_WaveComponent;
            }
        }
        public EnemyComponent Boss
        {
            get{
                foreach(var enemy in CurrentEnemy)
                {
                    if(enemy.Type == EnemyType.Boss)
                    {
                        return enemy;
                    }
                }

                return null;
            }
        }


        void Awake()
        {
            m_WaveComponent = this;
        }

    	// Use this for initialization
    	void Start () {
        
        }
        
        // Update is called once per frame
        void Update () {
    	}


        public void Initialize()
        {
            m_AllWave = PrototypeSystem.Instance.CastWaveInfoList();
            CurrentWaveIndex = 0;

      
            
        }

        public void InitializeCurrentEnemy()
        {
            if (m_AllWave.Count > 0)
            {
                InitializeEnemy(m_AllWave[CurrentWaveIndex].EnemyInfo);
            }

        }

        public void InitializeEnemy(List<UnitInfo> enemyInfoList)
        {
            if (enemyInfoList == null)
            {
                return;
            }

            foreach(var enemyInfo in enemyInfoList)
            {

                var enemy = enemyInfo as EnemyInfo;
                GameObject enemy_instance = null;
                switch(enemy.Type)
                {
                    case EnemyType.Boss:
                        enemy_instance = Instantiate(BossPrefab) as GameObject;
                        break;
                    case EnemyType.Normal:
                        enemy_instance = Instantiate(MonsterPrefab) as GameObject;
                        break;
                }
               
                if (enemy_instance)
                {
                    var enemy_cmp = enemy_instance.GetComponent<EnemyComponent>();
                    if(enemy_cmp)
                    {
                        enemy_instance.transform.SetParent(EnemyLayer.transform);
                        enemy_instance.transform.localPosition = Vector3.zero;
                        enemy_instance.transform.localScale = Vector3.one;
                        
                        enemy_cmp.Initialize(enemyInfo);
                        m_CurrentEnemy.Add(enemy_cmp);
                        
                    }
                    else
                    {
                        Destroy(enemy_instance);
                        Debug.LogWarning("[InitializeCurrentEnemy] No has EnemyComponent");
                        
                    }
                    
                    
                    
                }
            }


        }



        IEnumerator LaunchEnemiesSkill()
        {
            foreach(var enemy in CurrentEnemy)
            {
                if(enemy.LaunchEnemySkill())
                {
                    yield return new WaitForSeconds(enemyAttackIntervalTime);
                }
            }

            enemiesAttack = true;
        }


        private bool enemiesAttack;
        private Coroutine enemiesSkillCoroutine = null;
        public bool EnemiesAttack()
        {
            if(enemiesSkillCoroutine == null)
            {
                enemiesAttack = false;
                enemiesSkillCoroutine = StartCoroutine(LaunchEnemiesSkill());
            }

            if(enemiesAttack)
            {
                enemiesSkillCoroutine = null;
            }

            return enemiesAttack;
        }

        public bool EnemiesSkillsIsReady()
        {

            foreach(var enemy in CurrentEnemy)
            {
                if(enemy.SkillsIsReady() == false)
                {
                    return false;
                }
            }


            return true;

        }


        public void ReduceEnemiesCD()
        {
            foreach(var enemy in CurrentEnemy)
            {
                enemy.ReduceSkillsCD();
            }
        }

        public void HandleDeadEnemies()
        {
            int i = 0;
            while(i < CurrentEnemy.Count)
            {
                var enemy = CurrentEnemy[i];
                if(enemy.HPProperty.Value == 0)
                {
                    Destroy(enemy.gameObject);
                    CurrentEnemy.Remove(enemy);
                    continue;
                }
                else
                {
                    i++;
                }

            }
        }

        public bool CheckEnemiesDead()
        {
            if (CurrentEnemy.Count == 0)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public bool IsBossWave()
        {
            var boss = Boss;

            return (boss) ? true : false;
        }


    }



}