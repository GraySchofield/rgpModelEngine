using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEngine.UI;

namespace GSStorm.RPG.Game
{
	public class PlayerController : BaseCharacterController
    {
        #region Public Properties
        #endregion


        #region Private Properties
        float _lastInputX = 0;
        float _lastInputY = 0;

        //Canvas UI control buttons
        Button _buttonAttack;  //TODO: make a button that can count Cd
       	#endregion


        // Use this for initialization
        protected override void Start()
        {
			base.Start ();

            _buttonAttack = GameObject.Find("UICanvas/ButtonAttackSkill").GetComponent<Button>();
            _buttonAttack.onClick.AddListener(this.CastMainAttackSkill);
        
		}

        // Update is called once per frame
        protected override void Update()
        {
			base.Update ();

			float speed = CurrentCharacter.Attributes.GetAttribute (AttributeType.MOVE_SPEED_C).Value;
				
            //Configure Animations, movement
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            if( Mathf.Abs(inputX) > 0.001 || Mathf.Abs(inputY) > 0.001)
            {
                _lastInputX = inputX;
                _lastInputY = inputY;
            }
           	
			_rigidBody.velocity = new Vector2 (inputX, inputY) * speed;

			var angle = Mathf.Atan2 (_lastInputY, _lastInputX) * Mathf.Rad2Deg;
			_characterRotation.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

            _animator.SetFloat("Speed", _rigidBody.velocity.magnitude);
            _animator.SetFloat("InputX", inputX);
            _animator.SetFloat("InputY", inputY);
            _animator.SetFloat("LastInputX", _lastInputX);
            _animator.SetFloat("LastInputY", _lastInputY);
		
			//TODO: pre-attack - post-attack , 硬直

			if (CurrentCharacter.AttackState == AttackState.PRE_ATTACK) {  // we will only enter pre_attack state when skill preparation is successful
				_rigidBody.velocity = Vector2.zero;

				//TODO: show pre-attack animation
			}

			if (CurrentCharacter.AttackState == AttackState.ATTACK) {
				_rigidBody.velocity = Vector2.zero;

				//TODO: show post-attack animation

				//TODO: detect input and cancel post-attack
				if (inputX > 0 || inputY > 0) {
					_rigidBody.velocity = new Vector2 (inputX, inputY) * speed;
					CurrentCharacter.CancelPostAttack ();
				}
			}
        }
			

        public void CastMainAttackSkill()
        {
            if(CurrentCharacter != null)
            {
                CurrentCharacter.PreAttack();
            }
        }


        /// <summary>
        /// Detect if there is any item near me
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("ITEM"))
            {
                //there is item nearby, pick it up
				collision.GetComponent<ItemController>().PickByPlayer((Player)CurrentCharacter);
            }
        }

    }

}