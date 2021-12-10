using UnityEngine.Events;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    public enum GameEventType
    {
    }


    public static class GameEventHandler
    {
        private static Dictionary<GameEventType, UnityAction<CombatUnit>> _events = new Dictionary<GameEventType, UnityAction<CombatUnit>>();

        public static void RegisterEvent(GameEventType eventName, UnityAction<CombatUnit> action)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName] += action;
            }
            else
            {
                _events[eventName] = action;
            }
        }

        public static void RemoveEvent(GameEventType eventName, UnityAction<CombatUnit> action)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName] -= action;   
            }
        }

        public static void DispatchEvent(GameEventType eventName, CombatUnit cu)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName](cu);
            }
        }

    }

}