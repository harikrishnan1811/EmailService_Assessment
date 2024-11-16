using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Constants
{
    public static class StatusCode
    {
        public const string STATUS_EMAIL_OK = "STATUS_EMAIL_OK";
        public const string STATUS_EMAIL_FAIL = "STATUS_EMAIL_FAIL";
        public const string STATUS_EMAIL_INVALID = "STATUS_EMAIL_INVALID";

        public const string STATUS_OTP_OK = "STATUS_OTP_OK";
        public const string STATUS_OTP_FAIL = "STATUS_OTP_FAIL";
        public const string STATUS_OTP_TIMEOUT = "STATUS_OTP_TIMEOUT";
    }
}
