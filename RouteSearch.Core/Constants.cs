using System;

namespace RouteSearch.Core
{
    public static class Constants
    {
        public static class JwtConfig
        {
            public static string JwtIssuer = "Arch.Api";
            public static string JwtAudience = "Arch.Frontend";
            public static string JwtRefreshAudience = "Arch.Frontend.Refresh";
            public static TimeSpan DefaultAccessTokenLifeTime = TimeSpan.FromSeconds(300);
            public static TimeSpan DefaultRefreshTokenLifeTime = TimeSpan.FromDays(1);
        }

        public static int OtpLength = 20;
        public static TimeSpan OtpDefaultLifeTime = TimeSpan.FromDays(1);
        public const string RefreshTokenCookieName = "refreshToken";
        public const string RefreshAuthScheme = "Refresh";
    }
}