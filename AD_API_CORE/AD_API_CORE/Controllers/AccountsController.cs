using AD_API_CORE.Authorizations;
using AD_Entities.HelperClasses;
using AD_Entities.Models;
using AD_Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AD_API_CORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly IAccounts _iAccounts;


        public AccountsController(IConfiguration Configuration, IAccounts Acc)
        {
            _iAccounts = Acc;
            _configuration = Configuration;
        }

        // GET api/<VideoController>/5
        [HttpGet("{id}")]
        public async Task<DefaultResponse> Get(long id)
        {
            try
            {
                return await _iAccounts.GetEmployeeDetails(id);
            }
            catch (Exception _e)
            {
                return new DefaultResponse
                {
                    Status = false,
                    Message = _e.Message
                };
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetGroup")]
        public async Task<DefaultResponse> GetGroup([FromQuery] int empid, [FromQuery] string dateFrom,  [FromQuery] string dateTo, [FromQuery] string tType)
        {
            try
            {
                return await _iAccounts.GetAccountsGroup( empid,  dateFrom,   dateTo,  tType);
            }
            catch (Exception _e)
            {
                return new DefaultResponse
                {
                    Status = false,
                    Message = _e.Message
                };
            }
        }

        // GET api/<AccountsController>/5
        [HttpGet]
        [Authorize]
        [Route("GetList")]//[Route("GetList/{pageno?}/{pagesize?}")]
        public async Task<DefaultResponse> GetList([FromQuery] int? pageno = 1, [FromQuery] int? pagesize = 10)
        {
            try
            {
                DefaultResponse _defaultResponse = new DefaultResponse();
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string _usertype = claims.Where(p => p.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
                    string _email = claims.Where(p => p.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
                    string _userid = claims.Where(p => p.Type == ClaimTypes.MobilePhone).FirstOrDefault()?.Value;

                    if (!string.IsNullOrEmpty(_usertype) && _usertype.ToLower().Trim() == "admin")
                    {
                        return await _iAccounts.GetAccounts((int)pageno, (int)pagesize);
                    }
                    else
                    {
                        _defaultResponse.Message = "You must be an admin to see all accounts!";
                        _defaultResponse.Status = false;
                        return _defaultResponse;
                    }
                }
                else
                {
                    _defaultResponse.Message = "User Identity not Found!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

            }
            catch (Exception _e)
            {
                return new DefaultResponse
                {
                    Status = false,
                    Message = _e.Message
                };
            }
        }

        [HttpPost]
        [Authorize]
        public DefaultResponse Post([FromBody] LoginAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                _defaultResponse = _iAccounts.LoginUser(model);
                if (_defaultResponse.Status == true)
                {
                    JwtMiddleware _middleware = new JwtMiddleware(_configuration.GetValue<string>("Config:JwtKey"));
                    _defaultResponse.Data.Token = _middleware.GenerateJSONWebToken(_defaultResponse.Data.UserEMAIL, _defaultResponse.Data.IsAdmin, _defaultResponse.Data.id_afterlogin);

                }

                return _defaultResponse;

            }
            catch (Exception _e)
            {
                _defaultResponse.Status = false;
                _defaultResponse.Message = _e.Message;
                return _defaultResponse;
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("RegisterUser")]
        public async Task<DefaultResponse> RegisterUser([FromBody] RegisterAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                _defaultResponse = await _iAccounts.RegisterUser(model);
               

                return _defaultResponse;

            }
            catch (Exception _e)
            {
                _defaultResponse.Status = false;
                _defaultResponse.Message = _e.Message;
                return _defaultResponse;
            }

        }

        [HttpPost]
        [Authorize]
        [Route("RegisterTravels")]
        public async Task<DefaultResponse> RegisterTravels([FromBody] TravelsRegisterAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                _defaultResponse = await _iAccounts.RegisterTravels(model);
               
                return _defaultResponse;

            }
            catch (Exception _e)
            {
                _defaultResponse.Status = false;
                _defaultResponse.Message = _e.Message;
                return _defaultResponse;
            }

        }

        [HttpGet]
        [Route("Token")]
        [AllowAnonymous]
        public DefaultResponse Token()
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                _defaultResponse = _iAccounts.QuickToken();
                if (_defaultResponse.Status == true)
                {
                    JwtMiddleware _middleware = new JwtMiddleware(_configuration.GetValue<string>("Config:JwtKey"));
                    string _email = _defaultResponse.Data.EmployeeEmail;
                    bool _isadmin = _defaultResponse.Data.IsAdmin;
                    string _userid = _defaultResponse.Data.Id.ToString();
                    string _token = _middleware.GenerateJSONWebToken(_email, _isadmin, _userid);
                    _defaultResponse.Data = _token;
                    _defaultResponse.Message = _email;

                }

                return _defaultResponse;

            }
            catch (Exception _e)
            {
                _defaultResponse.Status = false;
                _defaultResponse.Message = _e.Message;
                return _defaultResponse;
            }

        }


        [HttpPost]
        [Route("LoginUser")]
        [AllowAnonymous]
        public DefaultResponse LoginUser([FromBody] LoginAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                _defaultResponse = _iAccounts.LoginUser(model);
                if (_defaultResponse.Status == true)
                {
                    JwtMiddleware _middleware = new JwtMiddleware(_configuration.GetValue<string>("Config:JwtKey"));
                    _defaultResponse.Data.Token = _middleware.GenerateJSONWebToken(_defaultResponse.Data.UserEMAIL, _defaultResponse.Data.IsAdmin, _defaultResponse.Data.id_afterlogin);

                }

                return _defaultResponse;

            }
            catch (Exception _e)
            {
                _defaultResponse.Status = false;
                _defaultResponse.Message = _e.Message;
                return _defaultResponse;
            }


        }


    }
}
