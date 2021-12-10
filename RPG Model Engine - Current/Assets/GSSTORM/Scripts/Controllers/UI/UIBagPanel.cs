using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEngine.UI;

namespace GSStorm.RPG.Game
{
    public class UIBagPanel : UIPanel
    {
		#region Private Variables
        Bag _currentBag;

        List<Tuple<GameObject, Item>> _btnItems; //the current btn items in bag

        List<Tuple<GameObject, Item>> _removeList;
		#endregion

        // Use this for initialization
        void Start()
        {
            _currentBag = CoreGameController.Current.CurrentPlayer.Bag;
            _btnItems = new List<Tuple<GameObject, Item>>();
            _removeList = new List<Tuple<GameObject, Item>>();
        }

        // Update is called once per frame
        void Update()
        {
            //remove invalid items first
            _removeList.Clear();

            foreach (var tuple in _btnItems)
            {
                if (!_currentBag.ContainsItem(tuple.Item2))
                {
                    //item already not in bag
                    tuple.Item1.SetActive(false);

					ObjectPoolManager.Current.ReleaseToPool(PrefabConst.BAG_BUTTON_ITEM, tuple.Item1);

                    _removeList.Add(tuple);
                }
                else
                {
                    //item still in bag, if countable need to update count
                    if(tuple.Item2 is CountableItem)
                    {
                        ((CountableItem)tuple.Item2).Count = ((CountableItem)_currentBag.GetItem(tuple.Item2.TypeId)).Count;
                    }

                    SetBtnItemCount(tuple.Item1, _currentBag.GetItem(tuple.Item2.TypeId));
                }
            }

            //remove
            foreach (var tuple in _removeList)
            {
                _btnItems.Remove(tuple);
            }

            //try to find any new items
            foreach (var item in _currentBag.GetItems())
            {
                if (!ContainsItem(item))
                {
					GameObject btnItem = ObjectPoolManager.Current.GetObject (PrefabConst.BAG_BUTTON_ITEM);
					btnItem.transform.parent = transform.Find("Scroll View/Viewport/Content");
					SetBtnForItem (btnItem, item);
					_btnItems.Add(new Tuple<GameObject, Item>(btnItem, item));
					btnItem.SetActive (true);
                }
            }


        }

        void SetBtnForItem(GameObject btnItem, Item item)
        {
            Button btn = btnItem.GetComponent<Button>();
            
            btnItem.GetComponent<Image>().sprite = Resources.Load<Sprite>(PrefabConst.ICON_TEXTURE_PATH + item.ImageFIleName);  //TODO: we should put these in to sprite atlas for real, now just for demo
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Player player = CoreGameController.Current.CurrentPlayer;
                if (item is Gear)
                {
                    Gear swappedGear;
                    player.EquipGear(item as Gear, out swappedGear);
                }
                else if (item is Rune)
                {
                    Gear weapon = player.Gears.Get(GearBodyPosition.Weapon);
                    if (weapon == null) return;
                    for(int i = 0; i < weapon.RuneSockets.Count; i++){
                        RuneSocket socket = weapon.RuneSockets[i];
                        Rune newRune = item as Rune;
                        if(socket.Rune == null){
                            Rune swappedRune;
                            weapon.PluginRune(newRune, i, out swappedRune);
                            break;
                        }
                    }
                    player.Bag.RemoveItem(item);
                }
            });

            SetBtnItemCount(btnItem, item);
        }

        void SetBtnItemCount(GameObject btnItem, Item item)
        {
            Text itemCount = btnItem.transform.Find("Count").GetComponent<Text>();

            if( item is CountableItem)
            {
                itemCount.text = ((CountableItem)item).Count.ToString();

            }
            else
            {
                itemCount.enabled = false;
            }
        }

        /// <summary>
        /// if an item exists in bag panel
        /// 
        /// We are ignoring the max stack count for countable items for now
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ContainsItem(Item item)
        {
            if (item is CountableItem)
            {
                return _btnItems.FindIndex(v => v.Item2.TypeId.Equals(item.TypeId)) != -1;
            }
            else
            {
                return _btnItems.FindIndex(v => v.Item2 == item) != -1;
            }

        }

    }
}
