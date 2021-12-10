using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base AI class for modeling  character AI behaviour and 
/// state transaction
/// 
/// 
/// This AI is mono, and need to be attached to work
/// </summary>
/// 

namespace GSStorm.RPG.Engine{

	public enum AIState{
		IDLE,
		CHASING,
		ATTACK,  //includes pre-attack, attacking and post attack
		DEAD,
		FOLLOWING,  //Pet ?
		FLEE //Run away
	}

	public delegate bool TransactionCondition ();

	public class BaseAI : MonoBehaviour {

		#region Private Variables;
		Dictionary<Tuple<AIState, AIState>, TransactionCondition> _normalTransactions; //dictionary to store all transactions in this AI

		Dictionary<AIState, TransactionCondition> _absoluteTransactions; //transactions regardless of initial state
		#endregion


		#region Public Variables
		public AIState CurrentState {
			get;
			set;
		}
	
		/// <summary>
		/// The character that this AI controls
		/// </summary>
		/// <value>The current character.</value>
		public Character CurrentCharacter {
			get;
			set;
		}

		/// <summary>
		/// The character that will affect the current character's behaviour
		/// </summary>
		/// <value>The target character.</value>
		public Character TargetCharacter{
			get;
			set;
		}

		#endregion

		//Awake is called right after instantiate 
		void Awake(){
			CurrentState = AIState.IDLE;  //base AI init at IDLE state
			_normalTransactions = new Dictionary<Tuple<AIState, AIState>, TransactionCondition>();
			_absoluteTransactions = new Dictionary<AIState, TransactionCondition> ();
		}
		
		// Update is called once per frame
		public virtual void Update () {
			
			foreach (var trans in _normalTransactions) {
				if (CurrentState == trans.Key.Item1) {
					if (trans.Value ()) {
						//transaction happened
						CurrentState = trans.Key.Item2;  // Note, state check is not perfromed here, need to make sure one state uniquely transform to another
					}
				}
			}


			foreach (var trans in _absoluteTransactions) {
				if (trans.Value ()) {
					CurrentState = trans.Key;
				}
			}
		
		}

	
		/// <summary>
		/// Registers the transaction.
		/// </summary>
		/// <param name="tuple">Tuple.</param>
		/// <param name="condition">Condition.</param>
		public void RegisterTransaction(Tuple<AIState, AIState> tuple, TransactionCondition condition){
			if (_normalTransactions.ContainsKey (tuple)) {
				_normalTransactions [tuple] = condition;
			} else {
				_normalTransactions.Add (tuple, condition);
			}
		}


		/// <summary>
		/// Registers the default monster bebaviour.
		/// 
		/// This is useful for an average monster
		/// </summary>
		public void RegisterDefaultMonsterBebaviour(){
			_normalTransactions.Clear ();
			_absoluteTransactions.Clear ();

			//From idle to chasing
			Tuple<AIState, AIState> temp = new Tuple<AIState, AIState> (AIState.IDLE, AIState.CHASING);

			_normalTransactions.Add (temp, () => {
				float alertRange = CurrentCharacter.Attributes[AttributeType.ALERT_RANGE_C].Value;

				if(Vector3.Distance(CurrentCharacter.Position, TargetCharacter.Position) <= alertRange){
					return true;
				}

				return false;
			});

			//From chasing to attack, within  attack range and cool down is ready
			temp = new Tuple<AIState, AIState> (AIState.CHASING, AIState.ATTACK);

			_normalTransactions.Add (temp, () => {
				float attackRange = CurrentCharacter.Attributes[AttributeType.ATTACK_RANGE_C].Value;

				if(Vector3.Distance(CurrentCharacter.Position, TargetCharacter.Position) <= attackRange){
					return true;
				}

				return false;
			});
				
			//From attack to post_attack, this transaction should just be automatic
			temp = new Tuple<AIState, AIState> (AIState.ATTACK, AIState.CHASING);

			_normalTransactions.Add (temp, () => {
				float attackRange = CurrentCharacter.Attributes.GetAttribute(AttributeType.ATTACK_RANGE_C).Value;

				if(Vector3.Distance(CurrentCharacter.Position, TargetCharacter.Position) > attackRange){
					if(CurrentCharacter.AttackState == AttackState.IDLE){
						return true;
						//只有在完全完成攻击之后才会有机会切换到追逐状态
					}
				}

				return false;
			});


			temp = new Tuple<AIState, AIState> (AIState.CHASING, AIState.IDLE);

			_normalTransactions.Add (temp, () => {
				float alertRange = CurrentCharacter.Attributes[AttributeType.ALERT_RANGE_C].Value;

				if(Vector3.Distance(CurrentCharacter.Position, TargetCharacter.Position) > alertRange){
					return true;
				}

				return false;
			});
	
			_absoluteTransactions.Add (AIState.DEAD, () => {
				if(CurrentCharacter.Attributes[AttributeType.HP_CURRENT].Value <= 0){
					return true;
				}	

				return false;
			});
		}


	}

}