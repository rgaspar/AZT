using Azure.TestProject.DataTransfer.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Repositories.Blob
{
    public class EmailAttributesRepositoryBlob
    {
        public EmailAttributesRepositoryBlob()
        {
        }

        public void SaveEmailAttributeToBlob(EmailAttribute emailAttribute)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobContainer container = cloudStorageAccount.CreateCloudBlobClient().GetContainerReference(ConfigurationManager.AppSettings["BlobContainerName"]);

            string fileName = $"EmailAttribute{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt";
            string fileName2 = $"EmailAttribute{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt";

            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            if (!blob.Exists())
            {
                blob.UploadFromStream(new MemoryStream());
            }
            else
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        foreach (var attribute in emailAttribute.Attributes)
                        {
                            blob.DownloadToStream(ms);
                            byte[] dataToWrite = Encoding.UTF8.GetBytes($"Key:{emailAttribute.Key}, Email:{emailAttribute.Email}, Attribute:{attribute}\r\n");
                            ms.Write(dataToWrite, 0, dataToWrite.Length);
                            ms.Position = 0;
                            blob.UploadFromStream(ms);

                        }
                    }
                }
                catch (StorageException excep)
                {
                    if (excep.RequestInformation.HttpStatusCode != 404)
                    {
                        throw;
                    }
                }
            }

        }
    }
}
