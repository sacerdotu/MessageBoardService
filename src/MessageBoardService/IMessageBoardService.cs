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
    [ServiceContract]
    public interface IMessageBoardService
    {
        [OperationContract]
        UserDTO InsertNewUser(UserDTO user);
        [OperationContract]
        UserDTO CheckUserAndPassword(string username);
        [OperationContract]
        List<UserDTO> FillUsersGrid();
        [OperationContract]
        UserDTO GetUserDetails(int userID);
        [OperationContract]
        bool IsAdministrator(int userID);
        [OperationContract]
        bool ChangePassword(UserDTO user);
        [OperationContract]
        bool UpdateIsActive(List<UserDTO> users);
        [OperationContract]
        bool AddNewPost(PostDTO addPost);
        [OperationContract]
        List<PostDTO> FillPostsGrid();
        [OperationContract]
        bool ChangeProfilePicture(UserDTO user);
        [OperationContract]
        UserDTO GetProfilePicture(int userID);
        [OperationContract]
        List<CommentDTO> GetCommentsForPostID(int postID);
    }
}
