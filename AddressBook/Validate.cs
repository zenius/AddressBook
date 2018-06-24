using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public static string GetName()
        {
            string name = "";
            bool valid = false;
            while (!valid)
            {
                if ((name = Console.ReadLine()) != null)
                {
                    if (Regex.IsMatch(name, Constants.namePattern))
                    {
                        valid = true; 
                    }
                    else
                    {
                        Console.Write("\nEnter the valid Name:");
                    }
                }
            }
            return name;
        }

        public static string GetEmail()
        {
            string email = "";
            bool valid = false;

            while (!valid)
            {
                if ((email = Console.ReadLine()) != null)
                {
                    if (Regex.IsMatch(email, Constants.emailPattern))
                    {
                        valid = true; 
                    }
                    else
                    {
                        Console.Write("\nEnter Valid Email Address:");
                    }
                }
            }
            return email;
        }

        public static string GetMobileNumber()
        {
            string mobileNumber = "";
            bool valid = false;

            while (!valid)
            {
                if ((mobileNumber = Console.ReadLine()) != null)
                {
                    if (Regex.IsMatch(mobileNumber, Constants.mobileNumberPattern))
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.Write("\nEnter 10 digit Mobile Number:");
                    }
                }
            }
            return mobileNumber;
        }

        public static string GetAddress()
        {
            Console.WriteLine("\nPlease Enter \"Ctrl + Z\" to mark the End of Input");
            string result = "";
            string input = Console.ReadLine();
            while (input != null||result =="")
            {  
                if (input!=null)
                {
                    if (!input.Equals(ConsoleKey.Enter))
                    {
                        result += input + "\n"; 
                    }
                }
                if(result =="")
                {
                    Console.WriteLine("\nAddress cannot be Empty"); 
                }
                input = Console.ReadLine(); 
            }

            return result.Trim(); 
        }

        public static char GetValidChoice()
        {
            char choice = Console.ReadKey().KeyChar;
            while (choice != 'y' && choice != 'Y' && choice != 'n' && choice != 'N')
            {
                Console.Write("\nplease enter the valid continue choice('y' for yes /'n' for no): ");
                choice = Console.ReadKey().KeyChar;
            }
            return choice;
        }
    }
}
