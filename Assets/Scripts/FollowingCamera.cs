using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform followedObject = null;
    
    Vector3 baseLocalCameraPosition = Vector3.zero;

    private void Start()
    {
        baseLocalCameraPosition = transform.localPosition;
    }

    private void Update()
    {
        FollowObject();
    }

    public void ResetPosition()
    {
        transform.localPosition = baseLocalCameraPosition;
    }

    private void FollowObject()
    {
        if (followedObject == null) return;

        if (transform.position.y < GameManager.Instance.MinimumCameraHeight)
        {
            transform.position = new Vector3(transform.position.x, GameManager.Instance.MinimumCameraHeight, transform.position.z);
        }
        transform.LookAt(followedObject.position);
    }
}
