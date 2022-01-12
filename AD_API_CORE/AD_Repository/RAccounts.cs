using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using AD_Entities.HelperClasses;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AD_Services;
using AD_Entities.Models;
using AD_Infrastructure;

namespace AD_Repository
{
    public class RAccounts: IAccounts
    {
        Alaska_DemoContext _dbcontext;


        IConfiguration _configuration;


        public RAccounts(Alaska_DemoContext ProjectContext, IConfiguration Configuration)
        {
            _dbcontext = ProjectContext;
            _configuration = Configuration;

        }

        public async Task<DefaultResponse> GetEmployeeDetails(long id)
        {
            DefaultResponse _r = new DefaultResponse();
            try
            {
                var _v = await (from _emp in _dbcontext.Employee.AsNoTracking()
                                  join _travel in _dbcontext.Travel.AsNoTracking() on _emp.Id equals _travel.EmployeeCodeFk into _tLeft
                                  from _travel in _tLeft.DefaultIfEmpty()
                                  join _ttype in _dbcontext.TravelType.AsNoTracking() on _travel.TravelTypeFk equals _ttype.Id into _ttLeft
                                  from _ttype in _ttLeft.DefaultIfEmpty()
                                  where _emp.Id == id
                                  select new
                                  {
                                      Name = _emp.EmployeeFirstName + " " + _emp.EmployeeLastName,
                                      TravelOrigin = _travel.TravelOrigin,
                                      TravelDestination = _travel.TravelDestination,
                                      StartingJourney = _travel.TravelInititionDate,
                                      FinishingJourney = _travel.TravelCompletetionDate,
                                      TravelTypeCode = _ttype.TypeCode,
                                      TravelTypeName = _ttype.Name,
                                      TravellerName = _travel.TravelerName
                                  }).OrderByDescending(o=>o.StartingJourney).ToListAsync();

                _r.Data = _v;
                _r.Status = true;
            }
            catch (Exception _e)
            {
                _r.Status = false;
                _r.Message = "err : " + _e.Message;
            }

            return _r;
        }
        public async Task<DefaultResponse> GetAccounts(int pagenumber = 1, int pagesize = 10)
        {
            DefaultResponse _r = new DefaultResponse();
            try
            {
                var _v = await (from _travel in _dbcontext.Travel.AsNoTracking()
                            join _emp in _dbcontext.Employee.AsNoTracking() on _travel.EmployeeCodeFk equals _emp.Id into _eLeft
                            from _emp in _eLeft.DefaultIfEmpty()
                            join _ttype in _dbcontext.TravelType.AsNoTracking() on _travel.TravelTypeFk equals _ttype.Id into _ttLeft
                            from _ttype in _ttLeft.DefaultIfEmpty()
                            
                            select new 
                            {
                                Name = _emp.EmployeeFirstName + " "+ _emp.EmployeeLastName,
                                TravelOrigin = _travel.TravelOrigin,
                                TravelDestination = _travel.TravelDestination,
                                StartingJourney = _travel.TravelInititionDate,
                                FinishingJourney = _travel.TravelCompletetionDate,
                                TravelTypeCode = _ttype.TypeCode,
                                TravelTypeName = _ttype.Name,
                                TravellerName = _travel.TravelerName
                            }).Page((int)pagenumber, (int)pagesize).ToListAsync();

                _r.Data = _v;
                _r.Status = true;
            }
            catch(Exception _e)
            {
                _r.Status = false;
                _r.Message = "err : " + _e.Message;
            }

            return _r;
        }

