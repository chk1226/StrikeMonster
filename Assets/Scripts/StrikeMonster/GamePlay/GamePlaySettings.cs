using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

namespace StrikeMonster
{

	public class GamePlaySettings : MonoBehaviour
	{
		public static GamePlaySettings Instance = null;

        private int m_HeroLayer;
        public int HeroLayer
        {
            get{
                return m_HeroLayer;
            }
        }

        private int m_ActionLayer;
        public int ActionLayer
        {
            get{
                return m_ActionLayer;
            }
        }

        public enum SortingLayer
        {
            BackgroundLayer,
            BackEffectLayer,
            BattleLayer,
            FrontEffectLayer,
            BackUILayer,
            FrontUILayer,
            TouchLayer
        }

        readonly public float ThresholdVelocity = 0.3f;
//        public PlayMakerFSM GameFlowFSM;

        public bool IsPlayerRound
        {
            get{
                var fsmValue = FsmVariables.GlobalVariables.GetFsmBool("IsPlayerRound");
                if(fsmValue == null)
                {
                    Debug.LogWarning("IsPlayerRound bool value is null");
                    return false;
                }
                else
                {
                    return fsmValue.Value;
                }
            }
        }

        public bool CanStrike
        {
            get{
                var fsmValue = FsmVariables.GlobalVariables.GetFsmBool("CanStrike");
                if(fsmValue == null)
                {
                    Debug.LogWarning("CanStrike bool value is null");
                    return false;
                }
                else
                {
                    return fsmValue.Value;
                }
            }
        }

        public bool IsBossWave
        {
            get{
                var fsmValue = FsmVariables.GlobalVariables.GetFsmBool("IsBossWave");
                if(fsmValue == null)
                {
                    Debug.LogWarning("IsBossWave bool value is null");
                    return false;
                }
                else
                {
                    return fsmValue.Value;
                }
            }

            set{
                var fsmValue = FsmVariables.GlobalVariables.GetFsmBool("IsBossWave");
                if(fsmValue != null)
                {
                    fsmValue.Value = value;
                }
            }
        }



//		public bool IsSetupWave = false;
		public bool IsGamePaused = false;
//		public bool IsGemCDPaused = false;
//		public bool IsMonsterATBPaused = false;
//		public bool IsHeroATBPaused = false;
//		public bool IsManuallyUpdateMonsterATB = false;
//		public bool IsManuallyUpdateHeroATB = false;
//
//		public bool IsTargetEnabled = true;
//		public bool IsTeamSkillEnabled = true;
//		public bool IsWheelRotationEnabled = true;
//		public bool IsManuallyAbilityTrigger = false;
//		public bool CanMonsterAttackInAllCondition = false;
//		public bool CanHeroUseAbilityInMostCondition = false; // Still affected by GCD
//
//		protected float m_PreviousTimeScale = 1.0f;
//
//		private Stack<GamePlaySettings> m_SettingStack;

		//--------------------------------------------------------------------------------
		void Awake()
		{
			if (null == Instance)
			{
				Instance = this;
                m_HeroLayer = LayerMask.NameToLayer("HeroLayer");
                m_ActionLayer = LayerMask.NameToLayer("ActionLayer");
			}
		}

//		//--------------------------------------------------------------------------------
//		public void Pause()
//		{
//			m_PreviousTimeScale = Time.timeScale;
//
//			PushSetting();
//
//			Time.timeScale = 0.0f;
//			IsTargetEnabled = false;
//			IsWheelRotationEnabled = false;
//			IsGamePaused = true;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void Resume()
//		{
//			PopSetting();
//
//			Time.timeScale = m_PreviousTimeScale;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PauseATB()
//		{
//			PauseGemCD();
//			PauseMonsterATB();
//			PauseHeroATB();
//		}
//
//		//--------------------------------------------------------------------------------
//		public void ResumeATB()
//		{
//			ResumeGemCD();
//			ResumeMonsterATB();
//			ResumeHeroATB();
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PauseMonsterATB()
//		{
//			IsMonsterATBPaused = true;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void ResumeMonsterATB()
//		{
//			IsMonsterATBPaused = false;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void ResumeHeroATB()
//		{
//			IsHeroATBPaused = false;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PauseHeroATB()
//		{
//			IsHeroATBPaused = true;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void ResumeGemCD()
//		{
//			IsGemCDPaused = false;
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PauseGemCD()
//		{
//			IsGemCDPaused = true;
//		}

