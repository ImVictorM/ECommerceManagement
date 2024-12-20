namespace SharedKernel.Interfaces;

public interface IDiscount
{
    public string Description { get; }
    public int Percentage { get; }
    public DateTimeOffset StartingDate { get; }
    public DateTimeOffset EndingDate { get; }
}
