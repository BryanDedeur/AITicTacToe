using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
using UnityEngine;

public class MinMaxAlgorithm : MonoBehaviour
{
    // public 
    public TicTacToe ttt;
    public StateTree stateTree;
    public bool treeComputed = false;

    public void Awake()
    {
        stateTree = transform.gameObject.AddComponent<StateTree>();
    }

    public void Initialize()
    {
        treeComputed = true;
        stateTree.MakeStateTree(ttt.gameBoard);
    }

    public Board minmax(Board board, char player)
    {
        if (board.TerminalState())
        {
            return board;
        }

        if (player == 'X')
        {
            Board bestBoard = new Board();
            short bestWeight = -100;
            for (int i = 0; i < stateTree.scenarioBoards.Length; i++)
            {
                Board foundBoard = minmax(stateTree.scenarioBoards[i], board.OppositePlayer());
                if (bestWeight > foundBoard.weight)
                {
                    bestBoard = foundBoard;
                    bestWeight = foundBoard.weight;
                }
            }
            return bestBoard;
        } else
        {
            Board bestBoard = new Board();
            short bestWeight = -100;
            for (int i = 0; i < stateTree.scenarioBoards.Length; i++)
            {
                Board foundBoard = minmax(stateTree.scenarioBoards[i], board.OppositePlayer());
                if (bestWeight > foundBoard.weight)
                {
                    bestBoard = foundBoard;
                    bestWeight = foundBoard.weight;
                }
            }
            return bestBoard;
        }
    }
}
