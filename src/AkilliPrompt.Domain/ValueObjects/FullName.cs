using System.Globalization;
using System.Text.RegularExpressions;

namespace AkilliPrompt.Domain.ValueObjects
{
    /// <summary>
    /// Represents a user's full name as a value object.
    /// </summary>
    public sealed record FullName
    {
        private const string NamePattern = @"^[\p{L}][\p{L}'\- ]+$"; // Supports Unicode letters, apostrophes, hyphens, and spaces
        private const int MinNameLength = 2;
        private const int MaxNameLength = 50; // Adjusted per name part

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullName"/> record.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
        public FullName(string firstName, string lastName)
        {
            FirstName = ValidateName(firstName, nameof(firstName));
            LastName = ValidateName(lastName, nameof(lastName));
        }

        /// <summary>
        /// Validates the name according to defined rules.
        /// </summary>
        /// <param name="name">The name to validate.</param>
        /// <param name="paramName">The parameter name for exception messages.</param>
        /// <returns>The validated name.</returns>
        /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
        private static string ValidateName(string name, string paramName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

            if (name.Length < MinNameLength || name.Length > MaxNameLength)
                throw new ArgumentException($"{paramName} must be between {MinNameLength} and {MaxNameLength} characters.", paramName);

            if (!Regex.IsMatch(name, NamePattern))
                throw new ArgumentException($"{paramName} contains invalid characters.", paramName);

            // Optional: Capitalize the first letter and lowercase the rest for consistency
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        }

        /// <summary>
        /// Creates a <see cref="FullName"/> from a single string containing the full name.
        /// Assumes the last word is the last name and the rest constitute the first name.
        /// </summary>
        /// <param name="fullName">The full name string.</param>
        /// <returns>A new instance of <see cref="FullName"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
        public static FullName Create(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be null or whitespace.", nameof(fullName));

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
                throw new ArgumentException("Full name must contain at least first name and last name.", nameof(fullName));

            string firstName = string.Join(' ', parts.Take(parts.Length - 1));
            string lastName = parts.Last();

            return new FullName(firstName, lastName);
        }

        /// <summary>
        /// Creates a <see cref="FullName"/> from separate first and last name strings.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>A new instance of <see cref="FullName"/>.</returns>
        public static FullName Create(string firstName, string lastName)
        {
            return new FullName(firstName, lastName);
        }

        /// <summary>
        /// Explicitly converts a <see cref="FullName"/> to a string.
        /// </summary>
        /// <param name="fullName">The full name to convert.</param>
        public static explicit operator string(FullName fullName) => fullName.ToString();

        /// <summary>
        /// Explicitly converts a string to a <see cref="FullName"/>.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        public static explicit operator FullName(string value) => Create(value);

        /// <summary>
        /// Returns the full name as a single string.
        /// </summary>
        /// <returns>The full name.</returns>
        public override string ToString() => $"{FirstName} {LastName}";

        /// <summary>
        /// Gets the initials of the full name.
        /// </summary>
        /// <returns>The initials in the format "F.L.".</returns>
        public string GetInitials()
        {
            char firstInitial = FirstName.FirstOrDefault(char.IsLetter);
            char lastInitial = LastName.FirstOrDefault(char.IsLetter);

            return $"{char.ToUpper(firstInitial)}.{char.ToUpper(lastInitial)}.";
        }
    }
}
