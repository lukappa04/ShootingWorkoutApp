namespace SWBackend.ServiceLayer.Auth;

public static class CookiesMethod
{
    /// <summary>
    /// Questo metodo invia il refresh token al client tramite cooke HttpOnly, non leggibile da JavaScript.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="refreshToken"></param>
    /// <param name="expires"></param>
    public static void SetRefreshTokenCookie(HttpResponse response, string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // obbligatorio in produzione con HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = expires
        };
        response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}