using System.Collections.Generic;
using UnityEngine;

public class RobertIdDispatcher : MonoBehaviour
{
    public static RobertIdDispatcher Instance = null;

    private List<bool> idList = new List<bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetNewRobertId()
    {
        for (int i = 0; i < idList.Count; i++)
        {
            if (!idList[i])
            {
                idList[i] = true;
                return i;
            }
        }
        int newId = idList.Count;
        idList.Add(true);
        return newId;
    }

    public void FreeRobertId(int id)
    {
        if (id >= 0 && id < idList.Count)
        {
            idList[id] = false;
        }
    }
}
