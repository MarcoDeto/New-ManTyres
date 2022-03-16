namespace ManTyres.COMMON.Utils
{
    public class DataBridge<T>
    {
        public string? Message { get; set; }
        public DataBridgeStatusCode? StatusCode { get; set; }
        public T? Data { get; set; }
        public bool Success { get => StatusCode.HasValue && StatusCode.Value == DataBridgeStatusCode.Ok; }
    }
}
