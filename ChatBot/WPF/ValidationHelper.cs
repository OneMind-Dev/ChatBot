using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPF
{
    public static class ValidationHelper
    {
        public static readonly Regex EmailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

        public static readonly Regex PhoneRegex = new Regex(
            @"^\d{10}$",
            RegexOptions.Compiled);

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
        }

        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && PhoneRegex.IsMatch(phone);
        }

        public static bool IsValidUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username);
        }

        public static bool IsValidAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address);
        }
    }
}
