using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluate.Repository
{
    public class Common
    {
        public enum EmailType
        {
            Signup = 1,
            ForgotPassword = 2
        }

        public enum Path
        {
            EMail = 1
        }

        public enum Url
        {
            Signin = 1
        }
    }
}