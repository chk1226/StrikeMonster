using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StrikeMonster
{

    public class BaseSkill : MonoBehaviour {

        protected float damage = 0;
        protected float speed = 10;
        protected float lifeTime = 3;
        protected float emitterParticleSize = 1;
        protected int lineNumber = 8; 
        protected int waveNumber = 3;
        protected float intervalTime = 0.5f;
        protected float hitIntervalTime = 0.2f;
        protected Color normalColor;
        protected float size;
        protected float rotationDeg;
		protected float radius;
        [SerializeField]
        protected ParticleSystem emitter;

        protected List<UnitComponent> m_targets;
        public List<UnitComponent> Targets
        {
            get{
                return m_targets;
            }
            set{
                m_targets = value;
            }
        }
        /// <summary>
        /// parameter[0] is Thrust direction.
        /// </summary>
        public ArrayList Parameter = new ArrayList(5);

        public enum SkillState
        {
            Ready,
//            BeginFire,
            CurrentFire,
            EndFire,
//            Terminal
        }

        [HideInInspector]
        public SkillState State = SkillState.Ready;
        public CDPropertyComponent CDProperty;


        public virtual void Config(SkillInfo skillInfo)
        {
            speed = skillInfo.Speed;
            lineNumber = skillInfo.LineNumber;
            waveNumber = skillInfo.WaveNumber;
            lifeTime = skillInfo.LifeTime;
            normalColor = skillInfo.NormalColor;
            damage = skillInfo.Damage;
            intervalTime = skillInfo.IntervalTime;
            size = skillInfo.Size;
            rotationDeg = skillInfo.RotationDeg;
			radius = skillInfo.Radius;

            if(CDProperty)
            {
                CDProperty.Initialize(skillInfo.CDTime, 0, skillInfo.CDTime);
                CDProperty.SetCDTextLocalPosition(skillInfo.CDLocPos);
            }
        }


        void Awake()
        {
        }


        void Start () {
            OnStart();
        }

        void FixedUpdate(){
            if (State == SkillState.CurrentFire)
            {
                EveryWave();
                SwitchEndFire();
            }

            if(State != SkillState.Ready)
            {
                OnFixedUpdate();
                
                DectionParticleCollision2D();

            }

            if (State == SkillState.EndFire)
            {
                RecoveryReady();

                if(CDProperty && CDProperty.Value == 0)
                {
                    CDProperty.RecoveryCD();
                }
            }
        }

        protected virtual void OnStart(){}
        protected virtual void OnFixedUpdate(){}



        private int currentWaveNumber;
        protected int CurrentWaveNumber
        {
            get{ return currentWaveNumber; }
        }
        private float countTime = 0;
        public bool DoFire(){

            if (State == SkillState.Ready)
            {
                bool canFire = false;
                if(CDProperty && CDProperty.Value == 0)
                {
                    canFire = true;
                    State = SkillState.CurrentFire;
                    currentWaveNumber = waveNumber;
                }

                return canFire;
            }
            return false;
        }

       
        protected void EveryWave()
        {

            float now_time = Time.time;
            if (now_time > countTime && currentWaveNumber > 0)
            {

                AttackBehavior();

                countTime = now_time + intervalTime;
                currentWaveNumber--;

            }
           

        }

        protected virtual void AttackBehavior(){
        }

        private void SwitchEndFire()
        {

            if (currentWaveNumber == 0)
            {
                State = SkillState.EndFire;
            }

        }

        protected virtual void RecoveryReady()
        {
            State = SkillState.Ready;
        }

        protected virtual void DectionParticleCollision2D(){
        }
       

        protected virtual void CollisionBehavior(UnitComponent unit)
        {
            unit.OnHurt(damage);
        }





    }



}