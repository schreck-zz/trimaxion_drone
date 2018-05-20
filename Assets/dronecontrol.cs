using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dronecontrol : MonoBehaviour {

    private float thrust; // thruster strength (newtons)
    private float mass; // drone mass (kg)

    public Vector3 v; // velocity (m/s)
    private Vector3 left_throttle_position; // xlate
    private Vector3 right_throttle_position; // xlate

    private Transform port_rig;
    private Vector3 port_zero;
    private Transform starboard_rig;
    private Vector3 starboard_zero;
                                            
    // Use this for initialization
    void Start () {
        v = Vector3.zero;
        port_rig = transform.Find("port rig").gameObject.transform;
        port_zero = port_rig.position;
        starboard_rig = transform.Find("starboard rig").gameObject.transform;
        starboard_zero = starboard_rig.position;
        thrust = 1000f; // 1 mega newton
        mass = 100f;
    }

    void FixedUpdate()
    {
    }
	
	// Update is called once per frame
	void Update () {
        left_throttle_position = Vector3.zero;
        right_throttle_position = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            left_throttle_position += new Vector3(0f, 0f, .5f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            left_throttle_position += new Vector3(0f, 0f, -.5f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            left_throttle_position += new Vector3(-.5f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            left_throttle_position += new Vector3(.5f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.I))
        {
            right_throttle_position += new Vector3(0f, 0f, .5f);
        }
        if (Input.GetKey(KeyCode.K))
        {
            right_throttle_position += new Vector3(0f, 0f, -.5f);
        }
        if (Input.GetKey(KeyCode.J))
        {
            right_throttle_position += new Vector3(-.5f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.L))
        {
            right_throttle_position += new Vector3(.5f, 0f, 0f);
        }
        port_rig.localPosition = port_zero + right_throttle_position;
        starboard_rig.localPosition = starboard_zero + left_throttle_position;

        var w = starboard_zero.x - port_zero.x;
        var yaw = Mathf.Atan((right_throttle_position.z - left_throttle_position.z) / w);
        var roll = Mathf.Atan((right_throttle_position.y - left_throttle_position.y) / w);
        var rot = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(roll, Vector3.forward);
        var combined_xlate_throttle = right_throttle_position + left_throttle_position;
        v += (thrust / mass) * combined_xlate_throttle * Time.deltaTime;
        transform.position += v * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot*transform.rotation, .02f);
    }
}
