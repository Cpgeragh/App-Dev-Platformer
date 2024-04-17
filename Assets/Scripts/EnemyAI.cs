
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class EnemyAI : MonoBehaviour
{

    // Refernece to waypoints
   public List<Transform> points;
   // The int value for next point index
   public int nextID = 0;
   // ID value manipulator
   int iDChangeValue = 1;
   // Movement Speed
   public float speed = 2;


    private void Reset()
    {

        Init();

    }

    void Init()
    {

        // Box Collider trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        // Create Root object
        GameObject root = new GameObject(name + "_Root");

        // Reset Position of Root to Enemy Object
        root.transform.position = transform.position;
        
        // Set Enemy Object as child of Root
        transform.SetParent(root.transform);

        // Create Waypoints Object
        GameObject waypoints = new GameObject("Waypoints");

        // Reset Waypoints position to Root
        // Make Waypoints Object child of Root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        // Create to points and reset their position to Waypooiints object
        // Make two points children of Waypoint object
        // Point1
        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;
        // Point2
        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;
       
        // Init points list thrn add points to it
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);

    }

    private void Update()
    {

        MoveToNextPoint();

    }

    void MoveToNextPoint()
    {

        // Get next Point Transform
        Transform goalPoint = points[nextID];

        // Flip Enemy transform to into point's direction
        if(goalPoint.transform.position.x > transform.position.x)
        {

            transform.localScale = new Vector3(1, 1 , 1);

        }

        else
        {

            transform.localScale = new Vector3(-1, 1 , 1);

        }

        // Move Enemy towards Goal Point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed*Time.deltaTime);

        // Check distance between Enemy and Goal Point to trigger next point
        if(Vector2.Distance(transform.position, goalPoint.position) < 1f)
        {

            // Check if end of line reached (make the change -1)
            if(nextID == points.Count -1)
            {

                iDChangeValue = -1;

            }

            // Check is start of line reached (make the change +1)
            if(nextID == 0)
            {

                iDChangeValue = 1;

            }

            // Apply the change on the nextID
            nextID += iDChangeValue;

            // nextId = nextId + idChangeValue

        }

    }

    private void OnTriggerEnter2D(Collider2D collison)
    {

        if(collison.tag == "PlayerPig")
        {

            Debug.Log($"{name} Triggered");

            FindObjectOfType<HeartCount>().LoseHeart();

        }
        

    }

}
