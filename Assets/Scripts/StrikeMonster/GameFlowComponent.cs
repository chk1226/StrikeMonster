using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace StrikeMonster
{
    public class GameFlowComponent : MonoBehaviour {

        public BottomUIComponent BottomUI;
        public WallComponent Wall;
//        public TopUIComponent TopUI;
        public PlayMakerFSM GameFlowFSM;


        public readonly string DoneEvent = "Done";
        public readonly string WaitHeroBattleEndEvent = "WaitHeroBattleEnd";
        public readonly string NextWaveEvent = "NextWave";
        public readonly string GameOverEvent = "GameOver";
        
        public readonly float EnemyAttackIntervalTime = 0.8f;
        

        public static GameFlowComponent Instance = null;
        void Awake()
        {
            if (null == Instance)
            {
                Instance = this;
            }
        }

        public void Initialize()
        {
            TeamComponent.Instance.Initialize();
            WaveComponent.Instance.Initialize();
            BottomUI.Initialize();
        }


        public void HandleWaveStart()
        {
            WaveComponent.Instance.InitializeCurrentEnemy();

            if (WaveComponent.Instance.Boss)
            {
                GamePlaySettings.Instance.IsBossWave = true;
            } else
            {
                GamePlaySettings.Instance.IsBossWave = false;
                
            }


        }

        public void HandleEnemyRoundStart()
        {
            WaveComponent.Instance.EnemyLayer.BroadcastMessage("ReduceCD", SendMessageOptions.DontRequireReceiver);
        }


        IEnumerator LaunchEnemiesSkill()
        {
            
            for(int i = 0; i < WaveComponent.Instance.CurrentEnemy.Count; i++)
            {
                var enemy = WaveComponent.Instance.CurrentEnemy[i];
                if(enemy.LaunchEnemySkill())
                {
                    yield return new WaitForSeconds(EnemyAttackIntervalTime);
                }
            }
            
            GameFlowFSM.SendEvent(DoneEvent);
        }

        public void HandleEnemyActive()
        {
            StartCoroutine(LaunchEnemiesSkill());
        }

        public void HandleEnemyRoundEnd()
        {
            // check enemies skill is ready
            for(int i = 0; i < WaveComponent.Instance.CurrentEnemy.Count; i++)
            {
                var enemy = WaveComponent.Instance.CurrentEnemy[i];

                if(enemy.SkillsIsReady() == false)
                {
                    return;
                }
            }


            GameFlowFSM.SendEvent(DoneEvent);
            
        }


        public void HandlePlayerRoundStart()
        {
            TeamComponent.Instance.HandleHerosTrigger(true);
            TeamComponent.Instance.HeroLayer.BroadcastMessage("RestFriendlySkill", SendMessageOptions.DontRequireReceiver);
            Wall.DamageWallReduceTurn();
        }


        public void HandleWaitPlayerBattleEnd()
        {
            if (TeamComponent.Instance.HerosIsStillness() && TeamComponent.Instance.HerosFriendlySkillsReady())
            {
                GameFlowFSM.SendEvent(DoneEvent);
            }

        }

        public void HandlePlayerRoundEnd()
        {
            TeamComponent.Instance.HandleHerosTrigger(false);
            TeamComponent.Instance.NextHero();


        }


        public void HandleTurnEnd()
        {
            WaveComponent.Instance.HandleDeadEnemies();

            if(WaveComponent.Instance.CurrentEnemy.Count == 0)
            {
                GameFlowFSM.SendEvent(NextWaveEvent);
                return;
            }

            if (TeamComponent.Instance.IsGameOver())
            {
                GameFlowFSM.SendEvent(GameOverEvent);
                return;
            }
        }


    }


}