using System;
using System.Collections.Generic;
using Recipes.Identity.Domain.Generic;

namespace Recipes.Identity.Domain
{
    public class ApplicationUser : EntityWithStringId
    {
        public ApplicationUser() 
        {
            Roles = new List<string>();
        }

        public ApplicationUser(
            string id,
            string username,
            string email,
            List<string> roles,
            bool isActive,
            string lastmodifiedByUserId,
            DateTime createdDate,
            DateTime? lastModifiedDate,
            string refreshToken,
            byte[] passwordHash,
            byte[] passwordSalt,
            bool passwordResetRequested)
        {
            Id = id;
            Username = username;
            Email = email;
            Roles = roles;
            IsActive = isActive;
            LastModifiedByUserId = lastmodifiedByUserId;
            CreatedDate = createdDate;
            LastModifiedDate = lastModifiedDate;
            RefreshToken = refreshToken;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            PasswordResetRequested = passwordResetRequested;
        }

        public string Username { get; private set; }
        public string Email { get; private set; }
        public List<string> Roles { get; private set; }
        public bool IsActive { get; private set; }
        public string LastModifiedByUserId { get; private set; }
        public DateTime? LastModifiedDate { get; private set; }
        public string RefreshToken { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public bool PasswordResetRequested { get; private set; }
        public string CreatedByUserId { get => Id; }
        public virtual DateTime CreatedDate { get; protected set; }

        public void CreateUser(string username, string email, string token, byte[] hash, byte[] salt, DateTime createdDate)
        {
            Username = username;
            Email = email;
            Roles = new List<string> { "Member" };
            IsActive = true;
            PasswordResetRequested = false;
            CreatedDate = createdDate;
            UpdateRefreshToken(token);
            AddPassword(hash, salt);
        }

        public void UpdateEmailAndUsername(string username, string email)
        {
            Email = email;
            Username = username;
        }

        public void UpdateRoles(List<string> roles)
        {
            Roles = roles;
        }

        public void RequestPasswordReset(string token)
        {
            PasswordResetRequested = true;
            UpdateRefreshToken(token);
        }

        public void UpdateRefreshToken(string token)
        {
            RefreshToken = token;
        }

        public void DeleteUser(string modifiedByUserId, DateTime dateTime)
        {
            IsActive = false;
            UpdateAuditData(modifiedByUserId, dateTime);
        }

        public void UpdatePassword(byte[] hash, byte[] salt)
        {
            PasswordResetRequested = false;
            AddPassword(hash, salt);
        }

        public void AddPassword(byte[] hash, byte[] salt)
        {
            PasswordHash = hash;
            PasswordSalt = salt;
        }

        public void UpdateUser(ApplicationUser user, string modifiedByUserId, DateTime dateTime)
        {
            Username = user.Username;
            Email = user.Email;
            Roles = user.Roles;
            IsActive = user.IsActive;
            CreatedDate = user.CreatedDate;
            RefreshToken = user.RefreshToken;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;
            UpdateAuditData(modifiedByUserId, dateTime);
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            return string.IsNullOrWhiteSpace(refreshToken) || RefreshToken == refreshToken;
        }

        public bool IsResetPasswordTokenValid(string refreshToken)
        {
            return PasswordResetRequested && IsRefreshTokenValid(refreshToken);
        }

        public bool EmailMatches(string email)
        {
            return Email == email;
        }

        public bool UsernameMatches(string username)
        {
            return Username == username;
        }

        private void UpdateAuditData(string modifiedByUserId, DateTime dateTime)
        {
            LastModifiedByUserId = modifiedByUserId;
            LastModifiedDate = dateTime;
        }
    }
}
