using UnityEngine;

namespace GSStorm.RPG.Engine
{

	/// <summary>
	/// Attack skill crush land.
	/// 
	/// this is an impelemented skill
	/// use it to test the skill system
	///
	/// </summary>
	public class AttackSkillCrushLand : AttackSkill
	{
        #region Skill Process callbacks

        public override bool OnCast(CombatUnit caster)
        {
            if (!base.OnCast(caster)) return false;

            CreateGameObject(caster);

			Debug.Log ("Successfully created skill prefab for crush land !");

            return true;
		}

        #endregion

        private void CreateGameObject(CombatUnit caster)
        {
            GameObject skillPrefab = ObjectPoolManager.Current.GetObject(PrefabConst.SKILL_CRUSH_LAND);

            skillPrefab.transform.position = caster.Position;
            skillPrefab.transform.rotation = caster.Rotation;

            AttackSkillCrushLandBehaviour bevaviour = skillPrefab.GetComponent<AttackSkillCrushLandBehaviour>();

            bevaviour.Caster = caster;
            bevaviour.Skill = this;
            bevaviour.gameObject.SetActive(true);
        }
	}
}

