using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security;
using Microsoft.SharePoint.Client;


namespace AddressBook
{
    class Program
    {
        public static void ShowContact(ListItem item)
        {
            Console.WriteLine("\nName:{0}",item[Constants.FullName]);
            Console.WriteLine("\nMobile Number:{0}", item[Constants.CellPhone]);
            Console.WriteLine("\nAddress:{0}", item[Constants.WorkAddress]);
            Console.WriteLine("\nEmail:{0}",item[Constants.Email]);
        }

        public static void ShowAllContacts(ListItemCollection listItemCollection)
        {
            foreach(ListItem item in listItemCollection)
            {
                Console.WriteLine("\nContact ID:{0}", item.Id);
                Console.WriteLine("\nName:{0}", item[Constants.FullName]);
                Console.WriteLine("\nMobile Number:{0}", item[Constants.CellPhone]);
                Console.WriteLine("\nAddress:{0}", item[Constants.WorkAddress]);
                Console.WriteLine("\nEmail:{0}", item[Constants.Email]);
                Console.WriteLine("\n************************************"); 
            }
        }

        static void Main(string[] args)
        {
            Service service = new Service(); 

            Console.Write("Enter password: ");
            SecureString securePassword = service.GetPassword();
            //Context context = new Context(); 

            if (securePassword != null)
            {
                bool connectSuccess = context.Connect(securePassword); 
                if (connectSuccess)
                 {
                    Console.WriteLine("\nConnection Successful");

                    

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

                        switch (option)
                        {
                            case Constants.ADD:  /**create operation: insert new item**/
                                Contact contactToAdd = new Contact();
                                Console.Write("\nEnter Full Name:");
                                contactToAdd.Name = Validate.GetName();
                                Console.Write("\nEnter Mobile Number:");
                                contactToAdd.MobileNumber = Validate.GetMobileNumber();
                                Console.Write("\nEnter Address:");
                                contactToAdd.Address = Validate.GetAddress();
                                Console.Write("\nEnter Email Address:");
                                contactToAdd.Email = Validate.GetEmail();
                               
                                ListItem newContact = service.Add(contactToAdd, context.clientContext);

                                Console.WriteLine("\nNew Contact Added Successfully");

                                /*display the contact added*/
                                ShowContact(newContact); 
                                
                                break;
                            case Constants.UPDATE: 
                                /**display the list of contact available to the user**/
                                ShowAllContacts(service.listItemCollection);

                                Console.Write("\nEnter the Conact Id To Update: ");
                                int _id = Validate.GetId();

                                Contact contactToUpdate = new Contact();
                                string input = ""; 
                                Console.Write("\nName:");
                                input = Console.ReadLine(); 
                                if(!String.IsNullOrEmpty(input))
                                {
                                    contactToUpdate.Name = Validate.GetName();  
                                }
                             
                                //Console.Write("\nMobile Number:");
                                //contactToUpdate.MobileNumber = Validate.GetMobileNumber();
                                //Console.Write("\nAddress:");
                                //contactToUpdate.Address = Validate.GetAddress();
                                //Console.Write("\nEmail:");
                                //contactToUpdate.Email = Validate.GetEmail();

                                /**update operation:update an item**/

                                ListItem updatedContact = service.Update(_id); 
                                
                                
                                //if (itemToUpdate != null)
                                //{
                                //    char continueOption;
                                //    do
                                //    {
                                //        Contact contactToUpdate = new Contact();
                                //        Console.Write("\nName:");
                                //        contactToUpdate.Name = Validate.GetName();
                                //        Console.Write("\nMobile Number:");
                                //        contactToUpdate.MobileNumber = Validate.GetMobileNumber();
                                //        Console.Write("\nAddress:");
                                //        contactToUpdate.Address = Validate.GetAddress();
                                //        Console.Write("\nEmail:");
                                //        contactToUpdate.Email = Validate.GetEmail();

                                //        contact.Update(itemToUpdate, contactToUpdate);
                                //        Console.WriteLine("\nWhat do you want to Update?");
                                //        Console.WriteLine("\n1.Name");
                                //        Console.WriteLine("\n2.Mobile Number");
                                //        Console.WriteLine("\n3.Address");
                                //        Console.WriteLine("\n4.Email");

                                //        Console.Write("\nEnter the Option:");
                                //        const int choiceCount = 4;
                                //        int choice = GetOption(choiceCount);

                                //        Contact contactToUpdate = new Contact();

                                //        switch (choice)
                                //        {
                                //            case (int)Column.FULLNAME:
                                //                Console.Write("\nEnter Full Name:");
                                //                contactToUpdate.Name = GetName();
                                //                contact.Update(itemToUpdate, contactToUpdate, choice);
                                //                context.ExecuteQuery();
                                //                Console.WriteLine("\nSuccessfully Updated");
                                //                break;
                                //            case (int)Column.MOBILENUMBER:
                                //                Console.Write("\nEnter Mobile Number:");
                                //                contactToUpdate.MobileNumber = GetMobileNumber();
                                //                contact.Update(itemToUpdate, contactToUpdate, choice);
                                //                context.ExecuteQuery();
                                //                Console.WriteLine("\nSuccessfully Updated");
                                //                break;
                                //            case (int)Column.ADDRESS:
                                //                Console.Write("\nEnter Address:");
                                //                contactToUpdate.Address = GetAddress();
                                //                contact.Update(itemToUpdate, contactToUpdate, choice);
                                //                context.ExecuteQuery();
                                //                Console.WriteLine("\nSuccessfully Updated");
                                //                break;

                                //            case (int)Column.EMAIL:
                                //                Console.Write("\nEnter Email Address:");
                                //                contactToUpdate.Email = GetEmail();
                                //                contact.Update(itemToUpdate, contactToUpdate, choice);
                                //                context.ExecuteQuery();
                                //                Console.WriteLine("\nSuccessfully Updated");
                                //                break;
                                //            default:
                                //                Console.WriteLine("\nWrong Option");
                                //                break;
                                //        }
                                //        context.ExecuteQuery();

                                //        Console.Write("\ndo you want to update again: 'y' for yes / 'n' for no:");
                                //        continueOption = Validate.GetValidChoice();
                                //        Console.WriteLine();

                                //    } while (continueOption == 'y' || continueOption == 'Y');
                                //}
                                //else
                                //{
                                //    Console.WriteLine("\nID not found");
                                //}
                                break;

                            case Constants.DELETE:
                                /**display the list of contact available to the user**/
                                ShowAllContacts(service.listItemCollection);

                                Console.Write("\nEnter the Contact Id to DELETE: ");
                                int id = Validate.GetId();

                                /**delete operation: delete an item**/
                                bool deleteSuccess = service.Delete(id,context.clientContext);

                                if (deleteSuccess)
                                {
                                    Console.WriteLine("\n**********Successfully Deleted**********");
                                }
                                else
                                {
                                    Console.WriteLine("\nID not found");
                                }
                                break;

                            case Constants.SHOW_ALL_CONTACTS:
                                                            /**display the list of contact available to the user**/
                                                            ShowAllContacts(service.listItemCollection);
                                                            break; 
                            //default:
                            //    Console.WriteLine("wrong option");
                            //    break;
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
