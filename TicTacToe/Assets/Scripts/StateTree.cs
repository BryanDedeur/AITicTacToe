using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTree : MonoBehaviour
{
    public Board[] scenarioBoards;

    public void MakeStateTree(Board parentBoard)
    {
        scenarioBoards = new Board[parentBoard.numAvailableMoves];
        ushort counter = 0;
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                if (parentBoard.state[x, y] == parentBoard.emptyPosition)
                {
                    GameObject boardObject = new GameObject();
                    boardObject.name = "ScenarioBoard";
                    boardObject.transform.parent = transform;
                    Board board = boardObject.AddComponent<Board>();
                    board.Initialize();
                    scenarioBoards[counter] = board;
                    board.MirorBoard(parentBoard);
                    board.MakeMove(parentBoard.OppositePlayer(), x, y);
                    if (!board.TerminalState())
                    {
                        StateTree subTree = boardObject.AddComponent<StateTree>();
                        subTree.MakeStateTree(board);
                    }
                    counter += 1;
                }
            }
        }
    }
}
