public class Cell
{
    public (int,int) Coords { get; set; }
    public cellState State { get; set; }
    public int Value { get; set; }

    public Cell(int x, int y)
    {
        Coords = new(x,y);
        State = cellState.Hidden;
        Value = 0;
    }
}