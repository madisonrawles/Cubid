//This script is attached to the SceneContainer container object so that it can operate on the entire scene.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContainer : MonoBehaviour
{
    private GameObject cam;
    private GameObject player;
    private Vector3[] gravity_angle_list = {new Vector3(0,-9.8f,0),new Vector3(9.8f,0,0),new Vector3(0,9.8f,0), new Vector3(-9.8f,0,0)};
    private Vector3 start_camera_angle;
    private Vector3 end_camera_angle;
    private Vector3 camera_rotation;
    private float camera_rotation_start_time;
    private bool is_rotating;
    private double input_wait_time = 0.5;

    
    public int angle_index = 0;
    public float camera_rotation_time = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        camera_rotation = cam.transform.eulerAngles;
        player = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //is_rotating = Mathf.Round(cam.transform.eulerAngles.z) != Mathf.Round(end_camera_angle.z % 360);
        is_rotating = Time.time - camera_rotation_start_time < input_wait_time;

        //check for is_rotating before allowing any inputs
        if(Input.GetButtonDown("RotateClockwise") && !is_rotating)
        {
            Debug.Log(Time.time - camera_rotation_start_time);
            Debug.Log("2: " + camera_rotation_time);
            RotateClockwise();
        }
        else if(Input.GetButtonDown("RotateCounterClockwise") && !is_rotating)
        {
            RotateCounterClockwise();
        }

        

        if(is_rotating)
        {
            //interpolate camera rotation
            float fraction_complete = (Time.time - camera_rotation_start_time) / camera_rotation_time;
            cam.transform.eulerAngles = Vector3.Slerp(start_camera_angle,end_camera_angle,fraction_complete);
        }   
        
    }



    void RotateClockwise()
    {
        camera_rotation = cam.transform.eulerAngles;
        start_camera_angle = camera_rotation;
        end_camera_angle = camera_rotation + new Vector3(0,0,90);
        camera_rotation_start_time = Time.time;
        player.transform.eulerAngles = camera_rotation + new Vector3(0,0,90);
        
        //update gravity to match camera angles
        angle_index = angle_index < gravity_angle_list.Length - 1 ? angle_index + 1 : 0;
        player.GetComponent<basicMovement>().movement_index = angle_index;
        Physics2D.gravity = gravity_angle_list[angle_index];

        
    }

    //rotate camera by 90 degrees counterclockwise
    void RotateCounterClockwise()
    {
        camera_rotation = cam.transform.eulerAngles;
        start_camera_angle = camera_rotation;
        end_camera_angle = camera_rotation + new Vector3(0,0,-90);
        camera_rotation_start_time = Time.time;
        player.transform.eulerAngles = camera_rotation + new Vector3(0,0,-90);

        //update gravity to match camera angles
        angle_index = angle_index > 0 ? angle_index - 1 : gravity_angle_list.Length - 1;
        player.GetComponent<basicMovement>().movement_index = angle_index;
        Physics2D.gravity = gravity_angle_list[angle_index];
    }
}
