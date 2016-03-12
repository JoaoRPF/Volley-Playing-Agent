using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    [Serializable]
    public class Left : Action
    {
        public Left(GameObject r)
        {
            base.actName = "Left";
        }

        public override void executeAction(GameObject player, Vector3 playerPosition)
        {
            playerPosition = player.transform.position;
            if (playerPosition.x > -9.0f) {
                playerPosition.x -= 0.2f;
                player.transform.position = playerPosition;
            }
        }
    }
}
