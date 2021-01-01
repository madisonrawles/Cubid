using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicMovement : MonoBehaviour
{
    //public vars so we can play with them in the inspector and/or edit them from outside scripts
    public float movement_scalar;
    public float jump_scalar;
    public float max_speed;
    public int movement_index; 


    private Rigidbody2D rb;
    private bool is_jumping = false;
    private float x_movement;
    private bool is_on_ground = true;
    private Vector2[] movement_angle_list = {new Vector2(1,0),new Vector2(0,1),new Vector2(-1,0), new Vector2(0,-1)};
    


    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    //use this for collecting input and whatever else, *except* physics and movement
    void Update()
    {
        //jumping
        //we can set the Jump key in the Project Settings for this project
        //the default jump button is space
        if(Input.GetButtonDown("Jump") && is_on_ground)
        {
            is_jumping = true;
        }

        x_movement = Input.GetAxis("Horizontal");
    }

    //use FixedUpdate for physics/movement since it is framerate-independent
    void FixedUpdate()
    {
        //int gravityIndex = 
        //horizontal movement
        
        if(rb.velocity.magnitude < max_speed)
        {
            Vector2 movement = x_movement * movement_angle_list[movement_index];
            //instead of adjusting position, we apply a force to the rigidbody to move it
            rb.AddForce(5.0f*movement_scalar * movement);
        }
        
        //jumping
        if(is_jumping)
        {
            int jump_index = movement_index < movement_angle_list.Length - 1 ? movement_index + 1 : 0;
            Vector2 jump_angle = movement_angle_list[jump_index];
            Vector2 jump_force = jump_scalar * jump_angle;
            rb.AddForce(jump_force);
            is_jumping = false;
        }
    }

    //runs when this object enters a collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(CollisionIsWithGround(collision))
        {
            is_on_ground = true;
        }
    }

    //runs when this object exits a collision
    void OnCollisionExit2D(Collision2D collision)
    {
        if(!CollisionIsWithGround(collision))
        {
            is_on_ground = false;
        }
    }

    //check to see if the collision passed to this method is with the ground
    private bool CollisionIsWithGround(Collision2D collision)
    {
        bool is_with_ground = false;
        /*foreach(ContactPoint2D c in collision.contacts){
            Vector2 collision_direction_vector = c.point - rb.position;
            if(collision_direction_vector.y < 0){
                //collision happened below the character, so this collision was with the ground
                is_with_ground = true;
            }
        }*/

        //alternative method to check if one can jump
        //the tolerance is 0.01, which can be changed
        if (Math.Abs(rb.velocity.y) < 0.01)
        {
            is_with_ground = true;

        }

        return is_with_ground;
    }
}