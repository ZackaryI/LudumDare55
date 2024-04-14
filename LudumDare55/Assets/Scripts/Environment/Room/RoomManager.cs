using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerOnEnterExit2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RoomManager : MonoBehaviour
{
     
    private TriggerOnEnterExit2D enterTrigger;
    [Header("Enemies")]
    public List<Spawner> spawnersInRoom;
    public int totalEnemies = 0;
    public int killCount = 0;
    [Header("Rewards")]
    public Transform rewardPickupSpot; 

    public List<GameObject> rewards;
    private bool roomComplete = false; 

    // Start is called before the first frame update
    void Start()
    {
        enterTrigger =  GetComponent<TriggerOnEnterExit2D>();
        enterTrigger.eventOnEnter2D.AddListener(() => {
            for (int i = 0; i < spawnersInRoom.Count; i++)
            {
                spawnersInRoom[i].SpawnOnceWithRoom(this);
            }
        });
        foreach (Spawner item in spawnersInRoom)
        {
            foreach (Wave wave in item.waves)
            {
                foreach (ContentWave cntWave in wave.contentWave)
                {
                    totalEnemies = totalEnemies + cntWave.waveSize; 
                }
            } 
        }
    }

    void onEndOfRoom()
    {
        foreach (GameObject item in rewards)
        {
            GameObject e  = Instantiate(item);
            var randomPos = (Vector3)Random.insideUnitCircle * 5;
            randomPos += rewardPickupSpot.transform.position;
            e.transform.position = randomPos;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(killCount >= totalEnemies && roomComplete == false)
        {
            onEndOfRoom();
            roomComplete = true; 
        }
    }

    /// <summary>
    /// Adds up every kill. Called by Death Event of EnemyController.
    /// </summary>
    public void addToKill()
    {
        killCount += 1; 
    }
}
