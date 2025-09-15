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

        string dbPath = Path.Combine(Application.persistentDataPath, "SocialNodeGraph.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        db.Execute("PRAGMA foreign_keys = ON;");

        CreateTables();
        
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
        return db.Table<UserConnections>().Where(userCo => userCo.UserIdA == pUser.Id || userCo.UserIdB == pUser.Id).ToList();
    }
    public List<User> GetUsersConnectedToUser(User pUser) {

        List<int> lConnectionIds  = db.Table<UserConnections>().Where(userCo => userCo.UserIdA == pUser.Id).Select(userCo => userCo.UserIdB).ToList();
        return db.Table<User>().Where(user => lConnectionIds.Contains(user.Id)).ToList();
    }

}
