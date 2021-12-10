using UnityEngine;
using System.Collections;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Category represent the broad 
    /// categorization of the items
    /// 
    /// this enum could vary by the game
    /// we are making
    /// 
    /// the main use of this enum is 
    /// for the bag and inventory to 
    /// classify the items
    /// </summary>
    public enum CategoryType
    {
        Resource = 1,
        Currency = 2,
        Potion = 3,
        Gear = 4,
        QuestItem = 5,
        SpecialItem = 6,
		Receipe = 7,
        Rune = 8
    }
}
