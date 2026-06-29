using UnityEngine;

public class Gema : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemy; // referencia al enemigo

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // activar huida
            if (enemy != null)
            {
                enemy.shouldFlee = true;
            }

            // destruir la gema
            Destroy(gameObject);
        }
    }
}