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

        public int OurTurnCounter = 0;

        public readonly string DoneEvent = "Done";
        public readonly string WaitHeroBattleEndEvent = "WaitHeroBattleEnd";
        public readonly string CastActiveSkillEvent = "CastActiveSkill";
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

        public void HandleTurnStart()
        {
            for(int i = 0; i < WaveComponent.Instance.CurrentEnemy.Count; i++)
            {
                var enemy = WaveComponent.Instance.CurrentEnemy[i];
                enemy.RandomWeakPoint();
            }
        }


        public void HandleEnemyRoundStart()
        {
            WaveComponent.Instance.EnemyLayer.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
        }


        IEnumerator LaunchEnemiesSkill()
        {
         
            for(int i = 0; i < WaveComponent.Instance.CurrentEnemy.Count; i++)
            {
                var enemy = WaveComponent.Instance.CurrentEnemy[i];
                if(enemy.LaunchEnemySkill())
                {

                    foreach(var e in WaveComponent.Instance.CurrentEnemy)
                    {
                        e.RandomWeakPoint();
                    }

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
            OurTurnCounter++;
            BottomUI.UpdateOurActiveCounter();

            TeamComponent.Instance.HandleHerosTrigger(true);
            TeamComponent.Instance.HeroLayer.BroadcastMessage(InterfaceMehodName.RestFriendlySkill, SendMessageOptions.DontRequireReceiver);

            Wall.DamageWallReduceTurn();

            if(OurTurnCounter > 1)
            {
                // reduce hero action skill cd
//                for (int i = 0; i < BottomUI.HeroSlotList.Count; i ++)
//                {
//                    BottomUI.HeroSlotList[i].SendMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
//                }
                TeamComponent.Instance.HeroLayer.BroadcastMessage(InterfaceMehodName.ReduceCD, SendMessageOptions.DontRequireReceiver);
                

            }
        }


        public void HandleWaitPlayerBattleEnd()
        {
            if (TeamComponent.Instance.HerosIsStillness() && TeamComponent.Instance.HerosFriendlySkillsReady() && TeamComponent.Instance.HerosActiveSkillsReady())
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

            ComboComponent.Instance.Rest();
            ComboComponent.Instance.EnableCombo(false);


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