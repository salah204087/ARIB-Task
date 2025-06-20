using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class MessageEnum
    {
        public const string Create = " Created Successfully";
        public const string Update = " Updated Successfully";
        public const string Delete = " Deleted Successfully";
        public const string Get = " Returned Successfully";
        public const string booking = " booked successfully";
        public const string Cancel = " Canceled successfully";
        public const string Recover = " Recovered successfully";

        public const string Approve = " Approved successfully";
        public const string Reject = " Rejected successfully";

        public static string Created(string resourceName)
        {
            return resourceName + "" + Create;
        }
        public static string Updated(string resourceName)
        {
            return resourceName + "" + Update;
        }
        public static string Deleted(string resourceName)
        {
            return resourceName + "" + Delete;
        }
        public static string Getted(string resourceName)
        {
            return resourceName + "" + Get;
        }
        public static string Booking(string resourceName)
        {
            return resourceName + "" + booking;
        }
        public static string Canceled(string resourceName)
        {
            return resourceName + "" + Cancel;
        }
        public static string Recovered(string resourceName)
        {
            return resourceName + "" + Recover;
        }
        public static string Approved(string resourceName) => resourceName + Approve;
        public static string Rejected(string resourceName) => resourceName + Reject;
    }
}
