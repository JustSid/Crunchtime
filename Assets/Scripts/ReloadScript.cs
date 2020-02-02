using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraRigPrefab;
    public string[] levelNames;
    private int currentlevel = -1;
    public static ReloadScript inst;
    private SpawnPoint[] spawners;
    private Transform playerTransform;
    private PeekeyBoi cameraRig;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        NextLevel();
        //Respawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ReloadCurrentLevel();
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(levelNames[currentlevel]);
        //Respawn();
    }

    public void NextLevel()
    {
        currentlevel++;
        SceneManager.LoadScene(levelNames[currentlevel]);
        //Respawn();
    }

    //public void Respawn()
    //{
    //    if (playerTransform)
    //    {
    //        Destroy(playerTransform);
    //    }
    //    if (cameraRig)
    //    {
    //        Destroy(cameraRig);
    //    }
    //    playerTransform = Instantiate(playerPrefab).transform;
    //    //spawners = FindObjectsOfType<SpawnPoint>();
    //    //SpawnPoint spawner = GetClosestSpawner();
    //    SpawnPoint spawner = FindObjectOfType<SpawnPoint>();
    //    playerTransform.SetPositionAndRotation(spawner.transform.position, spawner.transform.rotation);
    //    cameraRig = Instantiate(cameraRigPrefab).GetComponent<PeekeyBoi>();
    //    cameraRig.followTarget = playerTransform;
    //    cameraRig.SnapToTarget();
    //}

    //SpawnPoint GetClosestSpawner()
    //{
    //    SpawnPoint best = null;
    //    float dist = float.MaxValue;
    //    foreach (var t in spawners)
    //    {
    //        if (!t) continue;
    //        float temp = Vector3.Distance(playerTransform.position, t.transform.position);
    //        if (!best && t.isActive)
    //        {
    //            best = t;
    //            dist = temp;
    //        }
    //        else if (temp < dist && t.isActive)
    //        {
    //            best = t;
    //            dist = temp;
    //        }
    //    }
    //    return best;
    //}
}
