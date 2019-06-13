using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckChecker : MonoBehaviour
{

    Vector2 savedPosition;

    public float resetThreshold = .2f;
    public float resetTimeLimit = 1f;
    float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        savedPosition = transform.position;
        StartCoroutine(UpdatePosition());
    }

    private void Update()
    {
        
        if (Vector2.Distance(savedPosition, transform.position) < resetThreshold)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
        }
        if (currentTime > resetTimeLimit)
        {
            GetComponent<CheckpointChecker>().ResetCar();
        }
        
    }

    IEnumerator UpdatePosition()
    {
        while(gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(.1f);
            savedPosition = transform.position;
        }
    }
}
