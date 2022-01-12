using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AD_Entities.HelperClasses;
using AD_Entities.Models;
using System.Security.Claims;

namespace AD_API_CORE.Authorizations
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtKey;

        public JwtMiddleware(string JwtKey)
        {
            _jwtKey = JwtKey;
        }


        public string GenerateJSONWebToken(string _userEmail, bool _isAdmin, string _userid)
        {
            var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            string _userType = _isAdmin ? "admin" : "user";
            var _claims = new[] {
                    new Claim(ClaimTypes.Email, _userEmail),
                    new Claim(ClaimTypes.Role, _userType),
                    new Claim(ClaimTypes.MobilePhone, _userid),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(null,
              null,
              _claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: _credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public string GenerateJSONWebToken_old(string _userEmail, bool _isAdmin, string _userid)
        //{
        //    var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        //    var _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        //    string _userType = _isAdmin ? "admin" : "user";
        //    var _claims = new[] {
        //            new Claim(JwtRegisteredClaimNames.GivenName, _userType),
        //            new Claim(JwtRegisteredClaimNames.Email, _userEmail),
        //            new Claim(JwtRegisteredClaimNames.Website, _userid),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };
        //    var token = new JwtSecurityToken(null,
        //      null,
        //      _claims,
        //      expires: DateTime.Now.AddMinutes(120),
        //      signingCredentials: _credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        //public JwtMiddleware(RequestDelegate next, IOptions<AppSettingsModel> appSettings)
        //{
        //    _next = next;
        //    _appSettings = appSettings.Value;
        //}

        //public async Task Invoke(HttpContext context, IUserService userService)
        //{
        //    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        //    if (token != null)
        //        attachUserToContext(context, userService, token);

        //    await _next(context);
        //}

        //private void attachUserToContext(HttpContext context, IUserService userService, string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        //        // attach user to context on successful jwt validation
        //        context.Items["User"] = userService.GetById(userId);
        //    }
        //    catch
        //    {
        //        // do nothing if jwt validation fails
        //        // user is not attached to context so request won't have access to secure routes
        //    }
        //}
    }
}
