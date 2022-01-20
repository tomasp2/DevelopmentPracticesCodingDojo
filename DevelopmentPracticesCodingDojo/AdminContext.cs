using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopmentPracticesCodingDojo
{
    public static class AdminContext
    {
        private static bool isInAdminMode;

        private const string adminPassword = "password";

        public static void SetAdminMode(bool value)
        {
            isInAdminMode = value;
        }

        public static bool Authenticate(string? password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return password == adminPassword;
        }

        public static bool IsInAdminMode()
        {
            return isInAdminMode;
        }
    }
}
