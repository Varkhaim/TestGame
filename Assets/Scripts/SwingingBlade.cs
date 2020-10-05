using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingBlade : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float maxAngleDeflection = 30.0f;
    [SerializeField] float speedOfPendulum = 1.0f;
    [SerializeField] float baseAngle = -180.0f;

    private void Update()
    {
        float angle = maxAngleDeflection * Mathf.Sin(Time.time * speedOfPendulum);
        transform.localRotation = Quaternion.Euler(0, 0, baseAngle + angle);
    }
}
