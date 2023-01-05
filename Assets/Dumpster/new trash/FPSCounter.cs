using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    //FPS counter
    public float deltaTime;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateText), 0, 1f);
    }

    void UpdateText()
    {
        text.text = (1f / Time.unscaledDeltaTime).ToString("N1") + " FPS";
    }
}