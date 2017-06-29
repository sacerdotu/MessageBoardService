using AutoMapper;
using MessageBoardCommon;
using MessageBoardDAL;
using MessageBoardDTO;
using MessageBoardService.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;

namespace MessageBoardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessageBoardService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageBoardService.svc or MessageBoardService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class MessageBoardService : IMessageBoardService
    {
        private int? _languageID;
        private bool _refreshComments = false;
        private string _connectionString = @"data source=DESKTOP-5U20CQ0\SQLEXPRESS1;initial catalog=MessageBoard;integrated security=True;MultipleActiveResultSets=True";
        private ReportResponseDTO reportResponse;
        private IMessageBoardClient _messageBoardClient = OperationContext.Current.GetCallbackChannel<IMessageBoardClient>();

        public MessageBoardService()
        {

        }

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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }

        }
        #endregion

        #region CheckUserAndPassword
        public UserDTO CheckUserAndPassword(string username, string password)
        {
            try
            {
                UserDTO user = new UserDTO();
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblUser, UserDTO>();
                    });
                    tblUser login = context.tblUsers.FirstOrDefault(x => x.Username == username);
                    if (login != null)
                    {
                        if (CheckUserPasswordByUsername(login, password))
                        {

                            IMapper mapper = config.CreateMapper();
                            user = mapper.Map<tblUser, UserDTO>(login);
                        }
                    }
                    return user;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        public bool CheckUserPasswordByUsername(tblUser username, string password)
        {
            if (username.PasswordHash == PasswordHelper.GetHash(password, username.PasswordSalt))
            {
                return true;
            }
            return false;
        }
        #region GetUserPasswordByUsername

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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                        if (user.LanguageID != null)
                        {
                            userDTO.LanguageName = context.tblLanguages.FirstOrDefault(x => x.LanguageID == user.LanguageID).Name;
                        }
                    }
                    return userDTO;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                        cfg.CreateMap<UserDTO, tblUser>();
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
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
                        cfg.CreateMap<PostDTO, tblPost>();
                        cfg.CreateMap<UserDTO, tblUser>();
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
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }

        }
        #endregion

        #region GetTranslations
        public List<TranslationDTO> GetTranslations(string languageName)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<tblTranslation, TranslationDTO>();
                    });

                    int languageID = GetLanguageID(languageName);
                    var translations = context.tblTranslations.Where(x => x.LanguageID == languageID);
                    IMapper mapper = config.CreateMapper();
                    List<TranslationDTO> returnTranslations = new List<TranslationDTO>();
                    if (translations != null)
                    {
                        foreach (var item in translations)
                        {
                            TranslationDTO translation = mapper.Map<tblTranslation, TranslationDTO>(item);
                            translation.LanguageName = languageName;
                            returnTranslations.Add(translation);
                        }
                        return returnTranslations;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region InsertTranslations
        public void InsertTranslations(Dictionary<string, string> translatedControls, string languageName)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<TranslationDTO, tblTranslation>();
                    });
                    IMapper mapper = config.CreateMapper();

                    int languageID = GetLanguageID(languageName);
                    foreach (var translation in translatedControls)
                    {
                        var findTranslation = context.tblTranslations.FirstOrDefault(x => x.LanguageID == languageID &&
                                x.TranslationKey == translation.Key);

                        if (findTranslation == null)
                        {
                            TranslationDTO newTranslation = new TranslationDTO();
                            //tblTranslation addTranslation = new tblTranslation();
                            newTranslation.LanguageID = languageID;
                            newTranslation.TranslationKey = translation.Key;
                            newTranslation.Translation = translation.Value;
                            newTranslation.DateAdded = DateTime.Now;
                            newTranslation.DateModified = DateTime.Now;

                            tblTranslation addTranslation = mapper.Map<TranslationDTO, tblTranslation>(newTranslation);
                            context.tblTranslations.Add(addTranslation);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region GetLanguageID
        private int GetLanguageID(string languageName)
        {
            try
            {
                int languageID = -1;
                using (var context = new MessageBoardEntities())
                {
                    var language = context.tblLanguages.FirstOrDefault(x => x.Name == languageName);
                    List<TranslationDTO> returnTranslations = new List<TranslationDTO>();
                    if (language != null)
                    {
                        languageID = language.LanguageID;
                    }
                }
                return languageID;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region UpdateUserLanguage
        public void UpdateUserLanguage(int userID, string languageName)
        {
            try
            {
                using (var context = new MessageBoardEntities())
                {
                    int languageID = GetLanguageID(languageName);
                    var userUpdated = context.tblUsers.FirstOrDefault(x => x.UserID == userID);
                    if (userUpdated != null)
                    {
                        userUpdated.LanguageID = languageID;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region GetInsertedCommentsNotifications
        public List<CommentDTO> GetInsertedCommentsNotifications(int lastCommentID, int postID, int userID)
        {
            try
            {
                List<CommentDTO> commentsDTO = new List<CommentDTO>();
                using (var context = new MessageBoardEntities())
                {
                    var comments = context.tblComments.Where(x => x.PostID == postID && x.CommentID > lastCommentID && x.UserID == userID);
                    if (comments != null)
                    {
                        foreach (var comment in comments)
                        {
                            CommentDTO commentDTO = new CommentDTO();
                            UserDTO userDTO = new UserDTO();
                            commentDTO.tblUser = userDTO;

                            commentDTO.CommentID = comment.CommentID;
                            commentDTO.MainComment = comment.MainComment;
                            commentDTO.tblUser.Username = comment.tblUser.Username;

                            commentsDTO.Add(commentDTO);
                        }
                    }
                }
                return commentsDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region GetInsertedPostsNotifications
        public List<PostDTO> GetInsertedPostsNotifications(int lastPostID)
        {
            try
            {
                List<PostDTO> addPosts = new List<PostDTO>();
                using (var context = new MessageBoardEntities())
                {
                    var posts = context.tblPosts.Where(x => x.PostID > lastPostID);
                    if (posts != null)
                    {
                        foreach (var post in posts)
                        {
                            PostDTO postDTO = new PostDTO();
                            UserDTO userDTO = new UserDTO();
                            postDTO.tblUser = userDTO;

                            postDTO.PostID = post.PostID;
                            postDTO.tblUser.Username = post.tblUser.Username;

                            addPosts.Add(postDTO);
                        }
                    }
                    return addPosts;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion


        #region SQL
        #region GetReport
        public List<ReportResponseDTO> GetReport(ReportRequestDTO request)
        {
            List<ReportResponseDTO> reportsList = new List<ReportResponseDTO>();
            try
            {
                if (request != null)
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        SqlCommand command = connection.CreateCommand();
                        command.CommandText = @"SELECT DISTINCT
                                                                tblUser.[UserID],
                                                                tblUser.[FirstName],
                                                                tblUser.[LastName],
                                                                tblUser.[UserName],
                                                                tblUser.[Country],
                                                                tblUser.[City],
                                                                (SELECT COUNT(tblComment.CommentID) FROM tblComment WHERE tblUser.UserID = tblComment.UserID AND tblComment.CreationDate > @CommentStartDate AND tblComment.CreationDate < @CommentEndDate) as NrOfComments ,
                                                                (SELECT COUNT(tblPost.PostID) FROM tblPost WHERE tblUser.UserID = tblPost.UserID AND tblPost.CreationDate > @PostStartDate AND tblPost.CreationDate < @PostEndDate) as NrOfPosts 
                                                                FROM tblUSer
                                                                LEFT JOIN tblPost ON tblPost.UserID = tblUser.UserID
                                                                LEFT JOIN tblComment ON tblComment.UserID = tblUser.UserID ";
                        if (request.RequestForPost)
                        {
                            command.Parameters.Add("@PostStartDate", System.Data.SqlDbType.Date).Value = request.StartDate;
                            command.Parameters.Add("@PostEndDate", System.Data.SqlDbType.Date).Value = request.EndDate;
                            command.Parameters.Add("@CommentStartDate", System.Data.SqlDbType.Date).Value = DateTime.MinValue;
                            command.Parameters.Add("@CommentEndDate", System.Data.SqlDbType.Date).Value = DateTime.MaxValue;
                        }
                        if(request.RequestForComment)
                        {
                            command.Parameters.Add("@CommentStartDate", System.Data.SqlDbType.Date).Value = request.StartDate;
                            command.Parameters.Add("@CommentEndDate", System.Data.SqlDbType.Date).Value = request.EndDate;
                            command.Parameters.Add("@PostStartDate", System.Data.SqlDbType.Date).Value = DateTime.MinValue;
                            command.Parameters.Add("@PostEndDate", System.Data.SqlDbType.Date).Value = DateTime.MaxValue;
                        }
                        if(!request.RequestForComment && !request.RequestForPost)
                        {
                            command.Parameters.Add("@PostStartDate", System.Data.SqlDbType.Date).Value = request.StartDate;
                            command.Parameters.Add("@PostEndDate", System.Data.SqlDbType.Date).Value = request.EndDate;
                            command.Parameters.Add("@CommentStartDate", System.Data.SqlDbType.Date).Value = DateTime.MinValue;
                            command.Parameters.Add("@CommentEndDate", System.Data.SqlDbType.Date).Value = DateTime.MaxValue;
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reportResponse = new ReportResponseDTO();
                                if (!reader.IsDBNull(reader.GetOrdinal("UserID")))
                                {
                                    reportResponse.UserID = (reader.GetInt32(reader.GetOrdinal("UserID")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("FirstName")))
                                {
                                    reportResponse.FullName = (reader.GetString(reader.GetOrdinal("FirstName")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("LastName")))
                                {
                                    reportResponse.FullName = reportResponse.FullName + " " + (reader.GetString(reader.GetOrdinal("LastName")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("Username")))
                                {
                                    reportResponse.Username = (reader.GetString(reader.GetOrdinal("Username")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("Country")))
                                {
                                    reportResponse.Country = (reader.GetString(reader.GetOrdinal("Country")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("City")))
                                {
                                    reportResponse.City = (reader.GetString(reader.GetOrdinal("City")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("NrOfPosts")))
                                {
                                    reportResponse.NrOfPosts = (reader.GetInt32(reader.GetOrdinal("NrOfPosts")));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("NrOfComments")))
                                {
                                    reportResponse.NrOfComments = (reader.GetInt32(reader.GetOrdinal("NrOfComments")));
                                }
                                reportsList.Add(reportResponse);
                            }
                        }
                        connection.Close();
                    }

                }
                return reportsList;
            }
            catch (Exception ex)
            {
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message + "\n" + "StackTrace: " + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #endregion
    }
}
