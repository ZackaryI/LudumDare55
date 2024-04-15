using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerOnEnterExit2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RoomManager : MonoBehaviour
{
    public bool bossRoom = false; 
    [Header("Text Reference")]
    public RoomStatusText roomStatus; 
    [Header("Escape Colliders")]
    public List<Collider2D> blockEscapes;
    private TriggerOnEnterExit2D enterTrigger;
    [Header("Enemies")]
    public List<Spawner> spawnersInRoom;
    public int totalEnemies = 0;
    public int killCount = 0;
    [Header("Rewards")]
    public bool isRewardRandomized = false;
    public int numberOfDrops = 2;
    public Transform rewardPickupSpot; 
    public List<GameObject> rewards;

    [Header("Events")]
    public UnityEvent onEndOfRoomEvent;
    public UnityEvent onStartOfRoomEvent;
    private bool roomComplete = false; 
    private bool roomFled = false; 
    private List<GameObject> ListEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

        for (int x = 0; x < blockEscapes.Count; x++)
        {
            blockEscapes[x].enabled = false;
        }
        for (int i = 0; i < spawnersInRoom.Count; i++)
        {
            spawnersInRoom[i].gameObject.SetActive(false);

        }
        roomStatus = FindObjectOfType<RoomStatusText>();
        enterTrigger = GetComponent<TriggerOnEnterExit2D>();
        enterTrigger.eventOnExit2D.AddListener(() => {checkIfCleared(); }) ;
        if (bossRoom)
        {

            enterTrigger.eventOnEnter2D.AddListener(() => {
                onStartOfRoomEvent?.Invoke();
                roomStatus.UpdateText("Room start !");
                for (int x = 0; x < blockEscapes.Count; x++)
                {
                    blockEscapes[x].enabled = true;
                }
                for (int i = 0; i < spawnersInRoom.Count; i++)
                {
                    spawnersInRoom[i].gameObject.SetActive(true); 

                }
            });
        } else
        {

            enterTrigger.eventOnEnter2D.AddListener(() => {
                onStartOfRoomEvent?.Invoke();
                roomStatus.UpdateText("Room start !");
                for (int x = 0; x < blockEscapes.Count; x++)
                {
                    blockEscapes[x].enabled = true;
                }
                for (int i = 0; i < spawnersInRoom.Count; i++)
                {
                    spawnersInRoom[i].gameObject.SetActive(true);
                    ListEnemies.AddRange(spawnersInRoom[i].SpawnOnceWithRoom(this));

                }
            });
        }
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

    void checkIfCleared()
    {
        if (roomComplete == false)
        {

            roomFled = true; 
            roomStatus.UpdateText("Room fled !");
            foreach (GameObject item in ListEnemies)
            {
                StartCoroutine(item.GetComponent<EnemyController>().Despawn());
            }

            ListEnemies.Clear();

            enterTrigger.eventOnEnter2D.RemoveAllListeners();
            enterTrigger.eventOnExit2D.RemoveAllListeners();

            for (int x = 0; x < blockEscapes.Count; x++)
            {
                blockEscapes[x].enabled = false;
            }

        }
    }
    void onEndOfRoom()
    {
        if(bossRoom == false)
        {

        List<int> rewardsPicked = new List<int>(); 
        onEndOfRoomEvent?.Invoke();
        for (int x = 0; x < blockEscapes.Count; x++)
        {
            blockEscapes[x].enabled = false;
        }

        if (isRewardRandomized == false)
        {
            for (int x = 0; x < rewards.Count; x++)
            {
                GameObject e = Instantiate(rewards[x]);
                var randomPos = (Vector3)Random.insideUnitCircle * 5;
                randomPos += rewardPickupSpot.transform.position;
                e.transform.position = randomPos;

            }
        } else
        {
            for (int x = 0; x < numberOfDrops; x++)
            {

                int rand = Random.Range(0, rewards.Count - 1);
                if(rewardsPicked.Count > 2)
                { 
                    while (rewardsPicked.Contains(rand) == true)
                    {
                        rand = Random.Range(0, rewards.Count - 1);
                    }
                } else
                {
                    rand = x; 
                }

                if (rewards[rand].name.Contains("Bow"))
                {
                    GameObject d = rewards.Where(x => x.name == "arrow").First(); 
                    var randPos = (Vector3)Random.insideUnitCircle * 5;
                    randPos += rewardPickupSpot.transform.position;
                    d.transform.position = randPos;
                    Instantiate(d);
                }
                 
                rewardsPicked.Add(rand); 
                GameObject e = Instantiate(rewards[rand]);
                var randomPos = (Vector3)Random.insideUnitCircle * 5;
                randomPos += rewardPickupSpot.transform.position;
                e.transform.position = randomPos;

            }
        }

        ListEnemies.Clear();

        enterTrigger.eventOnEnter2D.RemoveAllListeners();
        enterTrigger.eventOnExit2D.RemoveAllListeners();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(killCount >= totalEnemies && roomComplete == false && roomFled == false)
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
