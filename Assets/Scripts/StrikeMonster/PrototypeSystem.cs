using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace StrikeMonster
{

	public class PrototypeSystem 
	{
		private PrototypeSystem()
		{
		}
		static PrototypeSystem() {}
		private static PrototypeSystem m_Instance = null;
		public static PrototypeSystem Instance
		{
			get
			{
				if (m_Instance == null)
				{
//					GameObject game_object = new GameObject("PrototypeSystem");
//					m_Instance = game_object.AddComponent<PrototypeSystem>();
//					ManagerList.Instance.AddInToList(game_object);
//					DontDestroyOnLoad(game_object);

					m_Instance = new PrototypeSystem();
					m_Instance.Initialize();

				}
				
				return m_Instance;
			}
		}

		public class PrototypeInfo
		{
            public string ImgSrc;
            public float pos_x;
            public float pos_y;
			public float Hp;
			public float Attack;

		}

		public class PrototypeHeroInfo : PrototypeInfo
		{
            public string Name;
            public float Speed;
            public PrototypeSkill FriendlySkill;
            public string SpineData;
            public PrototypeSkill ActiveSkill;
		}

        public class PrototypeSkill
        {
            public string SkillName;
            public int CDTime;
            public float CDLocPosX;
            public float CDLocPosY;
            public float Speed;
            public int LineNumber;
            public int WaveNumber;
            public float IntervalTime;
            public float LifeTime;
            public float NormalColorR;
            public float NormalColorG;
            public float NormalColorB;
            public float Damage;
            public float RotationDeg;
            public float Size;


        }


        public class PrototypeEnemyInfo : PrototypeInfo
        {
            public int EnemyType;
            public List<PrototypeSkill> EnemySkill;
        }

        public class PrototypeWaveInfo
        {
            public List<PrototypeEnemyInfo> EnemyList;
        }


		public class PrototypeConfig
		{
			public List<PrototypeHeroInfo> HeroInfoList;
            public List<PrototypeWaveInfo> WaveInfoList;


            public PrototypeConfig()
            {
                this.HeroInfoList = new List<PrototypeHeroInfo>();
                this.WaveInfoList = new List<PrototypeWaveInfo>();
            }

		}

		public PrototypeConfig Config;

		//--------------------------------------------------------------------------------
		private bool Initialize()
		{
            m_Instance.Config = new PrototypeConfig();

            Deserialize("Config/Hero");
            Deserialize("Config/Level_1");
            
			
			return true;
		}

		//--------------------------------------------------------------------------------
		private bool Deserialize(string fileName)
		{
			try
			{
				TextAsset text = Resources.Load<TextAsset>(fileName);
				if (null != text)
				{
                    PrototypeConfig reg = Newtonsoft.Json.JsonConvert.DeserializeObject<PrototypeConfig>(text.text);

                    if(fileName.Contains("Hero"))
                    {
                        m_Instance.Config.HeroInfoList = reg.HeroInfoList;
                    }
                    else
                    {
                        m_Instance.Config.WaveInfoList = reg.WaveInfoList;
                    }
					return true;
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(string.Format("PrototypeSystem:Deserialize Error: {0}", e.ToString()));
			}

			return false;
		}


        public List<HeroInfo> CastHeroInfoList()
        {
            var heroInfoList = new List<HeroInfo>();

            foreach (var proto in Instance.Config.HeroInfoList)
            {
                heroInfoList.Add(new HeroInfo(proto));
            }

            return heroInfoList;
        }


        public List<WaveInfo> CastWaveInfoList()
        {
            var waveInfoList = new List<WaveInfo>();

            foreach (var proto in Instance.Config.WaveInfoList)
            {
                waveInfoList.Add(new WaveInfo(proto));
            }

            return waveInfoList;
        }

		//--------------------------------------------------------------------------------
//		public PrototypeHeroInfo GetHeroInfoByID(int id)
//		{
//			if (Config != null && Config.HeroInfoList != null)
//			{
//				foreach (var info in Config.HeroInfoList)
//				{
//					if (info.ID == id)
//					{
//						return info;
//					}
//				}
//			}
//			
//			return null;
//		}
//
//		//--------------------------------------------------------------------------------
//		public PrototypeMonsterInfo GetMonsterInfoByID(int id)
//		{
//			if (Config != null && Config.MonsterInfoList != null)
//			{
//				foreach (var info in Config.MonsterInfoList)
//				{
//					if (info.ID == id)
//					{
//						return info;
//					}
//				}
//			}
//
//			return null;
//		}
	} // class PrototypeSystem
} 
