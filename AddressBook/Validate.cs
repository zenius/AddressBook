using System;
using System.Linq;
using System.Text;
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
                    Console.Write("\nEnter Valid Option: ");
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
                    Console.Write("\nEnter Positive Number: ");
                }
            }
            return input;
        }

        public static string GetName(string name)
        {
            while(name == "" || !Regex.IsMatch(name, Constants.namePattern))
            {
                Console.Write("\nEnter the valid Name: ");
                name = Console.ReadLine(); 
            }
            return name;
        }

        public static string GetEmail(string email)
        {
            while (email == "" || !Regex.IsMatch(email, Constants.emailPattern))
            {
                Console.Write("\nEnter Valid Email Address: ");
                email = Console.ReadLine(); 
            }
            return email;
        }

        public static string GetMobileNumber(string mobileNumber)
        {
            while (mobileNumber=="" || !Regex.IsMatch(mobileNumber, Constants.mobileNumberPattern))
            {
                Console.Write("\nEnter 10 digit Mobile Number: ");
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
                Console.WriteLine("\nPlease Enter the Valid Department: ");
                department = Console.ReadLine();
            }

            bool doExist = false;

            Service service = new Service(); 
            TermCollection termCollection = service.GetTermCollection(context);

            /**check to see whether the term exists or not**/
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

            doExist = maritalStatusChoices.Any(t => t.ToString().Equals(maritalStatus));

            while (!doExist /*&& !Regex.IsMatch(maritalStatus, Constants.maritalStatusPattern)*/)
            {
                Console.Write("\nPlease Enter the Valid maritalStatus [Single/Married]: "); 
                maritalStatus = Console.ReadLine();
                doExist = maritalStatusChoices.Any(t => t.ToString().Equals(maritalStatus));
            }

            return maritalStatus; 
        }

        public static double GetSalary(string salary)
        {
            while (salary == "" || !Regex.IsMatch(salary, Constants.salaryPattern))
            {
                Console.Write("\nEnter the Valid Salary [positive number or positive number with decimal]: ");
                salary = Console.ReadLine();
            }
            
            return Convert.ToDouble(salary); 
        }


        public static DateTime GetDateOfBirth(string dateOfBirth)
        {
            string[] dateFormats = {"mm/dd/yyyy", "m/d/yyyy", "mm/d/yyyy","m/d/yyyy" }; 
            while(dateOfBirth =="" || !Regex.IsMatch(dateOfBirth, Constants.dateOfBirthPattern))
            {
                Console.Write("\nEnter the Valid Date of Birth [mm/dd/yyyy] (yyyy = 1900-2099): ");
                dateOfBirth = Console.ReadLine();
            }

           return DateTime.ParseExact(dateOfBirth, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None); 
        }

        public static bool IsHappy(string happy)
        {
            bool isHappy = false; 
            while (!Regex.IsMatch(happy, Constants.happyPattern))
            {
                Console.Write("\nPlease enter valid choice");
                Console.Write("\nAre you Happy? Press y for 'yes' and 'n' for 'no': "); 
                happy = Console.ReadLine();
            }
           
            if(happy.Equals("y") || happy.Equals("Y"))  { isHappy = true; }
            else  if(happy.Equals("n") || happy.Equals("N")) { isHappy = false; }

            return isHappy; 
        }

        public static string GetCompany(string company, Context context)
        {
            bool doExist = false; 

            Service service = new Service();
            ListItemCollection lookupListItemCollection = service.GetListItemCollection(context,Constants.lookupListName);

            doExist = lookupListItemCollection.Any(t => t[Constants.Title].ToString().Equals(company)); 

            while (!doExist /*&& !Regex.IsMatch(company, Constants.companyPattern)*/)
            {
                Console.WriteLine("\nAvailable Company List");
                foreach (ListItem _company in lookupListItemCollection)
                {
                    Console.WriteLine("\n{0}", _company[Constants.Title]);
                }
                Console.Write("\nPlease Enter the Valid Company Name: ");
                company = Console.ReadLine();
                doExist = lookupListItemCollection.Any(t => t[Constants.Title].ToString().Equals(company));
            }

            return company;
        }

        public static string GetSiteMember(string siteMember, Context context)
        {
            bool doExist = false;

            Service service = new Service();
            UserCollection userCollection = service.GetUserCollection(context, Constants.GroupName);

            //StringBuilder member = new StringBuilder();
            //member.Append(siteMember);
            //member.Append(Constants.DomainName); 

            string member = string.Format("{0}{1}", siteMember, Constants.DomainName);

            doExist = userCollection.Any(t => t.Title.Equals(member));

            while (!doExist)
            {
                Console.WriteLine("\nAvailable User Name List"); 
                foreach (User user in userCollection)
                { 
                    Console.WriteLine("\n{0}", user.Title);
                }

                Console.Write("\nPlease Enter the Valid User Name: ");
                siteMember = Console.ReadLine();

                doExist = userCollection.Any(t => t.Title.Equals(siteMember));
            }

            return siteMember;
        }

        public static string GetWebPageUrl(string webPageUrl,Context context)
        {
            while (webPageUrl == "" || !Regex.IsMatch(webPageUrl, Constants.webPageUrlPattern))
            {
                Console.Write("\nEnter Valid Web Page Url: ");
                webPageUrl = Console.ReadLine();
            }
            return webPageUrl.ToLower();
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
