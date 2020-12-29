using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMovement : MonoBehaviour
{
    //public vars so we can play with them in the inspector
    public float movement_scalar;
    public float jump_scalar;
    public float max_speed;


    private Rigidbody2D rb;
    private bool is_jumping = false;
    private float x_movement;
    private bool is_on_ground = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //use this for collecting input and whatever else, *except* physics and movement
    void Update(){
        //jumping
        //we can set the Jump key in the Project Settings for this project
        //the default jump button is space
        if(Input.GetButtonDown("Jump") && is_on_ground){
            is_jumping = true;
        }
    }

    //use FixedUpdate for physics/movement since it is framerate-independent
    void FixedUpdate(){
        //horizontal movement
        x_movement = Input.GetAxis("Horizontal");
        if(rb.velocity.magnitude < max_speed){
            Vector2 movement = new Vector2(x_movement,0);
            //instead of adjusting position, we apply a force to the rigidbody to move it
            rb.AddForce(movement_scalar * movement);
        }
        
        //jumping
        if(is_jumping){
            Vector2 jump_force = new Vector2(0,jump_scalar);
            rb.AddForce(jump_force);
            is_jumping = false;
        }
    }

    //runs when this object enters a collision
    void OnCollisionEnter2D(Collision2D collision){
        if(CollisionIsWithGround(collision)){
            is_on_ground = true;
        }
    }

    //runs when this object exits a collision
    void OnCollisionExit2D(Collision2D collision){
        if(!CollisionIsWithGround(collision)){
            is_on_ground = false;
        }
    }

    //check to see if the collision passed to this method is with the ground
    private bool CollisionIsWithGround(Collision2D collision){
        bool is_with_ground = false;
        foreach(ContactPoint2D c in collision.contacts){
            Vector2 collision_direction_vector = c.point - rb.position;
            if(collision_direction_vector.y < 0){
                //collision happened below the character, so this collision was with the ground
                is_with_ground = true;
            }
        }

        return is_with_ground;
    }
}