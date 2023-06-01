using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodigoResolucion : MonoBehaviour
{
    public TextMeshProUGUI resolutionText1;
    public TextMeshProUGUI resolutionText2;
    public TextMeshProUGUI resolutionText3;

    public void Aplicar()
    {
        if (resolutionText1.gameObject.activeSelf)
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        else if (resolutionText2.gameObject.activeSelf)
            Screen.SetResolution(1280, 1024, Screen.fullScreen);
        else if (resolutionText3.gameObject.activeSelf)
            Screen.SetResolution(1024, 768, Screen.fullScreen);
    }
}
