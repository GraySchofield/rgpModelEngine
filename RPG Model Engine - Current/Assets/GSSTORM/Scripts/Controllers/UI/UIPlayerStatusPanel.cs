using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;
using GSStorm.RPG.Game;
using UnityEngine.UI;


/// <summary>
/// Used to show some player's combat status
/// 
/// like Buffs, etc
/// </summary>
public class UIPlayerStatusPanel : UIPanel {
    CombatUnitBuffSet _buffs;
    List<Tuple<GameObject, Buff>> _buffIcons; //currently showing buff icons
    List<Tuple<GameObject, Buff>> _removeList;

	// Use this for initialization
	void Start () {
        _buffs = CoreGameController.Current.CurrentPlayer.Buffs;
        _buffIcons = new List<Tuple<GameObject, Buff>>();
        _removeList = new List<Tuple<GameObject, Buff>>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _removeList.Clear();

        foreach (var tuple in _buffIcons)
        {
            //Update timer status of buff
            Buff buff = tuple.Item2;
            if (buff.Duration != null)
            {
                //the button has a timer.
                float process = buff.Duration.Process / buff.Duration.TimeLimit;
                tuple.Item1.transform.Find("Fillimage").GetComponent<Image>().fillAmount = process;
                if (process >= 1f)
                {
                    //Timed up
                    ObjectPoolManager.Current.ReleaseToPool(PrefabConst.BUFF_STATE_ICON, tuple.Item1);
                    _removeList.Add(tuple);
                }
                
            }
        }

        //remove, timed out buffs
        foreach (var tuple in _removeList)
        {
            _buffIcons.Remove(tuple);
        }

        //try to find new buffs
        foreach (var buff in _buffs.GetBuffs())
        {
            if (!ContainsBuff(buff))
            {
                //Found a new buff
                GameObject buffIcon = ObjectPoolManager.Current.GetObject(PrefabConst.BUFF_STATE_ICON);
                buffIcon.transform.SetParent(transform);
                SetBuffIcon(buffIcon, buff);
                _buffIcons.Add(new Tuple<GameObject, Buff>(buffIcon, buff));
        
            }
        }
    }

    bool ContainsBuff(Buff buff)
    {
        return _buffIcons.FindIndex(v => v.Item2.TypeId.Equals(buff.TypeId)) != -1;
    }


    void SetBuffIcon(GameObject buffIcon, Buff buff)
    {
        buffIcon.transform.Find("Fillimage").GetComponent<Image>().sprite = Resources.Load<Sprite>(PrefabConst.BUFF_ICON_PATH + buff.ImageFileName);
        buffIcon.SetActive(true);
    }

}
