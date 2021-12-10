using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GSStorm.RPG.Engine
{
	/// <summary>
	//Note: Countable Item uses the same ItemTemplate as normal items
	/// </summary>
    
	public class CountableItemFactory : BaseItemFactory<ItemTemplate, CountableItem>
	{
		public override CountableItem Produce(string typeId)
		{
			ItemTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

			CountableItem item = base.Produce(typeId);
            item.MaxCount = template.MaxStackCount;

			return item;
		}
	}
}
