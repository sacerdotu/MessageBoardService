﻿using AutoMapper;
using MessageBoardCommon;
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion

        #region CheckUserAndPassword
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region FillUsersGrid
        public List<UserDTO> FillUsersGrid()
        {
            List<UserDTO> usersList = new List<UserDTO>();
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var users = context.tblUsers;
                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            UserDTO userDTO = new UserDTO();
                            userDTO.UserID = user.UserID;
                            userDTO.FirstName = user.FirstName;
                            userDTO.LastName = user.LastName;
                            userDTO.Username = user.Username;
                            userDTO.PasswordHash = user.PasswordHash;
                            userDTO.City = user.City;
                            userDTO.Country = user.Country;
                            userDTO.Function = user.Function;
                            userDTO.PasswordSalt = user.PasswordSalt;
                            userDTO.AccountCreationDate = user.AccountCreationDate;
                            userDTO.IsActive = user.IsActive;
                            userDTO.IsAdministrator = user.IsAdministrator;

                            usersList.Add(userDTO);
                        }
                        return usersList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region GetUserDetails
        public UserDTO GetUserDetails(int userID)
        {
            UserDTO userDTO = new UserDTO();
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var user = context.tblUsers.FirstOrDefault(x => x.UserID == userID);
                    if(user != null)
                    {
                        userDTO.UserID = user.UserID;
                        userDTO.FirstName = user.FirstName;
                        userDTO.LastName = user.LastName;
                        userDTO.Username = user.Username;
                        userDTO.PasswordHash = user.PasswordHash;
                        userDTO.City = user.City;
                        userDTO.Country = user.Country;
                        userDTO.Function = user.Function;
                        userDTO.PasswordSalt = user.PasswordSalt;
                        userDTO.AccountCreationDate = user.AccountCreationDate;
                        userDTO.IsActive = user.IsActive;
                        userDTO.IsAdministrator = user.IsAdministrator;
                    }
                    return userDTO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region IsAdministrator
        public bool IsAdministrator(string username)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var user = context.tblUsers.FirstOrDefault(x => x.Username == username);
                    if (user != null)
                    {
                        return user.IsAdministrator;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ChangePassword
        public void ChangePassword(UserDTO user)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var userUpdated = context.tblUsers.FirstOrDefault(x => x.Username == user.Username);
                    if (userUpdated != null)
                    {
                        userUpdated.PasswordHash = user.PasswordHash;
                        userUpdated.PasswordSalt = user.PasswordSalt;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UpdateIsActive
        public void UpdateIsActive(List<UserDTO> users)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    foreach (var item in users)
                    {
                        var userUpdated = context.tblUsers.FirstOrDefault(x => x.UserID == item.UserID);
                        if (userUpdated != null)
                        {
                            userUpdated.IsActive = item.IsActive;
                            context.SaveChanges();
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region AddNewPost
        public void AddNewPost(List<string> addPost)
        {
            try
            {
                string username = addPost[0];
                string post = addPost[1];
                using (var context = new MessageBoardEntities())
                {
                    var user = context.tblUsers.FirstOrDefault(x => x.Username == username);
                    if (user != null)
                    {
                        tblPost addNewPost = context.tblPosts.Create();
                        addNewPost.UserID = user.UserID;
                        addNewPost.PostText = post;
                        addNewPost.IsPublished = true;
                        addNewPost.CreationDate = DateTime.Now;

                        context.tblPosts.Add(addNewPost);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
