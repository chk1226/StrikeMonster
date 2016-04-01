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
            size = skillInfo.Size;
            rotationDeg = skillInfo.RotationDeg;


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


        protected bool IntersectsCircleToRect(Vector2 circlePos, float circleRadius, Bounds rect)
        {
            Vector2 distance = new Vector2(Mathf.Abs(circlePos.x - rect.center.x), Mathf.Abs(circlePos.y - rect.center.y));
            
            if (distance.x > (rect.extents.x + circleRadius))
                return false;
            if (distance.y > (rect.extents.y + circleRadius))
                return false;
            
            
            if (distance.x <= rect.extents.x)
                return true;
            if (distance.y <= rect.extents.y)
                return true;
            
            float cornerDistance = Mathf.Pow(distance.x - rect.extents.x, 2) + Mathf.Pow(distance.y - rect.extents.y, 2);
            
            return cornerDistance <= Mathf.Pow(circleRadius, 2);
            
        }

        protected bool IntersectsRectToRect(Bounds preRect, Bounds postRect)
        {
            Vector3 reflect = new Vector3(-1, 1, 1);

            if(preRect.Contains(postRect.max) || preRect.Contains(postRect.min) || 
               preRect.Contains(postRect.center + Vector3.Scale(postRect.extents, reflect)) ||
               preRect.Contains(postRect.center - Vector3.Scale(postRect.extents, reflect)))
            {
                return true;
            }



            if(postRect.Contains(preRect.max) || postRect.Contains(preRect.min) || 
               postRect.Contains(preRect.center + Vector3.Scale(preRect.extents, reflect)) ||
               postRect.Contains(preRect.center - Vector3.Scale(preRect.extents, reflect)))
            {
                return true;
            }

            
            return false;
        }
    }



}