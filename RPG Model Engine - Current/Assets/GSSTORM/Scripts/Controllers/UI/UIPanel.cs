using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GSStorm.RPG.Engine
{

    /// <summary>
    /// Common shared functionalities for all UIs
    /// </summary>
    public class UIPanel : MonoBehaviour
    {
        /// <summary>
        /// Shared close panel functions
        /// </summary>
        Button _btnClose;

        // Use this for initialization
        protected virtual void Awake()
        {
            Transform transformClose = transform.Find("ButtonClose");
            if (transformClose != null)
            {
                _btnClose = transformClose.GetComponent<Button>();
                _btnClose.onClick.AddListener(() => { gameObject.SetActive(false); });
            }
        }
    }
}
