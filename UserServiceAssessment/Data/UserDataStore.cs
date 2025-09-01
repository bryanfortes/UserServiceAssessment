using System.Collections.Concurrent;
using UserServiceAssessment.Data.Interfaces;
using UserServiceAssessment.Models.User;

namespace UserServiceAssessment.Data
{
    public class UserDataStore : IUserContext
    {
        private readonly ConcurrentDictionary<int, UserModel> _users = [];
        private int _nextId = 0;

        public int Add(CreateUserModel user)
        {
            var newUser = new UserModel
            {
                Id = Interlocked.Increment(ref _nextId),
                Name = user.Name,
                Email = user.Email
            };
            return _users.TryAdd(newUser.Id, newUser) ? newUser.Id : 0;
        }
        public bool Remove(int userId) => _users.TryRemove(userId, out _);


        public UserModel? Get(int userId) => _users.TryGetValue(userId, out UserModel? user) ? user : null;

        public bool UpdateUserEmailById(int userId, string newEmail)
        {
            return _users.TryGetValue(userId, out UserModel? existingUser) &&
                   _users.TryUpdate(userId,
                       new UserModel
                       {
                           Id = existingUser.Id,
                           Name = existingUser.Name,
                           Email = newEmail
                       },
                       existingUser);
        }

        public List<UserModel> GetAllUsers() => _users.Values.ToList();
    }
}