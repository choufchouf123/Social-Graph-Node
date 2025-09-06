using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class DataBaseManager : MonoBehaviour
{
    private SQLiteConnection db;

    private void Awake() {
        string dbPath = Path.Combine(Application.persistentDataPath, "SocialNodeGraph.db");
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
