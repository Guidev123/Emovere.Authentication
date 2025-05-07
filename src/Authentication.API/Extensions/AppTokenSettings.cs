namespace Authentication.API.Extensions
{
    public class AppTokenSettings
    {
        public int TokenExpirationInHours { get; set; }
        public int RefreshTokenExpirationInHours { get; set; }
    }
}