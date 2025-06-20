using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class ErrorEnum
    {
        public const string Required = " is required";
        public const string NotFound = " is not found ";
        public const string NotAvailable = " is not available right now";
        public const string notFoundAny = "Didn't find any  ";
        public const string existed = " Is Already Existed";
        public const string passwordAuth = " Password does not meet the requirements (should contain Upper case,lower case,spiceal char , number and min 8 length) or password is empty";
        public const string equalZero = " not valid or id =0";

        public static string RequiredMessage(string resourceName)
        {
            return resourceName + Required;
        }

        public static string NotFoundMessage(string resourceName)
        {
            return resourceName + NotFound;
        }

        public static string NotAvailableMessage(string resourceName)
        {
            return resourceName + NotAvailable;
        }

        public static string NotFoundAny(string resourceName)
        {
            return notFoundAny + resourceName;
        }
        public static string Existed(string resourceName)
        {
            return resourceName + existed;
        }
        public static string PasswordAuth()
        {
            return passwordAuth;
        }
        public static string EqualZero(string resourceName)
        {
            return resourceName + equalZero;
        }
    }
}
