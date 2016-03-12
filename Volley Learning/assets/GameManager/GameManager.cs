using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public const float ALTURA_MAX = 1.3f;
        public const float LARGURA_CAMPO = 10.0f;
        public const float ALTURA_CAMPO = 10.0f;
        public const float maxSpeedJogador = 2.0f;
        private const float UPDATE_INTERVAL = 0.0f;

        private float nextUpdateTime = 0.0f;
        public DatabaseInit db = new DatabaseInit();
        public Dictionary<State, ArrayList> states;
        public GameObject bola;
        public GameObject jogador;
        public int distx;
        public int disty;
        public int distParede;
        public int toq;
        public String distxStr;
        public String distyStr;
        public String toqStr;
        public String distParedeStr;
        public State state;
        public int toquesTotais = 0;
        public Action[] actions;
        public QValueStore store = null;
        QLearning qlearning;
        public bool tocouNoChao = false;
        public bool tocouBola = false;
        public Text countText;
        private String saves = @"C:\Users\Joao\Desktop\QValueStore";
        public float alpha = 0.7f;
        public float gamma = 0.75f;
        public float rho = 0.2f;
        public float[] xListPlayer;
        public Text toquesText;
        public Text maxText;
        public int breakar = 0;
        public int lastY = 0;
        public bool saving = false;
        public int maxToques = 0;
        public int golosTotais = 0;
        public int maxGolos = 0;
        public Text golosText;
        public Text maxGolosText;
        public bool marcou = false;
        public Text racioText;
        public float racioBest = 0.0f;
        public Text racioBestText;
        public float racio = 0.0f;

        public void Start()
        {
            qlearning = new QLearning();
            saves = Application.dataPath + "\\SAVES";
            distx = (int)((bola.transform.position.x - jogador.transform.position.x) * 5.0f);
            disty = (int)(bola.transform.position.y - jogador.transform.position.y);
            distParede = 0 - (int)(jogador.transform.position.x);
            toq = 0;
            distxStr = distx.ToString();
            distyStr = disty.ToString();
            distParedeStr = distParede.ToString();
            toqStr = toq.ToString();
            string[] colunas = { "distanciaX", "distanciaY", "distParede", "toques" };
            string[] valores = { distxStr, distyStr, distParedeStr, toqStr };
            state = new State(distx, disty, distParede, dirBola());
            actions = new Action[3];
            actions[0] = new Right(jogador);
            actions[1] = new Left(jogador);
            actions[2] = new DoNothing(jogador);
            xListPlayer = new float[41];
            int i = -1;
            for (float valor=-9.0f; valor <= -1.0f ;valor+= 0.2f)
            {
                i++;
                xListPlayer[i] = valor;
            }
            loadQValueStore(); //para inicializar o qValueStore
            //String query = db.preparaStringInsert("state", colunas, valores);
            //db.Insert(query);
        }

        public void Update()
        {
            if (Input.GetKeyUp("space"))
            {
                saving = true;
                saveQValueStore();
                saving = false;
            }
        }

        public void FixedUpdate()
        {
            if (saving)
                return;

            if ((int) bola.transform.position.y == lastY)
                breakar++;
            else{
                lastY = (int)bola.transform.position.y;
                breakar = 0;
            }

            if (toquesTotais > maxToques)
            {
                maxToques = toquesTotais;
                maxText.text = "Max Toques = " + maxToques;
                /*racioBest = racio;
                racioBestText.text = "Best Racio = " + racioBest;*/
            }

            if (golosTotais > maxGolos)
            {
                maxGolos = golosTotais;
                maxGolosText.text = "Max Golos = " + maxGolos;
            }

            if (Time.time > this.nextUpdateTime)
            {
                this.nextUpdateTime = Time.time + UPDATE_INTERVAL;
                distx = (int)((bola.transform.position.x - jogador.transform.position.x) * 5.0f);
                disty = (int)(bola.transform.position.y - jogador.transform.position.y);
                distParede = 0 - (int)(jogador.transform.position.x);
                toq = toquesTotais;
                state = new State(distx, disty, distParede, dirBola());
                ReinforcementProblem problem = new ReinforcementProblem(state, actions);
                qlearning.qLearning(problem, store, actions, jogador, bola, tocouNoChao, tocouBola, marcou, 0, alpha, gamma, rho, 0.2f);

                if (breakar > 200)
                {
                    restart();
                    if (tocouBola)
                        tocouBola = false;
                    return;
                }

                if (tocouNoChao)
                {
                    tocouNoChao = false;
                    
                    golosText.text = "Golos = " + golosTotais;
                    /*if (toquesTotais == 0)
                        racio = 0.0f;
                    else
                        racio = (float)golosTotais / (float)toquesTotais;
                    racioText.text = "Racio = " + racio;
                    golosTotais = 0;
                    toquesText.text = "Toques = " + toquesTotais;*/
                    toquesTotais = 0;
                    golosTotais = 0;
                    toquesText.text = "Toques = " + toquesTotais;
                    restart();
                    
                }
                if (tocouBola)
                {
                    tocouBola = false;
                    toquesText.text = "Toques = " + toquesTotais;
                    /*if (toquesTotais == 0)
                        racio = 0.0f;
                    else
                        racio = (float)golosTotais / (float)toquesTotais;
                    racioText.text = "Racio = " + racio;*/
                }
                if (marcou)
                {
                    /*if (toquesTotais == 0)
                        racio = 0.0f;
                    else
                        racio = (float)golosTotais / (float)toquesTotais;*/
                    marcou = false;
                    golosText.text = "Golos = " + golosTotais;
                    //racioText.text = "Racio = " + racio;
                }
            }
        }

        public void restart()
        {
            //toquesTotais = 0;
            //golosTotais = 0;
            float randomX = UnityEngine.Random.Range(-1.0f, -9.0f);
            float randomY = UnityEngine.Random.Range(4.0f, 9.0f);
            bola.GetComponent<Rigidbody2D>().position = new Vector2(randomX, randomY);
            bola.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            Vector2 initialForce = new Vector2(100.0f, 300.0f);
            bola.GetComponent<Rigidbody2D>().AddForce(initialForce);
            int randomXJ = UnityEngine.Random.Range(0,38);
            jogador.transform.position = new Vector2(xListPlayer[randomXJ], 0.95f);
        }

        public int velocidadeBola()
        {
            int vel = (int)bola.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (vel < 1)
                vel = 1;
            if (vel > 5)
                vel = 5;
            return vel;
        }

        public int dirBola()
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

        public int saltou()
        {
            if (jogador.transform.position.y < 1) return 0;
            return 1;
        }

        public void saveQValueStore()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(saves + "/qValueStore.dat");
            bf.Serialize(file, store);
            file.Close();
        }
        
        public void loadQValueStore()
        {
            if (File.Exists(saves + "/qValueStore.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(saves + "/qValueStore.dat", FileMode.Open);
                QValueStore load = (QValueStore)bf.Deserialize(file);
                file.Close();
                store = load;
            }
            else
            {
                store = new QValueStore(jogador);
            }
        }
    }
}
