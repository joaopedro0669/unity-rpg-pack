using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaENoite : MonoBehaviour
{
    [SerializeField] float duracaoDia = 180;
    [SerializeField] float luzMaxima;
    [SerializeField] float luzAmbienteMinima;
    [SerializeField] [Range(0, 24)] int horaInicial;
    public float luzAmbienteMaxima;
    float segundos;
    float rotationX;
    float multiplicador;
    Light lightObj;
    void Start()
    {
        lightObj = GetComponent<Light>();
        segundos = 86400 / 24 * (horaInicial - 6 + 24);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetAxis("Mouse ScrollWheel") > 0f) 
        {
            duracaoDia = duracaoDia * 1.3f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) 
        {
            duracaoDia = duracaoDia / 1.3f;
        }*/

        if (duracaoDia != 0)
            multiplicador = 86400 / duracaoDia;
        else
            multiplicador = 0;

        segundos += Time.deltaTime * multiplicador;

        if (segundos >= 86400)
            segundos -= 86400;
        else if (segundos <= 0)
            segundos += 86400;

        rotationX = Mathf.Lerp(0, 360, segundos / 86400);

        transform.rotation = Quaternion.Euler(rotationX, 135, 0);

        //Debug.Log(segundos);

        if (rotationX >= 170 && rotationX <= 180)
        {
            lightObj.intensity = Mathf.Lerp(luzMaxima, 0f, (rotationX - 170) / 10);
            RenderSettings.ambientIntensity = Mathf.Lerp(luzAmbienteMaxima, luzAmbienteMinima, (rotationX - 170) / 10);
        }
        else if (rotationX > 180 && rotationX < 360)
        {
            lightObj.intensity = 0;
            RenderSettings.ambientIntensity = luzAmbienteMinima;
        }
        else if (rotationX >= 0 && rotationX <= 10)
        {
            lightObj.intensity = Mathf.Lerp(0f, luzMaxima, rotationX / 10);
            RenderSettings.ambientIntensity = Mathf.Lerp(luzAmbienteMinima, luzAmbienteMaxima, rotationX / 10);
        }

    }
    
}
