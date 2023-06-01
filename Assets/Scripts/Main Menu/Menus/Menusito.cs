using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menusito : MonoBehaviour
{
    public GameObject menusito;
    public GameObject opsionsitas;

    [SerializeField]
    private TMP_Dropdown resdrop;
    private Resolution[] resolusiones;
    private List<Resolution> listadas;
    private float refresco;
    private int resind;

    void Start()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
        menusito.SetActive(true);
        opsionsitas.SetActive(false);
        string[] qualityNames = QualitySettings.names;
        foreach (string name in qualityNames)
        {
            Debug.Log(name);
        }

        QualitySettings.SetQualityLevel(5);

        resolusiones = Screen.resolutions;
        foreach (Resolution res in resolusiones)
        {
            Debug.Log(res);
        }
        listadas = new List<Resolution>();
        resdrop.ClearOptions();
        refresco = Screen.currentResolution.refreshRate;
        for (int x = 0; x < resolusiones.Length; x++)
        {
            if (resolusiones[x].refreshRate == refresco)
                listadas.Add(resolusiones[x]);
        }
        List<string> resolusionsitas = new List<string>();
        int resind = -1; // Inicializar en -1 para indicar que no se ha encontrado la resolución por defecto

        for (int x = 0; x < resolusiones.Length; x++)
        {
            if (resolusiones[x].width == 1920 && resolusiones[x].height == 1080)
            {
                resolusionsitas.Add("1920x1080");
                if (
                    resolusiones[x].width == Screen.width && resolusiones[x].height == Screen.height
                )
                    resind = x;
            }
            else if (resolusiones[x].width == 1600 && resolusiones[x].height == 900)
            {
                resolusionsitas.Add("1600x900");
                if (
                    resolusiones[x].width == Screen.width && resolusiones[x].height == Screen.height
                )
                    resind = x;
            }
            else if (resolusiones[x].width == 1280 && resolusiones[x].height == 1024)
            {
                resolusionsitas.Add("1280x1024");
                if (
                    resolusiones[x].width == Screen.width && resolusiones[x].height == Screen.height
                )
                    resind = x;
            }
        }

        if (resind == -1)
        {
            // Si no se encontró la resolución por defecto, se establece 1920x1080 como la opción seleccionada
            resolusionsitas.Insert(0, "1920x1080");
            resind = 0;
        }

        resdrop.ClearOptions();
        resdrop.AddOptions(resolusionsitas);
        resdrop.value = resind;
        resdrop.RefreshShownValue();
    }

    public void PonerRes(int resin)
    {
        Resolution res = resolusiones[resin];
        Screen.SetResolution(res.width, res.height, true);
    }

    public void Trillhouse(int qi)
    {
        switch (qi)
        {
            case 0:
                qi = 5;
                break;
            case 1:
                qi = 3;
                break;
            case 3:
                qi = 1;
                break;
        }
        QualitySettings.SetQualityLevel(qi);
    }

    public void Pantallita(bool completa)
    {
        Screen.fullScreen = completa;
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
