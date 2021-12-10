using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
	public class RuneFactory : BaseItemFactory<RuneTemplate, Rune>
	{
		public override Rune Produce(string typeId)
		{
			RuneTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

			Rune rune = base.Produce (typeId);

            List<Effect> effects = template.GetEffects(rune.Level).Effects;

			foreach (var effect in effects) {
				Effect e = new Effect(effect.Type,effect.Parameters);
				rune.AddEffect(e);
			}


            return rune;
		}
	}
}
