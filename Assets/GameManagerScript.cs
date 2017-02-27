using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    public bool gameOver;
    
	// Use this for initialization
	void Start () {
        gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectsWithTag("Satellite").Length == 0)
        {
            SceneManager.LoadScene("EndGame");
        }
        if (gameOver)
        {
            if(Input.anyKeyDown){
                SceneManager.LoadScene("Opening");
            }
        }
    }
}
