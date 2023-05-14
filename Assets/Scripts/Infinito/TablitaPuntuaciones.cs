using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablitaPuntuaciones : MonoBehaviour
{
    private Transform contenedor;
    private Transform templadito;
    GameObject objetito = new GameObject();

    private void Awake()
    {
        Transform transform = objetito.AddComponent<Transform>();
        contenedor=transform.Find("PlantillaRanking");
        templadito=transform.Find("Templadito");

        templadito.gameObject.SetActive(false);

        float altura = 20f;

        for (int x=0;x<10;x++){
            Transform entradita = Instantiate(templadito, contenedor);
            RectTransform entraditaTransformada = entradita.GetComponent<RectTransform>();
            entraditaTransformada.anchoredPosition = new Vector2(0, -altura * x);
            entradita.gameObject.SetActive(true);
        }
    }
}
