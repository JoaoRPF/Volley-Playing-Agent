using UnityEngine;
using System.Collections;
using Assets.GameManager;

public class BallPhysics : MonoBehaviour {

    public GameManager gm;
    
    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //rb.AddForce();
    }

    void OnCollisionEnter2D(Collision2D collision2D) {
       /* Vector2 normal = collision2D.contacts[0].normal;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float magnitude = rb.velocity.sqrMagnitude;
        if (magnitude > 10)
            magnitude = 10;
        Debug.Log(magnitude);
        rb.AddForce(normal*magnitude, ForceMode2D.Impulse);
        if (collision2D.collider.name.Equals("Player"))
            gm.toquesTotais++;
        if (collision2D.collider.name.Equals("Rede") && rb.transform.position.y > 3.0f)
            gm.toquesTotais = 0; */
    }
	
	void Update () {
	
	}
}
