using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class KillHitbox : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private TextMeshProUGUI textoUI;

    private void Start()
    {
        if (textoUI != null)
        {
            textoUI.gameObject.SetActive(false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ReiniciarEscenaConRetraso());
        }
    }

    private IEnumerator ReiniciarEscenaConRetraso()
    {
        if (textoUI != null)
        {
            textoUI.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}