using BookStore.DTOs;
using BookStore.Models;

namespace BookStore.HandleResponse {
     public class Response
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public object? Data { get; set; }
        public Book Book {get; set;}
        public List<Order> Items { get; set;}

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

         public Response(string message, int status, object? data, Book? book)
        {
            Message = message;
            Status = status;
            Data = data;
            Book = book;
        }

         public Response(string message, int status, List<Order> items)
        {
            Message = message;
            Status = status;
            Items = items;
        }

    }
}