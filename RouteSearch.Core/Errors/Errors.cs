using System;
using System.Diagnostics.CodeAnalysis;

namespace RouteSearch.Core.Errors
{
    public static class Errors
    {
        [DoesNotReturn]
        public static void ThrowError(Error error)
        {
            throw new LogicException(error);
        }

        [DoesNotReturn]
        public static void ThrowError(Error error, string message)
        {
            throw new LogicException(error, message);
        }

        [DoesNotReturn]
        public static void ThrowError(Error error, string message, Exception innerException)
        {
            throw new LogicException(error, message, innerException);
        }
        
        public static Error ValidationError = new() {Code = 1, Description = "Validation error"};
        public static Error InvalidCaptcha = new() {Code = 2, Description = "Captcha is invalid"};
        public static Error ProvidersNotConfigured = new() {Code = 3, Description = "Providers not  configured" };
        public static Error NoResults = new() {Code = 4, Description = "No results" };
        
    }
}