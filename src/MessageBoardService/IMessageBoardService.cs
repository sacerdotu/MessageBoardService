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
        void InsertNewUser(UserDTO user);
        [OperationContract]
        UserDTO CheckUserAndPassword(string username);
        [OperationContract]
        List<UserDTO> FillUsersGrid();
        [OperationContract]
        UserDTO GetUserDetails(int userID);
        [OperationContract]
        bool IsAdministrator(string username);
        [OperationContract]
        void ChangePassword(UserDTO user);
        [OperationContract]
        void UpdateIsActive(List<UserDTO> users);
        [OperationContract]
        void AddNewPost(List<string> addPost);
        [OperationContract]
        Dictionary<PostDTO, DateTime?> FillPostsGrid();
    }
}
