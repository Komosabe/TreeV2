namespace TreeV2.Utilities
{
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }

        private OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static OperationResult CreateSuccess()
        {
            return new OperationResult(true, string.Empty);
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult(false, message);
        }
    }
}
