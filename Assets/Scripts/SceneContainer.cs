//This script is attached to the SceneContainer container object so that it can operate on the entire scene.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneContainer : MonoBehaviour
{
    private GameObject cam;
    private GameObject player;
    private Vector3[] gravity_angle_list = {new Vector3(0,-9.8f,0),new Vector3(9.8f,0,0),new Vector3(0,9.8f,0), new Vector3(-9.8f,0,0)};

    public int angle_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        player = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("RotateClockwise"))
        {
            RotateClockwise();
        }
        else if(Input.GetButtonDown("RotateCounterClockwise"))
        {
            RotateCounterClockwise();
        }
    }

    void RotateClockwise()
    {
        //rotate camera by 90 degrees clockwise
        Vector3 rotation = cam.transform.eulerAngles;
        rotation.z += 90;
        cam.transform.eulerAngles = rotation;
        player.transform.eulerAngles = rotation;
        //update gravity to match camera angles
        angle_index = angle_index < gravity_angle_list.Length - 1 ? angle_index + 1 : 0;
        player.GetComponent<basicMovement>().movement_index = angle_index;
        Physics2D.gravity = gravity_angle_list[angle_index];
    }

    void RotateCounterClockwise()
    {
        //rotate camera by 90 degrees counterclockwise
        Vector3 rotation = cam.transform.eulerAngles;
        rotation.z -= 90;
        cam.transform.eulerAngles = rotation;
        player.transform.eulerAngles = rotation;
        //update gravity to match camera angles
        angle_index = angle_index > 0 ? angle_index - 1 : gravity_angle_list.Length - 1;
        player.GetComponent<basicMovement>().movement_index = angle_index;
        Physics2D.gravity = gravity_angle_list[angle_index];
    }
}
