using AutoMapper;
using MessageBoardDAL;
using MessageBoardDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
        #region InsertNewUser
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
                    addUser.PasswordSalt = user.PasswordSalt;
                    addUser.AccountCreationDate = DateTime.Now;
                    addUser.IsActive = true;
                    addUser.IsAdministrator = false;

                    context.tblUsers.Add(addUser);
                    context.SaveChanges();
                }
                
            }
            catch (EntityException ex)
            {
                throw (new EntityException("There is the following error: {0}", ex));
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion

        public UserDTO CheckUserAndPassword(string username)
        {
            try
            {
                UserDTO user = new UserDTO();
                using (var context = new MessageBoardEntities())
                {
                    var login = context.tblUsers.FirstOrDefault(x => x.Username == username);

                    if(login != null)
                    {
                        user.Username = login.Username;
                        user.PasswordHash = login.PasswordHash;
                        user.PasswordSalt = login.PasswordSalt;

                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
