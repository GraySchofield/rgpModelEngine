using UnityEngine;
using System.Collections.Generic;

namespace GSStorm.RPG.Engine
{
    [CreateAssetMenu(fileName = "AttributeSet", menuName = "Template/AttributeSet", order = 1)]
    public class AttributeSetTemplate : ScriptableObject
    {
        public List<AttributeTemplate> Attributes;
    }

}