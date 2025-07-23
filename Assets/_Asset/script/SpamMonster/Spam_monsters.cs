using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GroupData
{
    //public List<Vector3> locations;
    //public List<int> quantities;
    //public List<int> types;
    //public List<int> sequentially;
    public int locations;
    public int quantities;
    public int types;
    public List<int> sequentially;

}


[System.Serializable]
public class TurndData
{
    public List<GroupData> groups; // danh sách group trong 1 round
}


[System.Serializable]
public class RoundData
{
    public List<TurndData> turns; // danh sách group trong 1 round
}

[System.Serializable]
public class LevelData
{
    public List<RoundData> rounds; // danh sách round trong 1 level
}

public class Spam_monsters : MonoBehaviour
{
    [Header("Setup Levels")]
    public List<LevelData> levels; // danh sách level
}
