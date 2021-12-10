using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;


namespace GSStorm.RPG.Engine
{
	public class GSStormRPGContentEditor : EditorWindow
	{
		public readonly string[] ModuleNames = {
			"Item",
			"Gear",
			"Potion" ,
			"Receipe",
			"WorkStation",
			"AttributeSet",
			"Map",
			"Rune",
			"Buff",
			"Character"
		};

		#region Item variables

		public List<ItemTemplate> Items;
		//public ItemTemplate CurrentItem;

		#endregion

		#region AttributeSet variables

		public List<AttributeSetTemplate> AttributeSets;
		//public AttributeSetTemplate CurrentAttributeSet;

		#endregion

		#region Gear variables

		public List<GearTemplate> Gears;
		//public GearTemplate CurrentGear;

		#endregion

		#region Potion variables

		public List<PotionTemplate> Potions;

		#endregion

		#region WorkStation & Receip variables

		public List<ReceipeTemplate> Receipies;

		public List<WorkStationTemplate> WorkStations;
		//public WorkStationTemplate CurrentWorkStation;

		#endregion

		#region Map

		public List<MapTemplate> Maps;

		#endregion

		#region Rune

		public List<RuneTemplate> Runes;

		private EffectType _selectedType = EffectType.NONE;

		#endregion

		#region Buff

		public List<BuffTemplate> Buffs;

		#endregion

		#region Character

		public List<CharacterTemplate> Characters;

		#endregion

		public ScriptableObject CurrentSelection;

		private string _currentType;
		private const string _topDirectoryPath = "Assets/GSSTORM/Resources/GSStormGameContent/";
		private string _newFileName = "";

		Vector2 scrollpos;

		[MenuItem ("GSStorm/GSStorm RPG Content Editor %#g")]
		static void Init ()
		{
			EditorWindow.GetWindow (typeof(GSStormRPGContentEditor));
		}

		void OnEnable ()
		{
			LoadContent ();

			//Get all the assests under the relative directories
			_currentType = "Item";
		}

