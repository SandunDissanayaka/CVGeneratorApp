using System;
using System.Text.RegularExpressions;

namespace CVGeneratorApp
{
    public static class ValidationHelper
    {
        // Check if the input field is empty
        public static bool IsEmpty(string input)
        {
            return string.IsNullOrWhiteSpace(input?.Trim());
        }

        // Check if the email format is strictly correct
        public static bool IsValidEmail(string email)
        {
            if (IsEmpty(email)) return false;

            // Strict regex for proper email format
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$";
            return Regex.IsMatch(email, pattern);
        }

        // Check if the password contains at least one letter
        public static bool HasLetter(string password)
        {
            return Regex.IsMatch(password, @"[a-zA-Z]");
        }

        // Check if the password contains at least one number
        public static bool HasNumber(string password)
        {
            return Regex.IsMatch(password, @"[0-9]");
        }

        // Check if the password contains at least one special symbol
        public static bool HasSymbol(string password)
        {
            // Checks for any character that is NOT a letter or a number
            return Regex.IsMatch(password, @"[^a-zA-Z0-9]");
        }

        // Basic length check (This is used inside RegisterWindow now)
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 6;
        }

        // Safely convert text to a number
        public static bool IsValidAge(string ageInput, out int age)
        {
            bool isNumber = int.TryParse(ageInput, out age);
            if (isNumber && age >= 18) return true;
            return false;
        }
    }
}