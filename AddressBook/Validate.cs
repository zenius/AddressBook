using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client; 
using Microsoft.SharePoint.Client.Taxonomy;
using System.Globalization; 

namespace AddressBook
{
    class Validate 
    {
        public static int GetOption()
        {
            bool valid = false;
            int input = 0;
            while (!valid)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input > 0 /*&& input <= limit*/)
                {
                    valid = true;
                }
                else
                {
                    Console.Write("\nEnter Valid Option:");
                }
            }
            return input;
        }

        public static int GetId()
        {
            bool valid = false;
            int input = 0;
            while(!valid)
            {
                if (int.TryParse(Console.ReadLine(), out input) && input > 0)
                {
                    valid = true; 
                }
                else
                {
                    Console.Write("\nEnter Positive Number:");
                }
            }
            return input;
        }

        public static string GetName(string name)
        {
            while(name == "" || !Regex.IsMatch(name, Constants.namePattern))
            {
                Console.Write("\nEnter the valid Name:");
                name = Console.ReadLine(); 
            }
            return name;
        }

        public static string GetEmail(string email)
        {
            while (email == "" || !Regex.IsMatch(email, Constants.emailPattern))
            {
                Console.Write("\nEnter Valid Email Address:");
                email = Console.ReadLine(); 
            }
            return email;
        }

        public static string GetMobileNumber(string mobileNumber)
        {
            while (mobileNumber=="" || !Regex.IsMatch(mobileNumber, Constants.mobileNumberPattern))
            {
                Console.Write("\nEnter 10 digit Mobile Number:");
                mobileNumber = Console.ReadLine(); 
            }
            return mobileNumber;
        }

        public static string GetAddress()
        {
            string address = "";
            Console.WriteLine("\nPlease Enter \"Ctrl + Z\" to mark the End of Input after pressing enter");

            string input = ""; 

            while (input != null || address == "")
            {
                if (input != null)
                {
                    if (!input.Equals(ConsoleKey.Enter))
                    {
                        address += input + "\n";
                    }
                }
                if (address == "" || input == "")
                {
                    Console.WriteLine("\nAddress cannot be Empty");
                    input = null;
                    address = "";
                }
                input = Console.ReadLine();
            }

            return address.Trim(); 
        }

        public static string GetDepartment(string department,Context context)
        { 
            while (!Regex.IsMatch(department, Constants.departmentPattern))
            {
                Console.WriteLine("\nPlease Enter the Valid Department");
                department = Console.ReadLine();
            }

            /**check to see whether the term exists or not**/
            bool doExist = false;

            Service service = new Service(); 
            TermCollection termCollection = service.GetTermCollection(context); 

            doExist = termCollection.Any(t => t.Name.ToUpper().Equals(department.ToUpper()));

            /** if the deparment does not exist
             * create a new department**/
            if (!doExist)
            {
                service.CreateTerm(context,department);  
            }
            return department.ToUpper(); 
        }

        public static string GetMaritalStatus(string maritalStatus, Context context)
        {
            bool doExist = false;

            Service service = new Service();
            FieldChoice maritalStatusChoice = service.GetMaritalStatusChoices(context);
            var maritalStatusChoices = maritalStatusChoice.Choices;

            while(!doExist && !Regex.IsMatch(maritalStatus, Constants.maritalStatusPattern))
            {
                Console.Write("\nPlease Enter the Valid maritalStatus [Single/Married]"); 
                doExist = maritalStatusChoices.Any(t => t.ToString().ToUpper().Equals(maritalStatus.ToUpper()));
                maritalStatus = Console.ReadLine();
            }

            return maritalStatus; 
        }

        public static double GetSalary(string salary)
        {
            while (salary == "" || !Regex.IsMatch(salary, Constants.salaryPattern))
            {
                Console.Write("\nEnter the Valid Salary [positive number or positive number with decimal]:");
                salary = Console.ReadLine();
            }
            
            return Convert.ToDouble(salary); 
        }


        public static DateTime GetDateOfBirth(string dateOfBirth)
        {
            string[] dateFormats = {"mm/dd/yyyy", "m/d/yyyy", "mm/d/yyyy","m/d/yyyy" }; 
            while(dateOfBirth =="" || !Regex.IsMatch(dateOfBirth, Constants.dateOfBirthPattern))
            {
                Console.Write("\nEnter the Valid Date of Birth [mm/dd/yyyy] (yyyy = 1900-2099):");
                dateOfBirth = Console.ReadLine();
            }

           return DateTime.ParseExact(dateOfBirth, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None); 
        }

        public static bool IsEmployed(string employed)
        {
            bool isEmployed = false; 
            while (!Regex.IsMatch(employed, Constants.employedPattern))
            {
                Console.Write("\nPlease enter valid option");
                Console.Write("\nAre you Employed? [Yes/No] Press y for 'yes' and 'n' for 'no': "); 
                employed = Console.ReadLine();
            }
           
            if(employed.Equals("y") || employed.Equals("Y"))  { isEmployed = true; }
            else  if(employed.Equals("n") || employed.Equals("N")) { isEmployed = false; }

            return isEmployed; 
        }

        public static string GetCompany(string company, Context context)
        {
            bool doExist = false; 

            Service service = new Service();
            ListItemCollection lookupListItemCollection = service.GetListItemCollection(context,Constants.lookupListName);

            Console.WriteLine("\nAvailable Company List");

            foreach(ListItem _company in lookupListItemCollection)
            {
                Console.WriteLine("\n{0}",_company["Title"]); 
            }

            doExist = lookupListItemCollection.Any(t => t[Constants.Title].ToString().Equals(company)); 

            while (!doExist /*&& !Regex.IsMatch(company, Constants.companyPattern)*/)
            {
                Console.Write("\nPlease Enter the Valid Company Name: ");
                company = Console.ReadLine();
                doExist = lookupListItemCollection.Any(t => t[Constants.Title].ToString().Equals(company));
            }

            return company;
        }

        public static char GetValidChoice()
        {
            char choice = Console.ReadKey().KeyChar;
            while (choice != 'y' && choice != 'Y' && choice != 'n' && choice != 'N')
            {
                Console.Write("\nPlease enter the valid continue choice('y' for yes /'n' for no): ");
                choice = Console.ReadKey().KeyChar;
            }
            return choice;
        }
    }
}
