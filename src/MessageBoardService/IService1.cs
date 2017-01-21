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
    public interface IService1
    {
        [OperationContract]
        void InsertNewUser(UserDTO user);
        [OperationContract]
        UserDTO CheckUserAndPassword(string username);
    }
}
