using SharedKernel.ValueObjects;

namespace SharedKernel.Interfaces;

public interface ICoupon
{
    public Discount Discount { get; }
    public string Code { get; }
    public int UsageLimit { get; }
    public bool AutoApply { get; }
}
