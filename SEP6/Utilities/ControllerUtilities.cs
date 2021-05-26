using System;
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

            var id = Int32.Parse(splitToken[0]);
            user = dbContext.Users
                .First(a => a.Id == id);
            
            if (user == null)
                isVerified = false;

            if (splitToken[1] != user.Token)
                isVerified = false;
        }
        
        public static bool TokenVerification(string token,MoviesDbContext dbContext)
        {
            var splitToken = token.Split("=");
            if (splitToken.Length > 2)
                return false;
            
            var id = Int32.Parse(splitToken[0]);
            var user = dbContext.Users
                .First(a => a.Id == id);
            
            if (user == null)
                return false;

            if (splitToken[1] != user.Token)
                return false;

            return true;
        }
    }
}