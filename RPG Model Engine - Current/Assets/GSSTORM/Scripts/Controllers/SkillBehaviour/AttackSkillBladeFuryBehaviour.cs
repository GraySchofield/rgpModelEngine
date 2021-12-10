using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
    public class AttackSkillBladeFuryBehaviour : AttackSkillBaseBehaviour<AttackSkillBladeFury>
    {
        #region Public Variables
        public float DamageInterval
        {
            get;
            set;
        }
        #endregion

        #region Private Variables

        List<CombatUnit> _hitTargets;

        float _timeFromLastManaConsume;

        #endregion

        void Awake()
        {
            _hitTargets = new List<CombatUnit>();

            DamageInterval = 1f;
        }

        void OnEnable()
        {
            if (Skill != null)
            {
                _timeFromLastManaConsume = 0f;
                _hitTargets.Clear();
                InvokeRepeating("DamageLoop", 0, DamageInterval);
            }
        }


        void OnDisable()
        {
            CancelInvoke("DamageLoop");    
        }


        void DamageLoop()
        {
            foreach (Character target in _hitTargets)
            {
                if (Skill != null)
                {
                    Skill.BeforeHit(Caster, target);
                    Skill.OnHit(Caster, target);
                }
            }
        }


        void Update()
        {
            if(Skill != null && Caster != null)
            {
                transform.position = Caster.Position; 
                transform.rotation = Quaternion.identity;

                _timeFromLastManaConsume += Time.deltaTime;

                if(_timeFromLastManaConsume >= 1f)
                {
                    float manaCost = Skill.Attributes.GetAttribute(AttributeType.MP_COST).Value;
                    if (Caster.Mana >= manaCost)
                    {
                        Caster.ConsumeMana(manaCost);
                    }
                    else
                    {
                        ObjectPoolManager.Current.ReleaseToPool(PrefabConst.SKILL_BLADE_FURY, gameObject);
                    }

                    _timeFromLastManaConsume = 0f;
                }

            } 
        
        }


        protected override void OnTriggerEnter2D(Collider2D other)
        {
            CombatUnit hitTarget = GetValidHitTarget(other);

            if (hitTarget == null)
                return;

            //hit some thing
            if (Skill == null)
            {
                Debug.LogError("Lost reference to the main attack skill !");
                return;
            }

            if (Caster == null)
            {
                Debug.LogError("Lost refernce to the caster !");
                return;
            }

            _hitTargets.Add(hitTarget);

        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            CombatUnit hitTarget = GetValidHitTarget(other);

            if (hitTarget == null)
                return;

            _hitTargets.Remove(hitTarget);
        }

    }
}