using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GSStorm.RPG.Engine
{
	public class PotionFactory : BaseItemFactory<PotionTemplate, Potion>
	{
		public override Potion Produce(string typeId)
		{
			PotionTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

			Potion potion = base.Produce(typeId);

			potion.CoolDown = new Timer(template.CoolDown);

			return potion;
		}

	}
}
