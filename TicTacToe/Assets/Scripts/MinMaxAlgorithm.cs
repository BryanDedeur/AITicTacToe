using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAlgorithm : MonoBehaviour
{
    // public 
    public Dictionary<string, int> scenarioMap; // string = encoded board state, move = next best move

    public char maximizingPlayer = 'X';
    // private

    private void Start()
    {
        scenarioMap = new Dictionary<string, int>();
    }

    public Board.move FindBestMove(char[,] board, char player)
    {
        List<Board.move> bestMoveList = new List<Board.move>();

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
                    string encodedBoard = Board.Encode(scenarioBoard);
                    // using memoization to speed things up
/*                    if (!scenarioMap.ContainsKey(encodedBoard))
                    {
                        Board.move someMove = new Board.move();
                        someMove.depth = 0;
                        someMove.x = x;
                        someMove.y = y;
                        someMove.score = Minimax(scenarioBoard, 0, player, x, y);
                        //scenarioMap.Add(encodedBoard, someMove);
                    }*/
  
                    Board.move someMove = new Board.move();
                    someMove.depth = 0;
                    someMove.x = x;
                    someMove.y = y;
                    someMove.score = Minimax(scenarioBoard, 0, player, x, y);

                    if (someMove.score > bestMove.score)
                    {
                        bestMove.x = x;
                        bestMove.y = y;
                        bestMove.score = someMove.score;
                    }
                    
/*                    if (scenarioMap[encodedBoard] > bestMove.score)
                    {
                        bestMove.x = x;
                        bestMove.y = y;
                        bestMove.score = scenarioMap[encodedBoard];
                        //bestMoveList.Clear();
                        //bestMoveList.Add(scenarioMap[encodedBoard]);
                        //bestMove = scenarioMap[encodedBoard];
                    } else if (scenarioMap[encodedBoard] == bestMove.score)
                    {
                        //bestMoveList.Add(scenarioMap[encodedBoard]);
                    }*/
                    print(encodedBoard);
                    print("Move (" + x + "," + y + ") score is: " + someMove.score);
                }
            }
        }
        
        // select random move
        
        return bestMove;
    }
    
    public int GetBoardScore(char[,] board, ushort depth, char player, ushort x, ushort y)
    {
        char winner = Board.GetWinner(board, player, x, y);
        if (winner == maximizingPlayer)
        {
            return 10;
        }
        else if (winner == Board.OppositePlayer(maximizingPlayer))
        {
            return -10;
        }

        return 0;
    }
    
    public int Minimax(char[,] board, ushort depth, char player, ushort x, ushort y)
    {
        string encodedBoard = Board.Encode(board);
        if (Board.TerminalState(board, player, x, y))
        {
            int score = GetBoardScore(board, depth, player, x, y);
/*            if (!scenarioMap.ContainsKey(encodedBoard))
            {
                scenarioMap.Add(encodedBoard, score);
            }*/
            return score;
        }
        
        //char[,] scenarioBoard = Board.MirorBoard(board);
        Board.move bestMove = new Board.move();
        bestMove.depth = depth;
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
                        char[,] subBoard = Board.MirorBoard(board);
                        subBoard[xIndex, yIndex] = Board.OppositePlayer(player);
                        string subEncoded = Board.Encode(subBoard);
                        if (!scenarioMap.ContainsKey(encodedBoard))
                        {
                            //scenarioMap.Add(subEncoded, Minimax(subBoard, depth, Board.OppositePlayer(player), xIndex, yIndex));
                        }

                        int score = Minimax(subBoard, depth, Board.OppositePlayer(player), xIndex, yIndex);
                        if (score > bestMove.score)
                        {
                            bestMove.score = score;
                            bestMove.x = xIndex;
                            bestMove.y = yIndex;
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
                        char[,] subBoard = Board.MirorBoard(board);
                        subBoard[xIndex, yIndex] = Board.OppositePlayer(player);
                        string subEncoded = Board.Encode(subBoard);
                        if (!scenarioMap.ContainsKey(encodedBoard))
                        {
                            //scenarioMap.Add(subEncoded, Minimax(subBoard, depth, Board.OppositePlayer(player), xIndex, yIndex));
                        }

                        int score = Minimax(subBoard, depth, Board.OppositePlayer(player), xIndex, yIndex);
                        if (score < bestMove.score)
                        {
                            bestMove.score = score;
                            bestMove.x = xIndex;
                            bestMove.y = yIndex;
                        }
                    }
                }
            }
        }
        
/*        if (!scenarioMap.ContainsKey(encodedBoard))
        {
            scenarioMap.Add(encodedBoard, bestMove.score);
        }
        else
        {
            scenarioMap[encodedBoard] = bestMove.score;
        }*/
        
        
        return bestMove.score;
    }
}
