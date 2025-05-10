using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Movimentação:")]
    Animator anim;
    float distancia;
    float rotY;
    float zonaMorta = 5;
    bool seguirPlayer;
    bool seguirPlayerDeteccao;
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

    [Header("Ataque:")]
    [SerializeField] float dano;
    [SerializeField] float danoMult;
    [SerializeField] float distanciaDeAtaque;
    [SerializeField] bool ataqueBloqueavel = true;
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
        distancia = Vector3.Distance(player.position, transform.position);

        if (seguirPlayer && tag != "Inimigo Morto" && !anim.GetBool("ataque"))
        {
            if (distancia > distanciaMinima)
            {
                orientacao.LookAt(Vector3.right * player.position.x + Vector3.up * player.position.y + Vector3.forward * player.position.z, Vector3.up);
                Giro(orientacao.eulerAngles.y, velocidadeGiro);
                transform.position += transform.forward * Time.deltaTime * velocidade;
                anim.SetBool("andando", true);
            }
            else
            {
                anim.SetBool("andando", false);
                anim.SetBool("ataque", true);
            }
        }
        else if (anim.GetBool("ataque"))
        {
            orientacao.LookAt(Vector3.right * player.position.x + Vector3.up * player.position.y + Vector3.forward * player.position.z, Vector3.up);
            Giro(orientacao.eulerAngles.y, velocidadeGiro);
        }
        else
        {
            anim.SetBool("andando", false);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            seguirPlayer = true;
            seguirPlayerDeteccao = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            seguirPlayerDeteccao = false;
            Invoke("DesativarSeguirPlayer", 2f);
        }
    }

    void DesativarSeguirPlayer()
    {
        if (!seguirPlayerDeteccao)
            seguirPlayer = false;
    }

    public void SairDoAtaque()
    {
        anim.SetBool("ataque", false);
    }

    public void Dano()
    {
        vidaAtual -= 5;
        if (vidaAtual <= 0)
        {
            anim.SetBool("morto", true);
            anim.SetBool("andando", false);
            anim.SetBool("ataque", false);
            tag = "Inimigo Morto";
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(vidaOBJ, 3f);
            vida.fillAmount = vidaAtual / vidaMax;
        }
    }

    public void Ataque()
    {
        if(distancia <= distanciaDeAtaque)
            player.GetComponent<PlayerController>().Dano(dano, ataqueBloqueavel);
    }
}
