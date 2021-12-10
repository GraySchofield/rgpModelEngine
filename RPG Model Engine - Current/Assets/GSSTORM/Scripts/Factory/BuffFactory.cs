using System;
using UnityEngine;

namespace GSStorm.RPG.Engine
{
	public class BuffFactory : BaseFactory<BuffTemplate, Buff>
	{
		public override Buff Produce(string typeId){
			BuffTemplate template = _loadedTemplates [typeId];
			if (template == null) {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
			}

			Buff buff = base.Produce (typeId);

			buff.Priority = template.Priority;
			buff.CanStack = template.CanStack;
			buff.UpdateRate = new Timer (template.UpdateFrequency);
         
            buff.UpdateRate.Start();

            if (template.LastTime > 0)
            {
                buff.Duration = new Timer(template.LastTime);
            }
            
			foreach (var effect in template.Effects) {
				Effect e = new Effect (effect.Type, effect.Parameters);
				buff.Effects.Add (e);
			}
           
            buff.Attributes = ProduceAttributeSetFromTemplate(template.Attributes);

            buff.ImageFileName = template.BuffIcon.name;

			return buff;
		}

	}
}

