using UnityEngine;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Attack skill blade fury.
    /// </summary>
    public class AttackSkillBladeFury : AttackSkill
    {
        #region Skill Process callbacks

        public override bool Prepare(CombatUnit caster)
        {
            return base.Prepare(caster);
        }


        public override bool OnCast(CombatUnit caster)
        {
            if (!base.OnCast(caster)) { return false; }

            CreateGameObject(caster);

            return true;
        }

        #endregion

        private void CreateGameObject(CombatUnit caster)
        {
            GameObject skillPrefab = ObjectPoolManager.Current.GetObject(PrefabConst.SKILL_BLADE_FURY);

            skillPrefab.transform.position = caster.Position;
            skillPrefab.transform.rotation = caster.Rotation;

            AttackSkillBladeFuryBehaviour bevaviour = skillPrefab.GetComponent<AttackSkillBladeFuryBehaviour>();

            bevaviour.Caster = caster;
            bevaviour.Skill = this;
            bevaviour.gameObject.SetActive(true);
        }
    }

}