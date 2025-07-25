﻿using Common.Exceptions;
using Common.Utilities;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Data.Reprositories;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
        }

        public async Task<User?> GetByUserAsync(string username, CancellationToken cancellationToken) 
            => await Table.Where(p => p.UserName == username).SingleOrDefaultAsync(cancellationToken);

        public Task UpdateSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            return UpdateAsync(user, cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(u => u.UserName == user.UserName);
            if (exists)
                throw new BadRequestException("این نام کاربری تکراری است.");

            var passwordHash = SecurityHelpers.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }

        public async Task<bool> CheckUserName(string userName, CancellationToken cancellationToken, params int[] ignoreIds) 
            => await Table.Where(u => u.UserName == userName && ignoreIds.All(i => i != u.Id)).AnyAsync(cancellationToken);
        public async Task<bool> CheckNationalCode(string code, CancellationToken cancellationToken, params int[] ignoreIds) 
            => await Table.Where(u => u.NationalCode == code && ignoreIds.All(i => i != u.Id)).AnyAsync(cancellationToken);
    }
}
