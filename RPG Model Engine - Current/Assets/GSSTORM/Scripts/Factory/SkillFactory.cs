using System;

namespace GSStorm.RPG.Engine
{

    /// <summary>
    /// Factory used to produce kills 
    /// 
    /// We may need to use template to build skills
    /// later, for now we just hard code the designed 
    /// 9 skills first.
    /// </summary>
    public class SkillFactory
    {
        public AttackSkill ProduceAttackSkill(string typeId)
        {
            switch (typeId)
            {
                case "skill_crush_land":
                    AttackSkillCrushLand skillCrushLannd = new AttackSkillCrushLand();
                    skillCrushLannd.TypeId = typeId;                  
                    skillCrushLannd.CoolDown.TimeLimit = 1f;
                    skillCrushLannd.Name = "Crush Land";
                    return skillCrushLannd;

			    case "skill_projectile":
				    AttackSkillProjectile skillProjectile = new AttackSkillProjectile ();
				    skillProjectile.TypeId = typeId;
                    skillProjectile.Name = "Fire Ball";
                    skillProjectile.Attributes.SetAttribute (new Attribute (AttributeType.PROJECTILE_FLYING_TIME, 1f));
				    skillProjectile.Attributes.SetAttribute (new Attribute (AttributeType.PROJECTILE_SPEED_C, 8f));
				    skillProjectile.CoolDown.TimeLimit = 1f;
				    return skillProjectile;

                case "skill_blade_fury":
                    AttackSkillBladeFury skillBladeFury = new AttackSkillBladeFury();
                    skillBladeFury.TypeId = typeId;
                    skillBladeFury.Name = "Blade Fury";
                    skillBladeFury.CoolDown.TimeLimit = 0.1f;
                    skillBladeFury.Attributes.SetAttribute(new Attribute(AttributeType.MP_COST, 5f));
                                 
                    return skillBladeFury;

                default:
                    throw new SkillNotFoundException("Skill " + typeId + " is not found.");
            }

            return null;
        }

        public class SkillNotFoundException : Exception
        {
            public SkillNotFoundException() { }

            public SkillNotFoundException(string message)
                : base(message) { }

            public SkillNotFoundException(string message, Exception inner)
                : base(message, inner) { }
        }
    }

}