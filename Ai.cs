using System.Drawing;

namespace othello
{
    public class Ai
    {
        public static Point FindMaxMinPoint(GameManager i_Game)
        {
            int maxMinScore = 0;
            Point maxMinPoint = new Point(0, 0);

            foreach (Point Move in i_Game.AvailableMoves)
            {
                GameManager ClonedGame = i_Game.Clone();
                ClonedGame.Board.MakeMove(Move, ClonedGame.ColorPlaying);
                ClonedGame.ColorPlaying = eCoinColor.Black;
                ClonedGame.GetAvailableMoves();
                if (ClonedGame.IsThereAvailableMove())
                {
                    int minScore = FindMinMovePointScore(ClonedGame) + GetRankOfPoint(Move, i_Game.Board.SizeOfBoard);
                    if (minScore > maxMinScore)
                    {
                        maxMinPoint = Move;
                        maxMinScore = minScore;
                    }
                }
                else
                {
                    return Move;
                }
            }

            return maxMinPoint;
        }

        private static int FindMinMovePointScore(GameManager i_Game)
        {
            int scoreInGame;
            int minScore = int.MaxValue; // Infinity

            foreach (Point MoveToCheck in i_Game.AvailableMoves)
            {
                GameManager ClonedGame = i_Game.Clone();
                ClonedGame.Board.MakeMove(MoveToCheck, ClonedGame.ColorPlaying);
                ClonedGame.GetScoreOfUsers(out int o_unusedValue, out scoreInGame);
                if (minScore > scoreInGame)
                {
                    minScore = scoreInGame;
                }
            }

            return minScore;
        }

        /*Area's Function*/
        /*Map of area's: https://i.imgur.com/9u5yxnS.jpg */
        public static int GetRankOfPoint(Point i_Cell, int i_SizeBoard)
        {
            if (isArea3(i_Cell, i_SizeBoard))
            {
                return 3;
            }
            else if (isArea2(i_Cell, i_SizeBoard))
            {
                return 2;
            }
            else if (isArea4(i_Cell, i_SizeBoard))
            {
                return 5;
            }
            else if (isArea5(i_Cell, i_SizeBoard))
            {
                return 7;
            }
            else
            {
                return 0;
            }
        }

        private static bool isArea2(Point i_Cell, int i_SizeBoard)
        {
            return ((i_Cell.X == 1 || i_Cell.X == i_SizeBoard - 2) && (i_Cell.Y > 1 && i_Cell.Y < i_SizeBoard - 2)) ||
                    ((i_Cell.Y == 1 || i_Cell.Y == i_SizeBoard - 2) && (i_Cell.X > 1 && i_Cell.X < i_SizeBoard - 2));
        }

        private static bool isArea3(Point i_Cell, int i_SizeBoard)
        {
            return i_Cell.X > 1 && i_Cell.X < i_SizeBoard - 2 && i_Cell.Y > 1 && i_Cell.Y < i_SizeBoard - 2;
        }

        private static bool isArea4(Point i_Cell, int i_SizeBoard)
        {
            return ((i_Cell.X == 0 || i_Cell.X == i_SizeBoard - 1) && (i_Cell.Y > 1 && i_Cell.Y < i_SizeBoard - 2)) || ((i_Cell.Y == 0 || i_Cell.Y == i_SizeBoard - 1) && (i_Cell.X > 1 && i_Cell.X < i_SizeBoard - 2));
        }

        private static bool isArea5(Point i_Cell, int i_SizeBoard)
        {
            return ((i_Cell.X == 0) || (i_Cell.X == i_SizeBoard - 1)) && ((i_Cell.Y == 0) || (i_Cell.Y == i_SizeBoard - 1));
        }
    }
}
