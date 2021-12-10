using System.Collections;
using System.Collections.Generic;
using System;

namespace GSStorm.RPG.Engine
{
    /// <summary>
    /// Report on how an effect change the status of a combat unit. 
    /// </summary>
    [Serializable]
    public class EffectReport: BaseModel
    {
        public float HPChange;
    }
}