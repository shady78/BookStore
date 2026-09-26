namespace BookStore.API.Common
{
    public class ApiResponse<T> 
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        //public List<string> Errors { get; set; }
        public object? Errors { get; set; }

        public static ApiResponse<T> Success(T data , string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> Failure(string error)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Errors = error
            };
        }

        public static ApiResponse<T> Failure(object errors, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }


    }
}
