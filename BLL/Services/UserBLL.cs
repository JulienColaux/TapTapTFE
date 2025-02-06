using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;

namespace BLL.Services
{
    public class UserBLL
    {

        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------

        private readonly UserDAL _userDAL;

        public UserBLL(UserDAL userDal )
        {
            _userDAL = userDal;
        }

        //-------------------------------ADD USER--------------------------------------------------------------------------------------------



        public bool AddUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            return _userDAL.AddUser(email, password);
        }


        //-------------------------------CHECK USER INFO--------------------------------------------------------------------------------


        public bool ValidateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            return _userDAL.ValidateUser(email, password);
        }
    }
}
