using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEngine.UI;

namespace GSStorm.RPG.Game{
    /// <summary>
    /// Monster controller.
    /// </summary>
	public class MonsterController : BaseCharacterController
	{
		BaseAI _monsterAI;

		// Use this for initialization
		protected override void Start ()
		{
			base.Start ();

			_monsterAI = GetComponent<BaseAI> ();
		
			_animator.SetBool("IsMoving", true);

		}
		
		// Update is called once per frame
		protected override void Update ()
		{
			base.Update ();

			//React to AI state behaviour
			switch (_monsterAI.CurrentState) {
				case AIState.IDLE:
					//TODO: show idle animation
					_rigidBody.velocity = Vector2.zero;

					break;

				case AIState.CHASING:
					{
//						_animator.SetBool("IsMoving", true);

						//Rotate towards the target 
						Vector3 direction =  RotateTowards(_monsterAI.TargetCharacter.Position);

						// moving towards player
                        _rigidBody.velocity =  direction * CurrentCharacter.Attributes[AttributeType.MOVE_SPEED_C].Value;

					}
					break;

				case AIState.ATTACK:
					{
//						_animator.SetBool("IsMoving", false);

						RotateTowards(_monsterAI.TargetCharacter.Position);

						_rigidBody.velocity = Vector2.zero;

						CurrentCharacter.PreAttack ();
						
					}

					break;

				case AIState.DEAD:
//					_animator.SetBool("IsMoving", false);

					//Dead, destroy the game object 
					//TODO: show dead animation
					//TODO: drop a weapon, harcde drop, this should come from a droplist
					if(UnityEngine.Random.Range(0,1f) < 0.5f)
					{
						CoreGameController.Current.DropMapItem("gear_weapon_iron_sword", transform.position, transform.rotation);

					}
					else
					{
						CoreGameController.Current.DropMapItem("gear_weapon_iron_bow", transform.position, transform.rotation);

					}

					Destroy(gameObject);
					break;
			}


			if (CurrentCharacter.AttackState == AttackState.PRE_ATTACK) {
				//TODO: show pre-attack animation
			}


			if (CurrentCharacter.AttackState == AttackState.ATTACK) {
				//TODO: show post-attack animation
			}
		
		}

			
	}
}
