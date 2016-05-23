using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace StrikeMonster
{
	public class BarrageSkill : BaseSkill {

		private List<Vector3> orginalPositions;


        public override void Config(SkillInfo skillInfo)
        {
            base.Config(skillInfo);

            orginalPositions = new List<Vector3>();
            float angle = (2 * Mathf.PI) / lineNumber;
            
            
            for (int i = 0; i < lineNumber; i++)
            {
                float x = Mathf.Cos( angle * i ) ;
                float y = Mathf.Sin( angle * i );
                orginalPositions.Add( new Vector3( x, y, 0 ) );
            }
        }



        protected Vector2 GetFireDirection(int lineNumber)
        {
            return orginalPositions[lineNumber];
        }


        protected override void AttackBehavior()
        {
            for(int i=0 ; i<lineNumber; i++)
            {
                Fire( GetFireDirection(i));            
            }
        }

        protected override void RecoveryReady()
        {
            if (emitter && emitter.particleCount == 0)
            {
                
                base.RecoveryReady();
            }
        }

        protected override void DectionParticleCollision2D()
        {
            if (!emitter || emitter.particleCount == 0)
            {
                return;
            }
            
            if(Targets == null)
            {
                return;
            }
            
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[emitter.particleCount];
            emitter.GetParticles(particles);
            
            bool isDirty = false;
            for(int i = 0; i< particles.Length; i++)
            {
                
                foreach(var target in Targets)
                {
                    var collider2D = target.gameObject.GetComponent<Collider2D>();
                    if(collider2D != null)
                    {
                        if(collider2D.bounds.Contains(particles[i].position))
                        {
                            particles[i].lifetime = 0;
                            isDirty = true;
                            
                            
                            CollisionBehavior(target);

                        }
                    }
                    
                }
                
            }
            
            
            if (isDirty)
            {
                emitter.SetParticles(particles, particles.Length);
            }
        }

        private void Fire (Vector3 _direction)
        {
            if (emitter)
            {
                emitter.Emit(transform.position, _direction.normalized * speed, emitterParticleSize, lifeTime, normalColor);
            }
        }
        


	}


}