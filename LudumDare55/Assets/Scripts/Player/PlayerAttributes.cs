using UnityEngine;

[CreateAssetMenu(fileName = "Player Attributes", menuName = "Player Attributes", order = 1)]
public class PlayerAttributes : ScriptableObject
{
    public float health;
    public float playerMoveSpeed;
    public float itemThowSpeed;
    public short maxSummons = 3;
    public GameObject DefualtSpawn;
}
