using System.Drawing;

namespace othello
{
    public enum eCoinColor
    {
        Black,
        White,
        Empty,
    }

    public class Board
    {
        private eCoinColor[,] m_BoardMatrix;
        private int m_SizeOfBoard;

        public eCoinColor[,] BoardMatrix
        {
            get { return m_BoardMatrix; }
            set { m_BoardMatrix = value; }
        }

        public int SizeOfBoard
        {
            get { return m_SizeOfBoard; }
            set { m_SizeOfBoard = value; }
        }

        public Board(int i_SizeOfBoard)
        {
            SizeOfBoard = i_SizeOfBoard;
            BoardMatrix = new eCoinColor[SizeOfBoard, SizeOfBoard];
            this.InitBoard();
        }

        public static eCoinColor SwapColor(eCoinColor i_ColorToSwap)
        {
            if (i_ColorToSwap == eCoinColor.Black)
            {
                return eCoinColor.White;
            }
            else if (i_ColorToSwap == eCoinColor.White)
            {
                return eCoinColor.Black;
            }
            else
            {
                return eCoinColor.Empty;
            }
        }

        public Board Clone()
        {
            Board clonedBoard = new Board(SizeOfBoard);

            for (int y = 0; y < clonedBoard.BoardMatrix.GetLength(0); ++y)
            {
                for (int x = 0; x < clonedBoard.BoardMatrix.GetLength(1); ++x)
                {
                    clonedBoard.BoardMatrix[x, y] = BoardMatrix[x, y];
                }
            }

            return clonedBoard;
        }

        public void InitBoard()
        {
            int middleOfHeight = BoardMatrix.GetLength(0) / 2;
            int middleOfWidth = BoardMatrix.GetLength(1) / 2;

            for (int x = 0; x < BoardMatrix.GetLength(0); ++x)
            {
                for (int y = 0; y < BoardMatrix.GetLength(1); ++y)
                {
                    BoardMatrix[x, y] = eCoinColor.Empty;
                }
            }

            // Init the middle 4 coins
            BoardMatrix[middleOfHeight, middleOfWidth - 1] = eCoinColor.Black;
            BoardMatrix[middleOfHeight - 1, middleOfWidth - 1] = eCoinColor.White;
            BoardMatrix[middleOfHeight - 1, middleOfWidth] = eCoinColor.Black;
            BoardMatrix[middleOfHeight, middleOfWidth] = eCoinColor.White;
        }

        public void MakeMove(Point i_PointToPlay, eCoinColor i_ColorPlaying)
        {
            setCellColor(i_PointToPlay, i_ColorPlaying);
            for (int x = -1; x < 2; ++x)
            {
                for (int y = -1; y < 2; ++y)
                {
                    if (IsLegalDirection(i_ColorPlaying, i_PointToPlay, x, y))
                    {
                        setColorInGivenDirection(i_PointToPlay, i_ColorPlaying, x, y);
                    }
                }
            }
        }

        private void setCellColor(Point i_Cell, eCoinColor i_ColorToSet)
        {
            BoardMatrix[i_Cell.X, i_Cell.Y] = i_ColorToSet;
        }

        private void setColorInGivenDirection(Point i_StartCell, eCoinColor i_CoinColor, int i_XDirection, int i_YDirection)
        {
            i_StartCell.X += i_XDirection;
            i_StartCell.Y += i_YDirection;
            while (IsInBorders(i_StartCell) && BoardMatrix[i_StartCell.X, i_StartCell.Y] != i_CoinColor)
            {
                setCellColor(i_StartCell, i_CoinColor);
                i_StartCell.X += i_XDirection;
                i_StartCell.Y += i_YDirection;
            }
        }

        public void UpdateScoreOfUser(out int o_SumOfBlackPoints, out int o_SumOfWhitePoints)
        {
            o_SumOfBlackPoints = 0;
            o_SumOfWhitePoints = 0;

            foreach (eCoinColor Cell in BoardMatrix)
            {
                if (Cell == eCoinColor.Black)
                {
                    ++o_SumOfBlackPoints;
                }
                else if (Cell == eCoinColor.White)
                {
                    ++o_SumOfWhitePoints;
                }
            }
        }

        public bool IsEmptyCell(Point i_Point)
        {
            return BoardMatrix[i_Point.X, i_Point.Y] == eCoinColor.Empty;
        }

        public bool IsInBorders(Point i_Cell)
        {
            return i_Cell.X >= 0 && i_Cell.X < BoardMatrix.GetLength(0) && i_Cell.Y >= 0 && i_Cell.Y < BoardMatrix.GetLength(1);
        }

        public bool IsLegalDirection(eCoinColor i_CoinColor, Point i_StartCell, int i_XDirection, int i_YDirection)
        {
            Point Cell = new Point(i_StartCell.X + i_XDirection, i_StartCell.Y + i_YDirection);
            if (IsInBorders(Cell) && BoardMatrix[Cell.X, Cell.Y] == SwapColor(i_CoinColor))
            {
                Cell.X += i_XDirection;
                Cell.Y += i_YDirection;
                while (IsInBorders(Cell) && BoardMatrix[Cell.X, Cell.Y] == SwapColor(i_CoinColor))
                {
                    Cell.X += i_XDirection;
                    Cell.Y += i_YDirection;
                }

                return IsInBorders(Cell) && BoardMatrix[Cell.X, Cell.Y] == i_CoinColor;
            }

            return false;
        }
    }
}