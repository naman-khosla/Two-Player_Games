using System;
using System.Linq;
using System.Collections.Generic;

namespace GameStation;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the GameStation!");
        Console.WriteLine("Please select a game to begin the fun:");
        Console.WriteLine("1. Connect Four");
        Console.WriteLine("2. SOS");

        string gameChoice = Console.ReadLine();

        Console.WriteLine("\nSelect game mode:");
        Console.WriteLine("1. Human vs. Human");
        Console.WriteLine("2. Human vs. Computer");

        string modeChoice = Console.ReadLine();

        BoardGameUI boardGame;

        List<IGamePlayer> players = new List<IGamePlayer>();

        if (gameChoice == "1")
        {
            boardGame = new ConnectFourGame();

            if (modeChoice == "1")
            {
                players.Add(new HumanPlayer("Player 1", "+"));
                players.Add(new HumanPlayer("Player 2", "-"));
            }
            else
            {
                players.Add(new HumanPlayer("Player 1", "+"));
                players.Add(new ComputerPlayer("Computer", "-"));
            }
        }
        else
        {
            boardGame = new SOSGame();
            if (modeChoice == "1")
            {
                players.Add(new HumanPlayer("Player 1", "S"));
                players.Add(new HumanPlayer("Player 2", "O"));
            }
            else
            {
                players.Add(new HumanPlayer("Player 1", "S"));
                players.Add(new ComputerPlayer("Computer", "O"));
            }
        }


        GameManager gameManager = new GameManager(boardGame, players);

        Console.WriteLine("Do you want to:");
        Console.WriteLine("1. Start a new game");
        Console.WriteLine("2. Load an existing game");

        string startChoice = Console.ReadLine();

        if (startChoice == "1")
        {
            gameManager.StartGame();
        }
        else
        {
            Console.WriteLine("Enter the file path to load the game:");
            string filePath = Console.ReadLine();
            gameManager.LoadGame(filePath);
            gameManager.StartGame();
        }
    }
}

public interface BoardGameUI
{
    int Rows { get; }
    int Columns { get; }
    void InitializeBoard(int rows, int columns);
    bool IsValidMove(int row, int column, string player);
    void MakeMove(int row, int column, string player);
    bool CheckForWinner(string player);
    void UndoMove();
    void RedoMove();
    void SaveGame(string filePath);
    void LoadGame(string filePath);
    void PrintBoard();
}

public abstract class BoardGame : BoardGameUI
{
    public int Rows { get; set; }
    public int Columns { get; set; }
    protected Board board;

    public BoardGame(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        board = new Board(rows, columns);
    }

    public virtual void InitializeBoard(int rows, int columns)
    {
        board = new Board(rows, columns);
    }

    public abstract bool IsValidMove(int row, int column, string player);

    public abstract void MakeMove(int row, int column, string player);

    public abstract bool CheckForWinner(string player);

    public void UndoMove()
    {
        // Implement undo logic here
    }

    public void RedoMove()
    {
        // Implement redo logic here
    }

    public void SaveGame(string filePath)
    {
        // Implement save logic here
    }

    public void LoadGame(string filePath)
    {
        // Implement load logic here
    }

    public virtual void PrintBoard()
    {
        board.PrintBoard();
    }
}

public class SOSGame : BoardGame
{
    public SOSGame() : base(3, 3)
    {


    }

    public override bool IsValidMove(int row, int column, string player)
    {
        if (row < 0 || row >= Rows || column < 0 || column >= Columns)
        {
            return false;
        }

        string cellContent = board.GetCell(row, column);
        if (cellContent != " ")
        {
            return false;
        }

        if (player != "S" && player != "O")
        {
            return false;
        }

        return true;
    }

    public override void MakeMove(int row, int column, string player)
    {
        board.SetCell(row, column, player);
    }

