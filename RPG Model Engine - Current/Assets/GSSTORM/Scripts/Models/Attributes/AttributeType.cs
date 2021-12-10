using System;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    [Serializable]
    public enum AttributeType
    {
        UKNOWN = 0,

        // HP related.
        HP_CURRENT = 1,

        HP_LIMIT_BASE = 2,
        HP_LIMIT_PERCENT = 3,
        HP_LIMIT_C = 4,

        HP_REGENERATION_BASE = 5,
        HP_REGENERATION_PERCENT = 6,
        HP_REGENERATION_C = 7,

        HP_ON_HIT = 8,  // gain hp on hit

        // MP related.
        MP_CURRENT = 9,

        MP_LIMIT_BASE = 10,
        MP_LIMIT_PERCENT = 11,
        MP_LIMIT_C = 12,

        MP_REGENERATION_BASE = 13,
        MP_REGENERATION_PERCENT = 14,
        MP_REGENERATION_C = 15,

        MP_ON_HIT = 16, // gain mana on hit

        MP_COST = 17, // mana cost

        // Attack related.
        PHYSICAL_ATTACK_BASE = 18,
        PHYSICAL_ATTACK_PERCENT = 19,
        PHYSICAL_ATTACK_C = 20,

		FIRE_ATTACK_BASE = 21,
	    FIRE_ATTACK_PERCENT = 22,
		FIRE_ATTACK_C = 23,

		COLD_ATTACK_BASE = 24,
		COLD_ATTACK_PERCENT = 25,
		COLD_ATTACK_C = 26,

		LIGHTNING_ATTACK_BASE = 27,
		LIGHTNING_ATTACK_PERCENT = 28,
		LIGHTNING_ATTACK_C = 29,

		DARK_ATTACK_BASE = 30,
		DARK_ATTACK_PERCENT = 31,
		DARK_ATTACK_C = 32,

		HOLY_ATTACK_BASE = 33,
		HOLY_ATTACK_PERCENT = 34,
		HOLY_ATTACK_C = 35,

        // Attack speed related.
        ATTACK_SPEED_BASE = 36,
        ATTACK_SPEED_PERCENT = 37,
        ATTACK_SPEED_C = 38,

        // Move speed related.
        MOVE_SPEED_BASE = 39,
        MOVE_SPEED_PERCENT = 40,
        MOVE_SPEED_C = 41,

        // Defence related.
        ARMOUR_BASE = 42,
        ARMOUR_PERCENT = 43,
        ARMOUR_C = 44,

		RESIS_FIRE = 45, 
		RESIS_COLD = 46,
		RESIS_LIGHTNING = 47,
		RESIS_HOLY = 48,
		RESIS_DARK = 49,
		RESIS_PHYSICAL = 50,

		EVASION_BASE = 51,
        EVASION_PERCENT = 52,
        EVASION_C = 53,

		//Skill related
        PROJECTILE_FLYING_TIME = 55,

		PROJECTILE_SPEED_BASE = 56,
        PROJECTILE_SPEED_PERCENT = 57,
        PROJECTILE_SPEED_C = 58,

        // AI
        ALERT_RANGE_BASE = 59,
        ALERT_RANGE_PERCENT = 60,
        ALERT_RANGE_C = 61,

        ATTACK_RANGE_BASE = 62,
        ATTACK_RANGE_PERCENT = 63,
        ATTACK_RANGE_C = 64,

        PRE_ATTACK_TIME_BASE = 65,
        PRE_ATTACK_TIME_PERCENT = 66,
        PRE_ATTACK_TIME_C = 67,

        POST_ATTACK_TIME_BASE = 68,
        POST_ATTACK_TIME_PERCENT = 69,
        POST_ATTACK_TIME_C = 70,
	}

	/// <summary>
	/// Compiled attribute type.
	/// 
	/// System will automatically comiple
	/// the values for the following types
	/// never assign them directly, call compile
	/// instead.
	/// </summary>
    public static class CompiledAttributeType {
        private static AttributeType[] _typeArray = {
            AttributeType.HP_LIMIT_C,
            AttributeType.HP_REGENERATION_C,

            AttributeType.MP_LIMIT_C,
            AttributeType.MP_REGENERATION_C,

            AttributeType.PHYSICAL_ATTACK_C,
            AttributeType.FIRE_ATTACK_C,
            AttributeType.COLD_ATTACK_C,
            AttributeType.LIGHTNING_ATTACK_C,
            AttributeType.DARK_ATTACK_C,
            AttributeType.HOLY_ATTACK_C,

            AttributeType.ATTACK_SPEED_C,
            AttributeType.MOVE_SPEED_C,

            AttributeType.ARMOUR_C,
            AttributeType.EVASION_C,

            AttributeType.PROJECTILE_SPEED_C,
			
            AttributeType.ALERT_RANGE_C,
            AttributeType.ATTACK_RANGE_C,

            AttributeType.PRE_ATTACK_TIME_C,
            AttributeType.POST_ATTACK_TIME_C, 
        };

        public static HashSet<AttributeType> Types = new HashSet<AttributeType>(_typeArray);

        public static bool IsCompiledType(AttributeType type)
        {
            return Types.Contains(type);
        }

        public static void Compile(AttributeSet set)
        {
            foreach(AttributeType type in Types)
            {
                Compile(set, type);
            }
        }

        public static void Compile(AttributeSet set, AttributeType type)
        {
            Attribute attr = null;
            switch (type)
            {
                case AttributeType.HP_LIMIT_C :
                    attr = CompileType(set, AttributeType.HP_LIMIT_C, AttributeType.HP_LIMIT_BASE, AttributeType.HP_LIMIT_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.HP_REGENERATION_C :
                    attr = CompileType(set, AttributeType.HP_REGENERATION_C, AttributeType.HP_REGENERATION_BASE, AttributeType.HP_REGENERATION_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.MP_LIMIT_C:
                    attr = CompileType(set, AttributeType.MP_LIMIT_C, AttributeType.MP_LIMIT_BASE, AttributeType.MP_LIMIT_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.MP_REGENERATION_C:
                    attr = CompileType(set, AttributeType.MP_REGENERATION_C, AttributeType.MP_REGENERATION_BASE, AttributeType.MP_REGENERATION_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.PHYSICAL_ATTACK_C:
                    attr = CompileType(set, AttributeType.PHYSICAL_ATTACK_C, AttributeType.PHYSICAL_ATTACK_BASE, AttributeType.PHYSICAL_ATTACK_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.FIRE_ATTACK_C:
                    attr = CompileType(set, AttributeType.FIRE_ATTACK_C, AttributeType.FIRE_ATTACK_BASE, AttributeType.FIRE_ATTACK_PERCENT);
					if (attr != null) set[type] = attr;
					break;
                case AttributeType.COLD_ATTACK_C:
                    attr = CompileType(set, AttributeType.COLD_ATTACK_C, AttributeType.COLD_ATTACK_BASE, AttributeType.COLD_ATTACK_PERCENT);
					if (attr != null) set[type] = attr;
					break;
                case AttributeType.LIGHTNING_ATTACK_C:
                    attr = CompileType(set, AttributeType.LIGHTNING_ATTACK_C, AttributeType.LIGHTNING_ATTACK_BASE, AttributeType.LIGHTNING_ATTACK_PERCENT);
					if (attr != null) set[type] = attr;
					break;
                case AttributeType.DARK_ATTACK_C:
                    attr = CompileType(set, AttributeType.DARK_ATTACK_C, AttributeType.DARK_ATTACK_BASE, AttributeType.DARK_ATTACK_PERCENT);
					if (attr != null) set[type] = attr;
					break;
                case AttributeType.HOLY_ATTACK_C:
                    attr = CompileType(set, AttributeType.HOLY_ATTACK_C, AttributeType.HOLY_ATTACK_BASE, AttributeType.HOLY_ATTACK_PERCENT);
					if (attr != null) set[type] = attr;
					break;
                case AttributeType.ATTACK_SPEED_C:
                    attr = CompileType(set, AttributeType.ATTACK_SPEED_C, AttributeType.ATTACK_SPEED_BASE, AttributeType.ATTACK_SPEED_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.MOVE_SPEED_C:
                    attr = CompileType(set, AttributeType.MOVE_SPEED_C, AttributeType.MOVE_SPEED_BASE, AttributeType.MOVE_SPEED_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.ARMOUR_C:
                    attr = CompileType(set, AttributeType.ARMOUR_C, AttributeType.ARMOUR_BASE, AttributeType.ARMOUR_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.EVASION_C:
                    attr = CompileType (set, AttributeType.EVASION_C, AttributeType.EVASION_BASE, AttributeType.EVASION_PERCENT);
					if (attr != null) set [type] = attr;
					break;
                case AttributeType.PROJECTILE_SPEED_C:
                    attr = CompileType(set, AttributeType.PROJECTILE_SPEED_C, AttributeType.PROJECTILE_SPEED_BASE, AttributeType.PROJECTILE_SPEED_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;

				case AttributeType.ALERT_RANGE_C:
					attr = CompileType (set, AttributeType.ALERT_RANGE_C, AttributeType.ALERT_RANGE_BASE, AttributeType.ALERT_RANGE_PERCENT);
					if (attr != null) set [type] = attr;
					break;
                case AttributeType.ATTACK_RANGE_C:
                    attr = CompileType(set, AttributeType.ATTACK_RANGE_C, AttributeType.ATTACK_RANGE_BASE, AttributeType.ATTACK_RANGE_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.PRE_ATTACK_TIME_C:
                    attr = CompileType(set, AttributeType.PRE_ATTACK_TIME_C, AttributeType.PRE_ATTACK_TIME_BASE, AttributeType.PRE_ATTACK_TIME_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;
                case AttributeType.POST_ATTACK_TIME_C:
                    attr = CompileType(set, AttributeType.POST_ATTACK_TIME_C, AttributeType.POST_ATTACK_TIME_BASE, AttributeType.POST_ATTACK_TIME_PERCENT);
                    if (attr != null) set[type] = attr;
                    break;

                default:
                    throw new ArgumentException(type + " is not a compiled attribute type.");
            }
        }

        private static Attribute CompileType(AttributeSet set, AttributeType compiledType, AttributeType baseType, AttributeType percentType)
        {
            if (set.ContainsType(baseType)) {
                if (set.ContainsType(percentType)) {
                    return new Attribute(compiledType, (set.GetAttribute(percentType, true).Value + 1f) * set.GetAttribute(baseType, true).Value);
                } else
                {
                    return new Attribute(compiledType, set.GetAttribute(baseType, true).Value);
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Raw attribute type.
    /// 
    /// Raw attribute type needs to be compiled for functional use.
    /// </summary>
    public static class RawAttributeType
    {
        private static AttributeType[] _typeArray = {
            AttributeType.HP_LIMIT_BASE,
            AttributeType.HP_LIMIT_PERCENT,
            AttributeType.HP_REGENERATION_BASE,
            AttributeType.HP_REGENERATION_PERCENT,

            AttributeType.MP_LIMIT_BASE,
            AttributeType.MP_LIMIT_PERCENT,
            AttributeType.MP_REGENERATION_BASE,
            AttributeType.MP_REGENERATION_PERCENT,

            AttributeType.PHYSICAL_ATTACK_BASE,
            AttributeType.PHYSICAL_ATTACK_PERCENT,
            AttributeType.FIRE_ATTACK_BASE,
            AttributeType.FIRE_ATTACK_PERCENT,
            AttributeType.COLD_ATTACK_BASE,
            AttributeType.COLD_ATTACK_PERCENT,
            AttributeType.LIGHTNING_ATTACK_BASE,
            AttributeType.LIGHTNING_ATTACK_PERCENT,
            AttributeType.DARK_ATTACK_BASE,
            AttributeType.DARK_ATTACK_PERCENT,
            AttributeType.HOLY_ATTACK_BASE,
            AttributeType.HOLY_ATTACK_PERCENT,

            AttributeType.ATTACK_SPEED_BASE,
            AttributeType.ATTACK_SPEED_PERCENT,
            AttributeType.MOVE_SPEED_BASE,
            AttributeType.MOVE_SPEED_PERCENT,

            AttributeType.ARMOUR_BASE,
            AttributeType.ARMOUR_PERCENT,
            AttributeType.EVASION_BASE,
            AttributeType.EVASION_PERCENT,

            AttributeType.PROJECTILE_SPEED_BASE,
            AttributeType.PROJECTILE_SPEED_PERCENT,

            AttributeType.ALERT_RANGE_BASE,
            AttributeType.ALERT_RANGE_PERCENT,
            AttributeType.ATTACK_RANGE_BASE,
            AttributeType.ATTACK_RANGE_PERCENT,

            AttributeType.PRE_ATTACK_TIME_BASE,
            AttributeType.PRE_ATTACK_TIME_PERCENT,
            AttributeType.POST_ATTACK_TIME_BASE,
            AttributeType.POST_ATTACK_TIME_PERCENT,
        };

        public static HashSet<AttributeType> Types = new HashSet<AttributeType>(_typeArray);

        public static bool IsRawType(AttributeType type)
        {
            return Types.Contains(type);
        }

        public static AttributeType GetCompiledType(AttributeType type){
            switch(type){
                case AttributeType.HP_LIMIT_BASE:
                case AttributeType.HP_LIMIT_PERCENT:
                    return AttributeType.HP_LIMIT_C;
                case AttributeType.HP_REGENERATION_BASE:
                case AttributeType.HP_REGENERATION_PERCENT:
                    return AttributeType.HP_REGENERATION_C;
                case AttributeType.MP_LIMIT_BASE:
                case AttributeType.MP_LIMIT_PERCENT:
                    return AttributeType.MP_LIMIT_C;
                case AttributeType.MP_REGENERATION_BASE:
                case AttributeType.MP_REGENERATION_PERCENT:
                    return AttributeType.MP_REGENERATION_C;
                case AttributeType.PHYSICAL_ATTACK_BASE:
                case AttributeType.PHYSICAL_ATTACK_PERCENT:
                    return AttributeType.PHYSICAL_ATTACK_C;
                case AttributeType.FIRE_ATTACK_BASE:
                case AttributeType.FIRE_ATTACK_PERCENT:
                    return AttributeType.FIRE_ATTACK_C;
                case AttributeType.COLD_ATTACK_BASE:
                case AttributeType.COLD_ATTACK_PERCENT:
                    return AttributeType.COLD_ATTACK_C;
                case AttributeType.LIGHTNING_ATTACK_BASE:
                case AttributeType.LIGHTNING_ATTACK_PERCENT:
                    return AttributeType.LIGHTNING_ATTACK_C;
                case AttributeType.DARK_ATTACK_BASE:
                case AttributeType.DARK_ATTACK_PERCENT:
                    return AttributeType.DARK_ATTACK_C;
                case AttributeType.HOLY_ATTACK_BASE:
                case AttributeType.HOLY_ATTACK_PERCENT:
                    return AttributeType.HOLY_ATTACK_C;
                case AttributeType.ATTACK_SPEED_BASE:
                case AttributeType.ATTACK_SPEED_PERCENT:
                    return AttributeType.ATTACK_SPEED_C;
                case AttributeType.MOVE_SPEED_BASE:
                case AttributeType.MOVE_SPEED_PERCENT:
                    return AttributeType.MOVE_SPEED_C;
                case AttributeType.ARMOUR_BASE:
                case AttributeType.ARMOUR_PERCENT:
                    return AttributeType.ARMOUR_C;
                case AttributeType.EVASION_BASE:
                case AttributeType.EVASION_PERCENT:
                    return AttributeType.EVASION_C;
                case AttributeType.PROJECTILE_SPEED_BASE:
                case AttributeType.PROJECTILE_SPEED_PERCENT:
                    return AttributeType.PROJECTILE_SPEED_C;
                case AttributeType.ALERT_RANGE_BASE:
                case AttributeType.ALERT_RANGE_PERCENT:
                    return AttributeType.ALERT_RANGE_C;
                case AttributeType.ATTACK_RANGE_BASE:
                case AttributeType.ATTACK_RANGE_PERCENT:
                    return AttributeType.ATTACK_RANGE_C;
                case AttributeType.PRE_ATTACK_TIME_BASE:
                case AttributeType.PRE_ATTACK_TIME_PERCENT:
                    return AttributeType.PRE_ATTACK_TIME_C;
                case AttributeType.POST_ATTACK_TIME_BASE:
                case AttributeType.POST_ATTACK_TIME_PERCENT:
                    return AttributeType.POST_ATTACK_TIME_C;
            }
            return AttributeType.UKNOWN;
        }
    }
}