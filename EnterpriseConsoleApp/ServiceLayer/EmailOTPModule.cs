using ServiceLayer.Constants;
using ServiceLayer.Helper;
using ServiceLayer.Interface;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using Timer = System.Timers.Timer;

namespace ServiceLayer
{
    public class EmailOTPModule : IEmailOTPModule
    {
        public int OtpDuration { get; set; }
        public int MaxTries { get; set; }

        private readonly ConcurrentDictionary<string, (string OTP, Timer Timer, DateTime ExpirationTime)> userOTPs = new();

        public void Start(int otpDuration = 60000, int maxTries = 10)
        {
            Console.WriteLine("Email OTP Module started.");
            OtpDuration = otpDuration;
            MaxTries = maxTries;
        }

        public void Close()
        {
            lock (userOTPs)
            {
                Console.WriteLine("Email OTP Module closed.");
                foreach (var timer in userOTPs.Values)
                {
                    EmailOTPHelper.DisposeTimer(timer.Timer);
                }
                userOTPs.Clear();
            }
        }

        public string GenerateOTPEmail(string userEmail)
        {
            if (!EmailOTPHelper.IsValidEmail(userEmail) || !userEmail.EndsWith("@dso.org.sg"))
                return StatusCode.STATUS_EMAIL_INVALID;

            try
            {
                string otp = EmailOTPHelper.GenerateRandomOTP();

                if (userOTPs.TryRemove(userEmail, out var existing))
                    EmailOTPHelper.DisposeTimer(existing.Timer);

                var timer = new Timer(OtpDuration);
                timer.Elapsed += (sender, e) => RemoveOTP(userEmail);
                timer.Start();

                userOTPs[userEmail] = (otp, timer, DateTime.UtcNow.AddMilliseconds(OtpDuration));

                string emailBody = $"Your OTP Code is {otp}. The code is valid for 1 minute";

                bool success = EmailOTPHelper.SendEmail(userEmail, emailBody);

                return success ? StatusCode.STATUS_EMAIL_OK : StatusCode.STATUS_EMAIL_FAIL;
            }
            catch (Exception)
            {
                return StatusCode.STATUS_EMAIL_FAIL;
            }
        }

        public async Task<string> CheckOTP(IOStream input)
        {
            int tries = 0;

            /*
             * Check if the OTP has already expired.
             */
            userOTPs.TryGetValue(input.UserEmail, out var otpData);
            if (otpData.ExpirationTime < DateTime.UtcNow)
                return StatusCode.STATUS_OTP_TIMEOUT;

            TimeSpan timeLeft = otpData.ExpirationTime - DateTime.UtcNow;
            int milliSecondsLeft = (int)timeLeft.TotalMilliseconds;

            /*
             * Create a cancellation token source with the remaining time left for the OTP to expire.
             */
            using (CancellationTokenSource cts = new CancellationTokenSource(milliSecondsLeft))
            {
                CancellationToken token = cts.Token;
                while (tries < MaxTries)
                {
                    try
                    {
                        string userInput = await ReadOTPWithTimeout(input, token);

                        /*
                         * Get the OTP data again in case it has been expired between tries.
                         */
                        userOTPs.TryGetValue(input.UserEmail, out otpData);
                        if (otpData.ExpirationTime < DateTime.UtcNow)
                        {
                            return StatusCode.STATUS_OTP_TIMEOUT;
                        }
                        else if (otpData.OTP == userInput)
                        {
                            RemoveOTP(input.UserEmail);
                            cts.Cancel();
                            return StatusCode.STATUS_OTP_OK;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return StatusCode.STATUS_OTP_TIMEOUT;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception occurred: {ex.Message}");
                    }
                    finally
                    {
                        Console.WriteLine($"Try again.");
                        tries++;
                    }
                }
            }
            return StatusCode.STATUS_OTP_FAIL;
        }

        private void RemoveOTP(string userEmail)
        {
            if (userOTPs.TryRemove(userEmail, out var otpData))
            {
                EmailOTPHelper.DisposeTimer(otpData.Timer);
            }
        }

        private async Task<string> ReadOTPWithTimeout(IOStream input, CancellationToken token)
        {
            var readTask = Task.Run(input.ReadOTP);
            var completedTask = await Task.WhenAny(readTask, Task.Delay(Timeout.Infinite, token));
            if (completedTask == readTask)
            {
                return await readTask;
            }
            else
            {
                throw new OperationCanceledException(token);
            }
        }
    }
}
