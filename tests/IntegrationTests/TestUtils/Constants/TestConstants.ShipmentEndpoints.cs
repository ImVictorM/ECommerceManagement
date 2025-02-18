namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the shipment resources.
    /// </summary>
    public static class ShipmentEndpoints
    {
        /// <summary>
        /// Endpoint to advance a shipment status.
        /// </summary>
        public static string AdvanceShipmentStatus(string id) => $"/shipments/{id}/status";
    }
}
