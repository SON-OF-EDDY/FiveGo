using UnityEngine;
using TMPro;

public class AdjustButtonSize : MonoBehaviour
{
    public GameObject[] rotateButtons;
    public int smallButtonSize = 100;  // Default size for smaller screens
    public int largeButtonSize = 200;  // Size for larger screens

    // Thresholds for screen dimensions to determine if the screen is "spacious"
    public float widthThreshold = 1920.0f;
    public float heightThreshold = 1080.0f;

    // TextMeshPro elements to adjust
    public TextMeshProUGUI endGameNotification;
    public float smallScreenEndGameFontSize = 24f;  // Font size for smaller screens
    public float largeScreenEndGameFontSize = 36f;  // Font size for larger screens
    public float smallScreenEndGameWidth = 300f;    // Width for smaller screens
    public float largeScreenEndGameWidth = 500f;    // Width for larger screens

    public TextMeshProUGUI errorNotification;
    public float smallScreenErrorFontSize = 20f;  // Font size for smaller screens
    public float largeScreenErrorFontSize = 30f;  // Font size for larger screens

    void Start()
    {
        AdjustButtonSizes();
        AdjustTextMeshProFontSizesAndWidth();
    }

    void AdjustButtonSizes()
    {
        // Get the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Check if both dimensions exceed the thresholds
        bool isSpacious = screenWidth >= widthThreshold && screenHeight >= heightThreshold;

        // Set the button size based on the screen size
        int buttonSize = isSpacious ? largeButtonSize : smallButtonSize;

        // Log the size used to the console
        Debug.Log(isSpacious ? "Using large button size." : "Using small button size.");

        // Apply the size to all rotate buttons
        foreach (GameObject button in rotateButtons)
        {
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
            }
        }
    }

    void AdjustTextMeshProFontSizesAndWidth()
    {
        // Get the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Check if both dimensions exceed the thresholds
        bool isSpacious = screenWidth >= widthThreshold && screenHeight >= heightThreshold;

        // Set font sizes and width based on the screen size
        float endGameFontSize = isSpacious ? largeScreenEndGameFontSize : smallScreenEndGameFontSize;
        float errorFontSize = isSpacious ? largeScreenErrorFontSize : smallScreenErrorFontSize;
        float endGameWidth = isSpacious ? largeScreenEndGameWidth : smallScreenEndGameWidth;

        // Log the font sizes and width used to the console
        Debug.Log("End Game Notification font size adjusted for " + (isSpacious ? "large" : "small") + " screen.");
        Debug.Log("Error Notification font size adjusted for " + (isSpacious ? "large" : "small") + " screen.");
        Debug.Log("End Game Notification width adjusted for " + (isSpacious ? "large" : "small") + " screen.");

        // Apply the font sizes and width to the TextMeshProUGUI elements
        if (endGameNotification != null)
        {
            endGameNotification.fontSize = endGameFontSize;
            RectTransform endGameRectTransform = endGameNotification.GetComponent<RectTransform>();
            if (endGameRectTransform != null)
            {
                endGameRectTransform.sizeDelta = new Vector2(endGameWidth, endGameRectTransform.sizeDelta.y);
            }
        }

        if (errorNotification != null)
        {
            errorNotification.fontSize = errorFontSize;
        }
    }
}
