using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;

namespace GSStorm.RPG.Game
{
    public class ItemController : MonoBehaviour
    {
        [HideInInspector]
        public Item CurrentItem;

        Animator _animator;
        SpriteRenderer _itemSprite;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _itemSprite = transform.Find("Image").GetComponent<SpriteRenderer>();
        }

        public void PickByPlayer(Player player)
        {
            if (CurrentItem != null)
            {

                //Move the item to the player 

                iTween.MoveTo(gameObject, player.Position, 0.5f);

                player.Bag.AddItem(CurrentItem);

                Destroy(gameObject, 0.51f);

            }
        }

        public void SetItemImage()
        {
            _itemSprite.sprite = Resources.Load<Sprite>(PrefabConst.ICON_TEXTURE_PATH + CurrentItem.ImageFIleName);          
        }
       

    }
}