using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public string[] levelNames;
    private int currentlevel;
    public static ReloadScript inst;
    private SpawnPoint[] spawners;
    private Transform playerTransform;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        Respawn();
    }

    public void NextLevel()
    {
        currentlevel++;
        SceneManager.LoadScene(levelNames[currentlevel]);
        Respawn();
    }

    public void Respawn()
    {
        if (playerTransform)
        {
            Destroy(playerTransform);
        }
        playerTransform = Instantiate(playerPrefab).transform;
        spawners = FindObjectsOfType<SpawnPoint>();
        SpawnPoint spawner = GetClosestSpawner();
        playerTransform.SetPositionAndRotation(spawner.transform.position, spawner.transform.rotation);
    }

    SpawnPoint GetClosestSpawner()
    {
        SpawnPoint best = null;
        float dist = float.MaxValue;
        foreach (var t in spawners)
        {
            if (!t) continue;
            float temp = Vector3.Distance(playerTransform.position, t.transform.position);
            if (!best && t.isActive)
            {
                best = t;
                dist = temp;
            }
            else if (temp < dist && t.isActive)
            {
                best = t;
                dist = temp;
            }
        }
        return best;
    }
}