		void OnGUI ()
		{
			GUILayout.BeginHorizontal ("OuterGroup", GUILayout.Width (400));


			GUILayout.BeginVertical ("LeftBar", GUILayout.Width (100));

			GUI.color = Color.blue;
			EditorGUILayout.LabelField ("Chooose Module", EditorStyles.whiteBoldLabel);
			EditorGUILayout.LabelField ("-----------------------", EditorStyles.whiteBoldLabel, GUILayout.Width (100));
			GUI.color = Color.white;


			foreach (string module in ModuleNames) {
				if (_currentType.Equals (module))
					GUI.color = Color.green;
				if (GUILayout.Button (module)) {
					_currentType = module;
				}
				GUI.color = Color.white;
			}

			GUILayout.EndVertical ();

			GUILayout.Space (30);

			EditorGUILayout.BeginVertical ("Middle List", GUILayout.Width (100));


			GUI.color = Color.blue;
			EditorGUILayout.LabelField ("Select Object(" + _currentType + ")", EditorStyles.whiteBoldLabel);
			EditorGUILayout.LabelField ("-----------------------", EditorStyles.whiteBoldLabel, GUILayout.Width (100));
			GUI.color = Color.white;


			_newFileName = EditorGUILayout.TextField ("FileName", _newFileName, GUILayout.Width (300));


			if (GUILayout.Button ("CREATE NEW")) {
				CreateObject (_newFileName);
			}

			GUILayout.Space (10);

			EditorGUILayout.LabelField ("=========================");

			EditorGUILayout.BeginScrollView (scrollpos, false, false);

			switch (_currentType) {
			case "Item":
				DrawSelectionButton<ItemTemplate> (Items, () => {
				});
				break;
              
			case "Gear":
				DrawSelectionButton<GearTemplate> (Gears, () => {
				});
				break;

			case "Potion":
				DrawSelectionButton<PotionTemplate> (Potions, () => {
				});
				break;
	          	
			case "Receipe":
				DrawSelectionButton<ReceipeTemplate> (Receipies, () => {
				});
				break;

			case "Rune":
				DrawSelectionButton<RuneTemplate> (Runes, () => {
				});
				break;

			case "Buff":
				DrawSelectionButton<BuffTemplate> (Buffs, () => {
				});
				break;

			case "Character":
				DrawSelectionButton<CharacterTemplate> (Characters, () => {
				});
				break;

			case "AttributeSet":
				{
					GUILayout.Label ("I am an attribute set !!!!");
				}
				break;
			case "WorkStation":
				{
					GUILayout.Label ("I am an work station !!!!");
				}
				break;
			case "Map":
				{
					DrawSelectionButton<MapTemplate> (Maps, () => {
						//Open a new map Editor
						MapContentEditor mapEditor = (MapContentEditor)EditorWindow.GetWindow (typeof(MapContentEditor), true, "GS Map Editor");
						mapEditor.CurrentMap = CurrentSelection as MapTemplate;
						mapEditor.Show ();
					});

				}
				break;

			default:
				break;
			}
			EditorGUILayout.EndScrollView ();

			EditorGUILayout.EndVertical ();


			GUILayout.Space (30);


			GUILayout.BeginVertical ("RightContent", GUILayout.Width(250));

			GUI.color = Color.blue;
			EditorGUILayout.LabelField ("Object Content", EditorStyles.whiteBoldLabel);
			EditorGUILayout.LabelField ("-----------------------", EditorStyles.whiteBoldLabel, GUILayout.Width (100));
			GUI.color = Color.white;

			switch (_currentType) {
			case "Item":
				{
					if (CurrentSelection != null && CurrentSelection is ItemTemplate) {
						DrawItemUI (CurrentSelection as ItemTemplate);
					}
				}
				break;

			case "Map":
				{
				}
				break;

			case "Gear":
				{
					if (CurrentSelection != null && CurrentSelection is GearTemplate) {
						DrawGearUI (CurrentSelection as GearTemplate);
					}
				}
				break;

			case "Potion":
				{
					if (CurrentSelection != null && CurrentSelection is PotionTemplate) {
						DrawPotionUI (CurrentSelection as PotionTemplate);
					}
				}
				break;

			case "Receipe":
				{
					if (CurrentSelection != null && CurrentSelection is ReceipeTemplate) {
						DrawReceipeUI (CurrentSelection as ReceipeTemplate);
					}
				}
				break;

			case "Rune":
				{
					if (CurrentSelection != null && CurrentSelection is RuneTemplate) {
						DrawRuneUI (CurrentSelection as RuneTemplate);
					}
				}
				break;

			case "Buff":
				{
					if (CurrentSelection != null && CurrentSelection is BuffTemplate) {
						DrawBuffUI (CurrentSelection as BuffTemplate);
					}
				}
				break;

			case "Character":
				{
					if (CurrentSelection != null && CurrentSelection is CharacterTemplate) {
						DrawCharacterUI (CurrentSelection as CharacterTemplate);
					}
				}
				break;

			case "AttributeSet":
				{
				}
				break;



			default:
				break;
			}

			GUILayout.EndVertical ();

			GUILayout.EndHorizontal ();


			//Update the values if anything is changed
			if (GUI.changed) {
				if (CurrentSelection != null) {
					EditorUtility.SetDirty (CurrentSelection);
				}
			}

		}


		#region Utitliy Functions

		private void DrawSelectionButton<T> (List<T> input, UnityAction buttonAction) where T : BaseTemplate
		{
			foreach (var template in input) {

				if (CurrentSelection == template) {
					//Color the selected one as green
					GUI.color = Color.green;
				}
				if (GUILayout.Button (template.TypeId)) {
					CurrentSelection = template;

					//Call the additional action that we need to take 
					buttonAction ();
				}

				GUI.color = Color.white;

			}
		}


		private void LoadContent ()
		{
			Items = GetAllObjectsOfType<ItemTemplate> ("Item");

			AttributeSets = GetAllObjectsOfType<AttributeSetTemplate> ("AttributeSet");

			Gears = GetAllObjectsOfType<GearTemplate> ("Gear");

			Potions = GetAllObjectsOfType<PotionTemplate> ("Potion");

			Receipies = GetAllObjectsOfType<ReceipeTemplate> ("Receipe");

			WorkStations = GetAllObjectsOfType<WorkStationTemplate> ("WorkStation");

			Maps = GetAllObjectsOfType<MapTemplate> ("Map");

			Runes = GetAllObjectsOfType<RuneTemplate> ("Rune");

			Buffs = GetAllObjectsOfType<BuffTemplate> ("Buff");

			Characters = GetAllObjectsOfType<CharacterTemplate> ("Character");
		}

