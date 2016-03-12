using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.GameManager
{
    [Serializable]
    public class QValueStore
    {
        public Dictionary<State, Dictionary<Action, float>> superStore = new Dictionary<State, Dictionary<Action, float>>();
        public Dictionary<Action, float> dicActionValue;

        public QValueStore(GameObject player)
        {
            for (int x=-50; x<=50; x++)
            {
                for (int y=-2; y<=10; y++)
                {
                    for (int p=0; p<=10; p++)
                    {
                            for (int d = 1; d <= 8; d++) {
                                Dictionary<Action, float> dict = new Dictionary<Action, float>();
                                dict.Add(new Right(player), 0);
                                dict.Add(new Left(player), 0);
                                dict.Add(new DoNothing(player), 0);
                                superStore.Add(new State(x, y, p, d), dict);
                            }
                    }
                }
            }
        }

        public float getQValue(State state, Action action)
        {
            dicActionValue = superStore[state];
            //Debug.Log(action.actName + " = " + dicActionValue[action]);
            return dicActionValue[action];
        }

        public Action getBestAction(State state)
        {
            dicActionValue = superStore[state];
            float bestQValue = -9999.0f;
            float value = 0.0f;
            int contador = 0;
            Action bestAction = dicActionValue.First().Key;
            foreach (var key in dicActionValue.Keys)
            {
                value = dicActionValue[key];
                if (value == 0)
                    contador++;
                if (value > bestQValue)
                {
                    bestQValue = value;
                    bestAction = key;
                }
            }
            if (contador == 3)
            {
                int random = UnityEngine.Random.Range(0, 3);
                var actions = dicActionValue.Keys.ToArray();
                bestAction = actions[random];
            }
            return bestAction;
        }

        public void storeQValue(State state, Action action, float value)
        {
            dicActionValue = superStore[state];
            dicActionValue[action] = value;
            superStore[state] = dicActionValue;
        }
    }
}
