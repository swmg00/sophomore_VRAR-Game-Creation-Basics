using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 Offset;

   
    void Update()
    {
        transform.position = targetTransform.position + Offset;

    }
}
