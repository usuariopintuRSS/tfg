using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MusiquitaDeFondo : MonoBehaviour
{
    public float fadeDuration = 1.0f;
 
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
 
    IEnumerator FadeOutCoroutine()
    {
        float startVolume = GetComponent<AudioSource>().volume;
 
        while (GetComponent<AudioSource>().volume > 0)
        {
            GetComponent<AudioSource>().volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
 
        GetComponent<AudioSource>().Stop();
    }
}