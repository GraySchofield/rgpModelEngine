using UnityEngine;
using System.Collections.Generic;
using System;

namespace GSStorm.RPG.Engine{
    /// <summary>
    /// Used as base class for factories
    /// 
    /// Factories subclassed from this class 
    /// is in charge of generating dynamic 
    /// game data from static template.
    /// </summary>
	public abstract class BaseFactory<S_T,R_T> 
        where S_T : BaseTemplate
		where R_T : BaseModel, new()
	{
		#region Private & Protected Fields

		/// <summary>
        /// Dictionary for all loaded templates
        /// 
		/// TypeId -> Scriptable object
		/// </summary>
		protected Dictionary<string, S_T> _loadedTemplates;

		#endregion

		#region Public Fields

		#endregion
	
		#region Priate & Protected Functions

        protected AttributeSet ProduceAttributeSetFromTemplate(List<AttributeTemplate> template)
        {
            AttributeSet attributeSet = new AttributeSet();
            foreach (var aTemplate in template)
            {
                attributeSet.SetAttribute(aTemplate.Type, aTemplate.Value);
            }

            return attributeSet;
        }

        #endregion

        #region Public Functions

        public BaseFactory(){
			_loadedTemplates = new Dictionary<string, S_T> ();
		}

		/// <summary>
		/// Load all the templates within a folder
		/// </summary>
		/// <param name="templateFolderName">Template folder name.</param>
		public void LoadAllTemplates(string templateFolderName){
            UnityEngine.Object[] resources = Resources.LoadAll(templateFolderName, typeof(S_T));
            foreach (var r in resources)
            {
                S_T objectLoaded = r as S_T;
                _loadedTemplates[objectLoaded.TypeId] = objectLoaded;
            }
		}

		/// <summary>
		/// Load a single specified template file
		/// </summary>
		/// <param name="templateFolderName">Template folder name.</param>
		/// <param name="templateName">Template name.</param>
		public void LoadTemplate(string templateFolderName, string templateName){
			string fullPath = templateFolderName + "/" + templateName;

			S_T objectLoaded = Resources.Load (fullPath, typeof(S_T)) as S_T;
		    _loadedTemplates[objectLoaded.TypeId] = objectLoaded;
		}

        /// <summary>
        /// Produce an object based on the TypeId of the Scriptable object.
        /// This function will automatically produce the right type for each 
        /// subclass factory
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
		public virtual R_T Produce(string typeId){
			S_T template = _loadedTemplates[typeId];
			if (template == null) {
                throw new TemplateNotFoundException("Template "+typeId+" is not found.");
			}

			R_T result = new R_T ();
			result.Name = template.Name;
			result.TypeId = template.TypeId;

			return result;
		}

		#endregion
	
	}

    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException(){}

        public TemplateNotFoundException(string message)
            : base(message){}

        public TemplateNotFoundException(string message, Exception inner)
            : base(message, inner){}
    }
}

