using BookStore.DTOs;

namespace BookStore.HandleResponse {
     public class Response
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public object? Data { get; set; }

        public Response(string message, int status)
        {
            Message = message;
            Status = status;
            Data= null;
        }

        public Response(string message, int status, object? data)
        {
            Message = message;
            Status = status;
            Data = data;
        }
    }
}