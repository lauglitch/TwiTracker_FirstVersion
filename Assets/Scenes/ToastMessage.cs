using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    public float displayTime = 2f; // Tiempo de visualización del mensaje en segundos

    private TMP_Text messageText; // Referencia al componente Text del objeto de texto
    private CanvasGroup canvasGroup; // Referencia al componente CanvasGroup del objeto de mensaje

    private void Awake()
    {
        // Obtener las referencias a los componentes
        messageText = GetComponentInChildren<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowMessage(string message)
    {
        // Establecer el texto del mensaje
        messageText.text = message;

        // Mostrar el mensaje
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Desactivar el mensaje después del tiempo especificado
        Invoke("HideMessage", displayTime);
    }

    private void HideMessage()
    {
        // Ocultar el mensaje
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}

