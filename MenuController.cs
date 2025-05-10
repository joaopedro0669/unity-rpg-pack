using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //new game e continuar
    [SerializeField] GameObject BarraCarreg_Fundo;
    [SerializeField] Image barraDeCarregamento;
    [SerializeField] GameObject botaoContinuar;
    [SerializeField] GameObject inicio;
    [SerializeField] GameObject dificuldade;
    Coroutine CSAS;
    bool introAtiva = false;

    //confs//
    [Header("Configurações Links:")]
    [SerializeField] GameObject configuracoesMenu;
    [SerializeField] GameObject videoMenu;
    [SerializeField] GameObject graficosMenu;
    [SerializeField] GameObject posProcessamentoMenu;
    [SerializeField] GameObject controlesMenu;
    [SerializeField] GameObject jogabilidadeMenu;
    [SerializeField] VideoPlayer introVideo;
    KeyCode novaTecla;

    [Header("Links Vídeo:")]
    [SerializeField] Dropdown resolucoes;
    //[SerializeField] InputField resolucaoW;
    //[SerializeField] InputField resolucaoH;
    [SerializeField] Toggle telaCheia;
    [SerializeField] Dropdown framesPorSegundo;
    [SerializeField] Toggle vSync;
    [SerializeField] Toggle mostrarFPS;
    [SerializeField] Slider fov;
    [SerializeField] Text fovNum;
    List<string> resDisponiveisString = new List<string>();
    string[] res = new string[2];

    [Header("Links Gráficos:")]
    [SerializeField] Dropdown QDTexturas;
    [SerializeField] Dropdown Sombras;
    [SerializeField] Dropdown AAQualidade;
    [SerializeField] Dropdown AAMode;
    [SerializeField] Dropdown Qualidade;

    [Header("Links Pós-Processamento:")]
    [SerializeField] Dropdown PosProcessamento;

    [Header("Links Controles")]
    [SerializeField] InputField sensibilidade;
    //[SerializeField] Slider suavizacao;
    //[SerializeField] Text suavizacaoNum;
    [SerializeField] Text frenteText;
    [SerializeField] Text trasText;
    [SerializeField] Text esquerdaText;
    [SerializeField] Text direitaText;
    [SerializeField] Text correrText;
    [SerializeField] Text bloqueioText;
    [SerializeField] Text interagirText;
    [SerializeField] Text modoCombateText;
    [SerializeField] Text ataqueText;
    [SerializeField] Text pularLegendasText;
    bool escolherTecla = false;
    Event evento;

    //[Header("Links Jogabilidade:")]
    //[SerializeField] Dropdown Dificuldade;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (PlayerPrefs.GetInt("Save_ObjetivoAtual") == 0)
            botaoContinuar.SetActive(false);

        //confs
        Resolution[] resDisponiveis = Screen.resolutions;

        foreach (Resolution r in resDisponiveis)
        {
            if (resDisponiveisString.Count >= 1)
            {
                if (resDisponiveisString[resDisponiveisString.Count - 1] != r.width + " X " + r.height)
                    resDisponiveisString.Add(r.width + " X " + r.height);
            }
            else
                resDisponiveisString.Add(r.width + " X " + r.height);
        }

        resolucoes.AddOptions(resDisponiveisString);


        if (PlayerPrefs.GetString("Aberto") == "") //pra setar configurações padrões na primeira vez que abrir
        {
            PlayerPrefs.SetInt("SaveConfs_Resolucao", resDisponiveisString.Count - 1);
            PlayerPrefs.SetInt("SaveConfs_TelaCheia", 1); //1 = true
            PlayerPrefs.SetInt("SaveConfs_FPS", 2);
            PlayerPrefs.SetInt("SaveConfs_VSync", 0); //0 = false
            PlayerPrefs.SetInt("SaveConfs_MostrarFPS", 0); //0 = false
            PlayerPrefs.SetFloat("SaveConfs_FOV", 50);

            PlayerPrefs.SetInt("SaveConfs_QualidadeDasTexturas", 0);
            PlayerPrefs.SetInt("SaveConfs_QualidadeDasSombras", 3);
            PlayerPrefs.SetInt("SaveConfs_ModoAntiAliasing", 3);
            PlayerPrefs.SetInt("SaveConfs_ForcaAntiAliasing", 2);
            PlayerPrefs.SetInt("SaveConfs_Qualidade", 2);

            PlayerPrefs.SetInt("SaveConfs_PosProcessamento", 3); //vai mudar com o tempo { oclusão, bloom(talvez), motion blur, etc }

            PlayerPrefs.SetFloat("Sensibilidade", 2);



            PlayerPrefs.SetString("Aberto", "Sim");
        }



        if (PlayerPrefs.GetString("KeyCode_Frente") == "")
        {
            PlayerPrefs.SetString("KeyCode_Frente", "W");
            PlayerPrefs.SetString("KeyCode_Tras", "S");
            PlayerPrefs.SetString("KeyCode_Esquerda", "A");
            PlayerPrefs.SetString("KeyCode_Direita", "D");
            PlayerPrefs.SetString("KeyCode_Correr", "LeftShift");
            PlayerPrefs.SetString("KeyCode_Bloqueio", "Q");
            PlayerPrefs.SetString("KeyCode_Interagir", "E");
            PlayerPrefs.SetString("KeyCode_ModoCombate", "Space");
            PlayerPrefs.SetString("KeyCode_Ataque", "Mouse0");
            PlayerPrefs.SetString("KeyCode_PularLegendas", "Space");
        }

        Carregar();

        Aplicar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            VoltarAsConfigurações();
            configuracoesMenu.SetActive(false);
            dificuldade.SetActive(false);

            inicio.SetActive(true);
        }

        fovNum.text = fov.value.ToString(); //atualizar valores

        if(introAtiva && Input.GetKeyDown(KeyCode.Space))
        {
            ComecarRotinaCarregarSceneASync(introVideo);
        }
    }

    public void CarregarScene(string cena)
    {
        StartCoroutine(CarregarSceneASync(cena));
    }
    IEnumerator CarregarSceneASync(string cena)
    {
        BarraCarreg_Fundo.SetActive(true);
        AsyncOperation OperacaoCarregamento = SceneManager.LoadSceneAsync(cena);
        while (!OperacaoCarregamento.isDone)
        {
            float progresso = Mathf.Clamp01(OperacaoCarregamento.progress / 0.9f);
            barraDeCarregamento.fillAmount = progresso;
            yield return null;
        }
    }
    public void NewGame()
    {
        dificuldade.SetActive(true);
        inicio.SetActive(false);
    }
    public void CarregarNovoJogo(int dif)
    {
        PlayerPrefs.SetInt("Save_Introducao", 0);
        PlayerPrefs.SetFloat("Save_PlayerPosicaoX", -536.7739f);
        PlayerPrefs.SetFloat("Save_PlayerPosicaoY", -1.506744f);
        PlayerPrefs.SetFloat("Save_PlayerPosicaoZ", 41.05523f);
        PlayerPrefs.SetInt("Save_ObjetivoAtual", 0);
        PlayerPrefs.SetString("Save_ObjetivoEscrito", "");
        PlayerPrefs.SetString("LocalMarcado", "Não");

        PlayerPrefs.SetInt("Dificuldade", dif);

        dificuldade.SetActive(false);
        introVideo.gameObject.SetActive(true);
        introAtiva = true;
        introVideo.loopPointReached += ComecarRotinaCarregarSceneASync;

        //StartCoroutine(CarregarSceneASync("Jogo"));
    }

    void ComecarRotinaCarregarSceneASync(VideoPlayer vp)
    {
        introVideo.gameObject.SetActive(false);
        if(CSAS == null)
            CSAS = StartCoroutine(CarregarSceneASync("Jogo"));
    }

    //confs

    private void OnGUI()
    {
        evento = Event.current;
        if (escolherTecla && (evento.isKey || evento.isMouse))
        {
            if (evento.isKey)
                novaTecla = evento.keyCode;
            if (evento.isMouse)
                novaTecla = (KeyCode)System.Enum.Parse(typeof(KeyCode), $"Mouse{evento.button}");

            if (novaTecla != KeyCode.None)
                escolherTecla = false;
            }
    }

    public void VoltarInicio()
    {
        inicio.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void AtivarConfiguracoes()
    {
        Carregar();
        configuracoesMenu.SetActive(true);
        inicio.SetActive(false);
    }

    public void Sair()
    {
        Application.Quit();
    }

    public void AtivarVideo()
    {
        videoMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void AtivarGraficos()
    {
        graficosMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void AtivarPosProcessamento()
    {
        posProcessamentoMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void AtivarControles()
    {
        controlesMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void AtivarJogabilidade()
    {
        jogabilidadeMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
    }

    public void VoltarAsConfigurações()
    {
        videoMenu.SetActive(false);
        graficosMenu.SetActive(false);
        posProcessamentoMenu.SetActive(false);
        controlesMenu.SetActive(false);
        jogabilidadeMenu.SetActive(false);

        configuracoesMenu.SetActive(true);

        Carregar();
    }

    //Funções ---------
    void Carregar()
    {
        //vídeo
        resolucoes.value = PlayerPrefs.GetInt("SaveConfs_Resolucao");
        telaCheia.isOn = PlayerPrefs.GetInt("SaveConfs_TelaCheia") == 1;
        framesPorSegundo.value = PlayerPrefs.GetInt("SaveConfs_FPS");
        vSync.isOn = PlayerPrefs.GetInt("SaveConfs_VSync") == 1;
        mostrarFPS.isOn = PlayerPrefs.GetInt("SaveConfs_MostrarFPS") == 1;
        fov.value = PlayerPrefs.GetFloat("SaveConfs_FOV");

        //gráficos
        QDTexturas.value = PlayerPrefs.GetInt("SaveConfs_QualidadeDasTexturas");
        Sombras.value = PlayerPrefs.GetInt("SaveConfs_QualidadeDasSombras");
        AAMode.value = PlayerPrefs.GetInt("SaveConfs_ModoAntiAliasing");
        AAQualidade.value = PlayerPrefs.GetInt("SaveConfs_ForcaAntiAliasing");
        Qualidade.value = PlayerPrefs.GetInt("SaveConfs_Qualidade");

        //pós-processamento
        PosProcessamento.value = PlayerPrefs.GetInt("SaveConfs_PosProcessamento");

        //controles
        sensibilidade.text = PlayerPrefs.GetFloat("Sensibilidade").ToString();
        if (PlayerPrefs.GetFloat("Sensibilidade") <= 0)
        {
            PlayerPrefs.SetFloat("Sensibilidade", 2);
        }
        frenteText.text = PlayerPrefs.GetString("KeyCode_Frente");
        trasText.text = PlayerPrefs.GetString("KeyCode_Tras");
        esquerdaText.text = PlayerPrefs.GetString("KeyCode_Esquerda");
        direitaText.text = PlayerPrefs.GetString("KeyCode_Direita");
        correrText.text = PlayerPrefs.GetString("KeyCode_Correr");
        bloqueioText.text = PlayerPrefs.GetString("KeyCode_Bloqueio");
        interagirText.text = PlayerPrefs.GetString("KeyCode_Interagir");
        modoCombateText.text = PlayerPrefs.GetString("KeyCode_ModoCombate");
        ataqueText.text = PlayerPrefs.GetString("KeyCode_Ataque");
        pularLegendasText.text = PlayerPrefs.GetString("KeyCode_PularLegendas");

        //jogabilidade
        //nada

    }

    public void Aplicar()
    {
        SetarQualidade();
        SetarResolucao();
        TelaCheia();
        SetarFPS();
        SetarVSync();
        MostrarFPS();
        SetarFOV();
        SetarQualidadeDasTexturas();
        SetarQualidadeDasSombras();
        SetarPosProcessamento();
        SetarAntiAliasingMode();
        SetarAntiAliasingQuality();
        SetarSensibilidade();
        SetarTeclas();
    }

    public void VincularTeclaFunction(string tecla) => StartCoroutine(VincularTecla(tecla));

    IEnumerator VincularTecla(string tecla)
    {
        escolherTecla = true;

        switch (tecla)
        {
            case "frente":
                frenteText.text = "";
                break;
            case "tras":
                trasText.text = "";
                break;
            case "esquerda":
                esquerdaText.text = "";
                break;
            case "direita":
                direitaText.text = "";
                break;
            case "correr":
                correrText.text = "";
                break;
            case "bloqueio":
                bloqueioText.text = "";
                break;
            case "interagir":
                interagirText.text = "";
                break;
            case "modoCombate":
                modoCombateText.text = "";
                break;
            case "ataque":
                ataqueText.text = "";
                break;
            case "pularLegendas":
                pularLegendasText.text = "";
                break;
            default:
                Debug.Log("Deu ruim.");
                break;
        }

        while (escolherTecla)
            yield return null;

        //Debug.Log(novaTecla);

        switch (tecla)
        {
            case "frente":
                frenteText.text = $"{novaTecla}";
                break;
            case "tras":
                trasText.text = $"{novaTecla}";
                break;
            case "esquerda":
                esquerdaText.text = $"{novaTecla}";
                break;
            case "direita":
                direitaText.text = $"{novaTecla}";
                break;
            case "correr":
                correrText.text = $"{novaTecla}";
                break;
            case "bloqueio":
                bloqueioText.text = $"{novaTecla}";
                break;
            case "interagir":
                interagirText.text = $"{novaTecla}";
                break;
            case "modoCombate":
                modoCombateText.text = $"{novaTecla}";
                break;
            case "ataque":
                ataqueText.text = $"{novaTecla}";
                break;
            case "pularLegendas":
                pularLegendasText.text = $"{novaTecla}";
                break;
            default:
                Debug.Log("Deu ruim.");
                break;
        }
    }

    void SetarTeclas()
    {
        PlayerPrefs.SetString("KeyCode_Frente", frenteText.text);
        PlayerPrefs.SetString("KeyCode_Tras", trasText.text);
        PlayerPrefs.SetString("KeyCode_Esquerda", esquerdaText.text);
        PlayerPrefs.SetString("KeyCode_Direita", direitaText.text);
        PlayerPrefs.SetString("KeyCode_Correr", correrText.text);
        PlayerPrefs.SetString("KeyCode_Bloqueio", bloqueioText.text);
        PlayerPrefs.SetString("KeyCode_Interagir", interagirText.text);
        PlayerPrefs.SetString("KeyCode_ModoCombate", modoCombateText.text);
        PlayerPrefs.SetString("KeyCode_Ataque", ataqueText.text);
        PlayerPrefs.SetString("KeyCode_PularLegendas", pularLegendasText.text);
    }

    void SetarResolucao()
    {
        res = resDisponiveisString[resolucoes.value].Split('X');

        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[1]), Screen.fullScreen);

        PlayerPrefs.SetInt("SaveConfs_Resolucao", resolucoes.value);
    }

    void TelaCheia()
    {
        Screen.fullScreen = telaCheia.isOn;
        if (telaCheia.isOn) PlayerPrefs.SetInt("SaveConfs_TelaCheia", 1);
        else PlayerPrefs.SetInt("SaveConfs_TelaCheia", 0);
    }

    void SetarFPS()
    {
        switch (framesPorSegundo.value)
        {
            case 0:
                Application.targetFrameRate = 20;
                break;

            case 1:
                Application.targetFrameRate = 30;
                break;

            case 2:
                Application.targetFrameRate = 60;
                break;

            case 3:
                Application.targetFrameRate = 120;
                break;

            case 4:
                Application.targetFrameRate = 144;
                break;

            case 5:
                Application.targetFrameRate = 165;
                break;

            case 6:
                Application.targetFrameRate = 240;
                break;

            case 7:
                Application.targetFrameRate = 360;
                break;

            case 8:
                Application.targetFrameRate = -1;
                break;


        }

        PlayerPrefs.SetInt("SaveConfs_FPS", framesPorSegundo.value);
    }

    void SetarVSync()
    {
        if (vSync.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        if (vSync.isOn) PlayerPrefs.SetInt("SaveConfs_VSync", 1);
        else PlayerPrefs.SetInt("SaveConfs_VSync", 0);
    }

    void MostrarFPS()
    {
        if (mostrarFPS.isOn) PlayerPrefs.SetInt("SaveConfs_MostrarFPS", 1);
        else PlayerPrefs.SetInt("SaveConfs_MostrarFPS", 0);
    }

    void SetarFOV()
    {
        PlayerPrefs.SetFloat("SaveConfs_FOV", fov.value);
    }

    void SetarQualidadeDasTexturas()
    {
        QualitySettings.masterTextureLimit = QDTexturas.value;

        PlayerPrefs.SetInt("SaveConfs_QualidadeDasTexturas", QDTexturas.value);
    }

    void SetarQualidadeDasSombras()
    {
        PlayerPrefs.SetInt("SaveConfs_QualidadeDasSombras", Sombras.value);
    }

    void SetarPosProcessamento()
    {
        PlayerPrefs.SetInt("SaveConfs_PosProcessamento", PosProcessamento.value);
    }

    void SetarAntiAliasingMode()
    {
        PlayerPrefs.SetInt("SaveConfs_ModoAntiAliasing", AAMode.value);
    }

    void SetarAntiAliasingQuality()
    {
        PlayerPrefs.SetInt("SaveConfs_ForcaAntiAliasing", AAQualidade.value);
    }

    void SetarQualidade()
    {
        PlayerPrefs.SetInt("SaveConfs_Qualidade", Qualidade.value);
    }

    void SetarSensibilidade()
    {
        if (sensibilidade.text != "")
        {
            if (float.Parse(sensibilidade.text) <= 0) sensibilidade.text = PlayerPrefs.GetFloat("Sensibilidade").ToString();
            PlayerPrefs.SetFloat("Sensibilidade", float.Parse(sensibilidade.text));
        }
        else { sensibilidade.text = PlayerPrefs.GetFloat("Sensibilidade").ToString(); }
    }
}
