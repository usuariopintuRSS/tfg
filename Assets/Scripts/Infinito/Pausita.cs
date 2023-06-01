using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausita : MonoBehaviour
{
    public static bool PausaPalCafe = false;

    public GameObject MenusitoPausa;

    Scene escenita;

    void Start()
    {
        MenusitoPausa.SetActive(false);
    }

    void Update()
    {
        escenita = SceneManager.GetActiveScene();
        if (
            escenita.name == "JueguesitoContrarreloj"
            || escenita.name == "JueguesitoInfinito"
            || escenita.name == "JueguesitoDia1"
            || escenita.name == "JueguesitoDia2"
            || escenita.name == "JueguesitoDia3"
            || escenita.name == "JueguesitoDia4"
            || escenita.name == "JueguesitoDia5"
        )
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PausaPalCafe)
                    Palante();
                else
                    QuietoParao();
            }
        }
    }

    public void Palante()
    {
        MenusitoPausa.SetActive(false);
        Time.timeScale = 1f;
        PausaPalCafe = false;
    }

    public void QuietoParao()
    {
        MenusitoPausa.SetActive(true);
        Time.timeScale = 0f;
        PausaPalCafe = true;
    }

    public void LaCarta()
    {
        Time.timeScale = 1f;
    }
}
