using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{

    [SerializeField]
    Collider2D currentCheckPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(currentCheckPoint != null)
        {
            if (collision.tag == "CheckPoint")
            {
                if (CheckPoints.Instance.GetNextCheckPoint(currentCheckPoint) != collision)
                {
                    ResetCar();
                }
                else
                {
                    currentCheckPoint = collision;
                }
            }
        }
        else
        {
            currentCheckPoint = collision;
        }
       
    }

    public void ResetCar()
    {
        transform.position = currentCheckPoint.transform.position;
        transform.rotation = currentCheckPoint.transform.rotation;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Mover>().FixedUpdate();
    }
}

