using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WorkFlowTaskManager.Application.Extensions
{
    public class Guard
    {
        public static class Against
        {
            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public static void Null(object input, string parameterName)
            {
                if (null == input)
                {
                    throw new ArgumentNullException(parameterName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty string.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void NullOrEmpty(string input, string parameterName)
            {
                Null(input, parameterName);
                if (input == String.Empty)
                {
                    throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty string.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void NullOrEmpty(Guid input, string parameterName)
            {
                Null(input, parameterName);
                if (input == Guid.Empty)
                {
                    throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty enumerable.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void NullOrEmpty<T>(IEnumerable<T> input, string parameterName)
            {
                Null(input, parameterName);
                if (!input.Any())
                {
                    throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="input" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="input" /> is an empty or white space string.
            /// </summary>
            /// <param name="input"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void NullOrWhiteSpace(string input, string parameterName)
            {
                NullOrEmpty(input, parameterName);
                if (String.IsNullOrWhiteSpace(input))
                {
                    throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="startDate" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="startDate" /> is not valid.
            /// </summary>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            ///   /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidDate(DateTime startDate)
            {
                Null(startDate, nameof(startDate));
                if (startDate == DateTime.MinValue)
                    throw new ArgumentException($"{startDate} is not a valid date.", $"{startDate.ToString()}");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="startDate" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="startDate" /> is not valid.
            /// </summary>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            ///   /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidDate(DateTime? startDate)
            {
                Null(startDate, nameof(startDate));
                if (startDate == DateTime.MinValue)
                    throw new ArgumentException($"{startDate} is not a valid date.", $"{startDate.ToString()}");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="startDate" /> is null.
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="endDate" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="endDate" /> is greater than start date.
            /// </summary>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidDate(DateTime startDate, DateTime endDate)
            {
                Null(startDate, nameof(startDate));
                Null(endDate, nameof(endDate));
                if (DateTime.Compare(startDate, endDate) > 0)
                    throw new ArgumentException($"End date {endDate} is less than start date {startDate} .", $"{endDate.ToString()}, {startDate.ToString()}");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentException" /> if <see cref="phone" /> is invalid.
            /// </summary>
            /// <param name="email"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidPhone(string phone)
            {
                if (!string.IsNullOrEmpty(phone) && !new PhoneAttribute().IsValid(phone))
                    throw new ArgumentException($"Provided number {phone} is invalid");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="email" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="email" /> is an empty string.
            /// Throws an <see cref="ArgumentException" /> if <see cref="email" /> is invalid.
            /// </summary>
            /// <param name="email"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidEmail(string email, string parameterName)
            {
                NullOrEmpty(email, parameterName);
                if (!new EmailAddressAttribute().IsValid(email))
                    throw new ArgumentException($"Provided email {email} is invalid");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="first" /> is null.
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="second" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="first" /> is invalid compare.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <param name="firstParameterName"></param>
            /// <param name="secondParameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidPasswordCompare(string first, string second, string firstParameterName, string secondParameterName)
            {
                NullOrEmpty(first, firstParameterName);
                NullOrEmpty(second, secondParameterName);
                if (first != second)
                    throw new ArgumentException($"{firstParameterName} and {secondParameterName} doesn't match");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="first" /> is null.
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="second" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="first" /> is invalid compare.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <param name="firstParameterName"></param>
            /// <param name="secondParameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidCompare(string first, string second, string firstParameterName, string secondParameterName)
            {
                NullOrEmpty(first, firstParameterName);
                NullOrEmpty(second, secondParameterName);
                if (first != second)
                    throw new ArgumentException($"{firstParameterName} {first} and {secondParameterName} {second} doesn't match");
            }

            /// <summary>
            /// Throws an <see cref="ArgumentNullException" /> if <see cref="amount" /> is null.
            /// Throws an <see cref="ArgumentException" /> if <see cref="amount" /> is 0.
            /// </summary>
            /// <param name="amount"></param>
            /// <param name="parameterName"></param>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public static void InvalidAmount(decimal amount, string parameterName)
            {
                Null(amount, parameterName);
                if (amount == 0)
                    throw new ArgumentException($"Required input {parameterName} was empty.");
            }
        }
    }
}