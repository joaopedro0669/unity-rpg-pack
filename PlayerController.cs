using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustar à câmera:")]
    [SerializeField] Transform pivot;
    [SerializeField] float zonaMorta;
    float pivotY; //tbm usado em combate
    float rotY; //tbm usado em combate

    [Header("Configurações de movimento:")]
    [SerializeField] float velocidadeDeAjusteNormal = 200;
    [SerializeField] float velocPlayer;
    [SerializeField] float multVelocPlayer;
    float velocidadePlayer;
    KeyCode frenteKey;
    KeyCode trasKey;
    KeyCode esquerdaKey;
    KeyCode direitaKey;
    KeyCode correrKey;
    KeyCode bloqueioKey;
    KeyCode interagirKey;
    KeyCode modoCombateKey;
    KeyCode ataqueKey;

    Animator anim;
    GameObject interagirObject = null;

    [Header("Configurações de vigor e vida:")]
    [SerializeField] float vigorMax;
    [SerializeField] float vidaMax;
    [SerializeField] Image vigor;
    [SerializeField] Image vida;
    float vigorAtual;
    float vidaAtual;
    bool cansado = false;

    [Header("Configurações de combate:")]
    [SerializeField] float velocidadeDeAjusteCombate;
    [SerializeField] float velocidadePlayerCombate;
    //[SerializeField] float velocidadeInterpolação;
    [SerializeField] Transform alinhamentoProInimigo;
    int direcaoEmCombate;
    int idMenorDistancia;
    GameObject[] inimigos;
    Coroutine SalvarInimigos;
    Coroutine DetectarInimigoMaisProximo;
    bool emCombate = false;
    bool ativarCombate = false;
    bool estaSendoDetectadoEmCombate = false;
    float menorDistancia;
    float pivotYCombate;

    [Header("Outros:")]
    [SerializeField] GameObject Prefab_E;
    GameObject Prefab_E_Salvo;

    void Start()
    {
        anim = GetComponent<Animator>();
        PlayerPrefs.SetString("PlayerLivre", "Sim"); //tirar futuramente, talvez (se tirar daqui, vai ter que mexer no GameManager) (sem falar que tem coisa parecida no CameraController)
        vigorAtual = vigorMax;
        vidaAtual = vidaMax;
        anim.SetBool("morto", false);

        SetarTeclas();
    }

    // Update is called once per frame
    void Update()
    {
        //correr
        if ( (PlayerPrefs.GetString("PlayerLivre") == "Sim" && !emCombate) || (PlayerPrefs.GetString("PlayerLivre") == "Sim" && emCombate && !ativarCombate) )
        {
            Movimentacao();
            if (interagirObject != null && Input.GetKeyDown(interagirKey))
            {
                interagirObject.GetComponent<Interacao>().FuncaoInteracao();
                if (interagirObject.GetComponent<Interacao>().dialogoUnico && interagirObject.GetComponent<Interacao>().dialogo && Prefab_E_Salvo != null)
                {
                    Destroy(Prefab_E_Salvo);
                    interagirObject = null;
                }
            }
            anim.SetBool("combate", false);
            anim.SetBool("ataque", false);
        }
        else if (PlayerPrefs.GetString("PlayerLivre") == "Sim" && emCombate && ativarCombate)
        {
            MovimentacaoEmCombate();
        }
        else if (PlayerPrefs.GetString("PlayerLivre") == "Não")
        {
            anim.SetBool("correndo", false);
            anim.SetBool("andando", false);
            anim.SetBool("combate", false);
        }

        //entrar e sair de combate
        if (Input.GetKeyDown(modoCombateKey) && emCombate)
            ativarCombate = !ativarCombate;

        //controlar vida
        vida.fillAmount = vidaAtual / vidaMax;
        if (!anim.GetBool("morto"))
        {
            vidaAtual += Time.deltaTime * 3 / PlayerPrefs.GetInt("Dificuldade");
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMax);
        }

        



        //Dev();
    }
    //configurações de devenvolvedor
    /*void Dev()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Dano(10, true);
        }
    }*/

    //movimentação normal
    void Movimentacao()
    {
        //correr só com vigor e perdê-lo
        if (Input.GetKey(correrKey) && !cansado)
        {
            velocidadePlayer = velocPlayer * multVelocPlayer;
            anim.SetBool("correndo", true);
            if (anim.GetBool("andando"))
                vigorAtual -= Time.deltaTime; //não tem multiplicador - é pra cansar
            else
                vigorAtual += Time.deltaTime * 2 / (PlayerPrefs.GetInt("Dificuldade") / 2f); //pra regenerar msm quando pressionar Shift e não correr
        }
        else
        {
            velocidadePlayer = velocPlayer;
            anim.SetBool("correndo", false);
            vigorAtual += Time.deltaTime * 2 / (PlayerPrefs.GetInt("Dificuldade") / 2f);
        }

        //regenerar mais vigor ao ficar parado
        if (!anim.GetBool("andando"))
            vigorAtual += Time.deltaTime * 2 / (PlayerPrefs.GetInt("Dificuldade") / 2f);

        vigorAtual = Mathf.Clamp(vigorAtual, 0, vigorMax);

        if (vigorAtual == 0)
        {
            cansado = true;
        }

        if (cansado && vigorAtual >= vigorMax * 0.3f)
        {
            cansado = false;
        }

        vigor.fillAmount = vigorAtual / vigorMax;


        //escolher as direções
        if (Input.GetKey(frenteKey) && Input.GetKey(direitaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 45;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(frenteKey) && Input.GetKey(esquerdaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 315;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey) && Input.GetKey(esquerdaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 225;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey) && Input.GetKey(direitaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 135;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(frenteKey))
        {
            pivotY = pivot.transform.eulerAngles.y;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }

        else if (Input.GetKey(direitaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 90;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 180;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(esquerdaKey))
        {
            pivotY = pivot.transform.eulerAngles.y + 270;
            Giro(pivotY, velocidadeDeAjusteNormal);
            transform.position += transform.forward * Time.deltaTime * velocidadePlayer;
            anim.SetBool("andando", true);
        }
        else
        {
            anim.SetBool("andando", false);
        }
    }

    //movimentação em ataque
    void MovimentacaoEmCombate()
    {
        if (Input.GetKey(bloqueioKey))
            anim.SetBool("bloqueando", true);
        else
            anim.SetBool("bloqueando", false);



        if (Input.GetKeyDown(ataqueKey) && !anim.GetBool("ataque") && !anim.GetBool("bloqueando"))
        {
            anim.SetBool("ataque", true);
        }

        

        anim.SetBool("combate", true);

        if (SalvarInimigos == null)
            SalvarInimigos = StartCoroutine(SalvarInimigosFuncao());

        if (inimigos.Length >= 1) //pra caso matemos todos os inimigos
        {
            if (DetectarInimigoMaisProximo == null)
                DetectarInimigoMaisProximo = StartCoroutine(DetectarInimigoMaisProximoFuncao());

            alinhamentoProInimigo.LookAt(inimigos[idMenorDistancia].transform.position, Vector3.up);
        }
        else
        {
            emCombate = false;
            estaSendoDetectadoEmCombate = false;
        }

        Giro(alinhamentoProInimigo.eulerAngles.y, velocidadeDeAjusteCombate);//olha pro inimigo

        //escolher a direção correta entre as 8 existentes
        pivotYCombate = pivot.eulerAngles.y;
        if (Input.GetKey(frenteKey) && Input.GetKey(direitaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 45;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(frenteKey) && Input.GetKey(esquerdaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 315;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey) && Input.GetKey(esquerdaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 225;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey) && Input.GetKey(direitaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 135;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(frenteKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y;
            anim.SetBool("andando", true);
        }

        else if (Input.GetKey(direitaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 90;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(trasKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 180;
            anim.SetBool("andando", true);
        }
        else if (Input.GetKey(esquerdaKey))
        {
            pivotYCombate = pivot.transform.eulerAngles.y + 270;
            anim.SetBool("andando", true);
        }
        else
        {
            anim.SetBool("andando", false);
        }


        float rotYCombate = transform.eulerAngles.y;
        float diferencaYCombate = pivotYCombate - rotYCombate; //variação = final - inicial

        while (diferencaYCombate < 0)
            diferencaYCombate += 360;

        while (diferencaYCombate > 360)
            diferencaYCombate -= 360;

        direcaoEmCombate = Mathf.RoundToInt(diferencaYCombate / 45);
        if (direcaoEmCombate == 8)
            direcaoEmCombate = 0;

        anim.SetInteger("direcao", direcaoEmCombate);


        if (anim.GetBool("andando") && !anim.GetBool("bloqueando"))
        {
            switch (direcaoEmCombate)
            {
                case 0:
                    transform.position += transform.forward * Time.deltaTime * velocidadePlayerCombate;
                    break;
                case 1:
                    transform.position += (transform.forward + transform.right) * Time.deltaTime * velocidadePlayerCombate / 1.41f;
                    break;
                case 2:
                    transform.position += transform.right * Time.deltaTime * velocidadePlayerCombate;
                    break;
                case 3:
                    transform.position += (-transform.forward + transform.right) * Time.deltaTime * velocidadePlayerCombate / 1.41f;
                    break;
                case 4:
                    transform.position += -transform.forward * Time.deltaTime * velocidadePlayerCombate;
                    break;
                case 5:
                    transform.position += (-transform.forward - transform.right) * Time.deltaTime * velocidadePlayerCombate / 1.41f;
                    break;
                case 6:
                    transform.position += -transform.right * Time.deltaTime * velocidadePlayerCombate;
                    break;
                case 7:
                    transform.position += (transform.forward - transform.right) * Time.deltaTime * velocidadePlayerCombate / 1.41f;
                    break;
            }
        }
    }

    //fazer ele girar para a direção da câmera
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

    IEnumerator SalvarInimigosFuncao()
    {
        inimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        yield return null;
        //frame 2
        yield return null;
        //frame 3
        yield return null;
        //frame 4
        yield return null;
        //frame 5
        yield return null;
        //frame 6
        yield return null;
        //frame 7
        yield return null;
        //frame 8
        yield return null;
        //frame 9
        yield return null;
        //frame 10
        yield return null;
        //frame 11
        yield return null;
        //frame 12
        yield return null;
        //frame 13
        yield return null;
        //frame 14
        yield return null;
        //frame 15
        yield return null;
        //frame 16
        yield return null;
        //frame 17
        yield return null;
        //frame 18
        yield return null;
        //frame 19
        yield return null;
        //frame 20
        SalvarInimigos = null;
        yield return null;

    }

    IEnumerator DetectarInimigoMaisProximoFuncao()
    {
        menorDistancia = Vector3.Distance(transform.position, inimigos[0].transform.position);
        idMenorDistancia = 0;
        for (int i = 0; i < inimigos.Length; i++)
        {
            if (Vector3.Distance(transform.position, inimigos[i].transform.position) < menorDistancia)
            {
                menorDistancia = Vector3.Distance(transform.position, inimigos[i].transform.position);
                idMenorDistancia = i;
            }
        }
        yield return null;
        //frame 2
        yield return null;
        //frame 3
        yield return null;
        //frame 4
        yield return null;
        //frame 5
        DetectarInimigoMaisProximo = null;
        yield return null;
    }

    public void Dano(float dano, bool bloqueavel)
    {
        if (bloqueavel && anim.GetBool("bloqueando"))
        {
            //nada acontece
        }
        else
        {
            vidaAtual -= dano;
            if (vidaAtual <= 0)
            {
                anim.SetBool("morto", true);
                PlayerPrefs.SetString("PlayerLivre", "Não");
            }
        }
    }

    public void Morrer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Jogo");
    }

    public void SetarTeclas()
    {
        frenteKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Frente"));
        trasKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Tras"));
        esquerdaKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Esquerda"));
        direitaKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Direita"));
        correrKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Correr"));
        bloqueioKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Bloqueio"));
        interagirKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Interagir"));
        modoCombateKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_ModoCombate"));
        ataqueKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_Ataque"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interação"))
        {
            interagirObject = other.gameObject;
            if (interagirObject.GetComponent<Interacao>().inicioAutomatico == true)
                interagirObject.GetComponent<Interacao>().FuncaoInteracao();
            else
            {
                if (Prefab_E_Salvo != null) Destroy(Prefab_E_Salvo); //Pra destruir e evitar cópias
                Prefab_E_Salvo = Instantiate(Prefab_E, other.transform.position, Quaternion.identity);
                Prefab_E_Salvo.SetActive(true);
            }

        }
        if (other.CompareTag("Inimigo"))
        {
            emCombate = true;
            estaSendoDetectadoEmCombate = true;
        }
        if (other.CompareTag("TiroInimigo"))
        {
            Dano(25, true);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Inimigo"))
        {
            emCombate = true;
            estaSendoDetectadoEmCombate = true;

        }
        else estaSendoDetectadoEmCombate = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interação") && other.gameObject == interagirObject)
        {
            interagirObject = null;
            if(Prefab_E_Salvo != null)
            {
                Destroy(Prefab_E_Salvo);
                Prefab_E_Salvo = null;
            }

        }
        if (other.CompareTag("Inimigo"))
        {
            estaSendoDetectadoEmCombate = false;
            Invoke("DesativarEmCombate", 2);
        }
    }

    void DesativarEmCombate()
    {

        if (!estaSendoDetectadoEmCombate)
        {
            emCombate = false;

        }
    }

    public void PlayerAtaque()
    {
        if (inimigos[idMenorDistancia].TryGetComponent<EnemyController>(out EnemyController inimigo))
        {
            if (inimigos[idMenorDistancia] != null && Vector3.Distance(transform.position, inimigos[idMenorDistancia].transform.position) <= inimigo.distanciaDeGolpe && emCombate && !anim.GetBool("bloqueando"))
            {
                Debug.Log("inimigo");
                inimigo.Dano();
            }
        }
        else if (inimigos[idMenorDistancia].TryGetComponent<BossGelo>(out BossGelo boss))
        {
            if (inimigos[idMenorDistancia] != null && Vector3.Distance(transform.position, inimigos[idMenorDistancia].transform.position) <= boss.distanciaDeGolpe && emCombate && !anim.GetBool("bloqueando"))
            {
                Debug.Log("boss");
                boss.Dano();
            }
        }
    }

    public void DesativarAtaque()
    {
        anim.SetBool("ataque", false);
    }
}
