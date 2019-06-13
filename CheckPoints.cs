using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public static CheckPoints Instance;


    public void Awake()
    {
        Instance = this;
    }

    public Collider2D[] checkPoints;


    public Collider2D GetNextCheckPoint(Collider2D currentCheckPoint)
    {
        for(int i = 0;i<checkPoints.Length;i++)
        {
            if(currentCheckPoint == checkPoints[i])
            {
                if (i == checkPoints.Length - 1)
                    return checkPoints[0];
                else
                {
                    return checkPoints[i + 1];
                }
            }
        }
        return null;
    }
}
