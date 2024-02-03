namespace Shared;

public record ServiceResponse<T>(bool Success, T? Data, string Message) where T : class;
