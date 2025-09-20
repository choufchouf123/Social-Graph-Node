using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLineConnecter : MonoBehaviour
{
    [HideInInspector]public Transform pointA;
    [HideInInspector]public Transform pointB;

    [SerializeField] LineRenderer line;


    private void Update() {
        ConnectToPoints();
    }
    private void ConnectToPoints() {
        if (pointA == null || pointB == null) return;
        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);
    }
}
