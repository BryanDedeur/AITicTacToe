using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Board : MonoBehaviour
{
    //public string encoded;
    public short numAvailableMoves;
    public char lastPlayer;
    public char winner;
    public short weight;

    public ushort lastMoveX;
    public ushort lastMoveY;
    public char[,] state;
    public char emptyPosition = '_';

    // Start is called before the first frame update
    public void Initialize()
    {
        lastPlayer = emptyPosition;
        numAvailableMoves = 9;
        winner = emptyPosition;
        state = new char[3, 3];
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                state[x, y] = emptyPosition;
            }
        }
    }

    public char OppositePlayer()
    {
        if (lastPlayer == 'X')
        {
            return 'O';
        }
        return 'X';
    }
    
    public bool IsValidMove(char player, ushort x, ushort y)
    {
        if (player == lastPlayer)
            return false;
        if (!(x < 3 && y < 3))
            return false;
        if (state[x, y] != emptyPosition)
            return false;
        return true;
    }

    public bool MakeMove(char player, ushort x, ushort y)
    {
        if (IsValidMove(player, x, y))
        {
            numAvailableMoves -= 1;
            lastPlayer = player;
            state[x, y] = player;
            return true;
        }
        return false;
    }

    public string Print()
    {
        string output = "";
        for (ushort x = 0; x < 3; x++)
        {
            string line = "";
            for (ushort y = 0; y < 3; y++)
            {
                line += state[y, x];
            }
            output += line + '\n';
        }
        return output;
    }

    public void MirorBoard(Board passedBoard)
    {
        for (ushort x = 0; x < 3; x++)
        {
            for (ushort y = 0; y < 3; y++)
            {
                state[x, y] = passedBoard.state[x,y];
            }
        }
        lastPlayer = passedBoard.lastPlayer;
        numAvailableMoves = passedBoard.numAvailableMoves;
        lastMoveX = passedBoard.lastMoveX;
        lastMoveY = passedBoard.lastMoveY;
        winner = passedBoard.winner;
    }

    public char CheckForWin()
    {
        // not possible to win with only 5 moves on board
        if (numAvailableMoves <= 5)
            return emptyPosition;
    
        // check win scenarios only on verticle and horizontal line of current move
        ushort count = 0;
        // vertical to current move
        for (ushort y = 0; y < 3; y++)
            if (state[lastMoveX, y] == lastPlayer)
                count += 1;
            else
                break;

        if (count > 2)
            return lastPlayer;
        else
            count = 0;
    
        // horizontal to current move
        for (ushort x = 0; x < 3; x++)
            if (state[x, lastMoveY] == lastPlayer)
                count += 1;
            else
                break;
    
        if (count > 2)
            return lastPlayer;

        // diagonals if move is in diagonal space
        if (Mathf.Abs(lastMoveX - lastMoveY) == 0 || Mathf.Abs(lastMoveX - lastMoveY) == 2)
        {
            if (state[0, 0] == lastPlayer && state[1, 1] == lastPlayer && state[2, 2] == lastPlayer)
                return lastPlayer;
            if (state[2, 0] == lastPlayer && state[1, 1] == lastPlayer && state[0, 2] == lastPlayer)
                return lastPlayer;
        }
    
        return emptyPosition;
        
    }

    public bool TerminalState()
    {
        if (numAvailableMoves == 0)
        {
            weight = 0;
            return true;
        }

        winner = CheckForWin();
        if (winner != emptyPosition)
        {
            if (winner == 'X')
            {
                weight = 1;
            }
            else
            {
                weight = -1;
            }
            return true;
        }

        return false;
    }
    
    private bool IsMovesLeft()
    {
        if (numAvailableMoves == 0)
        {
            return false;
        }
        return true;
    }
}
