
namespace TaskManagement.Domain.Exceptions
{
    public class InvalidTaskDataException : DomainException
    {
        public InvalidTaskDataException(string message) 
            : base(message, "invalid_task_data")
        {
        }
    }
}
