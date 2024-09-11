// SAFE CODE VERSION 2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public Tilemap tilemapTopLeft;      // Reference to the top-left Tilemap
    public Tilemap tilemapTopRight;     // Reference to the top-right Tilemap
    public Tilemap tilemapBottomLeft;   // Reference to the bottom-left Tilemap
    public Tilemap tilemapBottomRight;  // Reference to the bottom-right Tilemap
    public Tilemap underlyingTilemap;   // Reference to the static Tilemap

    public TileBase blackTile;          // The new tile to place
    public TileBase whiteTile;          // The tile for player two

    private Camera mainCamera;

    // 6x6 game board array
    private int[,] gameBoard = new int[6, 6];

    public GameObject RotationButtonsParent;
    //public GameObject EngGameNotification;
    public TextMeshProUGUI EngGameNotification;
    public TextMeshProUGUI ErrorNotification;

    //public MoveAndRotateParent moveandrotateparent;
    public bool underRotationNow = false;
    public bool aPlayerHasWon = false;
    public bool stalemateHasOccurred = false;
    public bool anyColorTokenHasBeenPlaced = false;
    public bool aRotationHasJustOccurredAfterTokenPlacement = false;
    public bool gameStart = true;

    // Black goes first
    public float playerTurn = 1.0f;
    public int playerNumber;
    public AudioSource backgroundSong;
    private bool userWantsMusic = true;

    public AudioSource clickSFX;
    public AudioSource winSFX;

    //public Animator buttonAnimator;
    //public Animator[] buttonAnimators;

    // Track rotation state for each quadrant
    private Dictionary<string, int> quadrantRotationStates = new Dictionary<string, int>
    {
        { "TopLeftParent", 0 },
        { "TopRightParent", 0 },
        { "BottomLeftParent", 0 },
        { "BottomRightParent", 0 }
    };

    private ButtonAnimatorManager buttonAnimatorManager;

    void Start()
    {
        // Initialize the game board to 0 (empty)
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                gameBoard[i, j] = 0;
            }
        }

        // Get the main camera
        mainCamera = Camera.main;

        SetPlayerTurnNotification();

        buttonAnimatorManager = FindObjectOfType<ButtonAnimatorManager>();

        //buttonAnimators = GetComponentsInChildren<Animator>();


    }

    



    public void SetPlayerTurnNotification()
    {
        if (playerTurn > 0)
        {
            playerNumber = 1;
        } else
        {
            playerNumber = 2;
        }

        if (!aPlayerHasWon)
        {
            EngGameNotification.text = "PLAYER " + playerNumber;
        }
        
    }

    public void ToggleMusic ()
    {
        userWantsMusic = !userWantsMusic;

        clickSFX.Play();
        
        if (userWantsMusic)
        {
            backgroundSong.Play();
        } else 
        {
            backgroundSong.Pause();
        }
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Convert mouse position to world position
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;  // Ensure we're working in 2D space

            // Convert world position to cell position in the underlying Tilemap
            Vector3Int cellPosition = underlyingTilemap.WorldToCell(worldPosition);

            // Get the tile at the clicked cell in the underlying tilemap
            TileBase tile = underlyingTilemap.GetTile(cellPosition);

            if (tile != null)
            {
                // Map the clicked tile in the underlying tilemap to the 6x6 game board
                Vector2Int boardIndex = new Vector2Int(cellPosition.x, cellPosition.y);

                

                
                // check individual issues first, just to print out notification displays...
                if (!gameStart && !aRotationHasJustOccurredAfterTokenPlacement && !aPlayerHasWon)
                {
                    ErrorNotification.text = "PLAYER " + playerNumber + " MUST ROTATE A QUADRANT";
                } 
                
                
                
                
                // Check if the position on the game board array is empty
                if (((gameBoard[boardIndex.x, boardIndex.y] == 0) && (underRotationNow == false) && (aPlayerHasWon==false) && (stalemateHasOccurred == false) && (aRotationHasJustOccurredAfterTokenPlacement))|| (gameStart))
                {

                    // here we start the rotate button animation
                    //buttonAnimator.SetTrigger("startAnimation");
                    //StartAnimation();
                    buttonAnimatorManager.StartAnimation();


                    gameStart = false;
                    
                    anyColorTokenHasBeenPlaced = true;


                    // Determine which visual tilemap corresponds to the clicked position and get the adjusted cell position
                    Vector3Int adjustedCellPosition;
                    Tilemap visualTilemap = GetVisualTilemapForPosition(boardIndex, out adjustedCellPosition);

                    // Update the visual tilemap and game board array based on the player's turn
                    if (playerTurn > 0)
                    {
                        visualTilemap.SetTile(adjustedCellPosition, blackTile);
                        gameBoard[boardIndex.x, boardIndex.y] = 1;
                    }
                    else
                    {
                        visualTilemap.SetTile(adjustedCellPosition, whiteTile);
                        gameBoard[boardIndex.x, boardIndex.y] = 2;
                    }

                    // Print the current state of the game board
                    PrintBoardState();
                    CheckForWinner();

                    // Invert playerTurn to signify it's the other player's turn
                    playerTurn = -playerTurn;
                    

                    // ensures that a rotation needs to happen
                    aRotationHasJustOccurredAfterTokenPlacement = false;
                }
                else
                {
                    // The tile is not empty, so do nothing (or give feedback)
                    Debug.Log("Tile already occupied, rotation occuring, a quadrant has not been rotated or the game is over!");
                }
            }
        }
    }







    // Helper function to determine which visual tilemap corresponds to the board index
    Tilemap GetVisualTilemapForPosition(Vector2Int boardIndex, out Vector3Int adjustedCellPosition)
    {
        Tilemap visualTilemap = null;
        adjustedCellPosition = new Vector3Int();

        // Determine the quadrant and adjust cell position accordingly
        if (boardIndex.x < 3 && boardIndex.y >= 3)
        {
            visualTilemap = tilemapTopLeft;
            //adjustedCellPosition = AdjustPositionForRotation(boardIndex, "TopLeftParent");
            adjustedCellPosition = AdjustPositionForRotation(new Vector2Int(boardIndex.x + 0, boardIndex.y - 3), "TopLeftParent");

        }
        else if (boardIndex.x >= 3 && boardIndex.y >= 3)
        {
            visualTilemap = tilemapTopRight;
            adjustedCellPosition = AdjustPositionForRotation(new Vector2Int(boardIndex.x - 3, boardIndex.y - 3), "TopRightParent");
        }
        else if (boardIndex.x < 3 && boardIndex.y < 3)
        {
            visualTilemap = tilemapBottomLeft;
            adjustedCellPosition = AdjustPositionForRotation(boardIndex, "BottomLeftParent");
        }
        else if (boardIndex.x >= 3 && boardIndex.y < 3)
        {
            visualTilemap = tilemapBottomRight;
            adjustedCellPosition = AdjustPositionForRotation(new Vector2Int(boardIndex.x - 3, boardIndex.y), "BottomRightParent");
        }

        return visualTilemap;
    }

    // Adjust the cell position based on the quadrant's rotation state
    Vector3Int AdjustPositionForRotation(Vector2Int originalPosition, string quadrantTag)
    {
        int rotation = quadrantRotationStates[quadrantTag];
        Vector3Int adjustedPosition = new Vector3Int(originalPosition.x, originalPosition.y, 0);

        switch (rotation)
        {
            case 90:
                adjustedPosition.x = 2 - originalPosition.y;
                adjustedPosition.y = originalPosition.x;
                break;

            case 180:
                adjustedPosition.x = 2 - originalPosition.x;
                adjustedPosition.y = 2 - originalPosition.y;
                break;

            case 270:
                adjustedPosition.x = originalPosition.y;
                adjustedPosition.y = 2 - originalPosition.x;
                break;

            default: // 0 degrees
                break;
        }

        return adjustedPosition;
    }


    // Function to print the current state of the game board
    void PrintBoardState()
    {
        string boardState = "Game Board State:\n";
        for (int y = 5; y >= 0; y--) // Print rows from top to bottom
        {
            for (int x = 0; x < 6; x++)
            {
                boardState += gameBoard[x, y].ToString() + " ";
            }
            boardState += "\n";
        }
        Debug.Log(boardState);
    }

    // Rotate a quadrant of the game board
    public void RotateQuadrant(string quadrantTag, bool rotateClockwise)
    {
        int rotationChange = rotateClockwise ? 90 : -90;
        quadrantRotationStates[quadrantTag] = (quadrantRotationStates[quadrantTag] + rotationChange) % 360;

        if (quadrantRotationStates[quadrantTag] < 0)
            quadrantRotationStates[quadrantTag] += 360;


        int[,] quadrant = new int[3, 3];
        Vector2Int start = GetQuadrantStart(quadrantTag);

        // Extract the quadrant from the gameBoard
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                quadrant[i, j] = gameBoard[start.x + i, start.y + j];
            }
        }

        // Rotate the quadrant
        quadrant = rotateClockwise ? RotateMatrixClockwise(quadrant) : RotateMatrixCounterClockwise(quadrant);

        // Place the rotated quadrant back into the gameBoard
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gameBoard[start.x + i, start.y + j] = quadrant[i, j];
            }
        }

        // Optionally, print the board state to check the changes
        PrintBoardState();
        CheckForWinner();

        //buttonAnimator.SetTrigger("stopAnimation");
        //StopAnimation();
        buttonAnimatorManager.StopAnimation();
    }

    // Helper function to determine the start position of the quadrant in the game board
    Vector2Int GetQuadrantStart(string quadrantTag)
    {
        if (quadrantTag == "TopLeftParent") return new Vector2Int(0, 3);
        if (quadrantTag == "TopRightParent") return new Vector2Int(3, 3);
        if (quadrantTag == "BottomLeftParent") return new Vector2Int(0, 0);
        if (quadrantTag == "BottomRightParent") return new Vector2Int(3, 0);
        return Vector2Int.zero;
    }

    // Function to rotate a 3x3 matrix 90 degrees clockwise
    int[,] RotateMatrixClockwise(int[,] matrix)
    {
        int[,] rotated = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rotated[i, j] = matrix[2 - j, i];
            }
        }
        return rotated;
    }

    // Function to rotate a 3x3 matrix 90 degrees counterclockwise
    int[,] RotateMatrixCounterClockwise(int[,] matrix)
    {
        int[,] rotated = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rotated[i, j] = matrix[j, 2 - i];
            }
        }
        return rotated;
    }

    // Function to check if a player has won
    public bool CheckWin(int player)
    {
        // Check horizontal, vertical, and diagonal for win condition
        
        
        if (CheckHorizontal(player) || CheckVertical(player) || CheckDiagonal(player))
        {
            Debug.Log("PLAYER " + player + " WINS!");
            EngGameNotification.text = "PLAYER " + player + " WINS!";
            backgroundSong.Pause();
            winSFX.Play();
            aPlayerHasWon = true;
            ToggleButtons(false);
            EngGameNotification.enabled = true;
            return true;
            
        }

        // Check for stalemate condition
        if (IsStalemate())
        {
            Debug.Log("STALEMATE!");
            EngGameNotification.text = "STALEMATE!";
            stalemateHasOccurred = true;
            ToggleButtons(false);
            EngGameNotification.enabled = true;
            return false;
        }

        return false;
        


        
    }


    // Function to check if the game is a stalemate
    private bool IsStalemate()
    {
        int filledCells = 0;

        // Count filled cells on the board
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (gameBoard[i, j] != 0)
                {
                    filledCells++;
                }
            }
        }

        // If all 36 cells are filled and no player has won, it's a stalemate
        return filledCells == 36 && !aPlayerHasWon;
    }

    private bool CheckHorizontal(int player)
    {
        // Iterate through each row
        for (int y = 0; y < 6; y++)
        {
            // Iterate through each possible starting column for a win
            for (int x = 0; x <= 1; x++)  // x <= 1 to avoid out of bounds when checking 5 in a row
            {
                // Check if all 5 cells in the row match the player's token
                if (gameBoard[x, y] == player &&
                    gameBoard[x + 1, y] == player &&
                    gameBoard[x + 2, y] == player &&
                    gameBoard[x + 3, y] == player &&
                    gameBoard[x + 4, y] == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckVertical(int player)
    {
        // Iterate through each column
        for (int x = 0; x < 6; x++)
        {
            // Iterate through each possible starting row for a win
            for (int y = 0; y <= 1; y++)  // y <= 1 to avoid out of bounds when checking 5 in a column
            {
                // Check if all 5 cells in the column match the player's token
                if (gameBoard[x, y] == player &&
                    gameBoard[x, y + 1] == player &&
                    gameBoard[x, y + 2] == player &&
                    gameBoard[x, y + 3] == player &&
                    gameBoard[x, y + 4] == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckDiagonal(int player)
    {
        // Check downward diagonals (\ direction)
        for (int x = 0; x <= 1; x++)  // x <= 1 to avoid out of bounds when checking diagonals
        {
            for (int y = 0; y <= 1; y++)  // y <= 1 to avoid out of bounds when checking diagonals
            {
                if (gameBoard[x, y] == player &&
                    gameBoard[x + 1, y + 1] == player &&
                    gameBoard[x + 2, y + 2] == player &&
                    gameBoard[x + 3, y + 3] == player &&
                    gameBoard[x + 4, y + 4] == player)
                {
                    return true;
                }
            }
        }

        // Check upward diagonals (/ direction)
        for (int x = 0; x <= 1; x++)  // x <= 1 to avoid out of bounds when checking diagonals
        {
            for (int y = 4; y < 6; y++)  // y starts at 4 and goes to 5 to avoid out of bounds
            {
                if (gameBoard[x, y] == player &&
                    gameBoard[x + 1, y - 1] == player &&
                    gameBoard[x + 2, y - 2] == player &&
                    gameBoard[x + 3, y - 3] == player &&
                    gameBoard[x + 4, y - 4] == player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Call this function after every move to check if there's a winner
    void CheckForWinner()
    {
        if (CheckWin(1) || CheckWin(2))
        {
            // Handle win condition, e.g., stop the game, show a message, etc.
            Debug.Log("Game Over!");
        }
    }

    public void ResetGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ToggleButtons(bool state)
    {
        if (RotationButtonsParent != null)
        {
            // Get all Button components in the children of the parent object
            Button[] buttons = RotationButtonsParent.GetComponentsInChildren<Button>();

            // Loop through each button and set its interactable property
            foreach (Button button in buttons)
            {
                button.interactable = state;
            }
        }
    }

}
