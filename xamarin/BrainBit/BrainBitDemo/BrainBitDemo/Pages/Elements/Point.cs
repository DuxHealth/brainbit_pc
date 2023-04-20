namespace BrainBitDemo.Pages.Elements;

internal class Point
{
    public float Left   { get; set; }
    public float Right  { get; set; }
    public float Top    { get; set; }
    public float Bottom { get; set; }

    public Point(float x, float y)
    {
        Left   = x;
        Right  = x;
        Top    = y * 2;
        Bottom = y * 3;
    }

}
