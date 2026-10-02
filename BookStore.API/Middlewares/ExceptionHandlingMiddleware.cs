namespace BookStore.API.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(
                        "The response has already started, Trace: {TraceId}",
                        context.TraceIdentifier);
                    throw;
                }
                await HandleExceptionAsync(context, exception);
            }
        }
        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = GetStatusCode(exception);
            var message = GetMessage(exception);
            LogException(context, exception, statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object?>.Failure(message);
            var json = JsonSerializer.Serialize(
                response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            await context.Response.WriteAsync(json,
                context.RequestAborted);
        }
        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                BussinessRuleException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static string GetMessage(Exception exception)
        {
            return exception switch
            {
                AppException => exception.Message,
                _ => "An unexpected error occurred."
            };
        }

        private void LogException(
            HttpContext context,
            Exception exception,
            int statusCode)
        {
            if (statusCode >= (int)HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception occurred. " +
                    "Mehtod: {Method}," +
                    " Path: {Path}, StatusCode: {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    statusCode);
                return;
            }
            _logger.LogWarning(
                exception,
                "Handled exception occurred with status code {StatusCode} "
                + "Method: {Method}, Path: {Path}, TraceId: {TraceId}",
                statusCode,
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);
        }


    }
}
