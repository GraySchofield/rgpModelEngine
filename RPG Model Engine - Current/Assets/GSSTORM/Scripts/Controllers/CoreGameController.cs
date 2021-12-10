using System;
using GSStorm.RPG.Engine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace GSStorm.RPG.Game
{
    /// <summary>
    /// Core game controller.
    /// 
    /// Temparally used as the master of logic,
    /// to put all the pieces together
    /// </summary>
    public class CoreGameController : MonoBehaviour
    {

		#region Managers
		private List<IManager> _managerStartSequence;
		#endregion

        #region Factories Properties
        public RuneFactory RuneFactory
        {
            get;
            private set;
        }

        public BuffFactory BuffFactory
        {
            get;
            private set;
        }

        public CharacterFactory<CharacterTemplate, Character> CharacterFactory
        {
            get;
            private set;
        }

        public PlayerFactory PlayerFactory
        {
            get;
            private set;
        }

        public SkillFactory SkillFactory
        {
            get;
            private set;
        }

        public GearFactory GearFactory
        {
            get;
            private set;
        }

        public BaseItemFactory<ItemTemplate, Item> NormalItemFactory
        {
            get;
            private set;
        }

        public CountableItemFactory CountableItemFactory
        {
            get;
            private set;
        }

        #endregion


        #region Player Properties
        public GameObject PlayerPrefab;
        public Player CurrentPlayer;
        private PlayerController _playerController;

        #endregion

        #region Monster & Other Characters
        public GameObject MonsterPrefab;

        #endregion


        #region Initialization


        public static CoreGameController Current;

        void Awake()
        {
            Current = this;
		}

       
        void Start(){
			//Init Managers;
			_managerStartSequence = new List<IManager>();
			_managerStartSequence.Add (ObjectPoolManager.Current);
			StartCoroutine(StartupManagers());
        }


		private IEnumerator StartupManagers()
		{
			foreach (IManager manager in _managerStartSequence)
			{
				yield return StartCoroutine(manager.PreLaunch());
			}


			foreach (IManager manager in _managerStartSequence)
			{
				yield return StartCoroutine (manager.StartLaunch ());
			}

			foreach (IManager manager in _managerStartSequence)
			{
				yield return StartCoroutine (manager.PostLaunch ());
			}

			Debug.Log("All managers started up");

			yield return StartCoroutine (StartUpGame());
		}


		IEnumerator StartUpGame(){
			InitializeFactories ();
			Debug.Log ("Factories initialized ...");
			yield return null;

			InitPlayer ();
			Debug.Log ("Player initialized ... ");
			yield return null;
	
			InitUIs();
			Debug.Log ("UI initialized ...");
			yield return null;

			InvokeRepeating ("SpawnMonsters", 0, 5);
			Debug.Log ("Start Spawn Monsters ...");
			yield return null;

        }

		/// <summary>
		/// Initializes the factories.
        /// 
        /// TODO: Add loading and coroutine for this
        /// </summary>
        void InitializeFactories()
        {
            RuneFactory = new RuneFactory();
            RuneFactory.LoadAllTemplates("GSStormGameContent/Rune");

            BuffFactory = new BuffFactory();
            BuffFactory.LoadAllTemplates("GSStormGameContent/Buff");

            CharacterFactory = new CharacterFactory<CharacterTemplate, Character>();
            CharacterFactory.LoadAllTemplates("GSStormGameContent/Character");

            PlayerFactory = new PlayerFactory();
            PlayerFactory.LoadAllTemplates("GSStormGameContent/Character/Player");

            GearFactory = new GearFactory();
            GearFactory.LoadAllTemplates("GSStormGameContent/Gear");

            NormalItemFactory = new BaseItemFactory<ItemTemplate, Item>();
            NormalItemFactory.LoadAllTemplates("GSStormGameContent/Item");

            CountableItemFactory = new CountableItemFactory();
            CountableItemFactory.LoadAllTemplates("GSStormGameContent/Item");

            SkillFactory = new SkillFactory();

        }

        void InitPlayer()
        {
            //Instantiate the player object
            GameObject playerObject = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
            _playerController = playerObject.GetComponent<PlayerController>();

            //Create the player model instance and related stuffs
            CurrentPlayer = PlayerFactory.Produce("player");
            //Set the player model to player controller
            _playerController.CurrentCharacter = CurrentPlayer;

            //Set aggresive target
            CurrentPlayer.TargetEnemyLayers.Add(LayerMask.NameToLayer("Monster")); // player only attack monsters

            InitPlayerSkills();

            //InitPlayerRunes ();

            //Test code, gift weapon
            CurrentPlayer.Bag.AddItem(GearFactory.Produce("gear_weapon_iron_bow"));
            CurrentPlayer.Bag.AddItem(RuneFactory.Produce("rune_cd_reduce"));
            CurrentPlayer.Bag.AddItem(RuneFactory.Produce("rune_poison_self"));
            CurrentPlayer.Bag.AddItem(RuneFactory.Produce("rune_poison_on_hit"));

        }


        /// <summary>
        /// Inits the player skills.
        /// 
        /// The player can choose to learn and equip
        /// </summary>
        void InitPlayerSkills()
        {

            AttackSkill tempSkill = SkillFactory.ProduceAttackSkill("skill_crush_land");
            tempSkill.Learn(CurrentPlayer);

            tempSkill = SkillFactory.ProduceAttackSkill("skill_projectile");
            tempSkill.Learn(CurrentPlayer);

            tempSkill = SkillFactory.ProduceAttackSkill("skill_blade_fury");
            tempSkill.Learn(CurrentPlayer);

            CurrentPlayer.EquipAttackSkill("skill_blade_fury");
        }

        /// <summary>
        /// Put all runs in player's bag
        /// 
        /// players can choose to equip as he wants
        /// </summary>
        void InitPlayerRunes()
        {
        }


        void SpawnMonsters()
        {
            //Instantiate the player object
            GameObject monsterObject = Instantiate(MonsterPrefab, new Vector3(UnityEngine.Random.Range(1, 5), UnityEngine.Random.Range(1, 5), 0), Quaternion.identity);
            MonsterController monsterController = monsterObject.GetComponent<MonsterController>();
            monsterObject.SetActive(true);

            //Create the player model instance and related stuffs
            Character monster = CharacterFactory.Produce("monster_tiger");
            //Set the player model to player controller
            monsterController.CurrentCharacter = monster;

			//Set aggresive target
			monster.TargetEnemyLayers.Add (LayerMask.NameToLayer("Player")); // monster only attack players

			//Set Monster Ai 
			BaseAI ai = monsterObject.GetComponent<BaseAI>();
			ai.CurrentCharacter = monster;
			ai.TargetCharacter = CurrentPlayer;
			ai.RegisterDefaultMonsterBebaviour (); //Use default AI, WE can use RegisterTransaction to change specific behaviour

			//Create the monster's skill
			AttackSkill tempSkill = SkillFactory.ProduceAttackSkill("skill_projectile");
			tempSkill.CoolDown.TimeLimit = 4f;
			tempSkill.Learn(monster);
			monster.EquipAttackSkill("skill_projectile");
		}

        #endregion





        #region UI Controls
        Button _btnSkillPanel;
        Button _btnGearPanel;
        Button _btnBagPanel;


        GameObject _skillPanel;
        GameObject _bagPanel;
        GameObject _gearPanel;
        GameObject _runePanel;
        GameObject _playerStatusPanel;

        void InitUIs()
        {
            _btnSkillPanel = GameObject.Find("UICanvas/ControlButtons/BtnSkillPanel").GetComponent<Button>();
            _btnGearPanel = GameObject.Find("UICanvas/ControlButtons/BtnGearPanel").GetComponent<Button>();
            _btnBagPanel = GameObject.Find("UICanvas/ControlButtons/BtnBagPanel").GetComponent<Button>();

            _skillPanel = GameObject.Find("UICanvas").transform.Find("SkillPanel").gameObject;
            _bagPanel = GameObject.Find("UICanvas").transform.Find("BagPanel").gameObject;
            _gearPanel = GameObject.Find("UICanvas").transform.Find("GearPanel").gameObject;
            _runePanel = GameObject.Find("UICanvas").transform.Find("RunePanel").gameObject;
            _playerStatusPanel = GameObject.Find("UICanvas").transform.Find("PlayerStatusPanel").gameObject;
            _playerStatusPanel.SetActive(true);

            _btnSkillPanel.onClick.AddListener(() => { _skillPanel.SetActive(true); });

            _btnBagPanel.onClick.AddListener(() =>
            {
                _bagPanel.SetActive(true);
                _gearPanel.SetActive(true);

            });

            _btnGearPanel.onClick.AddListener(() =>
            {
                _gearPanel.SetActive(true);
            });

        }

        #endregion




        #region Public Functions
        public void DropMapItem(string typeId, Vector3 position, Quaternion rotation, int count = 1)
        {
            if (typeId.StartsWith("gear"))
            {
                //Drop a gear
                GearFactory.ProduceMapItem(typeId, position, rotation);
            }
            else if (typeId.StartsWith("countable"))
            {
                //Drop counttable item 
                CountableItemFactory.ProduceMapItem(typeId, position, rotation).Count = count;
            }
            else
            {
                //Drop other item
                NormalItemFactory.ProduceMapItem(typeId, position, rotation);
            }
        }


        #endregion

        void Update()
        {
            // Press R to Open/close rune panel
            if (Input.GetKeyUp(KeyCode.R))
            {
                _runePanel.SetActive(!_runePanel.activeSelf);

            }
        }
    }
}

