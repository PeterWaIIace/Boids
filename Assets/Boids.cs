using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 newPosition;
    public float velocity;
    public float AOV = 180.0f;
    public float FOV = 0.5f; // distance of sight

    // boundaries Constraint 
    public float upperConstraint = 0.0f;
    public float lowerConstraint = 0.0f;
    public float leftConstraint = 0.0f;
    public float rightConstraint = 0.0f;
    public float buffer = 2.0f; // set this so the spaceship disappears offscreen before re-appearing on other side
    public float distanceZ = -10.0f; 
    public Rigidbody2D rb2D;

    void Awake(){
        buffer = 2.0f;
        distanceZ=Camera.main.transform.position.z;
        lowerConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, -distanceZ) ).y;
        upperConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, Screen.height, -distanceZ) ).y;
        leftConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, -distanceZ) ).x;
        rightConstraint = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, 0.0f , -distanceZ) ).x;
    }

    void Start()
    {
        // rb2D = gameObject.AddComponent<Rigidbody2D>();
        rb2D = gameObject.AddComponent<Rigidbody2D>();
        if(!rb2D){
            rb2D = gameObject.GetComponent<Rigidbody2D>();
        }
        System.Random random = new System.Random();
        
        float directionAngle = System.Convert.ToSingle(random.NextDouble()*System.Math.PI*2);
        float x = System.Convert.ToSingle(System.Math.Cos(directionAngle)/10);
        float y = System.Convert.ToSingle(System.Math.Sin(directionAngle)/10);

        newPosition = new Vector2(x,y);
        velocity = UnityEngine.Random.Range(1.0f, 2.5f);
        FOV = 1.5f;
        AOV = 45.0f;
   
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.DrawLine(new Vector3(rb2D.position.x,rb2D.position.y), new Vector2(rb2D.position.x,rb2D.position.y)+3*newPosition, Color.green,2f);
        rb2D.MovePosition(rb2D.position+newPosition);
        checkBoundaries();
   
    }
    void FixedUpdate()
    {
        boids(2f);
    }

    void checkBoundaries(){
        // border lines
        // Debug.DrawLine(new Vector3(-1000,lowerConstraint - buffer), new Vector2(1000,lowerConstraint - buffer),Color.red,10f);
        // Debug.DrawLine(new Vector3(-1000,upperConstraint + buffer), new Vector2(1000,upperConstraint + buffer),Color.blue,10f);
        // Debug.DrawLine(new Vector3(leftConstraint - buffer,-1000), new Vector2(leftConstraint - buffer,1000),Color.red,10f);
        // Debug.DrawLine(new Vector3(rightConstraint + buffer,-1000), new Vector2(rightConstraint + buffer,1000),Color.blue,10f);
        
        if (rb2D.position.y < lowerConstraint - buffer) { // ship is past world-space view / off screen
            Vector2 move = new Vector2(rb2D.position.x,upperConstraint+buffer);
            rb2D.MovePosition(move);  // move ship to opposite side
        }

        else if (rb2D.position.y  > upperConstraint + buffer)
        {
            Vector2 move = new Vector2(rb2D.position.x,lowerConstraint-buffer);
            rb2D.MovePosition(move);
        }

        else if (rb2D.position.x  < leftConstraint - buffer)
        {
            Vector2 move = new Vector2(rightConstraint+buffer,rb2D.position.y);
            rb2D.MovePosition(move);
        }

        else if (rb2D.position.x  > rightConstraint + buffer)
        {
            Vector2 move = new Vector2(leftConstraint-buffer,rb2D.position.y);
            rb2D.MovePosition(move);
        }

    }

    void boids(float range = 1){
        var dots = GameObject.FindGameObjectsWithTag("Boids");

        Vector2 steer  = new Vector2(0,0);
        Vector2 sep  = separation(dots,range)*0.5f;
        steer += sep;
        Vector2 al = alligment(dots,range)*1f;
        steer += al;
        Vector2 coh = cohesion(dots,range)*1f;
        steer += coh;
        
        if(steer.magnitude > 0){
            steer = (steer/steer.magnitude)/10; // Reynolds steering = desired - velocity - 100 for limiting speed
        }
        else{
            steer = Vector2.zero;
        }
        
        float mag = newPosition.magnitude;
        newPosition = (steer/(mag*20)+newPosition/mag*velocity)/20;
    
    }

    Vector2 cohesion(GameObject[] objs,float range = 1f,float angleOfView = 180.0f){
        int countObjInFOV = 0;
        Vector2 curr = new Vector2(rb2D.transform.position.x,rb2D.transform.position.y);
        Vector2 steer = new Vector2(0,0);

        foreach(var obj in objs){
            if (obj != gameObject){
                Vector2 other = new Vector2(obj.transform.position.x, obj.transform.position.y);
                Vector2 tmp = other - curr;
                
                float distance = Vector2.Distance(curr,other);
                float angle = Vector2.Angle(other, curr);
                
                if(range > distance && System.Math.Abs(angle) < angleOfView){
                    // steer += tmp/(distance);
                    steer += tmp;
                    countObjInFOV+=1;
                }
            }
        }
        if(countObjInFOV > 0){
            steer /=countObjInFOV;
        }
        // Debug.DrawLine(new Vector3(rb2D.position.x,rb2D.position.y), new Vector2(rb2D.position.x,rb2D.position.y)+steer/10, Color.red,1f);

        return steer;
    }
    Vector2 alligment(GameObject[] objs,float range = 1f,float angleOfView = 180.0f){
    
        int countObjInFOV = 0;
        Vector2 curr = new Vector2(rb2D.transform.position.x,rb2D.transform.position.y);
        Vector2 steer = new Vector2(0,0);

        foreach(var obj in objs){
            if (obj != gameObject){
                Vector2 other = new Vector2(obj.transform.position.x, obj.transform.position.y);
            
                float distance = Vector2.Distance(curr,other);
                float angle = Vector2.Angle(other, curr);
                
                if(range > distance && System.Math.Abs(angle) < angleOfView){

                    steer += (obj.GetComponent<Boids>().newPosition)*(angle/distance);
    
                    countObjInFOV+=1;
                    
                }
            }
        }
        if(countObjInFOV > 0){
            steer /= countObjInFOV;
        }
        return steer;
    }
    
    Vector2 separation(GameObject[] objs,float range = 1f,float angleOfView = 180.0f){ // to make it trully separate from obj, the rb2D should also be passed
    
        int countObjInFOV = 0;
        Vector2 curr = new Vector2(rb2D.transform.position.x,rb2D.transform.position.y);
        Vector2 steer = new Vector2(0,0);

        foreach(var obj in objs){
            if ( obj != gameObject){
                
                Vector2 other = new Vector2(obj.transform.position.x, obj.transform.position.y);
                Vector2 tmp = curr - other;
                
                float distance = Vector2.Distance(curr,other);
                float angle = Vector2.Angle(other, curr);

                if(range > distance && System.Math.Abs(angle) < angleOfView){

                    steer += (tmp/tmp.magnitude)/(distance);
    
                    countObjInFOV+=1;
                    
                }
            }
        }
        if(countObjInFOV > 0){
            steer = steer/countObjInFOV;
        }
       
        return steer;
    }
}