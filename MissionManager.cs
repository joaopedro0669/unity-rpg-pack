using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public int ultimoSalvamento = 0;
    [SerializeField] TMP_Text objetivoEscritoText;
    [SerializeField] Transform player;
    [SerializeField] bool MarcarLocalInicial;
    [SerializeField] Transform localParaMarcar;
    [Header("Fionna Daniela:")]
    [SerializeField] List<Objetos> FionnaDaniela;
    [Header("Filha da Fionna:")]
    [SerializeField] List<Objetos> FilhaFionna;
    [Header("Sábio:")]
    [SerializeField] List<Objetos> Sabio;
    [Header("Ex-nômade:")]
    [SerializeField] List<Objetos> Nomade;
    [Header("Enemy Managers:")]
    [SerializeField] List<Objetos> EnManager;
    [Header("Outros:")]
    [SerializeField] List<Objetos> Outros;
    [Header("Objetivo Escrito:")]
    [SerializeField] List<Objetivo_Escrito> ObjetivoEscrito;

    void Start()
    {
        ultimoSalvamento = PlayerPrefs.GetInt("Save_ObjetivoAtual");
        Carregar(true);

        //PlayerPrefs.SetInt("Save_ObjetivoAtual", 0);
        if (PlayerPrefs.GetString("Save_ObjetivoEscrito") == "")
            PlayerPrefs.SetString("Save_ObjetivoEscrito", ObjetivoEscrito[0].objetivoText);

        if (PlayerPrefs.GetString("Save_ObjetivoEscrito")[PlayerPrefs.GetString("Save_ObjetivoEscrito").Length - 1].ToString() == ".")
            objetivoEscritoText.text = $"Objetivo({PlayerPrefs.GetInt("Save_ObjetivoAtual")}): {PlayerPrefs.GetString("Save_ObjetivoEscrito")}";
        else
            objetivoEscritoText.text = $"Objetivo({PlayerPrefs.GetInt("Save_ObjetivoAtual")}): {PlayerPrefs.GetString("Save_ObjetivoEscrito")}.";

        if(MarcarLocalInicial && PlayerPrefs.GetInt("Save_ObjetivoAtual") == 0)
        {
            PlayerPrefs.SetString("LocalMarcado", "Sim");
            PlayerPrefs.SetString("CoordenadasDeLocalMarcado", localParaMarcar.position.x + "x" + localParaMarcar.position.y + "x" + localParaMarcar.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ultimoSalvamento != PlayerPrefs.GetInt("Save_ObjetivoAtual")){
            Carregar(false);
            Salvar();
            if (PlayerPrefs.GetString("Save_ObjetivoEscrito")[PlayerPrefs.GetString("Save_ObjetivoEscrito").Length - 1].ToString() == ".")
                objetivoEscritoText.text = $"Objetivo({PlayerPrefs.GetInt("Save_ObjetivoAtual")}): {PlayerPrefs.GetString("Save_ObjetivoEscrito")}";
            else
                objetivoEscritoText.text = $"Objetivo({PlayerPrefs.GetInt("Save_ObjetivoAtual")}): {PlayerPrefs.GetString("Save_ObjetivoEscrito")}.";
        }

        
    }
    void Carregar(bool posicaoPlayer)
    {
        if (FionnaDaniela.Count > 0)
            foreach (var i in FionnaDaniela)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else
                {
                    i.objeto.SetActive(false);
                }

            }
        if (FilhaFionna.Count > 0)
            foreach (var i in FilhaFionna)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else
                {
                    i.objeto.SetActive(false);
                }

            }
        if (Sabio.Count > 0)
            foreach (var i in Sabio)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else
                {
                    i.objeto.SetActive(false);
                }

            }
        if (Nomade.Count > 0)
            foreach (var i in Nomade)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else
                {
                    i.objeto.SetActive(false);
                }

            }
        if (EnManager.Count > 0)
            foreach (var i in EnManager)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        //não vai acontecer nada, pq o próprio enemy manager se desativa ou simplesmente não é ativado
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        //não vai acontecer nada, pq o próprio enemy manager se desativa ou simplesmente não é ativado
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        //não vai acontecer nada, pq o próprio enemy manager se desativa ou simplesmente não é ativado
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        //não vai acontecer nada, pq o próprio enemy manager se desativa ou simplesmente não é ativado
                    }
                }
                else
                {
                    //não vai acontecer nada, pq o próprio enemy manager se desativa ou simplesmente não é ativado
                }

            }
        if (Outros.Count > 0)
            foreach (var i in Outros)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        i.objeto.SetActive(true);
                    }
                    else
                    {
                        i.objeto.SetActive(false);
                    }
                }
                else
                {
                    i.objeto.SetActive(false);
                }

            }
        if (ObjetivoEscrito.Count > 0)
            foreach (var i in ObjetivoEscrito)
            {
                if (i.condicional1 == ">" && i.minimo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", "Erro ao processar o objetivo atual.");
                    }
                }
                else if (i.condicional1 == "<" && i.minimo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", "Erro ao processar o objetivo atual.");
                    }
                }
                else if (i.condicional1 == "==" && i.minimo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", "Erro ao processar o objetivo atual.");
                    }
                }
                else if (i.condicional1 == "!=" && i.minimo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                {
                    if (i.condicional2 == ">" && i.maximo < PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "<" && i.maximo > PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "==" && i.maximo == PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else if (i.condicional2 == "!=" && i.maximo != PlayerPrefs.GetInt("Save_ObjetivoAtual"))
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", i.objetivoText);
                        break;
                    }
                    else
                    {
                        PlayerPrefs.SetString("Save_ObjetivoEscrito", "Erro ao processar o objetivo atual.");
                    }
                }
                else
                {
                    PlayerPrefs.SetString("Save_ObjetivoEscrito", "Erro ao processar o objetivo atual.");
                }

            }


        ultimoSalvamento = PlayerPrefs.GetInt("Save_ObjetivoAtual");

        if (posicaoPlayer)
        {
            player.position = Vector3.right * PlayerPrefs.GetFloat("Save_PlayerPosicaoX") + Vector3.up * PlayerPrefs.GetFloat("Save_PlayerPosicaoY") + Vector3.forward * PlayerPrefs.GetFloat("Save_PlayerPosicaoZ");
        }
    }
    public void Salvar()
    {
        PlayerPrefs.SetFloat("Save_PlayerPosicaoX", player.position.x);
        PlayerPrefs.SetFloat("Save_PlayerPosicaoY", player.position.y);
        PlayerPrefs.SetFloat("Save_PlayerPosicaoZ", player.position.z);
    }
}

[System.Serializable]
public class Objetos
{
    public GameObject objeto;
    public int minimo;
    public int maximo;
    public string condicional1;
    public string condicional2;

}

[System.Serializable]
public class Objetivo_Escrito
{
    public string objetivoText;
    public int minimo;
    public int maximo;
    public string condicional1;
    public string condicional2;

}