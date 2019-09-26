using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TicTacToe : MonoBehaviour
{
    // public variables
    //public Board gameBoard;
    private char human;
    private char computer;
    public bool captureInput;
    public ushort moveX;
    public ushort moveY;
    public char currentPlayer;
    public char[,] gameBoard;
    private bool moveMade = false;

    public GameObject boardTile;
    public GameObject[,] physicalBoard;
    public GameObject xPeice;
    public GameObject oPeice;
    
    public bool gameInProgress;
    private MinMaxAlgorithm minmax;
    public Camera playerCamera;
    public GameObject reset;

    public Text virtualConsole;

    public List<GameObject> peices;
    
    // private variables
    
    // Start is called before the first frame update
    void Start()
    {
        minmax = transform.gameObject.AddComponent<MinMaxAlgorithm>();
        minmax.ttt = this;
        //minmax.ttt = this;
        StartNewGame();
        float scale = 2.5f;
        physicalBoard = new GameObject[3,3];
        Vector3 offset = new Vector3(-2.5f,0,0);
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                physicalBoard[x, y] = Instantiate(boardTile, transform);
                physicalBoard[x, y].transform.position = offset + new Vector3(x * scale, 0, y * scale);
            }
        }
        transform.Rotate(new Vector3(180,0,0));
        peices = new List<GameObject>();
    }
     
    // StartNewGame starts a new game randomly selects the first player
    void StartNewGame()
    {
        human = 'X';
        computer = 'O';
        captureInput = false;
        gameInProgress = true;
        
        gameBoard = new char[3,3];
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                gameBoard[x, y] = Board.emptyPosition;
            }
        }

        foreach (GameObject peice in peices)
        {
            Destroy(peice);
        }
        peices.Clear();
        
        // randomly decide who is first player
        bool decider = (Random.value > 0.5f);
        if (decider)
        {
            currentPlayer = computer;
        }
        else
        {
            currentPlayer = human;
        }
        Board.Visualize(gameBoard);
        virtualConsole.text = "";
        VirtualPrint("----------- New Game Ready -----------");
    }

    public void VirtualPrint(string str)
    {
        virtualConsole.text = virtualConsole.text + str + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != '_') // game is waiting
        {
            if (currentPlayer == computer)
            {
                Board.move nextMove = minmax.FindBestMove(gameBoard, currentPlayer);
                gameBoard[nextMove.x, nextMove.y] = currentPlayer;
                peices.Add(Instantiate(oPeice, transform));
                peices[peices.Count - 1].transform.position =
                    physicalBoard[nextMove.x, nextMove.y].transform.position + new Vector3(0, 5, -2);
                if (Board.TerminalState(gameBoard, currentPlayer, nextMove.x, nextMove.y))
                    {
                        if (Board.GetWinner(gameBoard, currentPlayer, nextMove.x, nextMove.x) == computer)
                        {
                            VirtualPrint("The winner is the human");
                        }
                        else
                        {
                            VirtualPrint("The winner is nobody");
                        }
                        currentPlayer = Board.emptyPosition;
                    }
                else
                {
                    currentPlayer = Board.OppositePlayer(currentPlayer);
                }
                //VirtualPrint(Board.Visualize(gameBoard));

            }
            else if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
                if (Physics.Raycast(ray, out hit)) {
                    for (ushort x = 0; x < 3; x++)
                    {
                        for (ushort y = 0; y < 3; y++)
                        {
                            if (physicalBoard[x, y] == hit.transform.gameObject)
                            {
                                moveX = x;
                                moveY = y;
                                break;
                            }
                        }
                    }
                }
                
                if (Board.IsValidMove(gameBoard, currentPlayer, moveX, moveY))
                {
                        peices.Add(Instantiate(xPeice, transform));
                    peices[peices.Count - 1].transform.position =
                        physicalBoard[moveX, moveY].transform.position + new Vector3(0, 5, 0);
                    gameBoard[moveX, moveY] = currentPlayer;

                    if (Board.TerminalState(gameBoard, currentPlayer, moveX, moveY))
                    {
                        if (Board.GetWinner(gameBoard, currentPlayer, moveX, moveY) == human)
                        {
                            VirtualPrint("The winner is the human");
                        }
                        else
                        {
                            VirtualPrint("The winner is nobody");
                        }
                        currentPlayer = Board.emptyPosition;
                    }
                    else
                    {
                        currentPlayer = Board.OppositePlayer(currentPlayer);
                    }
                    //VirtualPrint(Board.Visualize(gameBoard));

                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (reset == hit.transform.gameObject)
                {
                    StartNewGame();
                    moveX = 5;
                }
            }
        }
    }
}
