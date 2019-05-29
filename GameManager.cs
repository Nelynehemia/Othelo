using System.Collections.Generic;
using System.Drawing;

namespace othello
{
    public class GameManager
    {
        /*Fields*/
        public enum GameOption
        {
            UserVsUser,
            UserVsComputer,
        }

        private readonly GameOption m_GameType;
        private readonly User m_BlackPlayer;
        private readonly User m_WhitePlayer;
        private List<Point> m_AvailableMoves = new List<Point>();
        private Board m_Board;
        private eCoinColor m_ColorPlaying = eCoinColor.White;

        public List<Point> AvailableMoves
        {
            get { return m_AvailableMoves; }
            set { m_AvailableMoves = value; }
        }

        public eCoinColor ColorPlaying
        {
            get { return m_ColorPlaying; }
            set { m_ColorPlaying = value; }
        }

        public Board Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }

        public GameManager()
        {
            User.UserOption whitePlayerType;
            UserInterface.Start(out int o_BoardSize, out GameOption o_GameType, out string o_NameOfBlackPlayer, out string o_NameOfWhitePlayer);

            Board = new Board(o_BoardSize);
            m_GameType = o_GameType;
            UserInterface.DrawBoard(Board);

            if (o_GameType == GameOption.UserVsComputer)
            {
                whitePlayerType = User.UserOption.Computer;
            }
            else
            {
                whitePlayerType = User.UserOption.Human;
            }

            m_BlackPlayer = new User(User.UserOption.Human, eCoinColor.Black, o_NameOfBlackPlayer);
            m_WhitePlayer = new User(whitePlayerType, eCoinColor.White, o_NameOfWhitePlayer);
        }

        public GameManager(GameOption i_GameType, User i_BlackPlayer, User i_WhitePlayer)
        {
            m_GameType = i_GameType;
            m_BlackPlayer = i_BlackPlayer;
            m_WhitePlayer = i_WhitePlayer;
        }

        public GameManager Clone()
        {
            GameManager clonedGame = new GameManager(m_GameType, m_BlackPlayer, m_WhitePlayer);
            clonedGame.m_ColorPlaying = m_ColorPlaying;
            clonedGame.Board = Board.Clone();
            m_AvailableMoves = CloneAvailableMoves();

            return clonedGame;
        }

        public List<Point> CloneAvailableMoves()
        {
            List<Point> ClonedAvailableMoves = new List<Point>();
            foreach (Point PointToMove in m_AvailableMoves)
            {
                ClonedAvailableMoves.Add(PointToMove);
            }

            return ClonedAvailableMoves;
        }

        public void StartGame()
        {
            swapTurns();
            gameOver(this);
        }

        private void swapTurns()
        {
            int countOfSwapTurnWithOutMoves = 0;

            while (countOfSwapTurnWithOutMoves != 2)
            {
                swapCurrentColorPlaying();
                GetAvailableMoves();

                if (!IsThereAvailableMove())
                {
                    ++countOfSwapTurnWithOutMoves;
                    UserInterface.PrintAnyValidMoves(m_ColorPlaying);
                }
                else
                {
                    countOfSwapTurnWithOutMoves = 0;
                    playTurn();
                }
            }
        }

        private void playTurn()
        {
            if (m_GameType == GameOption.UserVsComputer && m_ColorPlaying == m_WhitePlayer.CoinColor)
            {
                Point cell = Ai.FindMaxMinPoint(this);
                Board.MakeMove(cell, ColorPlaying);
                UserInterface.DrawBoard(Board);
            }
            else
            {
                bool isLegalMove = true;
                UserInterface.PrintFormatForTheUser();
                UserInterface.PrintUserPlayTurn(m_ColorPlaying, m_BlackPlayer, m_WhitePlayer);

                while (isLegalMove)
                {
                    Point cell = UserInterface.GetUserMove(Board, m_ColorPlaying);
                    if (IsPointInAvailableMoveList(cell))
                    {
                        Board.MakeMove(cell, ColorPlaying);
                        UserInterface.DrawBoard(Board);
                        isLegalMove = false;
                    }
                    else
                    {
                        UserInterface.PrintNotBlockCoins();
                    }
                }
            }
        }

        private void gameOver(GameManager i_Game)
        {
            Board.UpdateScoreOfUser(out int o_SumOfBlackPoints, out int o_SumOfWhitePoints);

            if (o_SumOfBlackPoints > o_SumOfWhitePoints)
            {
                UserInterface.PrintEndOfTheGame(o_SumOfBlackPoints, o_SumOfWhitePoints, m_BlackPlayer, i_Game);
            }
            else
            {
                UserInterface.PrintEndOfTheGame(o_SumOfBlackPoints, o_SumOfWhitePoints, m_WhitePlayer, i_Game);
            }

            startNewRound();
        }

        private void swapCurrentColorPlaying()
        {
            m_ColorPlaying = Board.SwapColor(m_ColorPlaying);
        }

        private void startNewRound()
        {
            if (UserInterface.IfTheUserWantNewGame())
            {
                Board.InitBoard();
                UserInterface.DrawBoard(Board);
                Board.SwapColor(eCoinColor.Black);
                StartGame();
            }
            else
            {
                System.Environment.Exit(1);
            }
        }

        public void GetScoreOfUsers(out int o_SumOfBlackPoints, out int o_SumOfWhitePoints)
        {
            Board.UpdateScoreOfUser(out o_SumOfBlackPoints, out o_SumOfWhitePoints);
        }

        public void GetAvailableMoves()
        {
            m_AvailableMoves.Clear();
            Point cell = new Point(0, 0);
            for (cell.X = 0; cell.X < Board.SizeOfBoard; ++cell.X)
            {
                for (cell.Y = 0; cell.Y < Board.SizeOfBoard; ++cell.Y)
                {
                    if (IsPointIsAvailableMove(cell))
                    {
                        m_AvailableMoves.Add(cell);
                    }
                }
            }
        }

        public bool IsThereAvailableMove()
        {
            return m_AvailableMoves.Count != 0;
        }

        public bool IsPointInAvailableMoveList(Point i_PointToCheck)
        {
            return m_AvailableMoves.Contains(i_PointToCheck);
        }

        public bool IsPointIsAvailableMove(Point i_PointToCheck)
        {
            bool isFoundAvailableMove = false;
            if (Board.IsEmptyCell(i_PointToCheck))
            {
                for (int x = -1; x < 2 && !isFoundAvailableMove; ++x)
                {
                    for (int y = -1; y < 2 && !isFoundAvailableMove; ++y)
                    {
                        isFoundAvailableMove = Board.IsLegalDirection(m_ColorPlaying, i_PointToCheck, x, y);
                    }
                }
            }

            return isFoundAvailableMove;
        }
    }
}