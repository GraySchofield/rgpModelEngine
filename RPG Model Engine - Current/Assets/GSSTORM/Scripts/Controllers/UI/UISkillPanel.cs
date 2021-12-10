using UnityEngine;
using UnityEngine.UI;
using GSStorm.RPG.Engine;
using System.Collections.Generic;

namespace GSStorm.RPG.Game
{
    public class UISkillPanel : UIPanel
    {
        Player _currentPlayer;
        CombatUnitSkillSet _playerSkillSet;
        List<GameObject> _skillBtns;
        
        // Use this for initialization
        void Start()
        {
            _skillBtns = new List<GameObject>();

            _currentPlayer = CoreGameController.Current.CurrentPlayer;

            _playerSkillSet = _currentPlayer.Skills;

            GameObject skillBtnPrefab = Resources.Load("Prefabs/UI/ButtonSkill") as GameObject;


            //Create skill buttons
            foreach (var skill in _playerSkillSet.LearntSkills)
            {
                GameObject skillBtn = Instantiate(skillBtnPrefab) as GameObject;
                skillBtn.transform.parent = transform.Find("ContentPanel");

                skillBtn.name = skill.TypeId;

                skillBtn.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/SkillIcons/" + skill.TypeId );

                skillBtn.GetComponent<Image>().enabled = _playerSkillSet.AttackSkillEquipped(skill.TypeId);


                //Add button listener
                skillBtn.GetComponent<Button>().onClick.AddListener(
                    () =>
                    {
                        _currentPlayer.EquipAttackSkill(skillBtn.name);
                    }
                );

                _skillBtns.Add(skillBtn);
            }



        }



        private void Update()
        {
            foreach (var skillBtn in _skillBtns)
            {
                skillBtn.GetComponent<Image>().enabled = _currentPlayer.Skills.AttackSkillEquipped(skillBtn.name);
            }
        }
    }
}