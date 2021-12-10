using UnityEngine;
using System.Collections;
using GSStorm.RPG.Game;

namespace GSStorm.RPG.Engine{

	/// <summary>
	/// Base beviour class for skill behaviours
	/// 
	/// Just put some sharing code for all here,
	/// for e.g when the skill stage callbacks 
	/// are executed
	/// </summary>
	public class AttackSkillBaseBehaviour<T>: MonoBehaviour where T : AttackSkill
	{
		/// <summary>
		/// The attack skill model object
		/// </summary>
		[HideInInspector]
		public T Skill;

		/// <summary>
		/// The caster of the skill
		/// </summary>
		[HideInInspector]
		public CombatUnit Caster;

		protected virtual void OnTriggerEnter2D(Collider2D other){
			//get the CombatUnit from other
            CombatUnit hitTarget = GetValidHitTarget(other);

			if (hitTarget == null)
				return;

			//hit some thing
			if (Skill == null) {
				Debug.LogError ("Lost reference to the main attack skill !");
				return;
			}

			if (Caster == null) {
				Debug.LogError ("Lost refernce to the caster !");
				return;
			}

			//Before hit stage callback
			Skill.BeforeHit (Caster, hitTarget);

			//On hit stage callback
			Skill.OnHit (Caster, hitTarget);
		}

        protected CombatUnit GetValidHitTarget(Collider2D other){
            CombatUnit hitTarget = null;

			if (!Caster.TargetEnemyLayers.Contains (other.gameObject.layer))
				return null;  //not attacking those out of caster's target range

            // Any better way to get the target model?
			if (other.GetComponent<MonsterController> () != null) {
				//Skill hits a monster
				hitTarget = other.GetComponent<MonsterController> ().CurrentCharacter;
			} else if (other.GetComponent<PlayerController> () != null) {
				//Skill hist a player
				hitTarget = other.GetComponent<PlayerController> ().CurrentCharacter;
			} else {
				return null;
			}

			if (hitTarget == Caster) {
				return null; // You are never going to hit yourself
			}

			return hitTarget;
		}

		protected IEnumerator ReleaseToPool(string poolName, float delay){
			yield return new WaitForSeconds (delay);
			ObjectPoolManager.Current.ReleaseToPool (poolName, gameObject);
		}
			
	}
}

