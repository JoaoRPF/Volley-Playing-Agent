using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    public class QLearning
    {
        Action action = null;
        private float previousQ;
        private State previousState = null;
        State state = null;

        public void qLearning(ReinforcementProblem problem, QValueStore store, Action[] actions, 
            GameObject jogador, GameObject bola, bool tocouNoChao, bool tocouBola, bool marcou,
            int iterations, float alpha, float gamma, float rho, float nu)
        {
            state = problem.getActualState();

            if (previousState != null) {
                float reward = 0.0f;
                State newState = problem.takeAction(state, action, out reward, jogador, bola, tocouNoChao, tocouBola, marcou);
                float maxq = store.getQValue(state, store.getBestAction(state));
                float q = (1 - alpha) * previousQ + alpha * (reward + gamma * maxq);
                store.storeQValue(previousState, action, q);
            }


            float random = Random.Range(0.0f, 1.0f);
           
            if (random < rho)
            //if(true)
            {
                int randomAction = Random.Range(0, 3);
                action = actions[randomAction];
            }
            else
                action = store.getBestAction(state);
            /*
            int reward = 0;
            State newState = problem.takeAction(state, action, out reward);
            float q = store.getQValue(state, action);
            float maxq = store.getQValue(newState, store.getBestAction(newState));

            q = (1 - alpha) * q + alpha * (reward + gamma * maxq);

            store.storeQValue(state, action, q);
            */
            previousQ = store.getQValue(state, action);
            previousState = state;
            
            //state = newState;
        }
    }
}