using UnityEngine;
using UnityEngine.UI;

public class FitToScreen : MonoBehaviour
{
    public Image image; // Assign your Image component in the Inspector

    void Start()
    {
        if (image != null)
        {
            AdjustBackgroundScale();
        }
        else
        {
            Debug.LogWarning("Image component not assigned.");
        }
    }

    void AdjustBackgroundScale()
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();

        // Set anchors to the middle of the parent (Canvas)
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // Ensures it scales from the center

        // Calculate screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Get the Image component's sprite size
        float imageWidth = rectTransform.rect.width;
        float imageHeight = rectTransform.rect.height;

        // Calculate scale factors
        float scaleX = screenWidth / imageWidth;
        float scaleY = screenHeight / imageHeight;

        // Apply the larger scale factor to ensure the image covers the entire screen
        float scale = Mathf.Max(scaleX, scaleY);

        // Adjust scale of the Image to fit the screen
        rectTransform.localScale = new Vector3(scale, scale, 1);

        // Center the image
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
