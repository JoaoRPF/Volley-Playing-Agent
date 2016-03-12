using UnityEngine;
using System.Collections;
using Assets.GameManager;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    public float maxSpeed = 2.0f;
    public float ALTURA = 1.3f;
    public List<State> states;
    public int toques = 0;
    private int saltou = 0;
    private Vector3 playerPosition;
	
    // Update is called once per frame
	void FixedUpdate () {

        float move = Input.GetAxis("Horizontal");
        playerPosition = transform.position;

        if (move > 0)
        {
            if (playerPosition.x < -1) {
                playerPosition.x += 0.2f;
                transform.position = playerPosition;
            }
        }
        if (move < 0)
        {
            if (playerPosition.x > -9.0f) {
                playerPosition.x -= 0.2f;
                transform.position = playerPosition;
            }
        }
	}
}
