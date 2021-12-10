using UnityEngine;
using System;

namespace GSStorm.RPG.Engine
{
    public enum CompileType
    {
        None = 0,
        Raw = 1,
        Compiled = 2,
    }

    /// <summary>
    /// Attribute.
    /// </summary>
    [Serializable]
    public class Attribute: BaseModel
    {
        public CompileType CompileType { get; set; }

		private AttributeType _type;

		/// <summary>
        /// Type of a attribute
        /// </summary>
        /// <value>The type.</value>
		public AttributeType Type { 
			get{ 
				return _type;
			}
			set{
				_type = value;
			}
		}

		private float _value;

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        /// <value>The value.</value>
        public float Value { 
			get{
				return _value;
			}
		}

        public Attribute():base() { }

        public Attribute(Attribute attr):this()
        {
            Type = attr.Type;
            CompileType = attr.CompileType;
            _value = attr.Value;
        }

        public Attribute(AttributeType type, float value) : this()
        {
            Type = type;
            SetCompileType();
            _value = value;
        }

        /// <summary>
        /// Sets the value.
        /// 
        /// Set method is created instead of set property on purpose.
        /// This method will only do raw data set, no compilation process is triggered.
        /// 
        /// Use this method when you don't need compilation.
        /// </summary>
        /// <param name="value">Value.</param>
        public void SetValue(float value){
            this._value = value;
        }

        /// <summary>
        /// Adds a <see cref="GSStorm.RPG.Engine.Attribute"/> to a <see cref="GSStorm.RPG.Engine.Attribute"/>, yielding
        /// a new <see cref="T:GSStorm.RPG.Engine.Attribute"/>.
        /// </summary>
        /// <param name="a1">The first <see cref="GSStorm.RPG.Engine.Attribute"/> to add.</param>
        /// <param name="a2">The second <see cref="GSStorm.RPG.Engine.Attribute"/> to add.</param>
        /// <returns>The <see cref="T:GSStorm.RPG.Engine.Attribute"/> that is the sum of the values of <c>a1</c> and <c>a2</c>.</returns>
        public static Attribute operator +(Attribute a1, Attribute a2)
        {
            if (a1.Type != a2.Type)
            {
                //we don't support addition for 
                //2 different types of attributes
                Debug.LogError("Invalid 'Type' for adding types!");
                return null;
            }
            Attribute attr = new Attribute();
            attr.Type = a1.Type;
            attr.CompileType = a1.CompileType;
            switch (a1.Type)
            {
                default:
                    attr.SetValue(a1.Value + a2.Value);
                    break;
            }

            return attr;
        }

		/// <summary>
		/// Subtracts a <see cref="GSStorm.RPG.Engine.Attribute"/> from another <see cref="GSStorm.RPG.Engine.Attribute"/>, yielding
		/// a new <see cref="T:GSStorm.RPG.Engine.Attribute"/>.
		/// </summary>
		/// <param name="a1">The first <see cref="GSStorm.RPG.Engine.Attribute"/> to be subtract.</param>
		/// <param name="a2">The second <see cref="GSStorm.RPG.Engine.Attribute"/> to subtract.</param>
		/// <returns>The <see cref="T:GSStorm.RPG.Engine.Attribute"/> that is the subtraction of the values of <c>a1</c> and <c>a2</c>.</returns>
		public static Attribute operator -(Attribute a1, Attribute a2)
        {
            if (a1.Type != a2.Type)
            {
                //we don't support subtraction for 
                //2 different types of attributes
                Debug.LogError("Invalid 'Type' for adding types!");
                return null;
            }
            Attribute attr = new Attribute();
            attr.Type = a1.Type;
            attr.CompileType = a1.CompileType;
            switch (a1.Type)
            {
                default:
                    attr.SetValue(a1.Value - a2.Value);
                    break;
            }

            return attr;
        }

		/// <summary>
        /// unary minus an attribute value.
		/// </summary>
		/// <param name="a1">the attribute.</param>
		public static Attribute operator -(Attribute a1)
        {
            Attribute attr = new Attribute();
            attr.Type = a1.Type;
            attr.CompileType = a1.CompileType;
            switch (a1.Type)
            {
                default:
                    attr.SetValue(-a1.Value);
                    break;
            }

            return attr;
        }

        public void SetCompileType(){
            if (RawAttributeType.IsRawType(Type))
            {
                CompileType = CompileType.Raw;
            }
            else if (CompiledAttributeType.IsCompiledType(Type))
            {
                CompileType = CompileType.Compiled;
            }
            else
            {
                CompileType = CompileType.None;
            }
        }
    }
}