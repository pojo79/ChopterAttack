using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {

    public GameObject player;
    public float health;
    protected Quaternion forwardRotation;

    // Use this for initialization
    void Start () {
        EnemySpecificStart();
  

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (health <= 0)
        {
            DoDeathSequence();
            Object.Destroy(this.gameObject);
        }
        EnemySpecificUpdate();
	}

    private void FixedUpdate()
    {
        EnemySpecificFixedUpdate();
    }


    public void DoDamage(float damage)
    {
        Debug.Log("Enemy Taking Damage " + damage);
        health -= damage;
    }


    protected abstract void EnemySpecificStart();
    protected abstract void EnemySpecificUpdate();
    protected abstract void DoDeathSequence();
    protected abstract void EnemySpecificFixedUpdate();
}
