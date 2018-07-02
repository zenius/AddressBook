using System;
using System.Security;
using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Linq;
using System.Configuration; 

namespace AddressBook
{
    class Program
    {   
        public static SecureString GetPassword()
        {
            string input = ConfigurationManager.AppSettings["loginPassword"]; /*Console.ReadLine();*/
            SecureString securePassword = new SecureString();
            foreach (char c in input.ToCharArray())
                securePassword.AppendChar(c);
            return securePassword;
        }

        public static void ShowContact(Contact contact)
        {
            Console.WriteLine("\n************************************");
            Console.WriteLine("\nContact ID:{0}", contact.Id);
            Console.WriteLine("\nName:{0}", contact.Name);
            Console.WriteLine("\nMobile Number:{0}", contact.MobileNumber);
            Console.WriteLine("\nAddress:{0}", contact.Address);
            Console.WriteLine("\nEmail:{0}", contact.Email);
            Console.WriteLine("\nDepartment:{0}", contact.Department);
            Console.WriteLine("\nMarital Status:{0}", contact.MaritalStatus);
            Console.WriteLine("\nSalary:{0}", contact.Salary);
            Console.WriteLine("\nDate of Birth:{0}", contact.DateOfBirth.ToString("mm/dd/yyyy"));
            Console.WriteLine("\nHappy:{0}", contact.Happy? "Yes": "No");
            Console.WriteLine("\nCompany:{0}", contact.Company);
            Console.WriteLine("\nSite Member:{0}", contact.SiteMember);
            Console.WriteLine("\nWeb Page Url:{0}", contact.WebPageUrl.ToLower());
            Console.WriteLine("\n************************************");
        }

        public static void ShowAllContacts(List<Contact> allContacts)
        {
            Console.WriteLine("\n************************************");

            foreach(var contact in allContacts)
            {
                Console.WriteLine("\nContact ID:{0}", contact.Id);
                Console.WriteLine("\nName:{0}", contact.Name);
                Console.WriteLine("\nMobile Number:{0}", contact.MobileNumber);
                Console.WriteLine("\nAddress:{0}",contact.Address);
                Console.WriteLine("\nEmail:{0}", contact.Email);
                Console.WriteLine("\nDepartment:{0}", contact.Department);
                Console.WriteLine("\nMarital Status:{0}", contact.MaritalStatus);
                Console.WriteLine("\nSalary:{0}", contact.Salary);
                Console.WriteLine("\nDate of Birth:{0}", contact.DateOfBirth.ToString("mm/dd/yyyy"));
                Console.WriteLine("\nHappy:{0}", contact.Happy ? "Yes" : "No");
                Console.WriteLine("\nCompany:{0}", contact.Company);
                Console.WriteLine("\nSite Member:{0}", contact.SiteMember);
                Console.WriteLine("\nWeb page Url:{0}", contact.WebPageUrl.ToLower());
                Console.WriteLine("\n************************************"); 
            }
        }

