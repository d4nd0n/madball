using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    [SerializeField]
    private GameObject _madBall;
    [SerializeField]
    private BallBehaviour _bb;
    [SerializeField]
    private GameObject moovingPoint;

    private void OnDrawGizmos()
    {
        // Show lookAt target for madball
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_madBall.transform.position, _bb.target);

    }
}
