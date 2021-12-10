using System;

namespace GSStorm.RPG.Engine
{
    [Serializable]
    public class Item : BaseModel
    {
        public int Level { get; set; }
        public int MaxLevel { get; set; }

        public RarityType Rarity { get; set; }

        public string Descritpion { get; set; }

        public float Vitality { get; set; }

        public int Tier { get; set; }

        public int Weight { get; set; }

        public CategoryType Category { get; set; }

        public string ImageFIleName { get; set; }

        /// <summary>
        /// This function only decrease
        /// the vitality of the item,
        /// 
        /// will not produce any effect
        /// </summary>
        /// <param name="cost">Cost.</param>
        public virtual void Consume(float cost){
			Vitality -= cost;
            if (Vitality < 0)
            {
                Vitality = 0;
            }
		}
    }
}
