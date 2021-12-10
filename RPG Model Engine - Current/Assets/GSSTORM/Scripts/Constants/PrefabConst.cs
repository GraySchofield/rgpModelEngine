using System;

namespace GSStorm.RPG.Engine
{
	public static class PrefabConst
	{
		#region Directory
		public const string SKILL_PATH = "Prefabs/Skills/";

		public const string MAP_ITEM_PATH = "Prefabs/UI/Item/MapItem/";

		public const string UI_ITEM_PATH = "Prefabs/UI/Item/UIItem/";

		public const string CHARACTER_PATH = "Prefabs/Characters/";

		public const string ICON_TEXTURE_PATH = "Textures/ItemIcons/";

        public const string BUFF_ICON_PATH = "Textures/BuffIcons/";
		#endregion

		#region Skill
		public const string SKILL_CRUSH_LAND = "SkillCrushLand";
		public const string SKILL_PROJECTILE = "SkillProjectile";
        public const string SKILL_BLADE_FURY = "SkillBladeFury";
		#endregion

		#region UI
		public const string BAG_BUTTON_ITEM = "BtnItem";
        public const string BUFF_STATE_ICON = "BuffStateIcon";
        #endregion

        #region Map
        public const string COMMON_MAP_ITEM = "item";
		#endregion
	
		#region Character
		public const string PLAYER = "Player";
		public const string MONSTER_DRAGON = "Monster";
		#endregion
	
	}
}

