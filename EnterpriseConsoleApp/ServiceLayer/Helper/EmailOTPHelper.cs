using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Timer = System.Timers.Timer;

namespace ServiceLayer.Helper
{
    public static class EmailOTPHelper
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var emailChecker = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return emailChecker.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        public static string GenerateRandomOTP()
        {
            int otp = RandomNumberGenerator.GetInt32(0, 1000000);
            return otp.ToString("D6");
        }

        public static bool SendEmail(string emailAddress, string emailBody)
        {
            Console.WriteLine($"Sending email to {emailAddress}: {emailBody}");
            return true;
        }

        public static void DisposeTimer(Timer timer)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
