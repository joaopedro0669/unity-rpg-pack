using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilhaFionnaController : MonoBehaviour
{
    [SerializeField] Animator anim;
    float distancia;
    float rotY;
    float zonaMorta = 5;
    [SerializeField] bool assustada;
    [SerializeField] bool seguirPlayer;
    [SerializeField] float velocidade;
    [SerializeField] float distanciaMinima;
    [SerializeField] Transform orientacao;
    Transform player;
    void Start()
    {
        if (assustada)
            anim.SetBool("assustada", true);
        else
            anim.SetBool("assustada", false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (seguirPlayer)
        {
            distancia = Vector3.Distance(player.position, transform.position);
            if (distancia > distanciaMinima)
            {
                orientacao.LookAt(Vector3.right * player.position.x + Vector3.up * player.position.y + Vector3.forward * player.position.z, Vector3.up);
                Giro(orientacao.eulerAngles.y, 150);
                transform.position += transform.forward * Time.deltaTime * velocidade;
                anim.SetBool("andando", true);
            }
            else
                anim.SetBool("andando", false);
        }
    }
    void Giro(float rotFinal, float velocidadeDeAjuste)
    {

        rotY = transform.eulerAngles.y; //pegar rotação player

        //igualar 
        while (rotFinal >= 360)
        {
            rotFinal -= 360;
        }
        while (rotFinal < 0)
        {
            rotFinal += 360;
        }

        if (rotY > rotFinal + zonaMorta || rotY < rotFinal - zonaMorta) //pra não funcionar o tempo inteiro e ficar feio, além de otimizar o código
        {



            while (rotY >= 360)
            {
                rotY -= 360;
            }
            while (rotY < 0)
            {
                rotY += 360;
            }



            if (rotY >= 180)
            {
                if (rotFinal >= (rotY - 180) && rotFinal < rotY)
                {
                    transform.eulerAngles -= Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
                if (rotFinal > (rotY - 180) && rotFinal > rotY)
                {
                    transform.eulerAngles += Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
                if (rotFinal < (rotY - 180))
                {
                    transform.eulerAngles += Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
            }

            if (rotY < 180)
            {
                if (rotFinal >= (rotY + 180))
                {
                    transform.eulerAngles -= Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
                if (rotFinal < (rotY + 180) && rotFinal > rotY)
                {
                    transform.eulerAngles += Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
                if (rotFinal < (rotY + 180) && rotFinal < rotY)
                {
                    transform.eulerAngles -= Vector3.up * Time.deltaTime * velocidadeDeAjuste;
                }
            }
        }
        //Debug.LogFormat("Rot: {0}, Pivot: {1}", rotY, pivotY);
    }

}
