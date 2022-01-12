using AD_Entities.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AD_Infrastructure
{
    public interface IAccounts
    {
        System.Threading.Tasks.Task<DefaultResponse> GetAccounts(int pageno = 1,int pagesize = 10);
        public  Task<DefaultResponse> GetEmployeeDetails(long id);
        public Task<DefaultResponse> RegisterUser(RegisterAPIModel model);
        public Task<DefaultResponse> RegisterTravels(TravelsRegisterAPIModel model);

        public DefaultResponse LoginUser(LoginAPIModel model);

        public DefaultResponse QuickToken();

        public  Task<DefaultResponse> GetAccountsGroup( int empid,  string dateFrom,  string dateTo,  string tType);
    }
}
