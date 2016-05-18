using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluate.ViewModels
{
    public class _Roles
    {
        private static readonly string Admin = "Admin";
        private static readonly string Studnet = "Student";

        public enum Roles
        {
            Admin = 1,
            Student = 2
        }
    }
}