using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.GameManager
{
    [Serializable]
    public class State
    {
        public int distanciaX;
        public int distanciaY;
        public int distParede;
        private int dir; // 1 - N ; 2 - NE ; 3 - E ; 4 - SE; 5 - S; 6 - SW; 7 - W; 8 - NW; 

        public State(int dx, int dy, int dp, int direc)
        {
            distanciaX = dx;
            distanciaY = dy;
            distParede = dp;
            dir = direc;
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as State;
            if (otherObj == null)
                return false;
            return distanciaX == otherObj.distanciaX &&
                    distanciaY == otherObj.distanciaY &&
                    distParede == otherObj.distParede &&
                dir == otherObj.dir;
        }

        public override int GetHashCode()
        {
            return distanciaX.GetHashCode() * distanciaY.GetHashCode() * distParede.GetHashCode()
                * dir.GetHashCode();
        }
    }
}
