using UnityEngine;

namespace GSStorm.RPG.Engine
{
	/// <summary>
	/// Attack skill projectile.
	/// </summary>
	public class AttackSkillProjectile : AttackSkill
	{
        #region Skill Process callbacksS

        public override bool OnCast(CombatUnit caster){
            if (!base.OnCast(caster)) { return false; }

            CreateGameObject(caster);

            return true;
        }

		#endregion

        public void CreateGameObject(CombatUnit caster)
        {
            GameObject skillPrefab = ObjectPoolManager.Current.GetObject(PrefabConst.SKILL_PROJECTILE);

            skillPrefab.transform.position = caster.Position;
            skillPrefab.transform.rotation = caster.Rotation;

            AttackSkillProjectileBehaviour bevaviour = skillPrefab.GetComponent<AttackSkillProjectileBehaviour>();

            bevaviour.Caster = caster;
            bevaviour.Skill = this;
            bevaviour.gameObject.SetActive(true);
        }

	}
}

