using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Defines payment constants.
    /// </summary>
    public static class Payment
    {
        /// <summary>
        /// The payment installments constant.
        /// </summary>
        public const int Installments = 1;
        /// <summary>
        /// The payment amount constant.
        /// </summary>
        public const decimal Amount = 500m;
        /// <summary>
        /// The payer id constant.
        /// </summary>
        public static readonly UserId PayerId = User.Id;
        /// <summary>
        /// The order id constant.
        /// </summary>
        public static readonly OrderId OrderId = Order.Id;
        /// <summary>
        /// The tokenized card data constant.
        /// </summary>
        public const string CardToken = "ff8080814c11e237014c1ff593b57b4d";
    }
}
