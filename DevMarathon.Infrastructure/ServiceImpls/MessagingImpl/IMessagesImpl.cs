using DevMarathon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMarathon.Infrastructure.ServiceImpls.MessagingImpl
{
    public interface IMessagesImpl
    {
        string GetMessage(MessageCodes code);
       
    }
}
