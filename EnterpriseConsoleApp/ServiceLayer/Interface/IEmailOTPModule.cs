using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IEmailOTPModule
    {
        /// <summary>
        /// Starts the Email OTP module.
        /// </summary>
        /// <param name="otpDuration">Duration for which an OTP is valid, in milliseconds.</param>
        /// <param name="maxTries">Maximum number of attempts allowed for OTP verification.</param>
        void Start(int otpDuration = 60000, int maxTries = 10);

        /// <summary>
        /// Stops the Email OTP module.
        /// </summary>
        void Close();

        /// <summary>
        /// Generates and sends an OTP email to the specified user.
        /// </summary>
        /// <param name="userEmail">The email address of the user to receive the OTP.</param>
        /// <returns>Status code.</returns>
        string GenerateOTPEmail(string userEmail);

        /// <summary>
        /// Checks the user's OTP input against the generated OTP for their email.
        /// </summary>
        /// <param name="input">The IOStream to read the OTP input from the user.</param>
        /// <returns>Status code.</returns>
        Task<string> CheckOTP(IOStream input);
    }
}