        static void Main(string[] args)
        {
           // Console.Write("\nEnter password: ");
           SecureString password = GetPassword();

            if (password != null)
            {
                Context context = new Context(); 
                bool isConnected = context.Connect(password); 

                if (isConnected)
                {
                   Console.WriteLine("\nConnection Successful");

                    Service service = new Service(); 

                    char continueChoice;
                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("\nAddress Book Menu");
                        Console.WriteLine("\n1.Add Contact");
                        Console.WriteLine("\n2.Update Contact");
                        Console.WriteLine("\n3.Delete Contact");
                        Console.WriteLine("\n4.Show All Contacts");

                        Console.Write("\nEnter the Option:");
                        int option = Validate.GetOption();

                        List<Contact> allContacts; 

                        switch (option)
                        {
                            case Constants.ADD:
                                Contact contactToAdd = new Contact(); 
                                Console.Write("\nEnter Full Name: ");
                                contactToAdd.Name = Validate.GetName(Console.ReadLine());
                                Console.Write("\nEnter Mobile Number: ");
                                contactToAdd.MobileNumber = Validate.GetMobileNumber(Console.ReadLine());
                                Console.Write("\nEnter Address: ");
                                contactToAdd.Address = Validate.GetAddress();
                                Console.Write("\nEnter Email Address: ");
                                contactToAdd.Email = Validate.GetEmail(Console.ReadLine());

                                Console.Write("\nEnter Department: ");
                                contactToAdd.Department = Validate.GetDepartment(Console.ReadLine(),context);

                                Console.Write("\nEnter Marital Status[Single|Married]: ");
                                contactToAdd.MaritalStatus = Validate.GetMaritalStatus(Console.ReadLine(), context);

                                Console.Write("\nEnter Salary:");
                                contactToAdd.Salary = Validate.GetSalary(Console.ReadLine());

                                Console.Write("\nEnter Date Of Birth [mm/dd/yyyy] (yyyy = 1900-2099): ");
                                contactToAdd.DateOfBirth = Validate.GetDateOfBirth(Console.ReadLine());

                                Console.Write("\nAre you Happy? Press y for 'yes' and 'n' for 'no': ");
                                contactToAdd.Happy = Validate.IsHappy(Console.ReadLine());

                                Console.Write("\nEnter Company: ");
                                contactToAdd.Company = Validate.GetCompany(Console.ReadLine(),context);

                                Console.Write("\nEnter name of Site Member: ");
                                contactToAdd.SiteMember = Validate.GetSiteMember(Console.ReadLine(),context);

                                Console.Write("\nEnter a Web Page Url: ");
                                contactToAdd.WebPageUrl = Validate.GetWebPageUrl(Console.ReadLine(), context);

                                contactToAdd = service.Add(contactToAdd, context);

                                Console.WriteLine("\nNew Contact Added Successfully");

                                /*display the contact added*/
                                ShowContact(contactToAdd);

                                break;
                            case Constants.UPDATE:
                               allContacts = service.GetAllContacts(context);

                                /**display the list of contact available to the user**/
                                ShowAllContacts(allContacts);

                                Console.Write("\nEnter the Contact Id to UPDATE: ");
                                int _id = Validate.GetId();

                                /**check whether the id is available or not,
                                 * if found update it,
                                 * otherwise display, id not found
                                 * **/
                                if (service.doIdExist(_id, context))
                                {
                                    /**show the the item to update details*/
                                    //ListItem itemToUpdate = service.GetItem(_id, context);
                                    Contact contactToUpdate = new Contact(); 
                                    contactToUpdate = service.GetContact(_id, context, contactToUpdate);

                                    /**displaying the selected contact to update**/
                                    Console.WriteLine("\n*******************Selected Contact To Update******************");
                                    ShowContact(contactToUpdate);

                                    Console.Write("\nName: ");
                                    string input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.Name = Validate.GetName(input); 
                                    }
                                   
                                    Console.Write("\nMobile Number: ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.MobileNumber = Validate.GetMobileNumber(input);
                                    }

                                    Console.Write("\nEmail: ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                       contactToUpdate.Email = Validate.GetEmail(input); 
                                    }

                                    Console.Write("\nDepartment: ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.Department = Validate.GetDepartment(input, context);
                                    }

                                    Console.Write("\nEnter Marital Status[Single|Married]: ");
                                    input = Console.ReadLine();
                                    if(input!="")
                                    {
                                        contactToUpdate.MaritalStatus = Validate.GetMaritalStatus(input, context);
                                    }
                                  
                                    Console.Write("\nEnter Salary:");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.Salary = Validate.GetSalary(input);
                                    }

                                    Console.Write("\nEnter Date of Birth[dd/mm/yyyy] (year = 1900-2099): ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.DateOfBirth = Validate.GetDateOfBirth(input);
                                    }

                                    Console.Write("\nAre you Happy? Press y for 'yes' and 'n' for 'no': ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.Happy = Validate.IsHappy(input);
                                    }

                                    Console.Write("\nEnter Company: ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.Company = Validate.GetCompany(input, context);
                                    }

                                    Console.Write("\nEnter name of Site Member: "); 
                                    input = Console.ReadLine(); 
                                    if(input!= "")
                                    {
                                        contactToUpdate.SiteMember = Validate.GetSiteMember(input, context);
                                    }

                                    Console.Write("\nEnter a Web Page Url: ");
                                    input = Console.ReadLine();
                                    if (input != "")
                                    {
                                        contactToUpdate.WebPageUrl = Validate.GetWebPageUrl(input, context);
                                    }

                                    contactToUpdate = service.Update(_id,contactToUpdate, context);

                                    /**Show the updated contact details**/
                                    ShowContact(contactToUpdate);

                                    Console.WriteLine("\n**********Successfully Updated**********");
                                }
                                else
                                {
                                    Console.WriteLine("\nID not found");
                                }

                                break;

                            case Constants.DELETE:
                                /**display the list of contact available to the user**/
                                allContacts = service.GetAllContacts(context);

                                Console.WriteLine("\n**************Available Contacts*************");
                                /**display the list of contact available to the user**/
                                ShowAllContacts(allContacts);

                                Console.Write("\nEnter the Contact Id to DELETE: ");
                                int id = Validate.GetId();

                                /**check whether the id is available or not,
                                 * if found delete it,
                                 * otherwise display, id not found
                                 * **/
                                if (service.doIdExist(id, context))
                                {
                                    service.Delete(id, context);
                                    Console.WriteLine("\n**********Successfully Deleted**********");
                                }
                                else
                                {
                                    Console.WriteLine("\nID not found");
                                }
                                break;

                            case Constants.SHOW_ALL_CONTACTS:
                                allContacts = service.GetAllContacts(context);

                                Console.WriteLine("\n**************Available Contacts*************");
                                /**display the list of contact available to the user**/
                                ShowAllContacts(allContacts);

                                break;

                            default:
                                Console.WriteLine("\nWrong Option");
                                break;
                        }

                        Console.Write("\nDo you want to continue: 'y' for Yes / 'n' for No:");
                        continueChoice = Validate.GetValidChoice();
                        Console.WriteLine();
                    } while (continueChoice == 'y' || continueChoice == 'Y');

                    Console.WriteLine("\nThank You");
                }
                else
                {
                    Console.WriteLine("\nPassword do not match");
                }
            }
            else
            {  
                Console.WriteLine("\nPassword cannot be empty");
            }
            Console.ReadKey();
        }
    }
}
