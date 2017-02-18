using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public float lifeSpan = 2f;

    private float bornOnDate;
	// Use this for initialization
	void Start () {
        this.bornOnDate = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - bornOnDate >= lifeSpan)
        {
            Object.Destroy(this.gameObject);
        }
	}
}
