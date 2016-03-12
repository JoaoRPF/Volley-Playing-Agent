using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    [Serializable]
    public class Up : Action
    {
        private int saltou = 0;

        public Up(GameObject r)
        {
            base.actName = "Up";
        }

        public override void executeAction(GameObject player, Vector3 playerPosition)
        {
           
        }
    }
}
