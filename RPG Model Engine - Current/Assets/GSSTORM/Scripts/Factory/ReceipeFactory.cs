using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GSStorm.RPG.Engine
{
	public class ReceipeFactory : BaseItemFactory<ReceipeTemplate, Receipe>
	{
		public override Receipe Produce(string typeId)
		{
			ReceipeTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

			Receipe receipe = base.Produce(typeId);

			foreach (ReceipeUnit ru in template.ReceipeCost)
			{
				receipe.ReceipeCost.Add(new ReceipeUnit(ru));
			}

			foreach (ReceipeUnit ru in template.ReceipeProduction)
			{
				receipe.ReceipeProduction.Add(new ReceipeUnit(ru));
			}

			return receipe;
		}
	}
}
