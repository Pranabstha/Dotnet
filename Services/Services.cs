using Todo.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Todo.NewFolder;


namespace Todo.Models
{
    public static class UsersService
    {
        public const string SeedUsername = "admin";
        public const string SeedPassword = "admin";

        private static void SaveAll(List<UserModel> users)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appUsersFilePath = Utils.GetAppUsersFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(appUsersFilePath, json);
        }

        public static List<UserModel> GetAll()
        {
            string appUsersFilePath = Utils.GetAppUsersFilePath();
            if (!File.Exists(appUsersFilePath))
            {
                return new List<UserModel>();
            }

            var json = File.ReadAllText(appUsersFilePath);

            return JsonSerializer.Deserialize<List<UserModel>>(json);
        }

        public static List<UserModel> Create(Guid userId, string username, string password, Role role)
        {
            List<UserModel> users = GetAll();
            bool usernameExists = users.Any(x => x.Username == username);

            if (usernameExists)
            {
                throw new Exception("Username already exists.");
            }

            users.Add(
                new UserModel
                {
                    Username = username,
                    PasswordHash = Utils.HashSecret(password),
                    Role = role,
                    CreatedBy = userId
                }
            );
            SaveAll(users);
            return users;
        }
        public static void SeedUsers()
        {
            var users = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

            if (users == null)
            {
                Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
            }
        }
    }
}