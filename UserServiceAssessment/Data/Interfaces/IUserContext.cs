using UserServiceAssessment.Models.User;

namespace UserServiceAssessment.Data.Interfaces
{
    public interface IUserContext
    {
        int Add(CreateUserModel user);
        bool Remove(int userId);
        UserModel? Get(int userId);
        bool UpdateUserEmailById(int userId, string newEmail);
        List<UserModel> GetAllUsers();
    }
}