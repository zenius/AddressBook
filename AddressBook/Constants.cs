using System;

namespace AddressBook
{
    class Constants
    {
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
        public const string Department = "Department";   //managed metadata column
        public const string MaritalStatus = "MaritalStatus";  //choice column
        public const string Salary = "Salary"; //currency column
        public const string DateOfBirth = "DateOfBirth";  // dateTime column

        /*regex expression for validation*/
        public const string namePattern = @"^[A-Za-z\s]+$";
        public const string  mobileNumberPattern = @"^[\d]{10}$";
        public const string emailPattern = @"^\w+([-.]\w+)*@\w+([.]\w+)*\.\w+$";
        public const string departmentPattern = @"^[A-Za-z\s]+$";
        public const string maritalStatusPattern = @"^[A-Z][a-z]+$";
        public const string salaryPattern = @"^[1-9]\d*(\.\d+)?$";
        public const string dateOfBirthPattern = @"^((0?[1-9])|(1[0-2]))/((0?[0-9])|([1-2][0-9])|(3[0-1]))/((19|20)\d{2})$"; // mm/dd/yyyy [yyyy = 1900-2099]

        /*local identifier for english language(Default)*/
        public const int lcid = 1033;

        /**list name**/
        public const string listName = "AddressBook";

        /**termSet Name**/
        public const string termSetName = "Departments"; 
    }
}
