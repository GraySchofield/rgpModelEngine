using UnityEngine;
using System;
using System.Collections;

namespace GSStorm.RPG.Engine
{
    [System.Serializable]
    public struct SerializableTransform
    {
        public SerializableVector3 position { get; set; }
        public SerializableQuaternion rotation { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SerializableTransform(Vector3 v, Quaternion q)
        {
            position = new SerializableVector3(v);
            rotation = new SerializableQuaternion(q);
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("position: {0} rotation: {1}", position, rotation);
        }

        /// <summary>
        /// Automatic conversion from Vector3 to SerializableVector3
        /// </summary>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static implicit operator SerializableTransform(Transform rValue)
        {
            return new SerializableTransform(rValue.position, rValue.rotation);
        }
    }
}