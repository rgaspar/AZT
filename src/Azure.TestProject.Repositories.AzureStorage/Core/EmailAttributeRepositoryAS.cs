using Azure.TestProject.Data.AzureStorage.Models.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories.AzureStorage.Core
{
    public class EmailAttributeRepositoryAS
    {
        public EmailAttributeRepositoryAS()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            string tableName = ConfigurationManager.AppSettings["StorageTableName"];
            CloudTable cloudTable = tableClient.GetTableReference(tableName);
            createNewTable(cloudTable);

        }

        private void createNewTable(CloudTable cloudTable)
        {
            if (!cloudTable.CreateIfNotExists())
            {
                Debug.WriteLine("Table {0} already exists", cloudTable.Name);
                return;
            }
            Debug.WriteLine("Table {0} created", cloudTable.Name);
        }

        public void SaveToAzureStorage(Azure.TestProject.DataTransfer.Core.EmailAttribute emailAttribute)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
            string tableName = ConfigurationManager.AppSettings["StorageTableName"];
            CloudTable cloudTable = tableClient.GetTableReference(tableName);

            foreach (var attribute in emailAttribute.Attributes)
            {
                EmailAttributesAS emailAttributes = new EmailAttributesAS();
                emailAttributes.CreatedDateUtc = DateTime.Now;
                emailAttributes.Key = emailAttribute.Key;
                emailAttributes.Email = emailAttribute.Email;
                emailAttributes.Attribute = attribute;

                emailAttributes.AssignPartitionKey();
                emailAttributes.AssignRowKey();
                EmailAttributesAS emailAttributesASEntity = retrieveRecord(cloudTable, emailAttribute.Email, emailAttributes.CreatedDateUtc.ToString());
                if (emailAttributesASEntity == null)
                {
                    TableOperation tableOperation = TableOperation.Insert(emailAttributes);
                    cloudTable.Execute(tableOperation);
                    Debug.WriteLine("Record inserted");
                }
                else
                {
                    Debug.WriteLine("Record exists");
                }

            }

            DisplayTableRecords(cloudTable);
        }

        private EmailAttributesAS retrieveRecord(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<EmailAttributesAS>(partitionKey, rowKey);
            TableResult tableResult = table.Execute(tableOperation);
            return tableResult.Result as EmailAttributesAS;
        }

        private void DisplayTableRecords(CloudTable table)
        {
            TableQuery<EmailAttributesAS> tableQuery = new TableQuery<EmailAttributesAS>();
            foreach (EmailAttributesAS entity in table.ExecuteQuery(tableQuery))
            {
                Debug.WriteLine($"Date : {entity.CreatedDateUtc.ToString()}");
                Debug.WriteLine($"Key : {entity.Key}");
                Debug.WriteLine($"Email : {entity.Email}");
                Debug.WriteLine($"Attribute : {entity.Attribute}");
                Debug.WriteLine("******************************");
            }
        }
    }
}
