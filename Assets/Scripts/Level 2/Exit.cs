using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTrigger : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    public float delayBeforeLoad = 2f;

    private bool activated = false;

    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene("Menu");
    }
}