using System;

namespace GSStorm.RPG.Engine
{
    [Serializable]
    abstract public class BaseModel
    {
        /// <summary>
        /// This id uniquely identifies an object.
        /// For e.g. this id would be different for 
        /// two identical weapons.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type id identifies what each model is.
        /// I.e. this id itself will be sufficient
        /// for telling us the natural of the paticular
        /// object.
        /// </summary>
        public string TypeId { get; set; } 

        /// <summary>
        /// The name of the object.
        /// This field is for display.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GSStorm.RPG.Engine.BaseModel"/> class.
        /// Id will be created during the contruction.
        /// </summary>
        public BaseModel() { Id = Guid.NewGuid().ToString("N"); }
    }
}