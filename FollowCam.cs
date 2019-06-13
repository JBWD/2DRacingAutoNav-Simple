using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform gameObjectToFollow;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(gameObjectToFollow.position.x, gameObjectToFollow.position.y, transform.position.z),5);
    }
}
