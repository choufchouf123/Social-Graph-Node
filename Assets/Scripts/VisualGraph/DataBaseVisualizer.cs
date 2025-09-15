using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject userNodeObj;
    private void Awake() {
        EventManager.eventDataBaseSetupFinished += DrawDataBase;
    }

    private void DrawDataBase() {
        List<User> lAllUserList =  DataBaseManager.instance.GetAllUsers();

        foreach (User user in lAllUserList) {
            List<User> connectedUsers = DataBaseManager.instance.GetUsersConnectedToUser(user);
            List<UserConnections> lConnectionsTest = DataBaseManager.instance.GetUserConnections(user);
            GameObject userNode = Instantiate(userNodeObj, transform);
            userNode.name = user.Name;
            userNode.transform.localScale *= (10 + connectedUsers.Count) / 10f;
            userNode.transform.localPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            //print(connectedUsers.Count);
            print(lConnectionsTest.Count);
        }
    }
}
