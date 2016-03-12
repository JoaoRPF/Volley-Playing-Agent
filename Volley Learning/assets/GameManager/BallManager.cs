using UnityEngine;
using System.Collections;
using Assets.GameManager;

public class BallManager : MonoBehaviour {

    public float constantSpeed;
    public float smoothingFactor;
    public GameObject gm;
    private GameManager gameManager;
    public Vector3 ballPosition;
    public Vector2 initialForce;
    public float ysn;
    public float xsn;
    // Use this for initialization
    void Start () {
        constantSpeed = 5.0f;
        smoothingFactor = 1.0f;
        gameManager = gm.GetComponent<GameManager>();
        initialForce = new Vector2(100.0f, 300.0f);
        ballPosition = transform.position;
        GetComponent<Rigidbody2D>().AddForce(initialForce);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Equals("Ground_3"))
        {
            gameManager.tocouNoChao = true;
            //gameManager.toquesTotais = 0;
            //gameManager.golosTotais = 0;
            //Debug.Log("bati no ground");
        }
        if (collision.collider.name.Equals("Player"))
        {
            //Debug.Log("bati no jogador");
            Vector2 velocity = gameManager.bola.GetComponent<Rigidbody2D>().velocity.normalized;
            float vecx = gameManager.bola.GetComponent<Rigidbody2D>().velocity.x;
            if (System.Math.Abs(velocity.y) < 0.25f)
            {
                ysn = velocity.y + 0.25f;
                xsn = Mathf.Sqrt(1.0f - (ysn * ysn));
                Vector2 vecSolucao;
                /*if (collision.transform.position.x < gameManager.bola.transform.position.x)
                    vecSolucao = gameManager.bola.GetComponent<Rigidbody2D>().velocity.magnitude * new Vector2(xsn, ysn);
                else*/
                vecSolucao = gameManager.bola.GetComponent<Rigidbody2D>().velocity.magnitude * new Vector2(xsn, ysn);
                gameManager.bola.GetComponent<Rigidbody2D>().velocity = vecSolucao;
            }
            Vector2 vec = gameManager.bola.GetComponent<Rigidbody2D>().velocity;
            if (vec.y < 0.0f)
            {
                gameManager.bola.GetComponent<Rigidbody2D>().velocity = new Vector2(vec.x, -vec.y);
            }
            if (collision.transform.position.x < gameManager.jogador.transform.position.x)
            {
                Vector2 aux = gameManager.bola.GetComponent<Rigidbody2D>().velocity;
                //gameManager.bola.GetComponent<Rigidbody2D>().velocity = new Vector2(aux.x, aux.y);
                
                if (vecx > 0)
                    gameManager.bola.GetComponent<Rigidbody2D>().velocity = new Vector2(-aux.x, aux.y);
                    
            }
            if (collision.transform.position.x > gameManager.jogador.transform.position.x)
            {
                Vector2 aux = gameManager.bola.GetComponent<Rigidbody2D>().velocity;
                if (vecx < 0)
                    gameManager.bola.GetComponent<Rigidbody2D>().velocity = new Vector2(-aux.x, aux.y);
            }
            gameManager.toquesTotais++;
            gameManager.tocouBola = true;
        }
        if (collision.collider.name.Equals("Baliza"))
        {
            gameManager.golosTotais++;
            gameManager.marcou = true;
        }
    }
}
