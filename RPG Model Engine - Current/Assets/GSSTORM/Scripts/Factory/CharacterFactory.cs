using System;
using UnityEngine;


namespace GSStorm.RPG.Engine{
	public class CharacterFactory <S_T, R_T> : BaseFactory<S_T, R_T> 
        where S_T : CharacterTemplate
        where R_T : Character, new()
    {
		public override R_T Produce(string typeId){
			S_T template = _loadedTemplates[typeId];
			if (template == null) {
                throw new TemplateNotFoundException("Template " + typeId + " is not found.");
			}

			R_T character = base.Produce(typeId);

            character.Attributes = ProduceAttributeSetFromTemplate(template.Attributes);
			character.Level = template.Level;

			//Create all the collections in CombatUnit
			character.Gifts = new GiftSet ();
			character.Skills = new CombatUnitSkillSet ();

			//TODO: Assign skills to the character
			return character;
		}
	}
}