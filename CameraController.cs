using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseX = 0;
    float mouseY = 0;
    RaycastHit hit;
    [SerializeField] Transform pivot;
    [SerializeField] float distCam;
    [SerializeField] float ajusteCol;
    [SerializeField] float ajusteScroll;
    //[SerializeField] float sensibilidade = 2;
    [SerializeField] bool ocultarMouse;
    [SerializeField] LayerMask layers;
    [SerializeField] Transform SegCam;

    void Start()
    {

        if (ocultarMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        PlayerPrefs.SetString("CâmeraSeguirPlayer", "Sim");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("CâmeraSeguirPlayer") == "Sim")
        {

            mouseX += Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("Sensibilidade");
            mouseY -= Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("Sensibilidade");

            mouseY = Mathf.Clamp(mouseY, -50, 50);

            pivot.eulerAngles = Vector3.right * mouseY + Vector3.up * mouseX;

            transform.position = pivot.position - pivot.forward * distCam;

            if (Physics.Linecast(pivot.position, transform.position, out hit, layers))
                transform.position = hit.point + transform.forward * ajusteCol;

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                distCam -= Input.GetAxis("Mouse ScrollWheel") * ajusteScroll * Time.timeScale;
            }
            distCam = Mathf.Clamp(distCam, 3, 10);

            SegCam.position = transform.position;
        }
        if (PlayerPrefs.GetString("CâmeraSeguirPlayer") == "")
        {
            transform.eulerAngles = pivot.eulerAngles;
            SegCam.eulerAngles = pivot.eulerAngles;
            PlayerPrefs.SetString("CâmeraSeguirPlayer", "Sim");
        }
    }
}
