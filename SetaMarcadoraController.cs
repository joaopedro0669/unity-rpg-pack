using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetaMarcadoraController : MonoBehaviour
{
    float distancia;
    string[] coordenadas = new string[3];
    [SerializeField] MeshRenderer mr;
    [SerializeField] Text distanciaText;
    [SerializeField] Transform playerPos;

    void Start()
    {
        //mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("LocalMarcado") == "Sim")
        {
            mr.enabled = true;
            coordenadas = PlayerPrefs.GetString("CoordenadasDeLocalMarcado").Split('x');

            
            transform.LookAt(Vector3.right * float.Parse(coordenadas[0]) + Vector3.up * float.Parse(coordenadas[1]) + Vector3.forward * float.Parse(coordenadas[2]), Vector3.up);
            distancia = Vector3.Distance(playerPos.position, Vector3.right * float.Parse(coordenadas[0]) + Vector3.up * float.Parse(coordenadas[1]) + Vector3.forward * float.Parse(coordenadas[2]));
            distanciaText.text = (int) distancia + " m";
        }
        else if (PlayerPrefs.GetString("LocalMarcado") == "Não")
        {
            mr.enabled = false;
            distanciaText.text = "";
        }
    }
}
