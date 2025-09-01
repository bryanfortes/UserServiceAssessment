using System.ComponentModel.DataAnnotations;
using UserServiceAssessment.Data;
using UserServiceAssessment.Data.Interfaces;
using UserServiceAssessment.Helpers.Exceptions;
using UserServiceAssessment.Models.User;

namespace UserServiceAssessment.Services
{
    public interface IUserService
    {
        int AddUser(CreateUserModel user);
        bool RemoveUserById(int userId);
        UserModel GetUserById(int userId);
        UserModel UpdateUserEmailById(int userId, string newEmail);
        List<UserModel> GetAllUsers();
    }
    public class UserService(IUserContext store) : IUserService
    {
        public int AddUser(CreateUserModel user)
        {
            if (String.IsNullOrEmpty(user.Name))
                throw new ArgumentException("Name cannot be blank.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Name, @"^[a-zA-Z\s-]+$"))
                throw new ArgumentException("Name can only contain letters, spaces, and hyphens.");
            CheckIfEmailIsValid(user.Email);
            return store.Add(user);
        }

        public UserModel GetUserById(int userId)
        {
            var user = store.Get(userId);
            return user is null ? throw new UserNotFoundException(userId) : user;
        }

        public bool RemoveUserById(int userId)
        {
            if (!store.Remove(userId))
                throw new UserNotFoundException(userId);
            return true;
        }

        public UserModel UpdateUserEmailById(int userId, string newEmail)
        {
            CheckIfEmailIsValid(newEmail);
            if (!store.UpdateUserEmailById(userId, newEmail))
                throw new UserNotFoundException(userId);
            return GetUserById(userId);
        }

        public List<UserModel> GetAllUsers() => store.GetAllUsers();

        private static void CheckIfEmailIsValid(string email)
        {
            var validator = new EmailAddressAttribute();
            if (!validator.IsValid(email))
            {
                throw new ArgumentException($"Invalid email format. \"{email}\" is not a valid email.");
            }
        }
    }
}