using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    [Serializable]
    abstract public class Action
    {
        public String actName;
        public float maxSpeed = 2.0f;
        public float ALTURA = 1.3f;
        abstract public void executeAction(GameObject player, Vector3 playerPosition);

        public Action()
        {
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as Action;
            if (otherObj == null)
                return false;
            return actName.Equals(otherObj.actName);
        }

        public override int GetHashCode()
        {
            return actName.GetHashCode();
        }
    }
}
