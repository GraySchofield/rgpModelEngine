/// <summary>
/// To make any class support singleton access
/// subclass from this abstract function
/// </summary>
using System;

namespace GSStorm.RPG.Engine
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _current;
        public static T Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new T();
                }

                return _current;
            }
        }
    }
}
