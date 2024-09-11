using UnityEngine;
using UnityEngine.UI;

public class ButtonPositioner : MonoBehaviour
{
    public GameObject buttonTopRightCW;
    public GameObject buttonTopRightCCW;
    public GameObject buttonTopLeftCW;
    public GameObject buttonTopLeftCCW;
    public GameObject buttonBottomRightCW;
    public GameObject buttonBottomRightCCW;
    public GameObject buttonBottomLeftCW;
    public GameObject buttonBottomLeftCCW;

    public Canvas canvas;  // Reference to the Canvas containing the buttons

    void Start()
    {
        PositionButton(buttonTopRightCW, "TopRightCW");
        PositionButton(buttonTopRightCCW, "TopRightCCW");
        PositionButton(buttonTopLeftCW, "TopLeftCW");
        PositionButton(buttonTopLeftCCW, "TopLeftCCW");
        PositionButton(buttonBottomRightCW, "BottomRightCW");
        PositionButton(buttonBottomRightCCW, "BottomRightCCW");
        PositionButton(buttonBottomLeftCW, "BottomLeftCW");
        PositionButton(buttonBottomLeftCCW, "BottomLeftCCW");
    }

    void PositionButton(GameObject button, string tag)
    {
        GameObject placeholder = GameObject.FindWithTag(tag);
        if (placeholder != null)
        {
            // Convert world position to screen point
            Vector3 worldPosition = placeholder.transform.position;
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

            // Convert screen point to UI Canvas position
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, canvas.worldCamera, out canvasPosition);

            // Set the button's position within the canvas
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = canvasPosition;
        }
        else
        {
            Debug.LogWarning("Placeholder with tag " + tag + " not found.");
        }
    }
}
