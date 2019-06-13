using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float moveSpeed = 1f;
    public float detectionDistance =  2f;
    public int turnChangeRate = 90;



    [Header("Eyes")]
    [Header("Wall Detectors")]

    public GameObject eyeLeft;
    public GameObject eyeMiddle;
    public GameObject eyeRight;
    [Header("RightSide")]
    public GameObject right90;
    public GameObject right60;
    public GameObject right30;

    [Header("LeftSide")]
    public GameObject left90;
    public GameObject left60;
    public GameObject left30;

    public float leftEyeDist;
    public float rightEyeDist;

    public GameObject leftCarDetector;
    public GameObject rightCarDetector;

    public float leftCarDist;
    public float rightCarDist;


    public bool eyeRightHit;
    public bool eyeLeftHit;


    [SerializeField]
    private int rightHits;
    [SerializeField]
    private int leftHits;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        bool slowMovement = false;
        DrawDetectionRays();
        DetectCars();

        DetectWalls(false);
        if(leftCarDetector != null && rightCarDetector != null)
        {
            slowMovement = true;

        }
       

        if (eyeMiddle != null)
        {
            slowMovement = true;
        }
        if (eyeRightHit && eyeLeftHit)
        {

                if (rightHits == leftHits)
                {
                    if (rightEyeDist < leftEyeDist)
                    {
                        AdjustRight(true);
                    }
                    else
                    {
                        AdjustLeft(true);
                    }
                }
                else if (rightHits < leftHits)
                {
                    AdjustLeft(true);
                }
                else
                {
                    AdjustRight(true);
                }

            

        }
        else if(eyeMiddle != null)
        {
            //Use improved turning
            if(eyeRight != null)
            {
                AdjustRight(true);
            }
            else
            {
                AdjustLeft(true);
            }
        }
        else if(eyeRightHit)
        {
            AdjustRight(false);
        }
        else if(eyeLeftHit)
        {
            AdjustLeft(false);
        }

        MoveForward(slowMovement);
        
    }

    void AdjustRight(bool fasterTurning)
    {

        if (_rb.angularVelocity < 0)
        {
            _rb.angularVelocity = 0;
        }
        _rb.AddTorque(turnChangeRate * Time.fixedDeltaTime);


    }
    void AdjustLeft(bool fasterTurning)
    {
        if(_rb.angularVelocity > 0)
        {
            _rb.angularVelocity = 0;
        }
        _rb.AddTorque(-turnChangeRate * Time.fixedDeltaTime);

    }



    
    /// <summary>
    /// Draws basic Rays to represent the different angles
    /// </summary>
    void DrawDetectionRays()
    {
        //Detectors
        Debug.DrawRay(transform.position, transform.up * detectionDistance * .5f, Color.black);
        Debug.DrawRay(transform.position + transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 85) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 85) * Mathf.Deg2Rad)) * detectionDistance * 1.25f, Color.black);
        Debug.DrawRay(transform.position - transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad)) * detectionDistance * 1.25f, Color.black);
        //30 Degrees
        Debug.DrawRay(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z +30) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z +30) * Mathf.Deg2Rad)) * detectionDistance, Color.black);
        Debug.DrawRay(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z +150) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z +150) * Mathf.Deg2Rad)) * detectionDistance, Color.black);

        //60 Degrees
        Debug.DrawRay(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z +60) * Mathf.Deg2Rad),Mathf.Sin((transform.rotation.eulerAngles.z +60 )* Mathf.Deg2Rad)) * detectionDistance, Color.black);
        Debug.DrawRay(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z +120) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z +120) * Mathf.Deg2Rad)) * detectionDistance, Color.black);
        //90 Degrees
        Debug.DrawRay(transform.position, transform.right * detectionDistance, Color.black);
        Debug.DrawRay(transform.position, -transform.right * detectionDistance, Color.black);
    }


    /// <summary>
    /// Other Vehicle Detection
    /// </summary>
    void DetectCars()
    {

        RaycastHit2D [] hits;
        hits = Physics2D.RaycastAll(transform.position - transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad),
           Mathf.Sin((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad)), detectionDistance /2, LayerMask.GetMask("Computer"));
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider != GetComponent<Collider2D>())
            {
                leftCarDetector = hit.collider != null ? hit.collider.gameObject : null;
                if (leftCarDetector != null)
                {
                    leftCarDist = hit.distance;
                    break;
                }
            }
           
        }
        if (hits.Length == 1)
        {
            leftCarDetector = null;
        }


        hits = Physics2D.RaycastAll(transform.position - transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 85) * Mathf.Deg2Rad),
           Mathf.Sin((transform.rotation.eulerAngles.z +85) * Mathf.Deg2Rad)), detectionDistance / 2, LayerMask.GetMask("Computer"));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != GetComponent<Collider2D>())
            {
                rightCarDetector = hit.collider != null ? hit.collider.gameObject : null;
                if (rightCarDetector != null)
                {
                    rightCarDist = hit.distance;
                    break;
                }
            }

        }
        if (hits.Length == 1)
        {
            rightCarDetector = null;
        }
    }


    /// <summary>
    /// Builds fans 
    /// </summary>
    /// <param name="reduceRadius"></param>
    void DetectWalls(bool reduceRadius)
    {
        eyeLeftHit = false;
        eyeRightHit = false;
        rightHits = 0;
        leftHits = 0;
        float distance = detectionDistance;

        RaycastHit2D hit;
        //LeftEye
        hit = Physics2D.Raycast(transform.position - transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad),
            Mathf.Sin((transform.rotation.eulerAngles.z + 95) * Mathf.Deg2Rad)) , detectionDistance * 1.25f,LayerMask.GetMask("Wall"));
        eyeLeft = hit.collider != null ? hit.collider.gameObject : null;
        if(eyeLeft != null)
        {
            leftEyeDist = hit.distance;
        }
        int numberOfHits = 0;
        float totalDistance = 0f;
        for (int i = 0; i < 20; i++)
        {
            hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 90 + i) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + 75 + i) * Mathf.Deg2Rad)), detectionDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                eyeLeftHit = true;
                numberOfHits++;
                totalDistance += hit.distance;
            }
        }
        if (numberOfHits > 0)
        {
            leftEyeDist = totalDistance / numberOfHits;
        }
        else
        {
            leftEyeDist = 0;
        }
        //RightEye
        hit = Physics2D.Raycast(transform.position + transform.right / 2, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 85) * Mathf.Deg2Rad),
            Mathf.Sin((transform.rotation.eulerAngles.z + 85) * Mathf.Deg2Rad)) , detectionDistance * 1.25f, LayerMask.GetMask("Wall"));
        eyeRight = hit.collider != null ? hit.collider.gameObject : null;
        if (eyeRight != null)
        {
            rightEyeDist = hit.distance;
        }
        numberOfHits = 0;
        totalDistance = 0f;
        for (int i = 0; i < 20; i++)
        {
            hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z +75+ i) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z +75 + i) * Mathf.Deg2Rad)), detectionDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                eyeRightHit = true;
                numberOfHits++;
                totalDistance += hit.distance;
            }
        }
        if (numberOfHits > 0)
        {
            rightEyeDist = totalDistance / numberOfHits;
        }
        else
        {
            rightEyeDist = 0;
        }
        //MiddleEye
        hit = Physics2D.Raycast(transform.position, transform.up, detectionDistance * .5f, LayerMask.GetMask("Wall"));
        eyeMiddle = hit.collider != null ? hit.collider.gameObject : null;

        
        ////RightSide
        for(int i = 0;i<75;i++)
        {
            hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + i) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + i) * Mathf.Deg2Rad)), detectionDistance, LayerMask.GetMask("Wall"));
            if(hit.collider != null)
            {
                rightHits++;
            }
        }





        ////LeftSide
        for(int i = 0;i<75;i++)
        {
            hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos((transform.rotation.eulerAngles.z + 180 - i) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z +180 - i) * Mathf.Deg2Rad)), detectionDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                leftHits++;
            }
        }

    }

    float ForwardDistance()
    {
        return GetHitDistance(transform.up);
    }

    float RightDistance()
    {
        float distance = GetHitDistance(-transform.right);
        Debug.Log("right Distance " + distance);

        return distance;
    }

    float LeftDistance()
    {
        float distance = GetHitDistance(transform.right);
        Debug.Log("Left Distance " + distance);

        return distance;
    }
    
    float GetHitDistance(Vector3 rayDirection)
    {
        RaycastHit2D hit;
        Debug.DrawRay(transform.position, rayDirection * 5f,Color.black);
        hit = Physics2D.Raycast(transform.position, rayDirection, detectionDistance);
        if (hit.collider == null)
            return -1f;
        return hit.distance;
    }


    void MoveForward(bool slowMovement)
    {
        _rb.AddRelativeForce(Vector2.up * moveSpeed);
    }

    void TurnRight()
    {
        _rb.AddTorque(1);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x , transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z- turnChangeRate);
        Debug.Log("Turning Right");
    }

    void TurnLeft()
    {
        _rb.AddTorque(1);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + turnChangeRate);
        Debug.Log("Turning Left");
    }



}
