using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEngine.UI;

namespace GSStorm.RPG.Game{

	/// <summary>
	/// Base character controller.
	/// 
	/// Stores shared functionalities betweeen all character controllers
	/// 
	/// Subclass to use it for correct case, like playerController, monsterController
	/// </summary>
	public class BaseCharacterController : MonoBehaviour {
		
		[HideInInspector]
		public Character CurrentCharacter;

		#region Protected Variables
		protected Text _lifeText;
		protected Text _manaText;
		protected Image _lifeBar;
		protected Image _manaBar;

		protected Animator _animator;
		protected Rigidbody2D _rigidBody;

		protected GameObject _characterRotation;
		#endregion

		// Use this for initialization
		protected virtual void Start () {
			_animator = GetComponent<Animator>();
			_rigidBody = GetComponent<Rigidbody2D>();

			_lifeText = transform.Find("UI/HealthBar/Text").GetComponent<Text>();
			_lifeBar = transform.Find("UI/HealthBar").GetComponent<Image>();

			_manaText = transform.Find("UI/ManaBar/Text").GetComponent<Text>();
			_manaBar = transform.Find("UI/ManaBar").GetComponent<Image>();

			_characterRotation = transform.Find ("CharacterDirection").gameObject;
		}
		
		// Update is called once per frame
		protected virtual void Update () {
			//Update Character UI
			float hpCurrent = CurrentCharacter.Attributes[AttributeType.HP_CURRENT].Value;
			float hpLimit = CurrentCharacter.Attributes[AttributeType.HP_LIMIT_C].Value;
			float mpCurrent = CurrentCharacter.Attributes[AttributeType.MP_CURRENT].Value;
			float mpLimit = CurrentCharacter.Attributes[AttributeType.MP_LIMIT_C].Value;

			_lifeText.text = hpCurrent + " / " + hpLimit;
			_lifeBar.fillAmount = hpCurrent / hpLimit;

			_manaText.text = mpCurrent + " / " + mpLimit;
			_manaBar.fillAmount = mpCurrent / mpLimit;

			//Character Model Update
			CurrentCharacter.UpdateLoop(Time.deltaTime, transform, _characterRotation.transform.rotation);
		}


		/// <summary>
		/// Rotate and set the animation towards the target
		/// </summary>
		/// <param name="position">Position.</param>
		public virtual Vector3 RotateTowards(Vector3 position){
			Vector3 direction =  (position - transform.position).normalized ;
			var angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			_characterRotation.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			//Animate monster direction
			_animator.SetFloat ("InputX", direction.x);
			_animator.SetFloat ("InputY", direction.y);

			return direction;  //the normalized direction that the character is facing
		}
	}
}
