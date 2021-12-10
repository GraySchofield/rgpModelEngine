using System;
using UnityEngine;
using GSStorm.RPG.Game;
using System.Collections;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Attack skill projectile behaviour.
	/// 
	/// controls the behaviour and life cycle of the attack skill projectile
	/// </summary>
	public class AttackSkillProjectileBehaviour : AttackSkillBaseBehaviour<AttackSkillProjectile>
	{
		private Vector3 _direction; //The direction where the projectile will fly towards
		private Timer _effectTimer; //The timer for the max time the skill projectile will fly
		private float _speed; //how fast the projectile will move

		private Animator _animator;
		private Rigidbody2D _rb;
		bool isExplode = false;

		void Awake(){
			_animator = GetComponent<Animator> ();
			_rb = GetComponent<Rigidbody2D> ();
		}

		void OnEnable(){
			if (Skill != null) {
                _effectTimer = new Timer (Skill.Attributes[AttributeType.PROJECTILE_FLYING_TIME].Value);
				_effectTimer.Start ();
				_speed = Skill.Attributes[AttributeType.PROJECTILE_SPEED_C].Value;
			}

			//get the correct direction the skill will fly
			if (Caster != null) {
				_direction =  (Quaternion)Caster.Rotation * Vector3.right;
			}

			isExplode = false;
		}

		void Update(){
			if (Skill != null && Caster != null && ! isExplode) {
				//move the project towards the direction  with the speed
				_rb.velocity = _direction.normalized * _speed;

				//Update Timer
				_effectTimer.Update (Time.deltaTime);

				//Check Timer finish
				if (_effectTimer.State == TimerState.FINISHED) {
					_rb.velocity = Vector3.zero;
					_animator.SetTrigger("Explode");

					StartCoroutine (ReleaseToPool (PrefabConst.SKILL_PROJECTILE, 0.3f));
				}
			}
		}


		protected override void OnTriggerEnter2D(Collider2D other){
			base.OnTriggerEnter2D (other);

			if (GetValidHitTarget(other) == null)
				return;

			//We have hit a Target;
			_rb.velocity = Vector3.zero;
			isExplode = true;

			_animator.SetTrigger("Explode");

			StartCoroutine (ReleaseToPool (PrefabConst.SKILL_PROJECTILE, 0.3f));

		}
			
	

	}
}

