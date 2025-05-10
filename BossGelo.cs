using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossGelo : MonoBehaviour
{
    [Header("Movimentação:")]
    Animator anim;
    float distancia;
    float rotY;
    float zonaMorta = 5;
    [SerializeField] float velocidade;
    [SerializeField] float velocidadeGiro;
    [SerializeField] float distanciaMinima;
    [SerializeField] Transform orientacao;
    Transform player;

    [Header("Vida:")]
    [SerializeField] Image vida;
    [SerializeField] GameObject vidaOBJ;
    [SerializeField] float vidaMax;
    public float distanciaDeGolpe;
    float vidaAtual;

    [Header("Ataque (soco):")]
    [SerializeField] float dano;
    [SerializeField] float danoMult;
    [SerializeField] float distanciaDeAtaque;
    [SerializeField] bool ataqueBloqueavel = true;
    int tipoAtaque = 1;
    [SerializeField] GameObject fragmentoGelo;
    [SerializeField] Transform localFragmentoGelo;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        tag = "Inimigo";
        vidaAtual = vidaMax;
        dano = (dano - danoMult) + danoMult * PlayerPrefs.GetInt("Dificuldade");
    }

    // Update is called once per frame
    void Update()
    {
        if(tipoAtaque == 0 && !anim.GetBool("ataque1") && !anim.GetBool("ataque2"))
        {
            tipoAtaque = Random.Range(1, 3);
        }
        distancia = Vector3.Distance(player.position, transform.position);

        orientacao.LookAt(Vector3.right * player.position.x + Vector3.up * player.position.y + Vector3.forward * player.position.z, Vector3.up);
        if (tag != "Inimigo Morto")
        {
            Giro(orientacao.eulerAngles.y, velocidadeGiro);
        }

        if (tag != "Inimigo Morto" && !anim.GetBool("ataque1") && !anim.GetBool("ataque2") && tipoAtaque != 2)
        {
            if (distancia > distanciaMinima)
            {
                transform.position += transform.forward * Time.deltaTime * velocidade;
                anim.SetBool("andando", true);
            }
            else
            {
                anim.SetBool("andando", false);
                anim.SetBool("ataque1", true);
            }
        }
        else
        {
            anim.SetBool("andando", false);
        }

        if(tipoAtaque == 2 && !anim.GetBool("ataque1") && !anim.GetBool("ataque2"))
        {
            anim.SetBool("ataque2", true);
        }

        //vida
        if (tag != "Inimigo Morto")
        {
            vida.fillAmount = vidaAtual / vidaMax;
            vidaAtual += Time.deltaTime * (PlayerPrefs.GetInt("Dificuldade") - 1) / 4f;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMax);
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
    
    public void SairDoAtaque()
    {
        anim.SetBool("ataque1", false);
        anim.SetBool("ataque2", false);
        tipoAtaque = 0;
    }

    public void Dano()
    {
        vidaAtual -= 5;
        if (vidaAtual <= 0)
        {
            anim.SetBool("morto", true);
            anim.SetBool("andando", false);
            anim.SetBool("ataque1", false);
            anim.SetBool("ataque2", false);
            tag = "Inimigo Morto";
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(vidaOBJ, 3f);
            vida.fillAmount = vidaAtual / vidaMax;
        }
    }

    public void Ataque1()
    {
        if(distancia <= distanciaDeAtaque)
            player.GetComponent<PlayerController>().Dano(dano, false);
    }
    public void Ataque2()
    {
        GameObject tiro = Instantiate(fragmentoGelo, localFragmentoGelo.position, Quaternion.identity);
        tiro.SetActive(true);
        tiro.transform.LookAt(Vector3.right * player.position.x + Vector3.up * player.position.y + Vector3.forward * player.position.z, Vector3.up);
        tiro.transform.eulerAngles = Vector3.right * tiro.transform.eulerAngles.x + Vector3.up * orientacao.transform.eulerAngles.y + Vector3.forward * tiro.transform.eulerAngles.z;
        tiro.GetComponent<Rigidbody>().velocity = transform.forward * 35;
        Destroy(tiro, 5f);
    }
}
