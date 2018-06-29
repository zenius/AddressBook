using System;

namespace AddressBook
{
    class Contact
    {   
        public int Id { get; set;  }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; } 
        public string Department { get; set; }
        public string MaritalStatus { get; set; }
        public double Salary { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Contact() { }

        public Contact( 
                        int Id, 
                        string Name, 
                        string MobileNumber, 
                        string Email, 
                        string Address, 
                        string MaritalStatus, 
                        double Salary,
                        DateTime DateOfBirth,
                        string Department
        )
        {
            this.Id = Id;
            this.Name = Name;
            this.MobileNumber = MobileNumber;
            this.Email = Email;
            this.Address = Address;
            this.MaritalStatus = MaritalStatus;
            this.Salary = Salary;
            this.DateOfBirth = DateOfBirth; 
            this.Department = Department; 
        }
    }
}
