using System;
namespace GSStorm.RPG.Engine{
	[Serializable]
	public class ReceipeUnit 
	{
		/// <summary>
		/// TypeId identifies what item is 
		/// in this receipe
		/// </summary>
		public string TypeId;

		/// <summary>
		/// The amount.
		/// can be the amount that is used
		/// or the amount that is produced
		/// </summary>
		public float Amount;

		public ReceipeUnit(string typeId, float amount){
			TypeId = typeId;
			Amount = amount;
		}

		public ReceipeUnit(ReceipeUnit source){
			TypeId = source.TypeId;
			Amount = source.Amount;
		}

		public ReceipeUnit(){
			
		}
	}
}

