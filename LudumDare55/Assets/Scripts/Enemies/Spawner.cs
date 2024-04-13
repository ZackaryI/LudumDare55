using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Spawn enemies on Spawner Component gameobject position. 
/// Can do three types of waves : Once, Repeat every seconds with the first wave (Repeat) or multiples waves (Burst)
/// </summary>
public class Spawner : MonoBehaviour
{

    [Header("Conditions")]
    [Tooltip("Do we spawn burst waves ?")]
    public bool spawnBurstBool = false;
    [Tooltip("Do we spawn waves indefinitely ?")]
    public bool spawnRepeatBool = false;
    public bool activatedOnce = false; 

    [Header("Timer Spawn (in sec)")]
    public float timerBurst = 360f;
    public float timerRepeat = 120f;
    private float timeRepeat = 0f;
    private float timeBurst = 0f;

    [Header("Wave (Loop)")]
    public List<Wave> waves = new List<Wave>();
    private List<Wave> backupWaves;

    private ObjectPool objPool;
    void Start()
    {
        backupWaves = waves;
        objPool = ObjectPool.sharedInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnBurstBool == true || spawnRepeatBool == true)
        {
            timeBurst += Time.deltaTime;
            timeRepeat += Time.deltaTime;

            if (timeRepeat >= timerRepeat)
            {
                timeRepeat = 0f;
                SpawnRepeat();

            }
            else if (timeBurst >= timerBurst)
            {
                timeBurst = 0f;
                SpawnBurst();
            }
        }
        else if(activatedOnce == true)
        {
            activatedOnce = false; 
            SpawnOnce();
        }

    }

    public void activateUniqueWave()
    {
        activatedOnce = true; 
    }

    public void SpawnBurst()
    {
        Wave currentWave = waves[0];
        for (int i = 0; i < currentWave.contentWave.Count; i++)
        {

            for (int x = 0; x < currentWave.contentWave[i].waveSize; x++)
            {
                Debug.Log("spawned burst" + currentWave.contentWave[i].enemyType.ToString());
                GameObject e = objPool.GetPooledObject(currentWave.contentWave[i].enemyType.ToString());
                e.GetComponent<EnemyController>().enemyTier = currentWave.contentWave[i].enemyTier;
                e.transform.position = gameObject.transform.position;
                e.SetActive(true);
            }
        }
    }

    public void SpawnRepeat()
    {
        Wave currentWave = waves[0];
        for (int x = 0; x < currentWave.contentWave[0].waveSize; x++)
        {
            Debug.Log("spawned repeat" + currentWave.contentWave[0].enemyType.ToString());
            GameObject e = objPool.GetPooledObject(currentWave.contentWave[0].enemyType.ToString());
            e.GetComponent<EnemyController>().enemyTier = currentWave.contentWave[0].enemyTier;
            e.transform.position = gameObject.transform.position;
            e.SetActive(true);
        }
    }

    /// <summary>
    /// Is activated by the method ActivateUniqueWave, useful for single wave. 
    /// </summary>
    public void SpawnOnce()
    {
        Wave currentWave = waves[0];
        for (int x = 0; x < currentWave.contentWave[0].waveSize; x++)
        {
            Debug.Log("spawned once" + currentWave.contentWave[0].enemyType.ToString()); 
            GameObject e = objPool.GetPooledObject(currentWave.contentWave[0].enemyType.ToString()); 
            e.GetComponent<EnemyController>().enemyTier = currentWave.contentWave[0].enemyTier; 
                e.transform.position = gameObject.transform.position;
                e.SetActive(true); 
        }
    }
}

public enum EnemyType
{
    Melee,
    Ranged,
    Magic
}

[System.Serializable]
public class Wave
{
    public List<ContentWave> contentWave = new List<ContentWave>();
}
[System.Serializable]
public class ContentWave
{

    public int waveSize = 50;
    public EnemyType enemyType;
    [Range(1, 3)]
    public int enemyTier; 

}

[CustomEditor(typeof(Spawner))]
class SpawnerEditor : Editor
{
    public Spawner spawner; 
    SerializedProperty activatedOnce;
    private void OnEnable()
    {
        spawner = (Spawner)target;
        activatedOnce = serializedObject.FindProperty(nameof(Spawner.activatedOnce));
    }

    public override void OnInspectorGUI()
    { 
        serializedObject.Update();
         
        if (activatedOnce.boolValue)
        {
            spawner.activatedOnce = activatedOnce.boolValue; 
        }
        if (GUILayout.Button("Activate wave Once"))
        {
            spawner.activatedOnce = !spawner.activatedOnce;
        } 

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}