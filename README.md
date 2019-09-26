# AI TicTacToe

An tic tac toe AI using minimax algorithm.

#### How to play

Navigate to this web address: https://www.cse.unr.edu/~bryand/ai-tictactoe/v1/index.html

Click on the tile that you want to place a move on. If the tile is valid a "X" will fall on the tile. When the game is over click "Reset" to clear the board. Standard tic tac toe rules apply to the game.

### Source Code

Source code can be retrieved from /TicTacToe/Assets/Scripts/...  The source code is writen in C# and can only be compiled through the Unity Engine version mentioned below. 
All code documentation can be found inside each file. Some areas are undocumented temporarily due to time constraints.

### Complications

The current min max calculations are not correct and debugging is a lengthy project not fit for the time granted for the project. I tried to use various methods of memoization initially to speed up the evaluation of the board. However this led to a debugging nightmare on its own. The move the minmax selects is sometimes right and sometimes wrong. I will need to spend more time tracing the tree to figure out the source of the issue. 

### Unfinished Tasks

- Fix the min max algorithm to select the proper move. 

### Version History

* 0.0.1 (Release)
    * contains all the development up to submission deadline

### Meta

By Bryan Dedeurwaerder – bdedeurwaerder@nevada.unr.edu  
Major - Computer Science and Engineering  
Minor - Digital Interactive Gaming  
Minor - Mathematics  
Research - Evolutionary Computing Systems Lab UNR

[https://github.com/bryandedeur](https://github.com/BryanDedeur)

## Development Environment

Primary Development: Windows 10 using Rider 
Version Control: Github  
IDE: Rider  
Unity: Version 2019.2.5f

