
public struct Cell
{
    public bool visited;
    public bool north, south, west, east;

    public override string ToString()
    {
        return $"n:{north}, s:{south}, w:{west}, e:{east}";
    }
}
