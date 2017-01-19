namespace Dots
{
    interface IDot
    {
        byte Value { get; set; }
        bool Chain { get; set; }
        bool Active { get; set; }
        bool Closed { get; set; }
        Dot Clone();
    }
}
