using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class IOStream
    {
        public string UserEmail { get; set; }
        public string ReadOTP()
        {
            Console.WriteLine("Enter OTP: ");
            var otp = Console.ReadLine();
            return otp;
        }
    }
}
