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
        [SerializeField]
        protected ParticleSystem emitter;
        [SerializeField]
        protected List<ParticleSystem> rayEmitter;

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


        public enum SkillState
        {
            Ready,
//            BeginFire,
            CurrentFire,
            EndFire,
//            Terminal
        }

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

            if(CDProperty)
            {
                CDProperty.Initialize(skillInfo.CDTime, 0, skillInfo.CDTime);
                CDProperty.SetCDTextLocalPosition(skillInfo.CDLocPos);
            }
        }


        void Awake()
        {
//            emitter = GetComponent<ParticleSystem>();
            
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

            OnFixedUpdate();

            DectionParticleCollision2D();

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


//        protected virtual Vector2 GetFireDirection(int lineNumber)
//        {
//            return Vector2.one;
//        }

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

        protected void Fire (Vector3 _direction)
        {
            Fire(_direction, speed, lifeTime, normalColor);
        }

        private void Fire ( Vector3 _direction, float _speed, float _lifeTime, Color _color)
        {
            if (emitter)
            {
                emitter.Emit(transform.position, _direction.normalized * _speed, emitterParticleSize, _lifeTime, _color);
            }
        }


        protected virtual void DectionParticleCollision2D(){
        }
       

        protected virtual void CollisionBehavior(UnitComponent unit)
        {
            unit.OnHurt(damage);
        }


    }



}