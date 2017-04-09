using AutoMapper;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessageBoardService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageBoardService.svc or MessageBoardService.svc.cs at the Solution Explorer and start debugging.
    public class MessageBoardService : IMessageBoardService
    {

        #region InsertNewUser
        public bool InsertNewUser(UserDTO user)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<UserDTO, tblUser>();
                    });
                    IMapper mapper = config.CreateMapper();
                    tblUser addUser = context.tblUsers.Create();
                    addUser = mapper.Map<UserDTO, tblUser>(user);

                    context.tblUsers.Add(addUser);
                    context.SaveChanges();

                    return true;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
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

                    if (login != null)
                    {
                        user.Username = login.Username;
                        user.PasswordHash = login.PasswordHash;
                        user.PasswordSalt = login.PasswordSalt;
                        user.UserID = login.UserID;
                    }
                    return user;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
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
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblUser, UserDTO>();
                    });
                    IMapper mapper = config.CreateMapper();
                    var users = context.tblUsers;

                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            UserDTO userDTO = mapper.Map<tblUser, UserDTO>(user);
                            usersList.Add(userDTO);
                        }
                    }
                }
                return usersList;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
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
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblUser, UserDTO>();
                    });
                    IMapper mapper = config.CreateMapper();
                    var user = context.tblUsers.FirstOrDefault(x => x.UserID == userID);
                    if (user != null)
                    {
                        userDTO = mapper.Map<tblUser, UserDTO>(user);
                    }
                    return userDTO;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region IsAdministrator
        public bool IsAdministrator(int userID)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var user = context.tblUsers.FirstOrDefault(x => x.UserID == userID);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region ChangePassword
        public bool ChangePassword(UserDTO user)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var userUpdated = context.tblUsers.FirstOrDefault(x => x.UserID == user.UserID);
                    if (userUpdated != null)
                    {
                        userUpdated.PasswordHash = user.PasswordHash;
                        userUpdated.PasswordSalt = user.PasswordSalt;
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region UpdateIsActive
        public bool UpdateIsActive(List<UserDTO> users)
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region AddNewPost
        public bool AddNewPost(PostDTO addPost)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<PostDTO, tblPost>();
                    });
                    IMapper mapper = config.CreateMapper();
                    tblPost addNewPost = context.tblPosts.Create();
                    addNewPost = mapper.Map<PostDTO, tblPost>(addPost);
                    context.tblPosts.Add(addNewPost);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region FillPostsGrid
        public List<PostDTO> FillPostsGrid()
        {
            try
            {
                List<PostDTO> addPosts = new List<PostDTO>();
                using (var context = new MessageBoardEntities())
                {
                    var posts = context.tblPosts;
                    if (posts != null)
                    {

                        foreach (var post in posts)
                        {
                            PostDTO postDTO = new PostDTO();
                            UserDTO userDTO = new UserDTO();
                            postDTO.tblUser = userDTO;

                            postDTO.PostID = post.PostID;
                            postDTO.PostText = post.PostText;
                            postDTO.PostImage = post.PostImage;
                            postDTO.IsPublished = post.IsPublished;
                            postDTO.CreationDate = post.CreationDate;
                            postDTO.tblUser.FullName = post.tblUser.FirstName + " " + post.tblUser.LastName;
                            postDTO.tblUser.Username = post.tblUser.Username;
                            var c = post.tblComments.Count(x => x.CreationDate != null);
                            if (c == 0)
                            {
                                postDTO.LastCommentDate = null;
                            }
                            else
                            {
                                postDTO.LastCommentDate = post.tblComments.Max(x => x.CreationDate);
                            }

                            addPosts.Add(postDTO);
                        }

                    }
                    return addPosts;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region ChangeProfilePicture
        public bool ChangeProfilePicture(UserDTO user)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var userUpdated = context.tblUsers.FirstOrDefault(x => x.UserID == user.UserID);
                    if (userUpdated != null)
                    {
                        userUpdated.ProfileImage = user.ProfileImage;
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region GetProfilePicture
        public UserDTO GetProfilePicture(int userID)
        {
            try
            {
                UserDTO user = new UserDTO();
                using (var context = new MessageBoardEntities())
                {
                    var userProfilePicture = context.tblUsers.FirstOrDefault(x => x.UserID == userID);

                    if (userProfilePicture != null)
                    {
                        user.ProfileImage = userProfilePicture.ProfileImage;
                    }
                    return user;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }
        }
        #endregion

        #region GetCommentsForPostID
        public List<CommentDTO> GetCommentsForPostID(int postID)
        {
            try
            {
                List<CommentDTO> commentsDTO = new List<CommentDTO>();
                using (var context = new MessageBoardEntities())
                {
                    var comments = context.tblComments.Where(x => x.PostID == postID);
                    if (comments != null)
                    {
                        foreach (var comment in comments)
                        {
                            CommentDTO commentDTO = new CommentDTO();
                            UserDTO userDTO = new UserDTO();
                            commentDTO.tblUser = userDTO;

                            commentDTO.CommentContent = comment.CommentContent;
                            commentDTO.CommentID = comment.CommentID;
                            commentDTO.CreationDate = comment.CreationDate;
                            commentDTO.IsBlocked = comment.IsBlocked;
                            commentDTO.MainComment = comment.MainComment;
                            commentDTO.UserID = comment.UserID;
                            commentDTO.tblUser.Username = comment.tblUser.Username;
                            commentDTO.tblUser.ProfileImage = comment.tblUser.ProfileImage;

                            commentsDTO.Add(commentDTO);
                        }
                    }
                    return commentsDTO;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                return null;
            }
        }
        #endregion

        #region AddComment
        public bool AddComment(CommentDTO addNewComment)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CommentDTO, tblComment>();
                    });
                    IMapper mapper = config.CreateMapper();
                    tblComment addComment = new tblComment();
                    addComment = mapper.Map<CommentDTO, tblComment>(addNewComment);
                    context.tblComments.Add(addComment);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
                throw ex;
            }

        }
        #endregion
    }
}
