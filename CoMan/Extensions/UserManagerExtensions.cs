﻿using CoMan.Models;
using Microsoft.AspNetCore.Identity;

namespace CoMan.Extensions
{
    public static class UserManagerExtensions
    {
        private static readonly HttpContextAccessor _contextAccessor = new();

        public static async Task<string> GetCurrentUserId(this UserManager<ApplicationUser> userManager)
        {
            var currentUser = await getCurrentUser(userManager);
            return currentUser == null ? String.Empty : currentUser.Id;
        }

        public static async Task<Boolean> IsCurrentUserInRole(this UserManager<ApplicationUser> userManager, string role)
        {
            var currentUser = await getCurrentUser(userManager);
            var currentUserRoles = await userManager.GetRolesAsync(currentUser);
            return currentUserRoles.Contains(role);
        }

        public static async Task<ApplicationUser> GetCurrentApplicationUser(this UserManager<ApplicationUser> userManager)
        {
            return await getCurrentUser(userManager);
        }

        private static async Task<ApplicationUser> getCurrentUser(UserManager<ApplicationUser> userManager)
        {
            if (_contextAccessor == null)
                throw new NullReferenceException(nameof(_contextAccessor));

            return await userManager.GetUserAsync(_contextAccessor.HttpContext!.User);
        }
    }
}
