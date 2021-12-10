using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSStorm.RPG.Engine;


namespace GSStorm.RPG.Game
{
    public class CameraFollow : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            Player player = CoreGameController.Current.CurrentPlayer;
            if(player != null)
            {
                Vector3 targetPos = new Vector3(player.Position.x, player.Position.y, -10);

                transform.position = Vector3.Lerp(transform.position, targetPos, 3 * Time.deltaTime);
            }
        }
    }

}