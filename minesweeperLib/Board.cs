public class Board
{
    const int MINE = -1;
    public int RowsAmount { get; set; }
    public int ColumnsAmount { get; set; }
    public int MinesAmount { get; set; }
    public List<Cell> Cells { get; set; }
    public gameState State { get; set; }

    public Board(int rows, int cols, int mines)
    {
        RowsAmount = rows;
        ColumnsAmount = cols;
        MinesAmount = mines;
        Cells = new();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Cells.Add(new Cell(i,j));
            }
        }
        State = gameState.Active;
    }
    public void SetMines()
    {
        for (int i = 0; i < MinesAmount; i ++)
        {
            var coords = GetRandomCoords();
            Console.WriteLine($"Mine at: ({coords.Item1}, {coords.Item2}) ");
            var cell = Cells.Where(c => c.Coords == coords && c.Value != -1).FirstOrDefault();
            if (cell != null)
                cell.Value = MINE;
            else
                continue;
        }
    }
    private (int,int) GetRandomCoords()
    {
        var rand = new Random();
        int xCoord = rand.Next(RowsAmount);
        int yCoord = rand.Next(ColumnsAmount);

        return (xCoord, yCoord);
    }
    public void RevealCell((int,int) coords)
    {
        if (coords.Item1 < RowsAmount && coords.Item2 < ColumnsAmount)
        {
            Cell cell = Cells.Where(c => c.Coords == coords).First();
            if (cell.State == cellState.Hidden)
            {
                cell.State = cellState.Revealed;
                if (cell.Value == MINE)
                    State = gameState.Lose;
                else 
                {
                    for (int i = coords.Item1-1; i <= coords.Item1 + 1; i++)
                    {
                        for (int j = coords.Item2-1; j <= coords.Item2 + 1; j++)
                        {
                            if (i >= 0 && j >= 0 && i < RowsAmount && j < ColumnsAmount)
                            {
                                Cell adjacentCell = Cells.Where(c => c.Coords == (i,j)).First();
                                if (adjacentCell.Value == MINE)
                                {
                                    cell.Value++;
                                }
                            }
                        }
                    }
                    if (cell.Value == 0)
                        RevealAdjacentCells(coords);
                    if (HaveYouWon())
                        State = gameState.Win;
                }
            }
        }
    }

    public void RevealAdjacentCells((int,int) coords)
    {
        for (int i = coords.Item1-1; i <= coords.Item1 + 1; i++)
        {
            for (int j = coords.Item2-1; j <= coords.Item2 + 1; j++)
            {
                if (i >= 0 && j >= 0 && i < RowsAmount && j < ColumnsAmount)
                {
                    RevealCell(new (i, j));
                }
            }
        }
    }

    public void FlagCell((int,int) coords)
    {
        if (coords.Item1 < RowsAmount && coords.Item2 < ColumnsAmount)
        {
            Cell cell = Cells.Where(c => c.Coords == coords).First();
            if (cell.State == cellState.Hidden)
                cell.State = cellState.Flagged;
            else if (cell.State == cellState.Flagged)
                cell.State = cellState.Hidden;
        }
    }
    private bool HaveYouWon()
    {
        int revealedCells = Cells.Where(c => c.State == cellState.Revealed).Count();
        if (revealedCells == (RowsAmount * ColumnsAmount - MinesAmount))
            return true;
        else
            return false;
    }
}