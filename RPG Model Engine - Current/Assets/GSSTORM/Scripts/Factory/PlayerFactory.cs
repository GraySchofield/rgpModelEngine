using System;
using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    public class PlayerFactory : CharacterFactory<CharacterTemplate, Player>
    {
        public override Player Produce(string typeId)
        {
            CharacterTemplate template = _loadedTemplates[typeId];
            if (template == null)
            {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
            }

            Player player = base.Produce(typeId);

            //Create bag, maybe we should put this in editor
            Bag bag = new Bag();
            bag.MaxWeight = 100;
            bag.Level = 1;
            player.Bag = bag;

            return player;
        }
    }
}