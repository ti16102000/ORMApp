namespace NHibernateApp
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse()
        {
            Success = true;
        }

        public ApiResponse(object data)
        {
            Success = true;
            Data = data;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }

}
