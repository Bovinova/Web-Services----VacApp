using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacApp_Bovinova_Platform.IAM.Interfaces.REST.Resources
{
    public record SignUpResource(
        string Username,
        string Password,
        string Email
    );
}