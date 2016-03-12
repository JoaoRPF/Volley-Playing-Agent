using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    public class ReinforcementProblem
    {
        State state = null;
        Action[] actions;

        public ReinforcementProblem(State s, Action[] acts)
        {
            state = s;
            actions = acts;
        }

        public State getActualState()
        {
            return state;
        }

        public Action[] getAvailableActions(State state)
        {
            return actions;
        }

        public State takeAction(State state, Action action, out float reward, GameObject jogador, GameObject bola,
            bool tocouNoChao, bool tocouBola, bool marcou)
        {
            reward = 0;
            int xAntes = state.distanciaX;
            action.executeAction(jogador, new Vector3());
            int distX = (int)((bola.transform.position.x - jogador.transform.position.x) * 5.0f);
            int distY = (int)(bola.transform.position.y - jogador.transform.position.y);
            int distP = 0 - (int)(jogador.transform.position.x);
            int dir = dirBola(bola);
            State newState = new State(distX, distY, distP, dir);

            if (tocouNoChao)
                reward = -2;
            /*else if (tocouBola)
                 reward = 0.2f; */
            else if (distX > 3 || distX < -3)
            {
                if (!(Math.Abs(distX) < Math.Abs(xAntes)))
                    reward = -0.5f;            
            }
            else if (marcou)
            {
                reward = 1.0f;
            }

            return newState;
        }

        public int dirBola(GameObject bola)
        {
            Vector2 vec = bola.GetComponent<Rigidbody2D>().velocity.normalized;
            if (vec.y == 0)
            {
                if (vec.x >= 0)
                    return 3;
                else
                    return 7;
            }
            float facejack = vec.y / vec.x;
            if ((facejack < -2 || facejack > 2) && vec.y > 0)
                return 1;
            //NORDESTE
            if ((facejack > 0.5 && facejack < 2) && vec.y >= 0)
                return 2;
            //ESTE
            if ((facejack < 0.5 && facejack > -0.5) && vec.x > 0)
                return 3;
            //SUDESTE
            if ((facejack < -0.5 && facejack > -2) && vec.y < 0)
                return 4;
            //SUL
            if ((facejack < -2 || facejack > 2) && vec.y < 0)
                return 5;
            //SUDOESTE
            if ((facejack > 0.5 && facejack < 2) && vec.y < 0)
                return 6;
            //OESTE
            if ((facejack < 0.5 && facejack > -0.5) && vec.x < 0)
                return 7;
            //NOROESTE
            if ((facejack < -0.5 && facejack > -2) && vec.y > 0)
                return 8;

            return 5;
        }
    }
}
