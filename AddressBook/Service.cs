using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Security; 

namespace AddressBook
{
    class Service
    {
        public ClientContext context; 
        public List list { get; set; }
        public ListItemCollection listItemCollection { get; set; }

        public Service()
        {
            context = new ClientContext(Constants.siteUrl);  //server running sharepoint
            //Web web = context.Web;
            //list = web.Lists.GetByTitle("AddressBook");  //retrieving the list: "AddressBook"
            //CamlQuery query = new CamlQuery();
            //query.ViewXml = "<View/>";

            //listItemCollection = list.GetItems(query);
            //Load(context);
        }

        public bool Connect(SecureString password)
        {
            context.Credentials = new SharePointOnlineCredentials(Constants.loginName, password);

            //it will handle the exception thrown, if the credentials do not match
            try
            {
                context.ExecuteQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Load(ClientContext context)
        {
            context.Load(list);
            context.Load(listItemCollection); 
            context.ExecuteQuery();
        }

        public SecureString GetPassword()
        {
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;
            else
            {
                SecureString securePassword = new SecureString();
                foreach (char c in input.ToCharArray())
                    securePassword.AppendChar(c);
                return securePassword;
            }
        }

        public ListItem Add(Contact contact, ClientContext context)
        {
            ListItemCreationInformation itemToAdd = new ListItemCreationInformation();
            ListItem newItem = list.AddItem(itemToAdd);
            newItem[Constants.WorkAddress] = contact.Address;
            newItem[Constants.FullName] = contact.Name;
            newItem[Constants.Email] = contact.Email;
            newItem[Constants.CellPhone] = contact.MobileNumber;

            newItem.Update();
            context.Load(list);
            context.Load(listItemCollection);
            
            context.ExecuteQuery();
            return newItem; 
        }

        public ListItem Update(int id)
        {
            //check whether the id is avaiable or not 
            ListItem itemToUpdate = listItemCollection.SingleOrDefault(item => item.Id.Equals(id));

            return itemToUpdate; 
        }
        //public void Update(ListItem item, Contact contactToUpdate)
        //{
        //    if (contactToUpdate.Name != null)
        //    {
        //        item[Field.FullName.ToString()] = contactToUpdate.Name;
        //    }

        //    if (contactToUpdate.MobileNumber != null)
        //    {
        //        item[Field.CellPhone.ToString()] = contactToUpdate.MobileNumber;
        //    }

        //    if (contactToUpdate.Address != null)
        //    {
        //        item[Field.WorkAddress.ToString()] = contactToUpdate.Address;
        //    }

        //    if (contactToUpdate.Email != null)
        //    {
        //        item[Field.Email.ToString()] = contactToUpdate.Email;
        //    }
        //    //switch(choice)
        //    // {
        //    //     case (int)Column.FULLNAME:
        //    //                                 item[Field.FullName.ToString()] = contactToUpdate.Name;
        //    //                                 break;
        //    //     case (int)Column.MOBILENUMBER:
        //    //                                 item[Field.CellPhone.ToString()] = contactToUpdate.MobileNumber;
        //    //                                 break;
        //    //     case (int)Column.EMAIL:
        //    //                                 item[Field.Email.ToString()] = contactToUpdate.Email;
        //    //                                 break;
        //    //     case (int)Column.ADDRESS:
        //    //                                 item[Field.WorkAddress.ToString()] = contactToUpdate.Address;
        //    //                                 break;
        //    // }
        //    item.Update();
        //}

        public bool Delete(int id,ClientContext context)
        {
            /*check whether the id is avaiable or not*/
            ListItem itemToDelete = listItemCollection.SingleOrDefault(item => item.Id.Equals(id));

            if (itemToDelete != null)
            {
                itemToDelete.DeleteObject();
                Load(context);
                return true; 
            }
            else
            {
                return false; 
            }
        }
    }
}
