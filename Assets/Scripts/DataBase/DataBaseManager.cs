using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class DataBaseManager : MonoBehaviour
{
    private SQLiteConnection db;

    public static DataBaseManager instance { get; private set; }
    private void Awake() {
        instance = this;

        //Creates the database and resets it if it already exists
        string dbPath = Path.Combine(Application.persistentDataPath, "SocialNodeGraph.db");
        if (File.Exists(dbPath)) File.Delete(dbPath);
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        db.Execute("PRAGMA foreign_keys = ON;");

        CreateTables();

        CreateRandomUsersAndConnections();
        EventManager.eventDataBaseSetupFinished?.Invoke();
    }

    private void CreateTables() {
        db.CreateTable<User>();
        db.CreateTable<UserConnections>();
    }
    public int AddUser(string pName) {
        var user = new User { Name = pName };
        db.Insert(user);
        return user.Id;
    }
    public UserConnections AddUserConnection(User pUserA, User pUserB) {
        // Prevent self-connection
        if (pUserA.Id == pUserB.Id) { 
            Debug.LogWarning("Cannot create a connection to oneself.");
            return null; 
        }

        var userConnection = new UserConnections { UserIdA = pUserA.Id, UserIdB = pUserB.Id };
        db.Insert(userConnection);
        return userConnection;
    }
    public List<User> GetAllUsers() {
        return db.Table<User>().ToList();
    }
    public User GetUserById(int pID) {
        return db.Find<User>(pID);
    }
    public List<User> GetUserByName(string pName) {
        return db.Table<User>().Where(user => user.Name == pName).ToList();
    }
    public List<UserConnections> GetUserConnections(User pUser) {
        return db.Table<UserConnections>().Where(userCo => userCo.UserIdA == pUser.Id).ToList();
    }
    //return all users connected to the given user
    public List<User> GetUsersConnectedToUser(User pUser) {

        List<int> lConnectionIds  = db.Table<UserConnections>().Where(userCo => userCo.UserIdA == pUser.Id).Select(userCo => userCo.UserIdB).ToList();

        List<User> lConnectedUsers = new List<User>();
        foreach (int id in lConnectionIds) {
            lConnectedUsers.Add(GetUserById(id));
        }
        //print(lConnectedUsers.Count);
        return lConnectedUsers;
    }
    public void CreateRandomUsersAndConnections() {
        var userIds = new List<int>();

        // Create 10 random users
        for (int i = 0; i < 10; i++) {
            string randomName = "User" + Random.Range(1, 1000); // random name
            int id = AddUser(randomName);
            userIds.Add(id);
        }

        // Create 23 random connections
        for (int i = 0; i < 23; i++) {
            int userAId = userIds[Random.Range(0, userIds.Count)];
            int userBId = userIds[Random.Range(0, userIds.Count)];

            // Avoid self-connections and duplicate entries
            if (userAId != userBId) {
                AddUserConnection(db.Find<User>(userAId), db.Find<User>(userBId));
                //print(userAId + " - " + userBId);
            }
            else
                i--; // retry
        }

        Debug.Log("Created 10 random users and 23 random connections.");
    }

}
