using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliControlScript : MonoBehaviour {

    public float speed = 75;
    public float rotate_speed = 10f;
    public float turn_speed = 15f;
    public GameObject rocket;
    public AudioSource rocketLaunchSound;
    public AudioSource fly_sound;
    public float rocket_speed = 10;
    public float rocket_down_force = 5000f;
    public float flyHeight = 75f;
    public float maxHeight = 200f;
    public float pitch_change = 3f;
    public float max_pitch = 15f;
    public float MaxHealth = 100f;

    private float currentHealth;
    private bool airborne = false;
    private CharacterController charControl;
    private Rigidbody rb;
    private float forwardMove;
    private float strafeMove;
    private float rotationMove;
    private Terrain levelTerrain;
    private Animator animator;
    private ParticleSystem smokeParticles;

    // Use this for initialization
    private void Start()
    {
        this.levelTerrain = Terrain.activeTerrain;
        this.charControl = this.GetComponent<CharacterController>();
        this.rb = this.GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        this.currentHealth = MaxHealth;
        this.smokeParticles = GetComponent<ParticleSystem>();
        rb.isKinematic = false;
    }

    private void Update()
    {
        if(currentHealth <= 0)
        {
            rb.isKinematic = true;
            GameManagerScript gm = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            gm.gameOver = true;
            return;
        }

        rotationMove = Input.GetAxis("Horizontal") * turn_speed;
        forwardMove = Input.GetAxis("Vertical") * speed;
        strafeMove = 0f;

        if (Input.GetButtonDown("Fire1"))
        {

            //Vector3 down30 = new Vector3(-45, 0, 0);
            Quaternion down30 = Quaternion.EulerAngles(-45, 0, 0);
            GameObject newRocket = Instantiate(rocket, transform.position+transform.forward*15, transform.rotation );
            newRocket.GetComponent<Rigidbody>().AddForce(transform.forward * rocket_speed, ForceMode.Impulse);
            newRocket.GetComponent<Rigidbody>().AddForce(Vector3.down*rocket_down_force);
            rocketLaunchSound.Play();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            strafeMove -= rotate_speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            strafeMove += rotate_speed;
        }

        //TODO: implement pitch on move.
        //AdjustPitch();
        animator.SetBool("flying", true);
        PlayFlySound();
        AdjustSmokeEmission();
    }

 
    private void AdjustPitch()
    {
        float current_pitch = transform.rotation.x;
        float pitch = 0f;
        if(forwardMove > 0)
        {
            pitch += pitch_change;
            Debug.Log("current pitch = " + current_pitch);
            if(current_pitch+pitch_change > max_pitch)
            {
                //pitch = max_pitch - current_pitch;
                pitch = 0;
            }
        }
        else if(forwardMove < 0)
        {
            Debug.Log("current pitch = " + current_pitch);
            pitch -= pitch_change;
            if (current_pitch - pitch_change < -max_pitch)
            {
                pitch = -max_pitch + current_pitch;
            }
        }
        else
        {
            if(current_pitch > 0)
            {
                pitch -= pitch_change;
            }
            if (current_pitch > 0)
            {
                pitch -= pitch_change;
            }
        }

        Debug.Log("pitch = " + pitch);
        transform.Rotate(new Vector3(pitch, 0, 0));
    }
    
    private void PlayFlySound()
    {
        if (!fly_sound.isPlaying)
        {
            fly_sound.Play();
        }
    }

    private void FixedUpdate()
    {
        Move();
        AdjustAltitude();
        Turn();
    }

    private void Move()
    {
        //float terrainHeight = levelTerrain.SampleHeight(transform.position);
        Vector3 move = transform.rotation * new Vector3(strafeMove, 0, forwardMove) * Time.deltaTime;
        //charControl.Move(move);
        this.rb.AddForce(move);
    }

    private void AdjustAltitude()
    {
        float newHeight = flyHeight;
        float terrainHeight = levelTerrain.SampleHeight(transform.position);
        if(terrainHeight >= maxHeight)
        {
            newHeight = maxHeight;
        }
        else
        {
            newHeight = terrainHeight + flyHeight;
        }
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private void Turn()
    {
        Vector3 rotation = new Vector3(0f, rotationMove, 0f) * Time.deltaTime;
        this.transform.Rotate(rotation * Time.deltaTime);
    }

    public void DoDamage(float damage)
    {
        Debug.Log("Taking " + damage + " damage");
        currentHealth -= damage;
    }

    public float GetHealth()
    {
        return this.currentHealth;
    }

    private void AdjustSmokeEmission()
    {
        Color smokeColor = Color.gray;
        float smokeRate = 70;
        if (currentHealth <= 80 && currentHealth > 60)
        {
            smokeColor = new Color(.35f, .35f, .35f, 1f);
        }
        if (currentHealth <= 60 && currentHealth > 30)
        {
            smokeColor = new Color(.25f, .25f, .25f, 1f);
            smokeRate = 100;
        }
        if (currentHealth <= 30)
        {
            smokeColor = new Color(.1f, .1f, .1f, 1f);
            smokeRate = 375;
        }

        ParticleSystem.EmissionModule emit = smokeParticles.emission;
        if (currentHealth <= 80)
        {
            ParticleSystem.MainModule main = smokeParticles.main;
            main.startColor = smokeColor;
            emit.rateOverTime = smokeRate;
            emit.enabled = true;
            
        }
        else
        {
            emit.enabled = false;
        }

    }

}
