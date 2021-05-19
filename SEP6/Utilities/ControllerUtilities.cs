using Microsoft.AspNetCore.Mvc;
using SEP6.Database;

namespace SEP6.Utilities
{
    public static class ControllerUtilities
    {
        public static bool TokenVerification(User user,int receivedId,string token)
        {
            if (user == null)
                return true;

            if (receivedId != user.Id)
                return true;
            
            return false; 
        }
    }
}