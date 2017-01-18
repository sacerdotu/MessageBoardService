using MessageBoardDAL;
using MessageBoardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MessageBoardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public void InsertNewUser(UserDTO user)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    tblUser addUser = context.tblUsers.Create();

                    addUser.FirstName = user.FirstName;
                    addUser.LastName = user.LastName;
                    addUser.Username = user.Username;
                    addUser.PasswordHash = user.PasswordHash;
                    addUser.City = user.City;
                    addUser.Country = user.Country;
                    addUser.Function = user.Function;

                    addUser.AccountCreationDate = DateTime.Now;
                    addUser.IsActive = true;
                    addUser.IsAdministrator = true;

                    context.tblUsers.Add(addUser);
                    context.SaveChanges();
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
