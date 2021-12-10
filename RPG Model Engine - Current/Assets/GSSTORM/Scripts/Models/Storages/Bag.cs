using System.Collections.Generic;
using System;
using System.Linq;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Bag, models the collections of all
    /// items the player is carrying with him
    /// Subclass it for more specific use,
    /// Like (SavageEraBag)
    /// </summary>
    [Serializable]
    public class Bag
    {
        // TODO: Max stack count for CountableItem is not considered yet.

        /// <summary>
        /// How much weight this bag can be put in
        /// </summary>
        /// <value>The max weight.</value>
        public int MaxWeight { get; set; }

        /// <summary>
        /// Gets the current weight.
        /// </summary>
        /// <value>The current weight.</value>
        public int CurrentWeight { get; private set; }

        public int Level { get; set; }

        protected List<Item> _items { get; set; }

        public Bag(){
            _items = new List<Item>();
        }
       
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
		public virtual bool AddItem(Item item){
			if (!CanAddItem (item))
				return false;

            // handle countable item.
            if (item is CountableItem)
            {
                CountableItem countable = item as CountableItem;
                CurrentWeight += countable.Weight * countable.Count;

                if (ContainsItem(item))
                {
                    ((CountableItem)GetItem(item.TypeId)).Count += countable.Count;
                } else {
                    _items.Add(item);
                }

                return true;
            }

            // handle other kind of item
            if (!ContainsItem(item))
            {
                CurrentWeight += item.Weight;
                _items.Add(item);
            }

			return true;
		}

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <returns><c>true</c>, if item was removed, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
		public virtual bool RemoveItem(Item item){
            if (!_items.Contains(item))
            {
                return false;
            }

			_items.Remove(item);

            if(item is CountableItem){
                CountableItem countable = item as CountableItem;
                CurrentWeight -= countable.Count * countable.Weight;
            } else {
                CurrentWeight -= item.Weight;
            }

			return true;
		}

        /// <summary>
        /// Containses the item.
        /// 
        /// If the item is countable, we will check if item with that type if presented.
        /// </summary>
        /// <returns><c>true</c>, if item was containsed, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
        public bool ContainsItem(Item item)
        {
            if(item is CountableItem)
            {
                return _items.FindIndex(v => v.TypeId.Equals(item.TypeId)) != -1;
            }
            else
            {
                return _items.Contains(item);
            }
        }

        /// <summary>
        /// Check whether the item can be added.
        /// </summary>
        /// <returns><c>true</c>, if item can be added, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
		protected bool CanAddItem(Item item){
            int weight = 0;
            if (item is CountableItem)
            {
                CountableItem countable = item as CountableItem;
                weight = countable.Count * countable.Weight;
            }
            else
            {
                weight = item.Weight;
            }

            if (MaxWeight >= (weight + CurrentWeight)) {
				return true;
			} else {
				return false;
			}
		}

        /// <summary>
        /// Gets a specific type of item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="typeId">Type identifier.</param>
        public Item GetItem(string typeId)
        {
            return _items.Find(v => v.TypeId.Equals(typeId));
        }

        /// <summary>
        /// Get the filtered bag content
        /// </summary>
        /// <param name="categoryType">Category type</param>
        /// <returns>items of the category</returns>
        public IEnumerable<Item> GetItemOfCategory(CategoryType categoryType)
        {
            var filteredItems = from item in _items where item.Category == categoryType select item;
            return filteredItems as IEnumerable<Item>;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns>The items in the bag.</returns>
        public IEnumerable<Item> GetItems(){
            return _items;
        }
    }
}
