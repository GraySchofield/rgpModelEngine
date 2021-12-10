using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;
using UnityEngine.UI;

namespace GSStorm.RPG.Game
{
    /// <summary>
    /// Panel for displaying runes of a weapon.
    /// </summary>
    public class UIRunePanel : UIPanel
    {
        Gear _displayedWeapon = null;

        List<Tuple<GameObject, Rune>> _runeButtons;

        void Start()
        {
            _runeButtons = new List<Tuple<GameObject, Rune>>();
        }

        void Update()
        {
            Gear currentWeapon = CoreGameController.Current.CurrentPlayer.Gears.Get(GearBodyPosition.Weapon);

            if(currentWeapon != _displayedWeapon){
                // clear up the old rune list
                ClearAllRuneButtons();

                if (currentWeapon != null)
                {
                    // new weapon, need to redraw the whole panel
                    transform.Find("ContentPanel/WeaponImage/Image").GetComponent<Image>().sprite 
                             = Resources.Load<Sprite>(PrefabConst.ICON_TEXTURE_PATH + currentWeapon.ImageFIleName);

                    foreach (RuneSocket socket in currentWeapon.RuneSockets)
                    {
                        AddRuneButton(socket);
                    }

                    // track the displayed weapon
                    _displayedWeapon = currentWeapon;
                }
            } else if (currentWeapon != null) {
                // We didn't switch the weapon, but rune may changed.
                for (int i = 0; i < currentWeapon.RuneSockets.Count; i++)
                {
                    RuneSocket socket = currentWeapon.RuneSockets[i];
                    if(i >= _runeButtons.Count){
                        AddRuneButton(socket);
                    } else {
                        UpdateRuneButton(socket, i);
                    }
                }
                // If some sockets are removed from the weapon, need to remove redundant sockets.
                while(_runeButtons.Count > currentWeapon.RuneSockets.Count){
                    RemoveRuneButton(_runeButtons.Count - 1);
                }
            }
        }
    
        private void ClearAllRuneButtons(){
            foreach (var tuple in _runeButtons)
            {
                //item already not in bag
                tuple.Item1.SetActive(false);
				ObjectPoolManager.Current.ReleaseToPool (PrefabConst.BAG_BUTTON_ITEM, tuple.Item1);
            }
            _runeButtons.Clear();
        }

        /// <summary>
        /// Adds the rune socket to the rune panel
        /// </summary>
        /// <param name="runeSocket">A rune socket.</param>
        private void AddRuneButton(RuneSocket runeSocket){
			GameObject btnObj = ObjectPoolManager.Current.GetObject (PrefabConst.BAG_BUTTON_ITEM);
			btnObj.transform.parent = transform.Find("ContentPanel/RunePanel");
			btnObj.SetActive (true);

            Rune rune = runeSocket.Rune;
            if (rune != null)  // the socket may have no rune
            {
                Sprite sprite = Resources.Load<Sprite>(PrefabConst.ICON_TEXTURE_PATH + rune.ImageFIleName);
                btnObj.GetComponent<Image>().sprite = sprite;
            } else {
                btnObj.GetComponent<Image>().sprite = null;
            }
            btnObj.SetActive(true);
            _runeButtons.Add(new Tuple<GameObject, Rune>(btnObj, rune));
            AddRuneButtonListener(_runeButtons.Count-1);
        }

        /// <summary>
        /// Updates the rune button. Called when a rune in the weapon is changed.
        /// </summary>
        /// <param name="runeSocket">Rune socket.</param>
        /// <param name="position">Position.</param>
        private void UpdateRuneButton(RuneSocket runeSocket, int position){
            GameObject btnObj = _runeButtons[position].Item1;
            Rune displayedRune = _runeButtons[position].Item2;

            if (runeSocket.Rune == displayedRune) return;

            Rune rune = runeSocket.Rune;

            if (rune != null)  // the socket may have no rune
            {
                btnObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(PrefabConst.ICON_TEXTURE_PATH + rune.ImageFIleName);
            }
            else
            {
                btnObj.GetComponent<Image>().sprite = null;
            }

            _runeButtons[position] = new Tuple<GameObject, Rune>(btnObj, rune);
            AddRuneButtonListener(position);
        }
    
        private void RemoveRuneButton(int position){
            Tuple<GameObject, Rune> tuple = _runeButtons[position];
            tuple.Item1.SetActive(false);
			ObjectPoolManager.Current.ReleaseToPool (PrefabConst.BAG_BUTTON_ITEM, tuple.Item1);
			_runeButtons.RemoveAt(position);
        }

        private void AddRuneButtonListener(int position){
            Tuple<GameObject, Rune> tuple = _runeButtons[position];
            GameObject btnObj = tuple.Item1;
            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Player player = CoreGameController.Current.CurrentPlayer;
                Gear weapon = player.Gears.Get(GearBodyPosition.Weapon);
                weapon.UnpluginRune(position);
                player.Bag.AddItem(tuple.Item2);
            });
        }
    }
}
