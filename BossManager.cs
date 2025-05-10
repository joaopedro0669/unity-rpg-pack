using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    bool acabou = false;
    [SerializeField] bool aumentarObjetivo = true;
    [SerializeField] bool marcarLocal = true;
    [SerializeField] bool desmarcarLocal;
    [SerializeField] Transform localParaMarcar;
    [SerializeField] float tempoDeDestruicao = 5;
    [SerializeField] GameObject[] inimigos;
    int inimigosVivos = 0;
    void Start()
    {
        BossGelo[] inimigosEC = this.gameObject.GetComponentsInChildren<BossGelo>();
        inimigos = new GameObject[inimigosEC.Length];

        for(int i = 0; i < inimigos.Length; i++)
        {
            inimigos[i] = inimigosEC[i].gameObject;
        }

        StartCoroutine(GerenciarInimigos());
    }

    // Update is called once per frame
    void Update()
    {
        if (acabou)
            this.gameObject.SetActive(false);
    }

    IEnumerator GerenciarInimigos()
    {
        yield return null;
        while (true)
        {
            inimigosVivos = 0;
            foreach (GameObject i in inimigos)
            {
                if (i.tag == "Inimigo")
                    inimigosVivos++;
                yield return null;
            }

            if(inimigosVivos == 0)
            {
                if (aumentarObjetivo)
                {
                    PlayerPrefs.SetInt("Save_ObjetivoAtual", PlayerPrefs.GetInt("Save_ObjetivoAtual") + 1);
                }

                if (marcarLocal)
                {
                    PlayerPrefs.SetString("LocalMarcado", "Sim");
                    PlayerPrefs.SetString("CoordenadasDeLocalMarcado", localParaMarcar.position.x + "x" + localParaMarcar.position.y + "x" + localParaMarcar.position.z);
                }
                if (desmarcarLocal)
                {
                    PlayerPrefs.SetString("LocalMarcado", "Não");
                }

                Invoke("Desligar", tempoDeDestruicao);
                break;
            }
        }
    }
    void Desligar()
    {
        acabou = true;
        this.gameObject.SetActive(false);
    }
}
