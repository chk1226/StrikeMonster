using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class WaveComponent : MonoBehaviour {

        public GameObject EnemyLayer;
        public GameObject SkillEffectLayer;
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

        private void InitializeEnemy(List<UnitInfo> enemyInfoList)
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

        public List<UnitComponent> GetTargets()
        {
            var targets = new List<UnitComponent>(); 

            for(int i = 0; i < CurrentEnemy.Count; i++)
            {
                targets.Add(CurrentEnemy[i] as UnitComponent);


                for(int j = 0; j < CurrentEnemy[i].WeakPointList.Count; j++)
                {
                    if(CurrentEnemy[i].WeakPointList[j].gameObject.activeSelf)
                    {
                        targets.Add(CurrentEnemy[i].WeakPointList[j] as UnitComponent);
                    }

                }

            }



            return targets;
        }


    }



}