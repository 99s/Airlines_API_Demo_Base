using AD_Entities.HelperClasses;
using AD_Entities.Models;
using AD_Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;

namespace AD_Repository_XUnit
{
    public class RAccountsTest
    {

        //Alaska_DemoContext _dbcontext;
        //IConfiguration _configuration;
        //RAccounts _rAccounts;


        //public RAccountsTest(Alaska_DemoContext ProjectContext, IConfiguration Configuration)
        //{
        //    _dbcontext = ProjectContext;
        //    _configuration = Configuration;
        //    //_rAccounts = new RAccounts(ProjectContext, Configuration);
        //}



        //[Fact]
        //public void GetEmployeeDetailsTest()
        //{
        //    DefaultResponse _result = _rAccounts.GetEmployeeDetails(8).Result;
        //    Assert.True(_result.Status);
        //}


        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(10, 2, 5)]
        [InlineData(6, 2, 3)]
        [InlineData(4, 2, 1)]
        [InlineData(10, 3, 3.33)]
        public void TestMethodTest(int input1, int input2, decimal expectedOutput)
        {
            decimal _d = RAccounts.TestMethod(input1, input2);
            Assert.Equal(expectedOutput, _d);
        }
    }
}
