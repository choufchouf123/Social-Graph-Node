using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _userNodeObj;
    [SerializeField] private VisualLineConnecter _connectionLineObj;

    private Dictionary<int, GameObject> _userNodes = new Dictionary<int, GameObject>();
    private List<User> _users = new List<User>();
    private void Awake() {
        EventManager.eventDataBaseSetupFinished += DrawDataBase;
    }

    private void DrawDataBase() {
        CreateAllUsers();
        CreateConnections();
    }
    private void CreateAllUsers() {
        _users = DataBaseManager.instance.GetAllUsers();
        foreach (User user in _users) {
            GameObject userNode = Instantiate(_userNodeObj, transform);
            userNode.name = user.Name;
            _userNodes.Add(user.Id, userNode);
            //temporary random position
            userNode.transform.localPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
        }
    }
    private void CreateConnections() {
        foreach (User user in _users) {
            List<UserConnections> lConnectionsToUser = DataBaseManager.instance.GetUserConnections(user);
            _userNodes[user.Id].transform.localScale *= 1 + (lConnectionsToUser.Count * 0.2f);
            foreach (UserConnections connection in lConnectionsToUser) {

                User connectedUser = DataBaseManager.instance.GetUserById(connection.UserIdB);

                if (_userNodes.ContainsKey(user.Id)) {
                    VisualLineConnecter line = Instantiate(_connectionLineObj, transform);
                    line.pointA = _userNodes[user.Id].transform;
                    line.pointB = _userNodes[connectedUser.Id].transform;
                }
            }
        }
    }
}
