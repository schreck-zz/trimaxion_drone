using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throttle
{
    private Dictionary<string, KeyCode> keyMap;
    private Transform controlSphere;
    private Vector3 zero;

    public Throttle(Dictionary<string,KeyCode> keyMap_,Transform controlSphere_)
    {
        keyMap = keyMap_;
        controlSphere = controlSphere_;
        zero = controlSphere_.position;
    }

    public Throttle(Transform controlSphere_)
    {
        keyMap = defaultKeyMap();
        controlSphere = controlSphere_;
        zero = controlSphere_.position;
    }

    public Vector3 value()
    {
        Debug.Log(keyInput() + controlSphere.position - zero);
        return keyInput()+controlSphere.position - zero;
    }

    public void reset()
    {
        zero = controlSphere.position;
    }

    private Dictionary<string,KeyCode> defaultKeyMap()
    {
        keyMap = new Dictionary<string, KeyCode>();
        keyMap.Add("forward", KeyCode.W);
        keyMap.Add("back", KeyCode.S);
        keyMap.Add("left", KeyCode.A);
        keyMap.Add("right", KeyCode.D);
        keyMap.Add("up", KeyCode.R);
        keyMap.Add("down", KeyCode.F);
        return keyMap;
    }

    private Vector3 keyInput()
    {
        Vector3 value = Vector3.zero;
        if (Input.GetKey(keyMap["forward"]))
        {
            value += new Vector3(0f, 0f, .5f);
        }
        if (Input.GetKey(keyMap["back"]))
        {
            value += new Vector3(0f, 0f, -.5f);
        }
        if (Input.GetKey(keyMap["left"]))
        {
            value += new Vector3(-.5f, 0f, 0f);
        }
        if (Input.GetKey(keyMap["right"]))
        {
            value += new Vector3(.5f, 0f, 0f);
        }
        if (Input.GetKey(keyMap["up"]))
        {
            value += new Vector3(0f, .5f, 0f);
        }
        if (Input.GetKey(keyMap["down"]))
        {
            value += new Vector3(0f, -.5f, 0f);
        }
        return value;
    }
}

public class dronecontrol : MonoBehaviour {


    public Transform left_controller;
    public Transform right_controller;

    private float thrust; // thruster strength (newtons)
    private float mass; // drone mass (kg)

    public Vector3 v; // velocity (m/s)
    private Throttle left_throttle;
    private Throttle right_throttle;

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
        left_throttle = new Throttle(left_controller);
        var right_map = new Dictionary<string, KeyCode>();
        right_map.Add("forward", KeyCode.I);
        right_map.Add("back", KeyCode.K);
        right_map.Add("left", KeyCode.J);
        right_map.Add("right", KeyCode.L);
        right_map.Add("up", KeyCode.P);
        right_map.Add("down", KeyCode.Semicolon);
        right_throttle = new Throttle(right_map, right_controller);
        thrust = 1000f; // 1 mega newton
        mass = 100f;
    }

    void FixedUpdate()
    {
    }
    
	
	// Update is called once per frame
	void Update () {

        var right_throttle_position = right_throttle.value();
        var left_throttle_position = left_throttle.value();

        port_rig.localPosition = port_zero + right_throttle_position;
        starboard_rig.localPosition = starboard_zero + left_throttle_position;

        var w = starboard_zero.x - port_zero.x;
        var yaw = Mathf.Atan((right_throttle_position.z - left_throttle_position.z) / w);
        var roll = Mathf.Atan((right_throttle_position.y - left_throttle_position.y) / w);
        var rot = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(roll, Vector3.forward);
        var combined_xlate_throttle = right_throttle_position + left_throttle_position;
        v += (thrust / mass) * combined_xlate_throttle * Time.deltaTime;
        //transform.position += v * Time.deltaTime;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot*transform.rotation, .2f);
    }
}
