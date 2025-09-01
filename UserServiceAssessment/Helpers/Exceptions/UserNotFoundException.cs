namespace UserServiceAssessment.Helpers.Exceptions;

public class UserNotFoundException : Exception
{
    public int UserId { get; set; }

    public UserNotFoundException(int userId) : base($"User with the Id of {userId} was not found.")
    {
        UserId = userId;
    }
    public UserNotFoundException(int userId, Exception innerException)
        : base($"User with ID {userId} was not found.", innerException)
    {
        UserId = userId;
    }
}