        public async Task<DefaultResponse> GetAccountsGroup(int empid, string dateFrom, string dateTo, string tType)
        {
            DefaultResponse _r = new DefaultResponse();
            try
            {

                DateTime _dF = Convert.ToDateTime(dateFrom);
                DateTime _dT = Convert.ToDateTime(dateTo);

                var _v = await (from _travel in _dbcontext.Travel.AsNoTracking()
                                  join _emp in _dbcontext.Employee.AsNoTracking() on _travel.EmployeeCodeFk equals _emp.Id into _eLeft
                                  from _emp in _eLeft.DefaultIfEmpty()
                                  join _ttype in _dbcontext.TravelType.AsNoTracking() on _travel.TravelTypeFk equals _ttype.Id into _ttLeft
                                  from _ttype in _ttLeft.DefaultIfEmpty()
                                  where _emp.Id == empid 
                                  &&
                                  (_dF <= _travel.TravelInititionDate && _dT >= _travel.TravelInititionDate)
                                  &&
                                  _ttype.TypeCode == tType
                                  

                                  select new
                                  {
                                      //Id = _emp.Id,
                                      Name = _emp.EmployeeFirstName + " " + _emp.EmployeeLastName,
                                      TravelOrigin = _travel.TravelOrigin,
                                      TravelDestination = _travel.TravelDestination,
                                      StartingJourney = _travel.TravelInititionDate,
                                      FinishingJourney = _travel.TravelCompletetionDate,
                                      TravelTypeCode = _ttype.TypeCode,
                                      TravelTypeName = _ttype.Name,
                                      TravellerName = _travel.TravelerName
                                  }).OrderBy(d=>d.StartingJourney).ToListAsync();

                //var _vg = _v.GroupBy(c => c.Id)
                //    .Select(grp => grp.ToList())
                //    .ToList();

                 //IOrderedEnumerable<IGrouping<string, dynamic>> groupedCustomerList;

        //var groupedList =
        //from v in _v
        //group v by v.Id into newGroup
        //orderby newGroup.Key
        //select newGroup;

                _r.Data = _v;
                _r.Status = true;
            }
            catch (Exception _e)
            {
                _r.Status = false;
                _r.Message = "err : " + _e.Message;
            }

            return _r;
        }



