using SQLite4Unity3d;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConnections : MonoBehaviour
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int UserIdA { get; set; }
    public int UserIdB { get; set; }
}
