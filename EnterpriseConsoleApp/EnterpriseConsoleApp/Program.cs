using ServiceLayer;
using ServiceLayer.Interface;
using ServiceLayer.Constants;

namespace EnterpriseConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEmailOTPModule emailOTPModule = new EmailOTPModule();
            emailOTPModule.Start();
            
            while (true)
            {
                Console.WriteLine("Enter your email: ");
                string email = Console.ReadLine();

                if (email == "exit")
                    break;

                if (email == null || email.Length == 0)
                    continue;

                string status = emailOTPModule.GenerateOTPEmail(email);
                if (status == StatusCode.STATUS_EMAIL_OK)
                {
                    Console.WriteLine(emailOTPModule.CheckOTP(new IOStream()
                    {
                        UserEmail = email,
                    }).Result);
                }
                else if (status == StatusCode.STATUS_EMAIL_INVALID)
                {
                    Console.WriteLine("Invalid email.");
                }
                else if (status == StatusCode.STATUS_BLOCKED_EMAIL)
                {
                    Console.WriteLine("Domain not whitelisted.");
                }
                else
                {
                    Console.WriteLine("Failed to send OTP.");
                }
            }

            emailOTPModule.Close();
            Console.ReadLine();
        }
    }
}