        public async Task<DefaultResponse> RegisterUser(RegisterAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {


                if (!AppServices.IsEmail(model.UserEMAIL.Trim()))
                {
                    _defaultResponse.Message = "invalid Email Format!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }
                if (string.IsNullOrEmpty(model.PassWord))
                {
                    _defaultResponse.Message = "Password Not Provided!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

                if (!AppServices.IsMin8Char(model.PassWord))
                {
                    _defaultResponse.Message = "Password Length Should Be 8 Char minimum!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

                if (!AppServices.IsAlphabets(model.UserFirstName))
                {
                    _defaultResponse.Message = "Enter Proper First Name!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

                if (!AppServices.IsAlphabets(model.UserLastName))
                {
                    _defaultResponse.Message = "Enter Proper Last Name!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

                var _u = (from _c in _dbcontext.Employee
                          where _c.EmployeeEmail.Equals(model.UserEMAIL.Trim())
                          select _c).FirstOrDefault();



                if (_u == null)
                {

                    EncryptionDecryption _ed = new EncryptionDecryption();

                    var hashPassword = _ed.HashPassword(model.PassWord.Trim());

                    Employee tblUser = new Employee();
                    tblUser.EmployeeEmail = model.UserEMAIL.Trim();
                    tblUser.EmployeePasswordHash = hashPassword;
                    tblUser.IsAdmin = model.IsAdmin;
                    tblUser.EmployeeFirstName = model.UserFirstName;
                    tblUser.EmployeeLastName = model.UserLastName;

                    await _dbcontext.Employee.AddAsync(tblUser);

                    await _dbcontext.SaveChangesAsync();


                    _defaultResponse.Status = true;
                    return _defaultResponse;
                }

                else
                {
                    _defaultResponse.Message = "User Already Exists";
                    _defaultResponse.Status = false;
                    return _defaultResponse;

                }

            }
            catch (Exception _e)
            {
                _defaultResponse.Message = "failed (e) : " + _e.Message;
                _defaultResponse.Status = false;
                return _defaultResponse;


            }

        }

        public async Task<DefaultResponse> RegisterTravels(TravelsRegisterAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {

                Travel _travel = new Travel();
                _travel.TravelDestination = model.TravelDestination;
                _travel.TravelerName = model.TravelerName;
                _travel.TravelInititionDate = model.TravelInititionDate;
                _travel.TravelCompletetionDate = model.TravelCompletetionDate;
                _travel.TravelStatusCode = model.TravelStatusCode;
                _travel.TravelTypeFk = model.TravelTypeFk;
                _travel.EmployeeCodeFk = model.EmployeeCodeFk;



                await _dbcontext.Travel.AddAsync(_travel);
                await _dbcontext.SaveChangesAsync();

                _defaultResponse.Status = true;
                return _defaultResponse;


            }
            catch (Exception _e)
            {
                _defaultResponse.Message = "failed (e) : " + _e.Message;
                _defaultResponse.Status = false;
                return _defaultResponse;


            }

        }


        public DefaultResponse LoginUser(LoginAPIModel model)
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {


                if (!AppServices.IsEmail(model.UserEMAIL.Trim()))
                {
                    _defaultResponse.Message = "invalid Email Format!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;
                }

                var _u = (from _c in _dbcontext.Employee.AsNoTracking()
                          where _c.EmployeeEmail.Equals(model.UserEMAIL)
                          select _c).FirstOrDefault();



                if (_u != null)
                {
                    if (string.IsNullOrEmpty(model.PassWord))
                    {
                        _defaultResponse.Message = "Password Not Provided!";
                        _defaultResponse.Status = false;
                        return _defaultResponse;
                    }
                    EncryptionDecryption ed = new EncryptionDecryption();

                    bool isPassmatched = ed.VerifyHashedPassword(_u.EmployeePasswordHash, model.PassWord);

                    if (!isPassmatched)
                    {
                        _defaultResponse.Message = "Password Missmatch!";
                        _defaultResponse.Status = false;
                        return _defaultResponse;


                    }


                    SessionManager.LoggedInUserEmail = _u.EmployeeEmail;
                    SessionManager.LoggedInUser_IsAdmin = (bool)_u.IsAdmin;



                    //HttpContext.Session.SetInt32("", 773);

                    _defaultResponse.Status = true;
                    model.IsAdmin = (bool)_u.IsAdmin;
                    model.id_afterlogin = _u.Id.ToString();
                    _defaultResponse.Message = _u.EmployeeFirstName + " " + _u.EmployeeLastName;
                    _defaultResponse.Data = model;
                    return _defaultResponse;
                }

                else
                {
                    _defaultResponse.Message = "No User Found!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;

                }

            }
            catch (Exception _e)
            {
                _defaultResponse.Message = "failed (e) : " + _e.Message;
                _defaultResponse.Status = false;
                return _defaultResponse;


            }

        }



        public DefaultResponse QuickToken()
        {
            DefaultResponse _defaultResponse = new DefaultResponse();
            try
            {




                var _u = (from _c in _dbcontext.Employee.AsNoTracking()
                          where _c.IsAdmin == true
                          select _c).FirstOrDefault();



                if (_u != null)
                {
                    _defaultResponse.Message = "User Found!";
                    _defaultResponse.Status = true;
                    _defaultResponse.Data = _u;
                    return _defaultResponse;
                }

                else
                {
                    _defaultResponse.Message = "No User Found!";
                    _defaultResponse.Status = false;
                    return _defaultResponse;

                }

            }
            catch (Exception _e)
            {
                _defaultResponse.Message = "failed (e) : " + _e.Message;
                _defaultResponse.Status = false;
                return _defaultResponse;


            }

        }


        public static decimal TestMethod(int a, int b)
        {
            try
            {
                if(b == 0)
                {
                    throw new DivideByZeroException();
                }
                return a / b;
            }
            catch(Exception _e)
            {
                throw new Exception(_e.Message);
            }
        }

    }
}
