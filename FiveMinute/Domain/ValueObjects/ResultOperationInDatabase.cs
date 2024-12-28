namespace FiveMinute.Data;

public struct ResultOperationInDatabase {
	public bool Success { get; private set; }
	public Exception Exception { get; private set; }

	public ResultOperationInDatabase(bool success, Exception exception) {
		Success = success;
		Exception = exception;
	}

	public static implicit operator bool(ResultOperationInDatabase resultOperationInDatabase)
		=> resultOperationInDatabase.Success;
}