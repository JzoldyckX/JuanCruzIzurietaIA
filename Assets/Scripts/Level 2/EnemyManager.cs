using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [Header("Enemies")]
    public HunterAI hunter;

    private void Awake()
    {
        Instance = this;
    }

    public void AlertHunter(Vector3 playerPosition)
    {
        if (hunter != null)
            hunter.ReceiveAlert(playerPosition);
    }
}