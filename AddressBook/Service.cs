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
            List list = GetList(context, Constants.listName);

            ListItem newItem = list.AddItem(itemToAdd);

             FieldLookupValue lookup = new FieldLookupValue();
             lookup.LookupId = GetLookupListItemId(contact.Company, context);

            /**pass lookup id to update the lookup field value**/
             newItem[Constants.LookupCompany] = lookup.LookupId;

             newItem.Update();

            /**changing the managed term value to the suitable format: "-1;managedTermValue|guid" **/
            newItem[Constants.Department] = String.Concat("-1;#", contact.Department,
                                                 "|", GetTerm(contact.Department.ToUpper(), context).Id.ToString());

             newItem[Constants.FullName] = contact.Name;
             newItem[Constants.Email] = contact.Email;
             newItem[Constants.WorkAddress] = contact.Address;
             newItem[Constants.CellPhone] = contact.MobileNumber;
             newItem[Constants.MaritalStatus] = contact.MaritalStatus;
             newItem[Constants.Salary] = contact.Salary;
             newItem[Constants.DateOfBirth] = contact.DateOfBirth;
             newItem[Constants.Employed] = contact.Employed;

             newItem.Update();
             context.clientContext.ExecuteQuery();

             contact.Id = newItem.Id;
             return contact;
         }

         public Contact Update(int id, Contact contact, Context context)
         {
             ListItem itemToUpdate = GetItem(id, context);

            FieldLookupValue lookupValue = new FieldLookupValue();
            lookupValue.LookupId = GetLookupListItemId(contact.Company, context);
            itemToUpdate[Constants.LookupCompany] = lookupValue.LookupId;

            itemToUpdate.Update(); 

            itemToUpdate[Constants.Department] = "-1;#" + contact.Department + "|" + GetTerm(contact.Department.ToUpper(), context).Id.ToString();
            itemToUpdate[Constants.FullName] = contact.Name;
            itemToUpdate[Constants.CellPhone] = contact.MobileNumber;
            itemToUpdate[Constants.WorkAddress] = contact.Address;
            itemToUpdate[Constants.Email] = contact.Email;
            itemToUpdate[Constants.MaritalStatus] = contact.MaritalStatus;
            itemToUpdate[Constants.Salary] = contact.Salary;
            itemToUpdate[Constants.DateOfBirth] = contact.DateOfBirth;
            itemToUpdate[Constants.Employed] = contact.Employed.ToString();

            itemToUpdate.Update();

            context.clientContext.ExecuteQuery();

            /**getting the updated contact details**/
            contact = GetContact(id, context, contact);

            return contact;
        }

        public bool Delete(int id, Context context)
        {
            GetItem(id, context).DeleteObject();
            context.clientContext.ExecuteQuery();

            return true;
        }

        public Contact GetContact(int id, Context context, Contact contact)
        {
            ListItem item = GetItem(id, context);

            FieldLookupValue lookupCompanyValue = item[Constants.LookupCompany] as FieldLookupValue;
            contact.Company = lookupCompanyValue.LookupValue;

            contact.Id = item.Id;
            contact.Name = item[Constants.FullName].ToString();
            contact.MobileNumber = item[Constants.CellPhone].ToString();
            contact.Address = item[Constants.WorkAddress].ToString();
            contact.Email = item[Constants.Email].ToString();
            contact.MaritalStatus = item[Constants.MaritalStatus].ToString();
            contact.Salary = Convert.ToDouble(item[Constants.Salary].ToString());
            contact.DateOfBirth = Convert.ToDateTime(item[Constants.DateOfBirth].ToString());
            contact.Employed = Convert.ToBoolean(item[Constants.Employed].ToString());

            TaxonomyFieldValue taxonomyFieldValue = item[Constants.Department] as TaxonomyFieldValue;
            contact.Department = taxonomyFieldValue.Label;

            return contact;
        }

        public List<Contact> GetAllContacts(Context context)
        {
            ListItemCollection listItemCollection = GetListItemCollection(context,Constants.listName);

            foreach (ListItem item in listItemCollection)
            {
                TaxonomyFieldValue taxonomyFieldValue = item[Constants.Department] as TaxonomyFieldValue;
                FieldLookupValue LookupCompanyValue = item[Constants.LookupCompany] as FieldLookupValue;
                AllContacts.Add(new Contact(
                        item.Id,
                        item[Constants.FullName].ToString(),
                        item[Constants.CellPhone].ToString(),
                        item[Constants.WorkAddress].ToString(),
                        item[Constants.Email].ToString(),
                        item[Constants.MaritalStatus].ToString(),
                        Convert.ToDouble(item[Constants.Salary].ToString()),
                        Convert.ToDateTime(item[Constants.DateOfBirth].ToString()),
                        Convert.ToBoolean(item[Constants.Employed].ToString()),

                        LookupCompanyValue.LookupValue,  /**we can make this name generic**/
                        taxonomyFieldValue.Label
                    ));
            }
            return AllContacts;
        }

        /**Note: make this function name more generic**/
        public FieldChoice GetMaritalStatusChoices(Context context)
        {
            Field field = GetList(context,Constants.listName).Fields.GetByTitle(Constants.MaritalStatus);
            FieldChoice maritalStatusChoices = context.clientContext.CastTo<FieldChoice>(field);

            context.clientContext.Load(maritalStatusChoices);
            context.clientContext.ExecuteQuery();

            return maritalStatusChoices;
        }

        public ListItem GetItem(int id, Context context)
        {
            ListItemCollection listItemCollection = GetListItemCollection(context,Constants.listName);
            return listItemCollection.SingleOrDefault(item => item.Id.Equals(id));
        }

        public int GetLookupListItemId(string lookupFieldValue, Context context)
        {
            ListItemCollection lookupListItemCollection = GetListItemCollection(context, Constants.lookupListName);

            return lookupListItemCollection.SingleOrDefault(item => item[Constants.Title].Equals(lookupFieldValue)).Id;

        }

        public List GetList(Context context, string listName)
        {
            Web web = context.clientContext.Web;
            context.clientContext.Load(web); 

            List list = web.Lists.GetByTitle(listName);

            context.clientContext.Load(list);

            context.clientContext.ExecuteQuery();
            return list;
        }

        public ListItemCollection GetListItemCollection(Context context, string listName)
        {
            List list = GetList(context,listName);
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

        public bool doIdExist(int id, Context context)
        {
            return GetItem(id, context) != null ? true : false;
        }
    }
}
