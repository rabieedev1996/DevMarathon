using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMarathon.Domain.Enums
{
    public enum ResponseCodes
    {
        SUCCESS=0,EXCEPTION=1,
        VALIDATION_ERROR = 2,
        INVALID_ACTIVATION_CODE
    }
}
