using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Represent a set of attributes.
    /// </summary>
	[Serializable]
    public class AttributeSet
    {
        private Dictionary<AttributeType, Attribute> _values;
        private HashSet<AttributeType> _dirtyType; // types that need to be recompiled

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.AttributeSet"/> class.
        /// </summary>
        public AttributeSet()
        {
            _values = new Dictionary<AttributeType, Attribute>();
            _dirtyType = new HashSet<AttributeType>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.AttributeSet"/> class.
        /// </summary>
        /// <param name="attributeSet">Attribute set.</param>
        public AttributeSet(AttributeSet attributeSet): this()
        {
            Add(attributeSet);
        }

        /// <summary>
        /// Check if the set contains a certain type of attribute.
        /// </summary>
        /// <returns><c>true</c>, if type of attribute is found, <c>false</c> otherwise.</returns>
        /// <param name="type">Type.</param>
        public bool ContainsType(AttributeType type)
        {
            return _values.ContainsKey(type);
        }

        /// <summary>
        /// Gets the attribute by the type.
        /// 
        /// Throw exception when trying to get a raw type with allowRawType equals to false.
        /// </summary>
        /// <returns>The attribute.</returns>
        /// <param name="type">Type.</param>
        /// <param name="allowRawType">allowRawType.</param>
        public Attribute GetAttribute(AttributeType type, bool allowRawType = false)
        {
			if (!_values.ContainsKey (type)) {
				_values [type] = new Attribute (type, 0f);
			}

            if(_dirtyType.Contains(type)){
                CompiledAttributeType.Compile(this, type);
                _dirtyType.Remove(type);
            }

            Attribute attr = _values[type];

            if (!allowRawType && attr.CompileType == CompileType.Raw)
            {
                throw new GetAttributeException("Type " + type.ToString() + " is a raw type.");
            }

			return attr;

        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        public void SetAttribute(Attribute attribute)
        {
            SetAttribute(attribute.Type, attribute.Value);
        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="type">AttributeType.</param>
        /// <param name="value">Value</param>
        public void SetAttribute(AttributeType type, float value)
        {
            Attribute attr;
            if (ContainsType(type))
            {
                attr = _values[type];
                attr.SetValue(value);
            }
            else
            {
                attr = new Attribute(type, value);
                _values[type] = attr;
            }
            if (attr.CompileType == CompileType.Raw) { MarkDirty(type); }
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="type">AttributeType.</param>
        /// <param name="value">Value</param>
        public void AddAttribute(AttributeType type, float value){
            if (ContainsType(type))
            {
                float newVale = _values[type].Value + value;
                SetAttribute(type, newVale);
            }
            else
            {
                SetAttribute(type, value);
            }
        }

        /// <summary>
        /// Substracts the attribute.
        /// </summary>
        /// <param name="type">AttributeType.</param>
        /// <param name="value">Value</param>
        public void SubstractAttribute(AttributeType type, float value)
        {
            AddAttribute(type, -value);
        }

        /// <summary>
        /// Gets or sets the <see cref="T:GSStorm.RPG.Engine.AttributeSet"/> with the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        public Attribute this[AttributeType type]
        {
            get { return GetAttribute(type); }
            set { SetAttribute(value); }
        }

        /// <summary>
        /// Add the specified attribute set.
        /// </summary>
        /// <param name="attributeSet">Attribute set to add.</param>
        public void Add(AttributeSet attributeSet)
        {
            foreach (Attribute attribute in attributeSet._values.Values)
            {
                if (attribute.CompileType == CompileType.Compiled) { continue; }
                AttributeType attributeType = attribute.Type;
                if (_values.ContainsKey(attributeType))
                {
                    _values[attributeType] += attributeSet._values[attributeType];
                }
                else
                {
                    _values.Add(attributeType, new Attribute(attributeSet._values[attributeType]));
                }
                if(attribute.CompileType == CompileType.Raw) { MarkDirty(attributeType);}
            }
        }

        /// <summary>
        /// Substract the specified attribute set.
        /// </summary>
        /// <param name="attributeSet">Attribute set to subtract.</param>
        public void Substract(AttributeSet attributeSet)
        {
            foreach (Attribute attribute in attributeSet._values.Values)
            {
                if (attribute.CompileType == CompileType.Compiled) { continue; }
                AttributeType attributeType = attribute.Type;
                if (_values.ContainsKey(attributeType))
                {
                    _values[attributeType] -= attributeSet._values[attributeType];
                }
                else
                {
                    _values.Add(attributeType, -attributeSet._values[attributeType]);
                }
                if (attribute.CompileType == CompileType.Raw) { MarkDirty(attributeType); }
            }
        }

        private void MarkDirty(AttributeType type)
        {
            _dirtyType.Add(RawAttributeType.GetCompiledType(type));
        }
    }

    public class GetAttributeException : Exception, ISerializable
    {
        public GetAttributeException(): base() {}
        
        public GetAttributeException(string message): base(message) {}
            
        public GetAttributeException(string message, Exception inner): base(message, inner) {}

        // This constructor is needed for serialization.
        protected GetAttributeException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}