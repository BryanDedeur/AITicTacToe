using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TicTacToe : MonoBehaviour
{
    // public variables
    public Board gameBoard;
    private char human;
    private char computer;
    public bool isHumanTurn;
    public bool captureInput;
    public ushort moveX;
    public ushort moveY;

    public bool gameInProgress;
    private MinMaxAlgorithm minmax;
    
    // private variables
    
    // Start is called before the first frame update
    void Start()
    {
        minmax = transform.gameObject.AddComponent<MinMaxAlgorithm>();
        minmax.ttt = this;
        StartNewGame();
    }
    
    // StartNewGame starts a new game randomly selects the first player
    void StartNewGame()
    {
        gameBoard = transform.gameObject.AddComponent<Board>();

        //gameBoard = new Board();
        gameBoard.Initialize();

        human = 'X';
        computer = 'O';
        captureInput = false;
        gameInProgress = true;
        
        // randomly decide who is first player
        bool decider = (Random.value > 0.5f);
        isHumanTurn = decider;

        if (decider)
        {
            gameBoard.lastPlayer = computer;
        }
        else
        {
            gameBoard.lastPlayer = human;
        }
        print("Ready");
        print(gameBoard.Print());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameBoard.winner == '_')
        {
            if (!isHumanTurn && captureInput)
            {
                if (!minmax.treeComputed)
                {
                    minmax.Initialize();
                }
                if (gameBoard.MakeMove(computer, moveX, moveY))
                {
                    isHumanTurn = !isHumanTurn;
                    print(gameBoard.Print());
                    captureInput = !captureInput;
                }

            }
            else if (isHumanTurn && captureInput)
            {
                if (gameBoard.MakeMove(human, moveX, moveY))
                {
                    isHumanTurn = !isHumanTurn;
                    print(gameBoard.Print());
                    captureInput = !captureInput;
                }
            }


        }
    }
}
