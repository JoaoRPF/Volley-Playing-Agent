using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    [Serializable]
    public class DoNothing : Action
    {
        public DoNothing(GameObject r)
        {
            base.actName = "DoNothing";
        }

        public override void executeAction(GameObject player, Vector3 playerPosition)
        {
           
        }
    }
}