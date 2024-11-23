using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerazoom : MonoBehaviour
{
    public float normalZoom = 5f; // Zoom padrão (orthographicSize ou Field of View)
    public float zoomOutLevel = 8f; // Zoom reduzido (orthographicSize ou Field of View)
    public float zoomSpeed = 2f; // Velocidade de transição do zoom

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("Este script deve ser anexado a um objeto com um componente Camera.");
        }
    }

    void Update()
    {
        if (cam == null) return;

        // Checa se a tecla "L" está pressionada
        if (Input.GetKey(KeyCode.L))
        {
            ApplyZoom(zoomOutLevel);
        }
        else
        {
            // Retorna ao zoom padrão
            ApplyZoom(normalZoom);
        }
    }

    void ApplyZoom(float targetZoom)
    {
        if (cam.orthographic)
        {
            // Ajusta o orthographicSize para câmeras orthographic
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        }
        else
        {
            // Ajusta o Field of View para câmeras em perspectiva
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);
        }
    }

}
