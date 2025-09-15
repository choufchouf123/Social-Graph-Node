using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseVisualizer : MonoBehaviour
{
    private void Awake() {
        EventManager.eventDataBaseSetupFinished += DrawDataBase;
    }

    private void DrawDataBase() {

    }
}
