using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GSStorm.RPG.Engine;
using System;
using UnityEngine.UI;

namespace GSStorm.RPG.Game{
	public class UIGearPanel : UIPanel
	{

		#region Private Variables

		/// <summary>
		/// The collection of player's current equipped gears
		/// </summary>
		GearSet _gears; 

		/// <summary>
		/// the ui object, indexed by body position
		/// </summary>
		Dictionary<GearBodyPosition, GameObject> _gearUIDict;

		#endregion

		// Use this for initialization
		void Start ()
		{
			_gears = CoreGameController.Current.CurrentPlayer.Gears;
			_gearUIDict = new Dictionary<GearBodyPosition, GameObject> ();

			foreach (GearBodyPosition type in Enum.GetValues (typeof(GearBodyPosition))) {
				if (transform.Find("ContentPanel/Gear_" +  ((int)type)) != null) {
					//We now support the body part 
					_gearUIDict.Add(type, transform.Find("ContentPanel/Gear_" +  ((int)type)).gameObject);
				}
			}
		
		}


		// Update is called once per frame
		void Update ()
		{
			foreach (var body_type in _gearUIDict.Keys) {
				Gear gear = _gears.Get (body_type);
				GameObject uiObject = _gearUIDict [body_type];
			    GameObject imageObject = uiObject.transform.Find("Image").gameObject;

				if (gear != null) {
					imageObject.SetActive (true);
					//this gear is not null, show correct image
					imageObject.GetComponent<Image> ().sprite = Resources.Load<Sprite> (PrefabConst.ICON_TEXTURE_PATH + gear.ImageFIleName);

					//TODO: Add the right listener to open the run socket panel
				} else {
					imageObject.SetActive (false);

					//Clear listeners
					uiObject.GetComponent<Button> ().onClick.RemoveAllListeners ();
				}
			}
		}
	}

}
