﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CustomDataAnnotations.Attributes
{
    public sealed class StrongPassword : ValidationAttribute
    {
        private readonly bool _numeric;
        private readonly bool _upperCase;
        private readonly bool _specialChar;
        private readonly int _minLength;

        public StrongPassword(bool numeric, bool upperCase, bool specialChar, int minLength)
        {
            _numeric = numeric;
            _upperCase = upperCase;
            _specialChar = specialChar;
            _minLength = minLength;
        }

        public override bool IsValid(object value)
        {
            var password = (string)value;
            const string digits = @"\d+"; //match digits
            const string upperLetters = @"[A-Z]+"; //match upper cases
            const string symbols = @"[!@#$%^&+=]"; //match symbols

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var matchDigits = Regex.IsMatch(password, digits);
            var matchUpper = Regex.IsMatch(password, upperLetters);
            var matchSymbol = Regex.IsMatch(password, symbols);

            if (password.Length < _minLength)
            {
                return false;
            }

            if (_upperCase && !matchUpper)
            {
                return false;
            }

            if (_numeric && !matchDigits)
            {
                return false;
            }

            return !_specialChar || matchSymbol;
        }
    }
}
