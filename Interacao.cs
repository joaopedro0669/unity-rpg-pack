using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interacao : MonoBehaviour
{
    //public static Interacao geral;

    [Header("Funções:")]
    [SerializeField] public bool inicioAutomatico;
    [SerializeField] bool AutoLink = true;
    [Space]
    [SerializeField] public bool dialogo;
    [SerializeField] bool marcarOuDesmarcarLocal;
    [SerializeField] bool fixarCameraOuPlayer;
    [SerializeField] bool ativarEDesativarObjetos;
    //[SerializeField] bool escreverObjetivoNaTela;
    [SerializeField] bool autoDestruirDepoisDaInteracao = false;
    [SerializeField] bool aumentarObjetivo = false;



    [Header("Diálogos Confs:")]
    [SerializeField] public bool dialogoUnico = true;
    [SerializeField] bool marcarLocalAposTerminar = true;
    [SerializeField] bool fixarCameraOuPlayerDuranteLegendas = true;
    [SerializeField] bool autodestruirAposTerminar = false;
    [SerializeField] bool aumentarObjetivoAposTerminar = true;
    [SerializeField] float delayLetras = 0.01f;

    [SerializeField] Text legendaLocal;
    [SerializeField] Text nome;
    [SerializeField] Text fundoLegendasInfo;
    [SerializeField] RawImage fundoLegendas;
    [SerializeField] GameObject[] rostos;
    [SerializeField] string[] nomes;
    [SerializeField] List<Dialogos> falas;
    [SerializeField] GameObject[] objectsParaOcultarEDesocultar;
    int idDialogo = 0;
    int letra = 0;
    float tempo = 0;
    Coroutine ControlarLegendas;
    KeyCode pularLegendas;



    [Header("Marcar Local Confs:")]
    [SerializeField] bool desmarcarLocal = false;
    [SerializeField] Transform localParaMarcar;



    [Header("Fixar player ou camera Confs:")]
    [SerializeField] bool fixarPlayer = true;
    [SerializeField] bool fixarCamera = true;
    [SerializeField] bool terminarComAsLegendas = true;
    [SerializeField] float tempoDeCameraOuPlayerFixos;
    
    [SerializeField] Transform player;
    [SerializeField] Transform camPlayer;
    [SerializeField] Transform camSegPlayer;
    [SerializeField] Transform localFixoPlayer;
    [SerializeField] Transform localFixoCamera;



    [Header("Ativar e(ou) desativar objetos Confs:")]
    [SerializeField] GameObject[] objetosParaAtivar;
    [SerializeField] GameObject[] objetosParaDesativar;



    [Header("Escrever objetivo na tela Confs:")]
    [SerializeField] string textoDoObjetivo;



    [Header("Autodestruição Confs:")]
    [SerializeField] GameObject MainObject;


    void Start()
    {
        //geral = this;
        if (AutoLink)
        {
            legendaLocal = GameObject.FindGameObjectWithTag("Legenda Local").GetComponent<Text>();
            nome = GameObject.FindGameObjectWithTag("Legenda Nome").GetComponent<Text>();
            fundoLegendasInfo = GameObject.FindGameObjectWithTag("Legenda Fundo Info").GetComponent<Text>();
            fundoLegendas = GameObject.FindGameObjectWithTag("Legenda Fundo").GetComponent<RawImage>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            camPlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            camSegPlayer = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Transform>();
        }
        if (dialogo)
        {
            legendaLocal.text = "";
            nome.text = "";
            fundoLegendasInfo.text = "";
            fundoLegendas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FuncaoInteracao()
    {
        if (dialogo)
        {
            pularLegendas = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("KeyCode_PularLegendas"));
            idDialogo = 0;
            legendaLocal.text = "";
            nome.text = "";
            fundoLegendasInfo.text = $"Pressione {PlayerPrefs.GetString("KeyCode_PularLegendas").ToUpper()} para pular";
            fundoLegendas.enabled = true;
            if (ControlarLegendas != null) StopCoroutine(ControlarLegendas);
            ControlarLegendas = StartCoroutine(Legendas());
        }

        if (marcarOuDesmarcarLocal)
        {
            MarcarLocal();
        }

        if (fixarCameraOuPlayer)
        {
            FixarCameraOuPlayer();
        }

        if (ativarEDesativarObjetos)
        {
            if (objetosParaAtivar.Length > 0) {
                foreach (GameObject atv in objetosParaAtivar)
                    atv.SetActive(true); }
            if (objetosParaDesativar.Length > 0) {
                foreach (GameObject desatv in objetosParaDesativar)
                    desatv.SetActive(false); 
            }
        }

        /*if (escreverObjetivoNaTela)
        {
            PlayerPrefs.SetString("Save_ObjetivoEscrito", textoDoObjetivo);
        }*/

        if (aumentarObjetivo && !dialogo)
        {
            PlayerPrefs.SetInt("Save_ObjetivoAtual", PlayerPrefs.GetInt("Save_ObjetivoAtual") + 1);
        }



        if (autoDestruirDepoisDaInteracao)
        {
            if (MainObject == null)
                Destroy(gameObject);
            else
                Destroy(MainObject);
        }
    }

    IEnumerator Legendas()
    {


        if (objectsParaOcultarEDesocultar.Length > 0)
        {
            foreach (GameObject obj in objectsParaOcultarEDesocultar)
            {
                obj.SetActive(false);
            }
        }

        if (fixarCameraOuPlayerDuranteLegendas)
        {

            FixarCameraOuPlayer();
            yield return null;
            FixarCameraOuPlayer();
            yield return null;
            FixarCameraOuPlayer();
        }

        while (true)
        {
            tempo += Time.deltaTime; //configurar delay entre as letras
            nome.text = nomes[falas[idDialogo].idPersonagem]; //pegar o nome correto do personagem de cada fala
            foreach (GameObject carinha in rostos)
            {
                carinha.SetActive(false);
            }
            rostos[falas[idDialogo].idPersonagem].SetActive(true); //ativar o personagem configurado, após todos serem desativados

            if (letra < falas[idDialogo].fala.Length && tempo >= delayLetras && idDialogo < falas.Count)
            {

                legendaLocal.text += falas[idDialogo].fala[letra];
                letra++;
                tempo = 0;
            }

            if (Input.GetKeyDown(pularLegendas))
            {
                idDialogo += 1;
                legendaLocal.text = "";
                letra = 0;
            }

            if (idDialogo >= falas.Count)
            {
                foreach (var carinha in rostos)
                {
                    carinha.gameObject.SetActive(false);
                }

                if (marcarLocalAposTerminar)
                {
                    MarcarLocal();
                }

                TerminarCameraOuPlayerFixo();

                legendaLocal.text = "";
                nome.text = "";
                fundoLegendasInfo.text = "";
                fundoLegendas.enabled = false;

                if (objectsParaOcultarEDesocultar.Length > 0)
                {
                    foreach (var obj in objectsParaOcultarEDesocultar)
                    {
                        obj.SetActive(true);
                    }
                }

                if (aumentarObjetivoAposTerminar)
                {
                    PlayerPrefs.SetInt("Save_ObjetivoAtual", PlayerPrefs.GetInt("Save_ObjetivoAtual") + 1);
                }

                //dialogo = !dialogoUnico;

                if (dialogoUnico)
                {
                    this.tag = "Untagged";
                    this.enabled = false;
                }

                if (autodestruirAposTerminar)
                {
                    if (MainObject == null)
                        Destroy(gameObject);
                    else
                        Destroy(MainObject);
                }

                break;
            }
            yield return null;
        }
        

        
    }

    void MarcarLocal()
    {
        if (!desmarcarLocal)
        {
            PlayerPrefs.SetString("LocalMarcado", "Sim");
            PlayerPrefs.SetString("CoordenadasDeLocalMarcado", localParaMarcar.position.x + "x" + localParaMarcar.position.y + "x" + localParaMarcar.position.z);
        }
        else PlayerPrefs.SetString("LocalMarcado", "Não");
    }

    void FixarCameraOuPlayer()
    {
        if (fixarCamera)
        {
            PlayerPrefs.SetString("CâmeraSeguirPlayer", "Não");
            camPlayer.position = localFixoCamera.position;
            camPlayer.eulerAngles = localFixoCamera.eulerAngles;
            camSegPlayer.gameObject.GetComponent<Camera>().enabled = false;
        }
        if (fixarPlayer)
        {
            PlayerPrefs.SetString("PlayerLivre", "Não");
            player.position = localFixoPlayer.position;
            player.eulerAngles = localFixoPlayer.eulerAngles;
        }

        if (!terminarComAsLegendas)
        {
            Invoke("TerminarCameraOuPlayerFixo", tempoDeCameraOuPlayerFixos);
        }
    }

    void TerminarCameraOuPlayerFixo()
    {
        
        PlayerPrefs.SetString("PlayerLivre", "Sim");
        PlayerPrefs.SetString("CâmeraSeguirPlayer", "");
        camSegPlayer.gameObject.GetComponent<Camera>().enabled = true;
    }
    

}

[System.Serializable]
public class Dialogos
{
    public string fala;
    public int idPersonagem;
}
