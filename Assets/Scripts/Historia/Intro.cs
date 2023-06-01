using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Intro : MonoBehaviour
{
    public GameObject introCanvas;
    public GameObject gameCanvas;
    public TextMeshProUGUI introText;

    private float textDelay = 3f;
    public float transitionDelay = 3f;
    public float transitionDuration = 5f;

    private void Start()
    {
        introText.gameObject.SetActive(false);
        Invoke("ShowText", textDelay);
    }

    private void ShowText()
    {
        introText.gameObject.SetActive(true);
        Invoke("ShowTransition", transitionDelay);
    }

    private void ShowTransition()
    {
        StartCoroutine(TransitionCanvas());
    }

    IEnumerator TransitionCanvas()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float alpha = elapsedTime / transitionDuration;

            introCanvas.GetComponent<CanvasGroup>().alpha = 1f - alpha;
            gameCanvas.SetActive(true);
            gameCanvas.GetComponent<CanvasGroup>().alpha = alpha;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        introCanvas.SetActive(false); // Desactiva el canvas de introducción al finalizar el fade out
    }
}
