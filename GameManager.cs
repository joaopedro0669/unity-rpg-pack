using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] float tempoDeAtualizacao;
    int frames = 0;
    float time = 0;
    Coroutine carregarFps;
    Coroutine AtualizarValores;

    [Header("Configurações Jogo:")]
    //[Tooltip("1 = Fácil; 2 = Médio; 3 = Difícil")] [SerializeField] [Range(1, 3)] int dificuldade = 1;

    [Header("Configurações Links:")]
    [SerializeField] Transform player;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject configuracoesMenu;
    [SerializeField] GameObject salvarMenu;
    [SerializeField] GameObject carregarMenu;
    [SerializeField] GameObject videoMenu;
    [SerializeField] GameObject graficosMenu;
    [SerializeField] GameObject posProcessamentoMenu;
    [SerializeField] GameObject controlesMenu;
    [SerializeField] GameObject jogabilidadeMenu;
    KeyCode novaTecla;
    bool menuAtivo = false;
    float sensCache;

    [Header("Links Salvar:")]
    [SerializeField] Text[] textInfoSlotsSalvar = new Text[8];
    
    [Header("Links Carregar:")]
    [SerializeField] Text[] textInfoSlotsCarregar = new Text[8];
    [SerializeField] Text modoCarregarText;
    [SerializeField] MissionManager MissionM;
    bool carregarSlot = true;

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
    [SerializeField] Text fpsText;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject cam2;
    List<string> resDisponiveisString = new List<string>();
    string[] res = new string[2];

    [Header("Links Gráficos:")]
    [SerializeField] Dropdown QDTexturas;
    [SerializeField] Dropdown Sombras;
    [SerializeField] Dropdown AAQualidade;
    [SerializeField] Dropdown AAMode;
    [SerializeField] Dropdown Qualidade;
    [SerializeField] Transform Luz_GB;
    [SerializeField] Light[] Luzes;
    [SerializeField] GameObject PosProcessamentoHigh;
    [SerializeField] GameObject PosProcessamentoMedium;
    [SerializeField] GameObject PosProcessamentoLow;

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

    [Header("Links Jogabilidade:")]
    [SerializeField] Dropdown Dificuldade;

    void Start()
    {
        //inicio

        //pegar resoluções do monitor
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
        


        

        for (int i = 1; i <= 8; i++) //não deixar os text info dos slots em branco
        {
            if (PlayerPrefs.GetString($"Save{i}_TextInfoSlot") == "")
                PlayerPrefs.SetString($"Save{i}_TextInfoSlot", "Não salvo.");
        }
        


        Luzes = Luz_GB.gameObject.GetComponentsInChildren<Light>();

        sensCache = PlayerPrefs.GetFloat("Sensibilidade"); // pro cache da sensibilidade já começar com o valor certo;



        Carregar();

        

        Aplicar();
    }

    // Update is called once per frame
    void Update()
    {
        //Fps();
        Pause();
    }

    private void OnGUI()
    {
        if (menuAtivo)
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
    }

    IEnumerator Fps()
    {
        while (true)
        {
            frames++;
            time += Time.deltaTime;
            if (time >= tempoDeAtualizacao)
            {
                fpsText.text = "FPS: " + Mathf.Round(frames / time);
                frames = 0;
                time -= tempoDeAtualizacao;
            }
            yield return null;
        }
    }

    IEnumerator AtualizarValoresFuncao()
    {
        while (true)
        {
            fovNum.text = fov.value.ToString();
            yield return null;
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuAtivo)
        {
            pauseMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PlayerPrefs.SetFloat("Sensibilidade", 0);
            Time.timeScale = 0;
            if (AtualizarValores != null) StopCoroutine(AtualizarValores);
            AtualizarValores = StartCoroutine(AtualizarValoresFuncao());

            menuAtivo = true;
        }

        
    }

    public void Retomar()
    {
        pauseMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetFloat("Sensibilidade", sensCache);
        Time.timeScale = 1;
        if (AtualizarValores != null) StopCoroutine(AtualizarValores);

        menuAtivo = false;
    }

    public void VoltarPauseMenu()
    {
        pauseMenu.SetActive(true);
        configuracoesMenu.SetActive(false);
        salvarMenu.SetActive(false);
        carregarMenu.SetActive(false);

        if (!carregarSlot)
            TrocarModoDoCarregar();
    }

    public void AtivarSalvar()
    {
        salvarMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void AtivarCarregar()
    {
        carregarMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void AtivarConfiguracoes()
    {
        Carregar();
        configuracoesMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void MenuPrincipal()
    {
        PlayerPrefs.SetFloat("Sensibilidade", sensCache);
        Time.timeScale = 1;
        if (AtualizarValores != null) StopCoroutine(AtualizarValores);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
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
        sensibilidade.text = sensCache.ToString();
        if (sensCache <= 0)
        {
            sensCache = 2;
            PlayerPrefs.SetFloat("Sensibilidade", sensCache);
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
        Dificuldade.value = PlayerPrefs.GetInt("Dificuldade") - 1;

        //Text Slots Salvar
        for (int i = 0; i < textInfoSlotsSalvar.Length; i++)
        {
            textInfoSlotsSalvar[i].text = PlayerPrefs.GetString($"Save{i+1}_TextInfoSlot");
        }

        //Text Slots Carregar
        for (int i = 0; i < textInfoSlotsCarregar.Length; i++)
        {
            textInfoSlotsCarregar[i].text = PlayerPrefs.GetString($"Save{i+1}_TextInfoSlot");
        }

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
        SetarDificuldade();
        SetarTeclas();
    }

    public void SalvarSlot(int slot)
    {
        string dif = "Sem acesso.";
        PlayerPrefs.SetFloat($"Save{slot}_PlayerPosicaoX", player.position.x);
        PlayerPrefs.SetFloat($"Save{slot}_PlayerPosicaoY", player.position.y);
        PlayerPrefs.SetFloat($"Save{slot}_PlayerPosicaoZ", player.position.z);
        PlayerPrefs.SetInt($"Save{slot}_ObjetivoAtual", PlayerPrefs.GetInt("Save_ObjetivoAtual"));
        PlayerPrefs.SetString($"Save{slot}_ObjetivoEscrito", PlayerPrefs.GetString("Save_ObjetivoEscrito"));
        PlayerPrefs.SetString($"Save{slot}_LocalMarcado", PlayerPrefs.GetString("LocalMarcado"));
        PlayerPrefs.SetString($"Save{slot}_CoordenadasDeLocalMarcado", PlayerPrefs.GetString("CoordenadasDeLocalMarcado"));
        PlayerPrefs.SetInt($"Save{slot}_Dificuldade", PlayerPrefs.GetInt("Dificuldade"));
        if (PlayerPrefs.GetInt("Dificuldade") == 1)
            dif = "Fácil";
        if (PlayerPrefs.GetInt("Dificuldade") == 2)
            dif = "Médio";
        if (PlayerPrefs.GetInt("Dificuldade") == 3)
            dif = "Difícil";

        PlayerPrefs.SetString($"Save{slot}_TextInfoSlot", $"{PlayerPrefs.GetString("Save_ObjetivoEscrito")}({PlayerPrefs.GetInt("Save_ObjetivoAtual")}); Local: {(int) player.position.x}, {(int) player.position.y}, {(int) player.position.z}; ({dif}).");

        //atualizar text info
        for (int i = 0; i < textInfoSlotsSalvar.Length; i++)
        {
            textInfoSlotsSalvar[i].text = PlayerPrefs.GetString($"Save{i + 1}_TextInfoSlot");
        }
        for (int i = 0; i < textInfoSlotsCarregar.Length; i++)
        {
            textInfoSlotsCarregar[i].text = PlayerPrefs.GetString($"Save{i + 1}_TextInfoSlot");
        }
    }

    public void TrocarModoDoCarregar()
    {
        carregarSlot = !carregarSlot;

        if (carregarSlot)
            modoCarregarText.text = "Carregar";
        else
            modoCarregarText.text = "Excluir";
    }

    public void CarregarOuExcluirSlot(int slot)
    {
        if (carregarSlot && PlayerPrefs.GetString($"Save{slot}_TextInfoSlot") != "Não salvo.")
        {
            PlayerPrefs.SetFloat("Save_PlayerPosicaoX", PlayerPrefs.GetFloat($"Save{slot}_PlayerPosicaoX"));
            PlayerPrefs.SetFloat("Save_PlayerPosicaoY", PlayerPrefs.GetFloat($"Save{slot}_PlayerPosicaoY"));
            PlayerPrefs.SetFloat("Save_PlayerPosicaoZ", PlayerPrefs.GetFloat($"Save{slot}_PlayerPosicaoZ"));
            PlayerPrefs.SetInt("Save_ObjetivoAtual", PlayerPrefs.GetInt($"Save{slot}_ObjetivoAtual"));
            PlayerPrefs.SetString("Save_ObjetivoEscrito", PlayerPrefs.GetString($"Save{slot}_ObjetivoEscrito"));
            PlayerPrefs.SetString("LocalMarcado", PlayerPrefs.GetString($"Save{slot}_LocalMarcado"));
            PlayerPrefs.SetString("CoordenadasDeLocalMarcado", PlayerPrefs.GetString($"Save{slot}_CoordenadasDeLocalMarcado"));
            PlayerPrefs.SetInt("Dificuldade", PlayerPrefs.GetInt($"Save{slot}_Dificuldade"));
            
            PlayerPrefs.SetFloat("Sensibilidade", sensCache);
            Time.timeScale = 1;
            if (AtualizarValores != null) StopCoroutine(AtualizarValores);

            MissionM.ultimoSalvamento = PlayerPrefs.GetInt("Save_ObjetivoAtual");

            UnityEngine.SceneManagement.SceneManager.LoadScene("Jogo");
        }

        if (!carregarSlot)
        {
            PlayerPrefs.SetString($"Save{slot}_TextInfoSlot", "Não salvo.");

            //atualizar text info
            for (int i = 0; i < textInfoSlotsSalvar.Length; i++)
            {
                textInfoSlotsSalvar[i].text = PlayerPrefs.GetString($"Save{i + 1}_TextInfoSlot");
            }
            for (int i = 0; i < textInfoSlotsCarregar.Length; i++)
            {
                textInfoSlotsCarregar[i].text = PlayerPrefs.GetString($"Save{i + 1}_TextInfoSlot");
            }
        }
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

        player.GetComponent<PlayerController>().SetarTeclas();
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
        if(telaCheia.isOn) PlayerPrefs.SetInt("SaveConfs_TelaCheia", 1);
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

        if(vSync.isOn) PlayerPrefs.SetInt("SaveConfs_VSync", 1);
        else PlayerPrefs.SetInt("SaveConfs_VSync", 0);
    }

    void MostrarFPS()
    {
        if (mostrarFPS.isOn)
        {
            fpsText.gameObject.SetActive(true);
            if (carregarFps != null) StopCoroutine(carregarFps);
            carregarFps = StartCoroutine(Fps());
        }
        else
        {
            fpsText.gameObject.SetActive(false);
            if (carregarFps != null) StopCoroutine(carregarFps);
        }

        if (mostrarFPS.isOn) PlayerPrefs.SetInt("SaveConfs_MostrarFPS", 1);
        else PlayerPrefs.SetInt("SaveConfs_MostrarFPS", 0);
    }

    void SetarFOV()
    {
        cam.GetComponent<Camera>().fieldOfView = fov.value;
        cam2.GetComponent<Camera>().fieldOfView = fov.value;

        PlayerPrefs.SetFloat("SaveConfs_FOV", fov.value);
    }

    void SetarQualidadeDasTexturas()
    {
        QualitySettings.masterTextureLimit = QDTexturas.value;

        PlayerPrefs.SetInt("SaveConfs_QualidadeDasTexturas", QDTexturas.value);
    }

    void SetarQualidadeDasSombras()
    {
        if (Sombras.value != 10)
        {
            foreach (Light l in Luzes)
            {
                QualitySettings.shadows = ShadowQuality.All;
                
                l.enabled = true;
                l.shadowCustomResolution = int.Parse(Sombras.captionText.text);
            }
            Luzes[0].GetComponent<CicloDiaENoite>().luzAmbienteMaxima = 1;
        }
        else if (Sombras.value == 10)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            foreach (Light l in Luzes)
            {
                l.enabled = false;
            }
            Luzes[0].GetComponent<CicloDiaENoite>().luzAmbienteMaxima = 1.5f;
        }

        PlayerPrefs.SetInt("SaveConfs_QualidadeDasSombras", Sombras.value);
    }

    void SetarPosProcessamento()
    {
        PosProcessamentoHigh.SetActive(PosProcessamento.value == 0);
        PosProcessamentoMedium.SetActive(PosProcessamento.value == 1);
        PosProcessamentoLow.SetActive(PosProcessamento.value == 2);

        PlayerPrefs.SetInt("SaveConfs_PosProcessamento", PosProcessamento.value);
    }

    void SetarAntiAliasingMode()
    {
        switch (AAMode.value)
        {
            case 0:
                cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                break;
            case 1:
                cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                break;
            case 2:
                cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                break;
            case 3:
                cam.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
                break;
        }

        PlayerPrefs.SetInt("SaveConfs_ModoAntiAliasing", AAMode.value);
    }

    void SetarAntiAliasingQuality()
    {
        switch (AAQualidade.value)
        {
            case 0:
                QualitySettings.antiAliasing = 8;
                break;
            case 1:
                QualitySettings.antiAliasing = 4;
                break;
            case 2:
                QualitySettings.antiAliasing = 2;
                break;
        }

        PlayerPrefs.SetInt("SaveConfs_ForcaAntiAliasing", AAQualidade.value);
    }

    void SetarQualidade()
    {
        switch (Qualidade.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(5, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(4, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(3, true);
                break;
            case 3:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 4:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 5:
                QualitySettings.SetQualityLevel(0, true);
                break;

        }

        PlayerPrefs.SetInt("SaveConfs_Qualidade", Qualidade.value);
    }

    void SetarSensibilidade()
    {
        if (sensibilidade.text != "")
        {
            if(float.Parse(sensibilidade.text) <= 0) sensibilidade.text = sensCache.ToString();
            sensCache = float.Parse(sensibilidade.text);
        }
        else { sensibilidade.text = PlayerPrefs.GetFloat("Sensibilidade").ToString(); }
    }

    void SetarDificuldade()
    {
        if (Dificuldade.value + 1 != PlayerPrefs.GetInt("Dificuldade"))
        {
            PlayerPrefs.SetInt("Dificuldade", Dificuldade.value + 1);

            PlayerPrefs.SetFloat("Sensibilidade", sensCache);
            Time.timeScale = 1;
            if (AtualizarValores != null) StopCoroutine(AtualizarValores);

            UnityEngine.SceneManagement.SceneManager.LoadScene("Jogo");
        }
    }
}
