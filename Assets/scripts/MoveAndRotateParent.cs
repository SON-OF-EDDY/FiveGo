using System.Collections;
using UnityEngine;

public class MoveAndRotateParent : MonoBehaviour
{
    public float moveAmount;
    public float duration;  // Duration of each animation phase in seconds

    private Vector3 originalPosition;
    private BoardManager boardManager;

    
    void Start()
    {
        // Store the original position of the Parent GameObject
        originalPosition = transform.position;

        moveAmount = 0.75f;
        duration = 1.0f;

        // Get reference to the BoardManager script
        boardManager = FindObjectOfType<BoardManager>();
    }

    // This method can be called from the OnClick event in the Unity Editor
    public void OnButtonClick(bool rotateClockwise)
    {
        if (boardManager.anyColorTokenHasBeenPlaced)
        {
            Debug.Log("A token HAS been placed; Rotation OKAY!!!");
            boardManager.ErrorNotification.text = "";
            // Start the coroutine when the button is clicked
            StartCoroutine(PerformSmoothAction(rotateClockwise));
            boardManager.aRotationHasJustOccurredAfterTokenPlacement = true;
            toggleInvisible();
            boardManager.underRotationNow = true;
        } else
        {
            Debug.Log("A token has not been placed yet; Rotation not allowed!");
            boardManager.SetPlayerTurnNotification();
            if (!boardManager.aPlayerHasWon)
            {
                boardManager.ErrorNotification.text = "PLAYER " + boardManager.playerNumber + " MUST PLACE A TOKEN";
            }
            
        }
        
        
    }

    IEnumerator PerformSmoothAction(bool rotateClockwise)
    {
        Vector3 targetPosition;
        // Determine the translation direction based on the GameObject's name or tag
        if (gameObject.CompareTag("TopLeftParent"))
        {
            targetPosition = originalPosition + new Vector3(-moveAmount, moveAmount, 0f);
        }
        else if (gameObject.CompareTag("TopRightParent"))
        {
            targetPosition = originalPosition + new Vector3(moveAmount, moveAmount, 0f);
        }
        else if (gameObject.CompareTag("BottomLeftParent"))
        {
            targetPosition = originalPosition + new Vector3(-moveAmount, -moveAmount, 0f);
        }
        else if (gameObject.CompareTag("BottomRightParent"))
        {
            targetPosition = originalPosition + new Vector3(moveAmount, -moveAmount, 0f);
        }
        else
        {
            // Default to no translation if the tag does not match
            targetPosition = originalPosition;
        }

        // Step 1: Smoothly translate to the target position
        yield return StartCoroutine(SmoothTranslate(transform, transform.position, targetPosition, duration));

        // Step 2: Smoothly rotate by 90 degrees based on the current rotation
        float rotationAngle = rotateClockwise ? -90f : 90f;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, rotationAngle);
        yield return StartCoroutine(SmoothRotate(transform, transform.rotation, targetRotation, duration));

        // Step 3: Smoothly translate back to the original position
        yield return StartCoroutine(SmoothTranslate(transform, transform.position, originalPosition, duration));

        // Ensure the final position is the original position, and rotation is updated to the new rotation
        transform.position = originalPosition;
        transform.rotation = targetRotation;

        // Update the game board in the BoardManager
        boardManager.RotateQuadrant(gameObject.tag, rotateClockwise);

        toggleVisible();
        boardManager.underRotationNow = false;
        boardManager.anyColorTokenHasBeenPlaced = false;
        boardManager.SetPlayerTurnNotification();

        // here we end the rotate button animation
        

    }

    IEnumerator SmoothTranslate(Transform obj, Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            obj.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.position = endPos;
    }

    IEnumerator SmoothRotate(Transform obj, Quaternion startRot, Quaternion endRot, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            obj.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.rotation = endRot;
    }

    void toggleVisible()
    {
        // Disables all buttons inside the parentObject
        boardManager.ToggleButtons(true);
    }

    void toggleInvisible()
    {
        // Disables all buttons inside the parentObject
        boardManager.ToggleButtons(false);
    }

}
