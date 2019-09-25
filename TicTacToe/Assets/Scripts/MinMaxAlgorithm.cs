using System.Collections.Generic;
using UnityEngine;


public class MinMaxAlgorithm : MonoBehaviour
{
    // public 
    private Dictionary<string, Board.move> scenarioMap; // string = encoded board state, move = next best move

    public char maximizingPlayer = 'X';
    // private

    public Board.move FindBestMove(char[,] board, char player)
    {
        Board.move bestMove = new Board.move();
        bestMove.score = -100;
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                if (board[x, y] == Board.emptyPosition)
                {
                    char[,] scenarioBoard = Board.MirorBoard(board);
                    scenarioBoard[x, y] = player;
                    string encodedBoard = Board.Encode(board);
                    // using memoization to speed things up
                    if (scenarioMap.ContainsKey(encodedBoard))
                    {
                        if (scenarioMap[encodedBoard].score > bestMove.score)
                        {
                            bestMove = scenarioMap[encodedBoard];
                        }
                    }
                    else
                    {
                        if (Minimax(scenarioBoard, 0, player, x, y) > bestMove.score)
                        {
                            bestMove = scenarioMap[encodedBoard];
                        }
                    }
                }
            }
        }
        return bestMove;
    }
    
    public short GetBoardScore(char[,] board, char player, ushort x, ushort y)
    {
        char winner = Board.GetWinner(board, player, x, y);
        if (winner == maximizingPlayer)
        {
            return -10;
        }
        else if (winner == Board.OppositePlayer(maximizingPlayer))
        {
            return 10;
        }

        return 0;
    }
    
    public short Minimax(char[,] board, ushort depth, char player, ushort x, ushort y)
    {
        Board.move someMove = new Board.move();
        someMove.depth = depth;
        someMove.x = x;
        someMove.y = y;
        string encodedBoard = Board.Encode(board);
        if (Board.TerminalState(board, player, x, y))
        {
            someMove.score = GetBoardScore(board, player, x, y);
            scenarioMap.Add(encodedBoard, someMove);
            return someMove.score;
        }
        
        Board.move bestMove = new Board.move();
        depth += 1;
        if (player == maximizingPlayer) // maximize
        {
            bestMove.score = -100;
            for (ushort xIndex = 0; xIndex < 3; xIndex++)
            {
                for (ushort yIndex = 0; yIndex < 3; yIndex++)
                {
                    if (board[xIndex, yIndex] == Board.emptyPosition)
                    {
                        if (!scenarioMap.ContainsKey(encodedBoard))
                        {
                            board[xIndex, yIndex] = Board.OppositePlayer(player);
                            someMove.score = Minimax(board, depth, Board.OppositePlayer(player), xIndex, yIndex);
                            scenarioMap.Add(encodedBoard, someMove);
                        }
                        if (someMove.score > bestMove.score)
                        {
                            someMove = scenarioMap[encodedBoard];
                        }
                    }
                }
            }
        }
        else // minimize
        {
            bestMove.score = 100;
            for (ushort xIndex = 0; xIndex < 3; xIndex++)
            {
                for (ushort yIndex = 0; yIndex < 3; yIndex++)
                {
                    if (board[xIndex, yIndex] == Board.emptyPosition)
                    {
                        if (!scenarioMap.ContainsKey(encodedBoard))
                        {
                            board[xIndex, yIndex] = Board.OppositePlayer(player);
                            someMove.score = Minimax(board, depth, Board.OppositePlayer(player), xIndex, yIndex);
                            scenarioMap.Add(encodedBoard, someMove);
                        }
                        if (someMove.score < bestMove.score)
                        {
                            someMove = scenarioMap[encodedBoard];
                        }
                    }
                }
            }
        }
        
        return someMove.score;
    }
}
