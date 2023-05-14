using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menusito : MonoBehaviour
{
    public GameObject menusito;
    public GameObject opsionsitas;

    void Start()
    {
        menusito.SetActive(true);
        opsionsitas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pafuera()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BotonsitoOpsiones()
    {
        menusito.SetActive(false);
        opsionsitas.SetActive(true);
    }

    public void BotonsitoVolver()
    {
        menusito.SetActive(true);
        opsionsitas.SetActive(false);
    }
}
