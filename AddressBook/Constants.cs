using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    class Constants
    {
        public const string siteUrl = "https://zeniuslama.sharepoint.com";
        public const string loginName = "bharatdong@zeniuslama.onmicrosoft.com";

        /*operation count*/
      //  public const int optionCount = 4;

        /*order of operation*/
        public const int ADD = 1;
        public const int UPDATE = 2;
        public const int DELETE = 3;
        public const int SHOW_ALL_CONTACTS = 4; 

        /*Column(Field) Name of the list*/
        public const string FullName = "FullName";
        public const string CellPhone = "CellPhone";
        public const string WorkAddress = "WorkAddress";
        public const string Email = "Email";

        /*regex expression for validation*/
        public const string namePattern = @"^[A-Za-z\s]+$";
        public const string  mobileNumberPattern = @"^[\d]{10}$";
        public const string emailPattern = @"^\w+([-.]\w+)*@\w+([.]\w+)*\.\w+$";
    }
}
