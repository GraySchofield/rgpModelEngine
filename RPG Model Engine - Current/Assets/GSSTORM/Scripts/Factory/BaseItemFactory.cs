using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GSStorm.RPG.Game;


namespace GSStorm.RPG.Engine
{
	public class BaseItemFactory<S_T, R_T> : BaseFactory<S_T, R_T>
		where S_T : ItemTemplate
		where R_T : Item, new()
	{
		public override R_T Produce(string typeId)
		{
			S_T template = _loadedTemplates[typeId];
			if (template == null){
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
			}

			R_T item = base.Produce (typeId);
			item.Level = template.Level;
			item.MaxLevel = template.MaxLevel;
			item.Rarity = template.Rarity;
			item.Descritpion = template.Description;
			item.Vitality = template.Vitality;
			item.Tier = template.Tier;
			item.Weight = template.Weight;
			item.Category = template.Category;
            item.ImageFIleName = template.MapIcon.name;
			return item;

		}

        /// <summary>
        /// Call this produce function ,
        /// when we need to drop an item to the ground
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public virtual R_T ProduceMapItem(string typeId, Vector3 position, Quaternion rotation)
        {
            R_T item = Produce(typeId);

            GameObject mapItem = GameObject.Instantiate(Resources.Load("Prefabs/UI/Item/MapItem/item"),
                position, rotation) as GameObject;

            mapItem.transform.SetParent(GameObject.Find("MapObjects").transform);

            //Assign item to controller
            ItemController ic = mapItem.GetComponent<ItemController>();
            ic.CurrentItem = item;

            ic.SetItemImage();

            return item;
            
        }
	}
}
