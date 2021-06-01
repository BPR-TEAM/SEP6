using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SEP6.DB;

namespace SEP6.Utilities
{
    public static class ControllerUtilities
    {
        public static void TokenVerification(string token,MoviesDbContext dbContext, out User user, out bool isVerified)
        {
            isVerified = true;
            
            var splitToken = token.Split("=");
            if (splitToken.Length > 2)
               isVerified = false;

            user = null;
            User tempUser = null;
            try
            {
                var id = Int32.Parse(splitToken[0]);
                tempUser = dbContext.Users
                    .First(a => a.Id == id);
            }
            catch (InvalidOperationException e)
            {
                isVerified = false;
            }

            if (isVerified)
            {
                user = tempUser;
                if (splitToken[1] != user.Token)
                {
                    isVerified = false;
                    user = null;
                }
            }
        }
        
        public static bool TokenVerification(string token,MoviesDbContext dbContext)
        {
            var splitToken = token.Split("=");
            if (splitToken.Length > 2)
                return false;

            User user;
            try
            {
                var id = Int32.Parse(splitToken[0]);
                user = dbContext.Users
                .First(a => a.Id == id);
            }
            catch (InvalidOperationException e)
            {
                return false;
            }

            if (splitToken[1] != user.Token)
                return false;

            return true;
        }
    }
}