using System;
using System.Linq;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace AddressBook
{
    class Service
    {
        List<Contact> AllContacts = new List<Contact>(); 

        public Contact Add(Contact contact, Context context)
        {
            ListItemCreationInformation itemToAdd = new ListItemCreationInformation();
            List list = GetList(context);
         
            ListItem newItem = list.AddItem(itemToAdd);
            //contact.Id = newItem.Id;
            /**changing the managed term value to the suitable format: "-1;managedTermValue|guid" **/
            newItem[Constants.Department] = "-1;#" + contact.Department + "|" + GetTerm(contact.Department.ToUpper(), context).Id.ToString();
            newItem[Constants.FullName] = contact.Name;
            newItem[Constants.Email] = contact.Email;
            newItem[Constants.WorkAddress] = contact.Address;
            newItem[Constants.CellPhone] = contact.MobileNumber;
            
            newItem.Update();

            context.clientContext.Load(newItem);
            context.clientContext.ExecuteQuery();

            contact.Id = newItem.Id;
            return contact; 
        }
        
        public Contact Update(int id, Contact contact, Context context)
        {
            ListItem itemToUpdate = GetItem(id, context);
            itemToUpdate[Constants.Department] = "-1;#" + contact.Department + "|" + GetTerm(contact.Department.ToUpper(), context).Id.ToString();
            itemToUpdate[Constants.FullName] = contact.Name;
            itemToUpdate[Constants.CellPhone] = contact.MobileNumber;
            itemToUpdate[Constants.WorkAddress] = contact.Address;
            itemToUpdate[Constants.Email] = contact.Email;

            itemToUpdate.Update();
            context.clientContext.Load(itemToUpdate);
            context.clientContext.Load(GetListItemCollection(context));

            context.clientContext.ExecuteQuery();

            return contact; 
        }
        
         public Contact GetContact(int id,Context context, Contact contact)
        {
            ListItem item = GetItem(id, context);
            contact.Id = item.Id;
            contact.Name = item[Constants.FullName] as string;
            contact.MobileNumber = item[Constants.CellPhone] as string;
            contact.Address = item[Constants.WorkAddress] as string;

            TaxonomyFieldValue taxonomyFieldValue = item[Constants.Department] as TaxonomyFieldValue;
            contact.Department = taxonomyFieldValue.Label; 

            return contact; 
        }

        public List<Contact> GetAllContacts(Context context)
        {
            ListItemCollection listItemCollection = GetListItemCollection(context); 
            foreach(ListItem item in listItemCollection)
            {
                TaxonomyFieldValue taxonomyFieldValue = item[Constants.Department] as TaxonomyFieldValue;
                AllContacts.Add(new Contact(
                        item.Id, 
                        item[Constants.FullName] as string,
                        item[Constants.CellPhone] as string,
                        item[Constants.WorkAddress] as string,
                        item[Constants.Email] as string,
                        taxonomyFieldValue.Label as string
                    )); 
            }
            return AllContacts; 
        }
        

        public bool Delete(int id, Context context)
        {
            GetItem(id, context).DeleteObject();
            context.clientContext.ExecuteQuery();

            return true;
        }

        public ListItem GetItem(int id, Context context)
        {
            ListItemCollection listItemCollection = GetListItemCollection(context);
            return listItemCollection.SingleOrDefault(item => item.Id.Equals(id));
        }

        public bool doIdExist(int id, Context context)
        {
            if (GetItem(id, context) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List GetList(Context context)
        {
            Web web = context.clientContext.Web; 
            List list = web.Lists.GetByTitle(Constants.listName);
            context.clientContext.Load(list);
            
            context.clientContext.ExecuteQuery();

            return list; 
        }

        public ListItemCollection GetListItemCollection(Context context)
        {
            List list = GetList(context); 
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<View/>";

            ListItemCollection listItemCollection = list.GetItems(query);

            context.clientContext.Load(listItemCollection); 

            context.clientContext.ExecuteQuery();

            return listItemCollection; 
        }

        public Term GetTerm(string department, Context context)
        {
            TermCollection termCollection = GetTermCollection(context); 

            context.clientContext.ExecuteQuery(); 

            return termCollection.SingleOrDefault(t => t.Name.ToString().ToUpper().Equals(department.ToUpper()));
        }

        public TermCollection GetTermCollection(Context context)
        {
            TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(context.clientContext);
            TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            TermGroup termGroup = termStore.GetSiteCollectionGroup(context.clientContext.Site, false);
            TermSet termSet = termGroup.TermSets.GetByName(Constants.termSetName);
            TermCollection termCollection = termSet.GetAllTerms(); 
            context.clientContext.Load(termCollection);

            context.clientContext.ExecuteQuery();

            return termCollection; 
        }

        public TermSet GetTermSet(Context context)
        {
            TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(context.clientContext);
            TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            TermGroup termGroup = termStore.GetSiteCollectionGroup(context.clientContext.Site, false);
            TermSet termSet = termGroup.TermSets.GetByName(Constants.termSetName);

            context.clientContext.Load(termSet);
            context.clientContext.ExecuteQuery();

            return termSet; 
        }

        public TermStore GetTermStore(Context context)
        {
            TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(context.clientContext);
            TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();  
            context.clientContext.Load(termStore);
        
            context.clientContext.ExecuteQuery();

            return termStore; 
        }

        public void CreateTerm(Context context, string department)
        {
            TermSet termSet = GetTermSet(context); 
            Term newTerm = termSet.CreateTerm(department.ToUpper(), Constants.lcid, Guid.NewGuid());

            TermStore termStore = GetTermStore(context);
            termStore.CommitAll();
            context.clientContext.Load(GetTermCollection(context));
            context.clientContext.ExecuteQuery();
        }
    }
}
