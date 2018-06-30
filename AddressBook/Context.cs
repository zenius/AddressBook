using System;
using System.Security;
using Microsoft.SharePoint.Client;
using System.Configuration; 

namespace AddressBook
{
    class Context
    {
        public ClientContext clientContext { get; set; }
  
        public bool Connect(SecureString password)
        {
            using (clientContext = new ClientContext(ConfigurationManager.AppSettings["siteUrl"].ToString()))
            {
                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings["loginName"].ToString(), password);

                //to handle the error, if the  credentials do not match 
                try
                {
                    clientContext.ExecuteQuery();
                    return true;
                }

                catch (Exception)
                {
                    return false;
                }
            }     
        }
    }
}
