using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Data manager in charged of game save and load.
    /// </summary>
    public class DataManager: Singleton<DataManager>, IManager
    {
        public DataManager() { }

        public ManagerStatus Status { get; private set; }

        private string filename;

        public IEnumerator StartLaunch()
        {
            Debug.Log("Data manager starting...");

            filename = Path.Combine(Application.persistentDataPath, "game.dat");

            Debug.Log(filename);

            // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
			Status = ManagerStatus.StarLaunching;
            LoadGameState();

            Status = ManagerStatus.Started;

            yield return null;
        }

        public void SaveGameState()
        {
            // TODO: this may not work, need code refine.
            Dictionary<string, object> gamestate = new Dictionary<string, object>();
            gamestate.Add("Demo", "Demo");

            FileStream stream = File.Create(filename);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, gamestate);
            stream.Close();
        }

        public void LoadGameState()
        {
            if (!File.Exists(filename))
            {
                Debug.Log("No saved game");
                return;
            }

            Dictionary<string, object> gamestate;

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(filename, FileMode.Open);
            gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();
        }

        public IEnumerator PreLaunch() { yield return null; }
        public IEnumerator PostLaunch() { yield return null; }
    }
}