    public override bool CheckForWinner(string player)
    {
        string checkSOS = "SOS";
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                if (board.GetCell(row, col) == checkSOS[0].ToString())
                {
                    if (col <= Columns - checkSOS.Length)
                    {
                        if (board.GetCell(row, col + 1) == checkSOS[1].ToString() &&
                            board.GetCell(row, col + 2) == checkSOS[2].ToString())
                        {
                            return true;
                        }
                    }

                    // Check vertically
                    if (row <= Rows - checkSOS.Length)
                    {
                        if (board.GetCell(row + 1, col) == checkSOS[1].ToString() &&
                            board.GetCell(row + 2, col) == checkSOS[2].ToString())
                        {
                            return true;
                        }
                    }

                    // Check primary diagonal
                    if (row <= Rows - checkSOS.Length && col <= Columns - checkSOS.Length)
                    {
                        if (board.GetCell(row + 1, col + 1) == checkSOS[1].ToString() &&
                            board.GetCell(row + 2, col + 2) == checkSOS[2].ToString())
                        {
                            return true;
                        }
                    }

                    // Check secondary diagonal
                    if (row <= Rows - checkSOS.Length && col >= checkSOS.Length - 1)
                    {
                        if (board.GetCell(row + 1, col - 1) == checkSOS[1].ToString() &&
                            board.GetCell(row + 2, col - 2) == checkSOS[2].ToString())
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}

public class Player : IGamePlayer
{
    public string Name { get; set; }
    public string Symbol { get; set; }

    public Player(string name, string symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public virtual void MakeMove(BoardGameUI boardGame)
    {
        throw new NotImplementedException("MakeMove should be implemented in derived classes.");
    }
}

public interface IGamePlayer
{
    string Name { get; set; }
    string Symbol { get; set; }
    void MakeMove(BoardGameUI boardGame);
}

public class HumanPlayer : Player
{
    public HumanPlayer(string name, string symbol) : base(name, symbol)
    {

    }

    public override void MakeMove(BoardGameUI boardGame)
    {
        int row = -1, column = -1;
        bool validMove = false;

        while (!validMove)
        {
            Console.WriteLine($"It's {Name}'s turn.");

            if (boardGame is ConnectFourGame)
            {
                // For ConnectFour, only ask for the column.
                Console.Write("Enter column: ");
                column = Convert.ToInt32(Console.ReadLine()) - 1;
            }
            else if (boardGame is SOSGame)
            {
                // For SOS, ask for both row and column.
                Console.Write("Enter row: ");
                row = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.Write("Enter column: ");
                column = Convert.ToInt32(Console.ReadLine()) - 1;
            }

            if (boardGame.IsValidMove(row, column, Symbol))
            {
                boardGame.MakeMove(row, column, Symbol);
                validMove = true;
                Console.WriteLine($"You chose row {row + 1} and column {column + 1}.");
            }
            else
            {
                Console.WriteLine("Invalid move. Please try again.");
            }
        }
    }
}

public class GameState
{
    private string[,] boardSnapshot;
    private string currentPlayer;

    public GameState(string[,] boardSnapshot, string currentPlayer)
    {
        if (boardSnapshot == null || currentPlayer == null)
        {
            throw new ArgumentNullException("Arguments to GameState cannot be null.");
        }

        this.boardSnapshot = (string[,])boardSnapshot.Clone();
        this.currentPlayer = currentPlayer;
    }

    public string[,] GetBoardSnapshot()
    {
        return (string[,])boardSnapshot.Clone();
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }
}

public class GameManager
{
    private BoardGameUI boardGame;
    private List<IGamePlayer> players;
    private int currentPlayerIndex;
    private GameState gameState;

    private string[,] boardSnapshot;  
    private string currentPlayer;    

    private Stack<GameState> undoStack = new Stack<GameState>();
    private Stack<GameState> redoStack = new Stack<GameState>();


    private void StoreCurrentGameState()
    {
        var currentState = new GameState(boardSnapshot, currentPlayer);
        undoStack.Push(currentState);
    }

    public GameManager(BoardGameUI boardGame, List<IGamePlayer> players)
    {
        this.boardGame = boardGame;
        this.players = players;
        currentPlayerIndex = 0;

        boardSnapshot = new string[boardGame.Rows, boardGame.Columns]; 
        currentPlayer = players[currentPlayerIndex].Symbol; 

        gameState = new GameState(boardSnapshot, currentPlayer);  

    }

    public void StartGame()
    {
        Console.WriteLine("Game Started!");

        boardGame.PrintBoard();

        while (!IsGameOver())
        {
            IGamePlayer currentPlayer = players[currentPlayerIndex];
            currentPlayer.MakeMove(boardGame);

            if (boardGame.CheckForWinner(currentPlayer.Symbol))
            {
                boardGame.PrintBoard();
                Console.WriteLine($"{currentPlayer.Name} has won!");
                return;
            }
            boardGame.PrintBoard();
            SwitchTurn();
        }

        Console.WriteLine("It's a tie!");
    }

    public bool IsGameOver()
    {
        // Implement logic to check if game is over
        return false;
    }

    public void SwitchTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

    public void SaveGame(string filePath)
    {
        // Implement save game logic
        boardGame.SaveGame(filePath);
    }

    public void LoadGame(string filePath)
    {
        // Implement load game logic
        boardGame.LoadGame(filePath);
    }

    public void Undo()
    {
        boardGame.UndoMove();
    }

    public void Redo()
    {
        boardGame.RedoMove();
    }

    public void ShowHelp()
    {
        // Implement a basic help system
        Console.WriteLine("Help: Use commands 'save', 'load', 'undo', 'redo', etc.");
    }
}

public class ConnectFourGame : BoardGame
{
    public ConnectFourGame() : base(6, 7) 
    {

    }

    public override bool IsValidMove(int row, int column, string player)
    {
        if (column < 0 || column >= Columns)
        {
            return false;
        }

        for (int i = Rows - 1; i >= 0; i--)
        {
            if (board.GetCell(i, column) == " ")
            {
                return true;
            }
        }

        return false;
    }

    public override void MakeMove(int row, int column, string player)
    {

        for (int i = Rows - 1; i >= 0; i--)
        {
            if (board.GetCell(i, column) == " ")
            {
                board.SetCell(i, column, player);
                break;
            }
        }


    }

    public override bool CheckForWinner(string player)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (CheckDirection(i, j, 1, 0, player) || 
                    CheckDirection(i, j, 0, 1, player) || 
                    CheckDirection(i, j, 1, 1, player) || 
                    CheckDirection(i, j, 1, -1, player))  
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckDirection(int row, int col, int dRow, int dCol, string player)
    {
        for (int i = 0; i < 4; i++) 
        {
            int newRow = row + i * dRow;
            int newCol = col + i * dCol;

            if (newRow < 0 || newRow >= 6 || newCol < 0 || newCol >= 7)
            {
                return false;
            }

            if (board.GetCell(newRow, newCol) != player)
            {
                return false;
            }
        }

        return true;
    }
}

public class ComputerPlayer : Player
{
    private Random random;

    public ComputerPlayer(string name, string symbol) : base(name, symbol)
    {
        random = new Random();
    }

    public override void MakeMove(BoardGameUI boardGame)
    {
        bool validMove = false;

        while (!validMove)
        {
            int row = random.Next(0, boardGame.Rows);
            int column = random.Next(0, boardGame.Columns);

            if (boardGame.IsValidMove(row, column, Symbol))
            {

                boardGame.MakeMove(row, column, Symbol);
                validMove = true;
                Console.WriteLine($"Computer chose row {row + 1} and column {column + 1}.");
            }
        }

    }
}

public static class CommandHelp
{
    public static void ShowHelp()
    {
        Console.WriteLine("Welcome to the Help Section.");
        Console.WriteLine("---------------------------");
        Console.WriteLine("Here are some basic commands and their usage:");

        Console.WriteLine("MOVE <row> <column> - To make a move at a given row and column.");

        Console.WriteLine("UNDO - To undo the last move.");

        Console.WriteLine("REDO - To redo the last undone move.");

        Console.WriteLine("SAVE <file_path> - To save the current game state to a file.");

        Console.WriteLine("LOAD <file_path> - To load a game state from a file.");

        Console.WriteLine("QUIT - To quit the game.");

        Console.WriteLine("Note: Replace <row>, <column>, and <file_path> with actual values.");
    }
}

public class Board
{
    private string[,] board;

    public Board(int rows, int columns)
    {
        board = new string[rows, columns];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = " ";
            }
        }
    }

    public void SetCell(int row, int column, string symbol)
    {
        if (IsValidCell(row, column))
        {
            board[row, column] = symbol;
        }
    }

    public string GetCell(int row, int column)
    {
        if (IsValidCell(row, column))
        {
            return board[row, column];
        }
        return null;
    }

    /*private*/
    public bool IsValidCell(int row, int column)
    {
        return row >= 0 && row < board.GetLength(0) && column >= 0 && column < board.GetLength(1);
    }

    public void PrintBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j]);
                if (j < board.GetLength(1) - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();

            if (i < board.GetLength(0) - 1)
            {
                Console.WriteLine(new string('-', board.GetLength(1) * 4 - 1));
            }
        }
    }
}