		private void CreateObject (string fileName)
		{
			switch (_currentType) {
			case "Item":
				{
					ItemTemplate asset = ScriptableObject.CreateInstance<ItemTemplate> ();
					asset.TypeId = fileName;
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "Gear":
				{
					GearTemplate asset = ScriptableObject.CreateInstance<GearTemplate> ();
					asset.TypeId = fileName;
					asset.Attributes = new List<AttributeTemplate> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "Potion":
				{
					PotionTemplate asset = ScriptableObject.CreateInstance<PotionTemplate> ();
					asset.TypeId = fileName;
					asset.PermanentAttributes = new List<AttributeTemplate> ();
					asset.TimedAttributes = new List<AttributeTemplate> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "Receipe":
				{
					ReceipeTemplate asset = ScriptableObject.CreateInstance<ReceipeTemplate> ();
					asset.TypeId = fileName;
					asset.ReceipeCost = new List<ReceipeUnit> ();
					asset.ReceipeProduction = new List<ReceipeUnit> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "Rune":
				{
					RuneTemplate asset = ScriptableObject.CreateInstance<RuneTemplate> ();
					asset.TypeId = fileName;
					asset.EffectsInLevel = new List<EffectList> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "Buff":
				{
					BuffTemplate asset = ScriptableObject.CreateInstance<BuffTemplate> ();
					asset.TypeId = fileName;
					asset.Effects = new List<Effect> ();
					asset.Attributes = new List<AttributeTemplate> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");

				}
				break;

			case "Character":
				{
					CharacterTemplate asset = ScriptableObject.CreateInstance<CharacterTemplate> ();
					asset.TypeId = fileName;
					asset.Attributes = new List<AttributeTemplate> ();
					asset.Skills = new List<string> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");

				}
				break;

			case "WorkStation":
				{
					WorkStationTemplate asset = ScriptableObject.CreateInstance<WorkStationTemplate> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;

			case "AttributeSet":
				{
					AttributeSetTemplate asset = ScriptableObject.CreateInstance<AttributeSetTemplate> ();
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");
				}
				break;
			case "Map":
				{
					MapTemplate asset = ScriptableObject.CreateInstance<MapTemplate> ();
					asset.TypeId = fileName;
					asset.GenerateMapData (fileName + "_Map");
					AssetDatabase.CreateAsset (asset, _topDirectoryPath + _currentType + "/" + fileName + ".asset");

				}
				break;
			}

			AssetDatabase.SaveAssets ();

			//Reload the content
			_newFileName = "";
			LoadContent ();
			GUI.FocusControl ("CREATE NEW");
		}


		private void DeleteObject (string fileName)
		{
			AssetDatabase.DeleteAsset (_topDirectoryPath + fileName);
			AssetDatabase.SaveAssets ();

			LoadContent ();
		}

		/// <summary>
		/// This draws the common part for all models 
		/// </summary>
		/// <param name="baseTemplate">base template</param>
		private void DrawBaseUI (BaseTemplate baseTemplate)
		{
			if (baseTemplate != null) {
				EditorGUILayout.LabelField ("BaseModel Fields", EditorStyles.boldLabel);
				EditorGUILayout.LabelField ("Type Id ", baseTemplate.TypeId);
				baseTemplate.Name = EditorGUILayout.TextField ("Name", baseTemplate.Name);
			}
		}

		/// <summary>
		/// This draws the common part for all items 
		/// </summary>
		/// <param name="item"></param>
		private void DrawCommonItemUI (ItemTemplate item)
		{
			if (item != null) {
				DrawBaseUI (item);

				GUILayout.Space (20);
				EditorGUILayout.LabelField ("Item Fields", EditorStyles.boldLabel);
				item.Level = EditorGUILayout.IntField ("Level", item.Level);
				item.MaxLevel = EditorGUILayout.IntField ("Max Level", item.MaxLevel);
				item.Rarity = (RarityType)EditorGUILayout.EnumPopup ("Rarity", item.Rarity);
				item.Description = EditorGUILayout.TextField ("Description", item.Description);
				item.Vitality = EditorGUILayout.Slider ("Vitality", item.Vitality, 0f, 1f);
				item.Tier = EditorGUILayout.IntField ("Tier", item.Tier);
				item.Weight = EditorGUILayout.IntField ("Weight", item.Weight);
                item.MapIcon = (Texture2D)EditorGUILayout.ObjectField("Image", item.MapIcon, typeof(Texture2D), false);
                
			}
		}



		#region Editor UI Utilities

		/// <summary>
		/// Creates the string list UI
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="title">Title.</param>
		private void CreateStringListUI(List<string> input, string title){
			GUILayout.Space (20);

			EditorGUILayout.LabelField (title);

			if (GUILayout.Button ("Add Value")) {
				input.Add ("");
			}

			for (int i = 0; i < input.Count; i++) {
				GUILayout.BeginHorizontal ();

				EditorGUILayout.LabelField ("String Value", GUILayout.Width (100));

				input [i] = EditorGUILayout.TextField (input [i]);

				GUI.color = Color.red;
				if (GUILayout.Button ("--")) {
					input.RemoveAt (i);
				}
				GUI.color = Color.white;

				GUILayout.EndHorizontal ();
			}
		}

		/// <summary>
		/// Creates the attribute set UI
		/// </summary>
		/// <param name="sourceAttributes">Source attributes.</param>
		/// <param name="title">Title.</param>
		private void DrawAttributeSetUI (List<AttributeTemplate> sourceAttributes, string title)
		{
			GUILayout.Space (20);

			EditorGUILayout.LabelField (title);

			if (GUILayout.Button ("Add Attribute")) {
				sourceAttributes.Add (new AttributeTemplate ());
			}


			for (int i = 0; i < sourceAttributes.Count; i++) {
				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Attribute Type", GUILayout.Width (100));
				sourceAttributes [i].Type = (AttributeType)EditorGUILayout.EnumPopup (sourceAttributes [i].Type, GUILayout.Width (150));

				EditorGUILayout.LabelField ("Value", GUILayout.Width (50));

				sourceAttributes [i].Value = EditorGUILayout.FloatField (sourceAttributes [i].Value);

				GUILayout.Space (5);

				GUI.color = Color.red;
				if (GUILayout.Button ("--")) {
					if (EditorUtility.DisplayDialogComplex ("DELETE ATTRIBUTE", "Confirm to delete this attribute?", "Delete", "Cancel", "Back") == 0) {
						sourceAttributes.RemoveAt (i);
					}
				}
				GUI.color = Color.white;

				GUILayout.EndHorizontal ();
			}
		}

		/// <summary>
		/// Creates the delete button.
		/// </summary>
		/// <param name="module">Module.</param>
		/// <param name="typeId">Type identifier.</param>
		private void CreateDeleteButton (string module, string typeId)
		{
			GUILayout.Space (10);
			GUI.color = Color.red;
			if (GUILayout.Button ("DELETE", EditorStyles.miniButtonMid, GUILayout.MaxWidth (100))) {
				DeleteObject (module + "/" + typeId + ".asset");
			}
			GUI.color = Color.white;
		}



		#endregion

		/// <summary>
		/// The Customized Inspector UI for 
		/// our item
		/// </summary>
		/// <param name="item">Item.</param>
		private void DrawItemUI (ItemTemplate item)
		{
			//This whole thing is inside a vertical layout
			DrawCommonItemUI (item);

			if (item != null) {
				item.Category = (CategoryType)EditorGUILayout.EnumPopup ("CategoryType", item.Category);
				item.IsCountable = EditorGUILayout.Toggle ("Countable", item.IsCountable);

				if (item.IsCountable) {
					item.MaxStackCount = EditorGUILayout.IntField ("Max Stack Count", item.MaxStackCount);
				}

				CreateDeleteButton ("Item", item.TypeId);
			}
		}

		private void DrawGearUI (GearTemplate gear)
		{
			//Draw the item part of the gear
			DrawCommonItemUI (gear);

			if (gear != null) {
				//Default some gear settings
				gear.Category = CategoryType.Gear; 
				gear.IsCountable = false;


				//Draw the additional part of the gear
				gear.GearType = (GearType)EditorGUILayout.EnumPopup ("Gear Type", gear.GearType);
				gear.BodyPosition = (GearBodyPosition)EditorGUILayout.EnumPopup ("Body Position", gear.BodyPosition);
                gear.SocketCount = EditorGUILayout.IntField("Number of sockets", gear.SocketCount);
				gear.MinRequiredLevel = EditorGUILayout.IntField ("Minimum Required Level", gear.MinRequiredLevel);

				//Gear Attribute Sets
				DrawAttributeSetUI (gear.Attributes, "Gear Attributes");


				CreateDeleteButton ("Gear", gear.TypeId);


			}
		}

		private void DrawReceipeUI (ReceipeTemplate receipe)
		{
			//Draw the item part of the receipe
			DrawCommonItemUI (receipe);

			if (receipe != null) {
				GUILayout.Space (10);

				EditorGUILayout.LabelField ("Cost", EditorStyles.boldLabel);
				if (GUILayout.Button ("Add Cost Unit")) {
					receipe.ReceipeCost.Add (new ReceipeUnit ());
				}

				foreach (ReceipeUnit ru in receipe.ReceipeCost) {
					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("TypeId", GUILayout.Width (50));
					ru.TypeId = EditorGUILayout.TextField (ru.TypeId);
					EditorGUILayout.LabelField ("Cost Amount", GUILayout.Width (100));
					ru.Amount = EditorGUILayout.FloatField (ru.Amount);
				
					GUI.color = Color.red;
					if (GUILayout.Button ("--")) {
						receipe.ReceipeCost.Remove (ru);
					}

					GUI.color = Color.white;
					GUILayout.EndHorizontal ();
				}
					
				GUILayout.Space (10);

				EditorGUILayout.LabelField ("Production", EditorStyles.boldLabel);
				if (GUILayout.Button ("Add Production Unit")) {
					receipe.ReceipeProduction.Add (new ReceipeUnit ());
				}

				foreach (ReceipeUnit ru in receipe.ReceipeProduction) {
					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("TypeId", GUILayout.Width (50));
					ru.TypeId = EditorGUILayout.TextField (ru.TypeId);
					EditorGUILayout.LabelField ("Produce Amount", GUILayout.Width (100));
					ru.Amount = EditorGUILayout.FloatField (ru.Amount);

					GUI.color = Color.red;
					if (GUILayout.Button ("--")) {
						receipe.ReceipeProduction.Remove (ru);
					}
					GUI.color = Color.white;

					GUILayout.EndHorizontal ();

				}
					
			}
		}

		private void DrawRuneUI (RuneTemplate runeTemplate)
		{
			if (runeTemplate != null) {
				DrawItemUI (runeTemplate);

				GUILayout.Space (10);

				EditorGUILayout.LabelField ("Effects in level", EditorStyles.boldLabel);

				if (GUILayout.Button ("Add A Level")) {
					if (runeTemplate.EffectsInLevel.Count <= runeTemplate.MaxLevel) {
						runeTemplate.EffectsInLevel.Add (new EffectList ());
					} else {
						EditorUtility.DisplayDialog ("Add A Level", "You have reach the maximum level.",
							"OK");
					}
				}

				for (int i = 1; i < runeTemplate.EffectsInLevel.Count; i++) {
					DrawEffectsUI (runeTemplate.GetEffects (i).Effects, i);
				}
			}
		}


		private void DrawEffectsUI (List<Effect> effects, int level)
		{
			EditorGUILayout.LabelField ("Rune Effects Level " + level, EditorStyles.boldLabel);
			_selectedType = (EffectType)EditorGUILayout.EnumPopup ("Effect Type", _selectedType);
			
			if (GUILayout.Button ("Add Rune Effect")) {
				if (_selectedType != EffectType.NONE) {
					Effect e = new Effect (_selectedType);
					effects.Add (e);
				} else {
					EditorUtility.DisplayDialog ("Select A Component Type", "Please select a non-none component type to add a component.",
						"OK");
				}
			}

			List<Effect> effectToRemove = new List<Effect> ();

			foreach (Effect e in effects) {
				if (e == null)
					continue;

				GUILayout.Space (10);
				EditorGUILayout.LabelField ("Effect Type ", e.Type.ToString ());

				List<Tuple<string, string>> paramsDefinition = EffectTypeDefinition.GetParameterDefinition (e.Type);
				for (int i = 0; i < paramsDefinition.Count; i++) {
					Tuple<string, string> t = paramsDefinition [i];
					string pName = t.Item1;
					string pType = t.Item2;
					switch (pType) {
					case "float":
						EditorGUILayout.LabelField (pName, GUILayout.Width (100));
						if (e.Parameters.Count <= i) {
							e.Parameters.Add (0f);
						}
						e.Parameters [i] = EditorGUILayout.FloatField (e.Parameters [i]);
						break;
					}
				}

				GUI.color = Color.red;
				if (GUILayout.Button ("--")) {
					effectToRemove.Add (e);
				}

				GUI.color = Color.white;
			}

			foreach (Effect e in effectToRemove) {
				effects.Remove (e);
			}
		}

		private void DrawBuffUI (BuffTemplate buffTemplate)
		{
			if (buffTemplate != null) {
				DrawBaseUI (buffTemplate);
				GUILayout.Space (10);

				buffTemplate.Priority = EditorGUILayout.IntField ("Priority", buffTemplate.Priority);
				buffTemplate.CanStack = EditorGUILayout.Toggle ("CanStack", buffTemplate.CanStack);
				buffTemplate.UpdateFrequency = EditorGUILayout.FloatField ("Update Frequency", buffTemplate.UpdateFrequency); //Float, time period
                buffTemplate.LastTime = EditorGUILayout.FloatField("Last Time", buffTemplate.LastTime);

                buffTemplate.BuffIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", buffTemplate.BuffIcon, typeof(Texture2D), false);


                this.DrawAttributeSetUI (buffTemplate.Attributes, "Buff Attributes");
				this.DrawEffectsUI (buffTemplate.Effects, 1);
			}
		}

		private void DrawCharacterUI(CharacterTemplate characterTemplate){
			if (characterTemplate != null) {
				DrawBaseUI (characterTemplate);
				GUILayout.Space (10);

				characterTemplate.Level = EditorGUILayout.IntField ("Level", characterTemplate.Level);

				this.DrawAttributeSetUI (characterTemplate.Attributes, "Character Attributes");
				this.CreateStringListUI (characterTemplate.Skills, "Skills");
			}	
		}


		private void DrawPotionUI (PotionTemplate potion)
		{
			//Draw the item part of the potion
			DrawCommonItemUI (potion);

			if (potion != null) {
				potion.CoolDown = EditorGUILayout.FloatField ("Cool Down", potion.CoolDown);

				GUILayout.Space (10);

				EditorGUILayout.LabelField ("Permanent Effect", EditorStyles.boldLabel);

				DrawAttributeSetUI (potion.PermanentAttributes, "Permant Attributes");

				GUILayout.Space (10);

				EditorGUILayout.LabelField ("Timed Effect", EditorStyles.boldLabel);

				potion.TimedEffectEffectiveTime = EditorGUILayout.FloatField ("Effective Time", potion.TimedEffectEffectiveTime);

				DrawAttributeSetUI (potion.TimedAttributes, "Timed Attributes");
			
				CreateDeleteButton ("Potion", potion.TypeId);

			}
		}

		private List<T> GetAllObjectsOfType<T> (string filePath) where T : ScriptableObject
		{
			List<T> objects = new List<T> ();
			string fullPath = _topDirectoryPath + filePath;
			var assetFilePaths = GetFiles (fullPath).Where (s => !s.Contains (".meta"));

			foreach (string f in assetFilePaths) {
				T objectLoaded = AssetDatabase.LoadAssetAtPath (f, typeof(T)) as T;
				objects.Add (objectLoaded);
			}

			return objects;
		}


		/// <summary>
		/// Get a collection of file names under 
		/// a directory and all of its sub-directories
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">Path.</param>
		private IEnumerable<string> GetFiles (string path)
		{
			Queue<string> queue = new Queue<string> ();
			queue.Enqueue (path);
			while (queue.Count > 0) {
				path = queue.Dequeue ();
				try {
					foreach (string subDir in Directory.GetDirectories(path)) {
						queue.Enqueue (subDir);
					}
				} catch (System.Exception ex) {
					Debug.LogError (ex.Message);
				}
				string[] files = null;
				try {
					files = Directory.GetFiles (path);
				} catch (System.Exception ex) {
					Debug.LogError (ex.Message);
				}
				if (files != null) {
					for (int i = 0; i < files.Length; i++) {
						yield return files [i];
					}
				}
			}
		}


		#endregion
	}

}

