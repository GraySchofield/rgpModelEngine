using System;

namespace GSStorm.RPG.Engine
{
    [Serializable]
    public class CountableItem : Item
    {
        public int Count { get; set; }

        /// <summary>
        /// The item may or may not have a max stack  count
        /// </summary>
        public int? MaxCount { get; set; }

		public override void Consume(float cost){
			base.Consume (cost);
			if (Math.Abs(Vitality) < 0.001) {
                Count -= 1;
				if (Count > 0) {
					Vitality = 1;
				}
			}
		}
    }
}