		//--------------------------------------------------------------------------------
		public float GetDeltaTime()
		{
			if (IsGamePaused)
			{
				return 0;
			}
			else
			{
				return Time.deltaTime;
			}
		}

//		//--------------------------------------------------------------------------------
//		public float GetGemCDDeltaTime()
//		{
//			if (IsGemCDPaused)
//			{
//				return 0;
//			}
//			else
//			{
//				return Time.deltaTime;
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		public float GetMonsterATBDeltaTime()
//		{
//			if (!CanMonsterAttackInAllCondition && (IsMonsterATBPaused || IsManuallyUpdateMonsterATB))
//			{
//				return 0;
//			}
//			else
//			{
//				return Time.deltaTime;
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		public float GetHeroATBDeltaTime()
//		{
//			if (IsHeroATBPaused || IsManuallyUpdateHeroATB)
//			{
//				return 0;
//			}
//			else
//			{
//				return Time.deltaTime;
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		public float GetBuffDebuffDeltaTime()
//		{
//			if (IsHeroATBPaused || IsMonsterATBPaused)
//			{
//				return 0;
//			}
//			else
//			{
//				return Time.deltaTime;
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		public void Restart()
//		{
//		}
//
//		//--------------------------------------------------------------------------------
//		public void Quit()
//		{
//		}
//
//		//--------------------------------------------------------------------------------
//		public GameObject GetCharacterByInstanceID(int instanceID, QUERY_TYPE type)
//		{
//			if (type == QUERY_TYPE.QUERY_TYPE_HERO)
//			{
//				return HeroTeamComponent.Current.GetHeroByInstanceID(instanceID);
//			}
//			else if (type == QUERY_TYPE.QUERY_TYPE_MONSTER)
//			{
//				return MonsterTeamComponent.Current.GetMonsterByInstanceID(instanceID);
//			}
//			else
//			{
//				GameObject found_object = HeroTeamComponent.Current.GetHeroByInstanceID(instanceID);
//				if (null == found_object)
//				{
//					found_object = MonsterTeamComponent.Current.GetMonsterByInstanceID(instanceID);
//				}
//				return found_object;
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PushSetting()
//		{
//			if (null == m_SettingStack)
//			{
//				m_SettingStack = new Stack<GamePlaySettings>(2);
//			}
//
//			GamePlaySettings current_setting = gameObject.AddComponent<GamePlaySettings>();
//			AssignSettings(this, current_setting);
//			m_SettingStack.Push(current_setting);
//		}
//
//		//--------------------------------------------------------------------------------
//		public void PopSetting()
//		{
//			if (null != m_SettingStack)
//			{
//				GamePlaySettings last_setting = m_SettingStack.Peek();
//				if (null != last_setting)
//				{
//					AssignSettings(last_setting, this);
//					m_SettingStack.Pop();
//					DestroyImmediate(last_setting);
//				}
//			}
//		}
//
//		//--------------------------------------------------------------------------------
//		private void AssignSettings(GamePlaySettings from, GamePlaySettings to)
//		{
//			to.IsGamePaused = from.IsGamePaused;
//			to.IsGamePaused = from.IsGamePaused;
//			to.IsGemCDPaused = from.IsGemCDPaused;
//			to.IsMonsterATBPaused = from.IsMonsterATBPaused;
//			to.IsHeroATBPaused = from.IsHeroATBPaused;
//			to.IsManuallyUpdateMonsterATB = from.IsManuallyUpdateMonsterATB;
//			to.IsManuallyUpdateHeroATB = from.IsManuallyUpdateHeroATB;
//			to.IsTargetEnabled = from.IsTargetEnabled;
//			to.IsTeamSkillEnabled = from.IsTeamSkillEnabled;
//			to.IsWheelRotationEnabled = from.IsWheelRotationEnabled;
//			to.IsManuallyAbilityTrigger = from.IsManuallyAbilityTrigger;
//			to.CanMonsterAttackInAllCondition = from.CanMonsterAttackInAllCondition;
//			to.CanHeroUseAbilityInMostCondition = from.CanHeroUseAbilityInMostCondition;
//			to.m_PreviousTimeScale = from.m_PreviousTimeScale;
//		}
	} 
}