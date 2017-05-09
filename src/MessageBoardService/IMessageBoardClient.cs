using MessageBoardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardService
{

    public interface IMessageBoardClient
    {
        [OperationContract]
        void ShowNotification(CommentDTO comment);
    }
}
