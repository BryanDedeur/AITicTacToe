using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
using UnityEngine;

public class HeuristicAlgorithm : MonoBehaviour
{
    // public 
    public TicTacToe ttt;
    
    // private
    
    //public bool CheckForWin(ref char player, ushort xmove, ushort ymove)

    private void FoundToWeight(ref List<char> found, ref ushort weight)
    {
        /* Weight System
            .. = 0
            XO = 0
            .X = 1
            .O = 1
            OO = 2
            XX = 2
        */
        
        if (found[0] == found[1])
        {
            if (found[0] == 'X' || found[0] == 'O')
            {
                weight += 2;
            }
        }
        else
        {
            if (found[0] == '.' || found[1] == '.')
            {
                weight += 1;
            }
            else
            {
                weight += 0;
            }
        }
    }

    private ushort CalculateWeight(ref char[,] board, ushort xPos, ushort yPos)
    {
        ushort weight = 0;

        List<char> found = new List<char>();
        for (ushort y = 0; y < 3; y++)
        {
            if (y == yPos)
                continue;
            found.Add(board[xPos, y]);
        }
        FoundToWeight(ref found, ref weight);
            
        found = new List<char>();
        for (ushort x = 0; x < 3; x++)
        {
            if (x == xPos)
                continue;
            found.Add(board[x, yPos]);
        }
        FoundToWeight(ref found, ref weight);

        // Calculate diagonals only if applicable
        if (Mathf.Abs(xPos - yPos) == 0 || Mathf.Abs(xPos - yPos) == 2)
        {
            if (Mathf.Abs(xPos - yPos) == 0)
            {
                found = new List<char>();
                for (ushort i = 0; i < 3; i++)
                {
                    if (i == xPos && i == yPos)
                        continue;
                    found.Add(board[i, i]);
                }
                FoundToWeight(ref found, ref weight);
            }

            if (Mathf.Abs(xPos - yPos) == 2 || xPos == 1)
            {
                found = new List<char>();
                for (ushort i = 0; i < 3; i++)
                {
                    if (i == xPos && 2 - i == yPos)
                        continue;
                    found.Add(board[i, 2 - i]);
                }
                FoundToWeight(ref found, ref weight);
            }
        }
        print("Position (" + xPos + "," + yPos + ") weight: " + weight);

        return weight;
    }
    Dictionary<Vector2, ushort> EvaluateBoard(ref char [,] board, ref char player)
    {
        Dictionary<Vector2, ushort> moves = new Dictionary<Vector2, ushort>();
        
        // find available moves with weights
        for (ushort x = 0; x < 3; x++)
            for (ushort y = 0; y < 3; y++)
                if (board[x, y] == '.')
                {
                    // add availabe to dictonary with weight
                    ushort weight = CalculateWeight(ref board, x, y);
                    moves.Add(new Vector2(x, y), weight);
                }
        
        return moves;
    }
    
    public void PrintWeights(ref Dictionary<Vector2, ushort> validMoves)
    {
        string output = "";
        for (ushort x = 0; x < 3; x++)
        {
            string line = "";
            for (ushort y = 0; y < 3; y++)
            {
                if (validMoves.ContainsKey(new Vector2(y, x)))
                {
                    int i = (int) validMoves[new Vector2(y, x)];
                    line += i.ToString();
                    //print(validMoves[new Vector2(y, x)].ToString());
                }
                else
                {
                    line += '.';
                }
            }
            output += line + '\n';
        }
        print(output);
    }

    public Vector2 Min(ref char[,] board, ref char player)
    {
        print("Minimizing");
        Dictionary<Vector2, ushort> validMoves = EvaluateBoard(ref board, ref player);
        PrintWeights(ref validMoves);
        Dictionary<Vector2, ushort> bestMoves = new Dictionary<Vector2, ushort>();;
        
        ushort minWeight = 9;
        foreach (KeyValuePair<Vector2, ushort> move in validMoves)
        {
            if (move.Value < minWeight)
            {
                bestMoves = new Dictionary<Vector2, ushort>();
                minWeight = move.Value;
                bestMoves.Add(move.Key, move.Value);
            } 
            else if (move.Value == minWeight)
            {
                bestMoves.Add(move.Key, move.Value);
            }
        }
        
        print(minWeight);

        return bestMoves.ElementAt(Random.Range(0, bestMoves.Count)).Key;
    }
    
    public Vector2 FindBestMove(ref char[,] board, ref char player)
    {
        Dictionary<Vector2, ushort> validMoves = EvaluateBoard(ref board, ref player);
        PrintWeights(ref validMoves);
        Dictionary<Vector2, ushort> bestMoves = new Dictionary<Vector2, ushort>();;
        
        ushort maxWeight = 0;
        foreach (KeyValuePair<Vector2, ushort> move in validMoves)
        {
            if (move.Value > maxWeight)
            {
                bestMoves = new Dictionary<Vector2, ushort>();
                maxWeight = move.Value;
                bestMoves.Add(move.Key, move.Value);
            } 
            else if (move.Value == maxWeight)
            {
                bestMoves.Add(move.Key, move.Value);
            }
        }
        
        return bestMoves.ElementAt(Random.Range(0, bestMoves.Count)).Key;
    }
    
}
