using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Ex02.ConsoleUtils;

namespace othello
{
    public class UserInterface
    {
        public static Dictionary<eCoinColor, char> CoinsLetters = new Dictionary<eCoinColor, char>
        {
            {
                eCoinColor.Black, 'X'
            },
            {
                eCoinColor.White, 'O'
            },
            {
                eCoinColor.Empty, ' '
            }
        };

        /*Start of the game*/
        public static void Start(out int o_BoardSize, out GameManager.GameOption o_GameType, out string o_NameOfPlayer1, out string o_NameOfPlayer2)
        {
            o_BoardSize = 0;
            o_GameType = GameManager.GameOption.UserVsComputer;
            o_NameOfPlayer2 = "Computer";
            bool validInput = true;

            // NameOfPlayer1
            Console.WriteLine("Type a user name: ");
            o_NameOfPlayer1 = Console.ReadLine();

            // GameOption
            while (validInput)
            {
                Console.WriteLine("For two players type 0 ,For play against the computer type 1: ");
                string selectUse = Console.ReadLine();

                if (selectUse == "0")
                {
                    o_GameType = GameManager.GameOption.UserVsUser;
                    Console.WriteLine("Second user name: ");
                    o_NameOfPlayer2 = Console.ReadLine();
                    validInput = false;
                }
                else if (selectUse == "1") 
                {
                    o_GameType = GameManager.GameOption.UserVsComputer;
                    validInput = false;
                }
                else
                {
                    Console.WriteLine("Invalid typing Try again ");
                }
            }

            validInput = true;

            // boardSize
            while (validInput)
            {
                Console.WriteLine("Select matrix size 6 or 8: ");
                int.TryParse(Console.ReadLine(), out int boardSize);
                if (boardSize == 8 || boardSize == 6)
                {
                    o_BoardSize = boardSize;
                    validInput = false;
                }
                else
                {
                    Console.WriteLine("Invalid typing Try again ");
                }
            }
        }

        /*Printing of The Board*/
        public static void DrawBoard(Board i_Board)
        {
            Screen.Clear();

            PrintFirstRowInTheMatrix(i_Board); //// first row
            PrintLineSeparator(i_Board); //// Line Separator "==="
            PrintMainMatrix(i_Board); ////The main matrix
        }

        private static void PrintMainMatrix(Board i_Board)
        {
            for (int y = 0; y < i_Board.SizeOfBoard; ++y)
            {
                Console.Write((y + 1) + " |");

                for (int x = 0; x < i_Board.SizeOfBoard; ++x)
                {
                    char typeColor = CoinsLetters[i_Board.BoardMatrix[x, y]];
                    Console.Write(" " + typeColor + " |");
                }

                PrintLineSeparator(i_Board);
            }
        }

        private static void PrintFirstRowInTheMatrix(Board i_Board)
        {
            char letter = 'A';
            Console.Write("    ");

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < i_Board.SizeOfBoard; i++)
            {
                builder.Append(letter++).Append("   ");
            }

            Console.Write(builder);       
        }

        private static void PrintLineSeparator(Board i_Board)
        {
            Console.Write("\n  ");
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i <= i_Board.SizeOfBoard * 4; i++)
            {
                builder.Append("=");
            }

            Console.WriteLine(builder);      
        }

        /*PlayMove functions*/
        public static Point GetUserMove(Board i_Board, eCoinColor i_ColorPlaying)
        {
            bool validInput = true;
            Point UserMovePoint = new Point();

            while (validInput)
            {
                char[] input = Console.ReadLine().ToCharArray();

                if (IsValidFormat(input, i_Board))
                {
                    UserMovePoint.X = input[0] - 'A';
                    UserMovePoint.Y = input[1] - '1';
                    validInput = false;
                }
                else if (input.Length == 1 && input[0] == 'Q')
                {
                    Screen.Clear();
                    Console.WriteLine("Hope you enjoyed the game");
                    System.Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine("Typing is not in range, Try again ");
                }
            }

            return UserMovePoint;
        }

        public static bool IsValidFormat(char[] i_input, Board i_Board)
        {
            bool isValidLength = i_input.Length <= 2 && i_input.Length != 0;
            bool isValidFormat = isValidLength && ((i_input[0] < 'A' + i_Board.SizeOfBoard) && (i_input[0] >= 'A')) &&
                                 i_input[1] >= '1' && i_input[1] < '1' + i_Board.SizeOfBoard;

            return isValidFormat;
        }

        public static void PrintAnyValidMoves(eCoinColor m_ColorPlaying)
        {
            Console.WriteLine("There were no moves to" + m_ColorPlaying + "\n");
        }

        public static void PrintFormatForTheUser()
        {
            Console.WriteLine("Enter a point in format A1");
        }

        public static void PrintUserPlayTurn(eCoinColor i_ColorPlaying, User m_Player1, User m_Player2)
        {
            if (i_ColorPlaying == eCoinColor.Black)
            {
                Console.Write(m_Player1.Name);
                Console.WriteLine(": (X) turn");
            }
            else
            {
                Console.Write(m_Player2.Name);
                Console.WriteLine(": White (O) turn");
            }
        }

        public static void PrintNotBlockCoins()
        {
            Console.WriteLine("Your point does not block opponent's coins, select again.");
        }

        /*End of game*/
        public static void PrintEndOfTheGame(int i_SumOfBlackPoints, int i_SumOfWhitePoints, User m_playerWin, GameManager m_Game)
        {
            Screen.Clear();

            if (i_SumOfBlackPoints == i_SumOfWhitePoints)
            {
                PrintEndOfGameDraw(i_SumOfWhitePoints);
            }
            else
            {
                PrintEndOfGameWithWinner(i_SumOfBlackPoints, i_SumOfWhitePoints, m_playerWin, m_Game);
            }
        }

        private static void PrintEndOfGameWithWinner(int i_SumOfBlackPoints, int i_SumOfWhitePoints, User playerWin, GameManager i_Game)
        {
            Console.WriteLine("GameOver!\n" + "The winner is: " + playerWin.Name + " With color " + playerWin.CoinColor);
            Console.WriteLine("Black coins:" + i_SumOfBlackPoints);
            Console.WriteLine("Whites coins:" + i_SumOfWhitePoints);
        }

        private static void PrintEndOfGameDraw(int i_SumOfPoints)
        {
            Console.WriteLine("GameOver!\n");
            Console.WriteLine("The result is a draw coins:" + i_SumOfPoints);
        }

        public static bool IfTheUserWantNewGame()
        {
            Console.WriteLine("\nDo you want to start a new game? Y / N");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                return true;
            }
            else
            {
                PrintOverTheGame();
                return false;
            }
        }

        private static void PrintOverTheGame()
        {
            Screen.Clear();
            Console.WriteLine("Hope you enjoyed the game");
        }
    }
}