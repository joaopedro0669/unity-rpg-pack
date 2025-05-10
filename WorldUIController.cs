using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIController : MonoBehaviour
{
    Transform camPlayer;
    [SerializeField] bool isE_Prefab = false;
    [SerializeField] Text text;
    void Start()
    {
        camPlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        if(isE_Prefab)
            text.text = PlayerPrefs.GetString("KeyCode_Interagir");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + camPlayer.forward, Vector3.up);
    }
}
