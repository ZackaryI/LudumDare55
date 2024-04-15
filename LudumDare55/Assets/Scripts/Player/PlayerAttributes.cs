using UnityEngine;

[CreateAssetMenu(fileName = "Player Attributes", menuName = "Player Attributes", order = 1)]
public class PlayerAttributes : ScriptableObject
{
    public float health;
    public float playerMoveSpeed;
    public float itemThowSpeed;
    public short maxSummons = 3;
    public GameObject DefualtSpawn;

    public AudioClip[] walkingSound;
    public AudioClip summonSound;
    public AudioClip[] swordSwing;
    public AudioClip[] bowShots;

    public AudioClip[] MagicLayer1;
    public AudioClip[] MagicLayer2;
    public AudioClip MagicLayer3;
}
