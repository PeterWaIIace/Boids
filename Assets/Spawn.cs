using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    public float directionAngle = 0.0f;
    public float upperConstraint = 0.0f;
    public float lowerConstraint = 0.0f;
    public float leftConstraint = 0.0f;
    public float rightConstraint = 0.0f;
    public float distanceZ = 10.0f;
    public GameObject obj;
    void Awake(){
        distanceZ=Camera.main.transform.position.z;
        lowerConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, distanceZ) ).y;
        upperConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, Screen.height , distanceZ) ).y;
        leftConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, distanceZ) ).x;
        rightConstraint = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, 0.0f , distanceZ) ).x;
    }
    void Start()
    {       
        int NSpawnObj = 50;
        obj = GameObject.Find("Boids");
        var random = new System.Random();
        for(int nSpawn = 0; nSpawn < NSpawnObj; nSpawn++){
            float x = System.Convert.ToSingle(random.NextDouble()*upperConstraint);
            float y = System.Convert.ToSingle(random.NextDouble()*rightConstraint);
            // Debug.Log($"random.NextDouble() {random.NextDouble()}");
            Instantiate(obj,new Vector2(x,y),Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
