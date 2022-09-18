using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    public GameBehaviour scripty;
    public Rigidbody cybertruck;
    public Rigidbody thisBody;
    public float speed = 0.7f;
    public float instCyberSpeed;
    public Vector3 maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.position.y > -2f){
            scripty = GameObject.Find("cybertruck").GetComponent<GameBehaviour>();
            cybertruck = GameObject.Find("cybertruck").GetComponent<Rigidbody>();
            thisBody = gameObject.AddComponent<Rigidbody>();
            thisBody.interpolation = RigidbodyInterpolation.Interpolate;
            speed = Random.Range(0.5f,0.8f);
            instCyberSpeed = cybertruck.velocity.magnitude;
            thisBody.mass = Random.Range(200,600);
            thisBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            maxSpeed = Vector3.zero;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y > -2f){ //check if original prefab, if not, run code
            if(scripty.ready){ //upon game start
                RaycastHit hit;
                Debug.DrawRay(transform.position, Vector3.forward, Color.green, 20f);
                thisBody.AddForce(Vector3.down *  thisBody.mass * 9.81f);
                
                if(Physics.Raycast(transform.position, Vector3.forward, out hit, 6f) && hit.transform.gameObject.tag != "Player"){ //if car in front, slow tf down
                    speed = speed * 0.9f;
                    calcSpeed();
                }else{// just keep swimming, just keep swimming
                    //transform.Translate(Vector3.forward * Time.deltaTime * speed * cybertruck.velocity.magnitude,Space.World);
                    //float rubberBandSpeed = Mathf.Clamp(sigmoid((transform.position - cybertruck.position).magnitude),0,1);
                    //thisBody.velocity = Vector3.forward * Mathf.Clamp(speed - rubberBandSpeed,0,1);
                    calcSpeed();
                }

                if(transform.position.z < (cybertruck.position.z - 12f) || transform.position.z > cybertruck.position.z + 150f){
                    Destroy(gameObject);
                }
    
            }
        }
    }

    float sigmoid(float x){
        return 2f/(1f + Mathf.Exp(-0.3f*x)) - 1f;
    }

    void calcSpeed(){
        Vector3 currSpeed = Vector3.forward * cybertruck.velocity.magnitude * speed;
        if(currSpeed.magnitude > maxSpeed.magnitude){
            thisBody.velocity = currSpeed;
            maxSpeed = currSpeed;
        }else{
            thisBody.velocity = maxSpeed;
        }
    }
}
