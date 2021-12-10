using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GSStorm.RPG.Engine
{
    public class GearFactory : BaseItemFactory<GearTemplate, Gear>
    {
        public override Gear Produce(string typeId)
        {
            GearTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

            Gear gear = base.Produce(typeId);
            gear.GearType = template.GearType;
            gear.BodyPosition = template.BodyPosition;
            if(template.SocketCount > 0){
                for (int i = 0; i < template.SocketCount; i++)
                {
                    gear.RuneSockets.Add(new RuneSocket(0));
                }
            }
            gear.Attributes = ProduceAttributeSetFromTemplate(template.Attributes);

            return gear;
            
        }
    }
}
