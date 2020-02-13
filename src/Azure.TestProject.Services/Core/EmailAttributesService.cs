using Autofac.Features.Indexed;
using AutoMapper;
using Azure.TestProject.Repositories.Contract;
using Azure.TestProject.Services.Contracts;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DTO = Azure.TestProject.DataTransfer.Core;
using BLL = Azure.Test.Project.Domain.Models.Core;
using Azure.TestProject.Repositories.AzureStorage.Core;
using System.Diagnostics;
using Azure.TestProject.Repositories.Blob;

namespace Azure.TestProject.Services
{
    public class EmailAttributesService : AZTService, IEmailAttributesService
    {
        private readonly IEmailAttribureRepository emailAttribureRepository;
        private readonly EmailAttributeRepositoryAS emailAttributeRepositoryAS;
        private readonly EmailAttributesRepositoryBlob emailAttributesRepositoryBlob;

        public EmailAttributesService(
            IEmailAttribureRepository emailAttribureRepository,
            IIndex<string, IUnitOfWork> unitsOfWork,
            IMapper mapper
        )
            : base(unitsOfWork, mapper)
        {
            this.emailAttribureRepository = emailAttribureRepository;
            emailAttributeRepositoryAS = new EmailAttributeRepositoryAS();
            emailAttributesRepositoryBlob = new EmailAttributesRepositoryBlob();
        }

        public async Task DoExecution(DTO.EmailAttribute emailAttribute)
        {
            SaveDataToSQLDB(emailAttribute);
            SaveAzureStorage(emailAttribute);
            SaveEmailAttributeToBlob(emailAttribute);
        }

        #region SQL
        private async void SaveDataToSQLDB(DTO.EmailAttribute emailAttribute)
        {
            try
            {
                if (isValidToSqlDB(emailAttribute))
                {
                    BLL.EmailAttribute bllEmailAttribute = Mapper.Map<BLL.EmailAttribute>(emailAttribute);

                    foreach (string attribute in emailAttribute.Attributes)
                    {
                        bllEmailAttribute.Attribute = attribute;
                        await emailAttribureRepository.CreateAsync(bllEmailAttribute);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveDataToSQLDB error :{ex.Message}");
            }
        }

        private bool isValidToSqlDB(DTO.EmailAttribute emailAttribute)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(emailAttribute.Key))
            {
                isValid = false;
            }
            else if (String.IsNullOrEmpty(emailAttribute.Email))
            {
                isValid = false;
            }
            else if (emailAttribute.Attributes == null)
            {
                isValid = false;
            }
            else if (emailAttribute.Attributes.Length == 0)
            {
                isValid = false;
            }
            else
            {
                /*isValid = Regex.IsMatch(emailAttribute.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);*/
            }

            return isValid;
        }

        #endregion

        #region Azure Table Storage
        private void SaveAzureStorage(DTO.EmailAttribute emailAttribute)
        {
            try
            {
                emailAttributeRepositoryAS.SaveToAzureStorage(emailAttribute);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveAzureStorage error :{ex.Message}");
            }
        }
        #endregion

        #region Azure Blob Storage
        private void SaveEmailAttributeToBlob(DTO.EmailAttribute emailAttribute)
        {
            try
            {
                emailAttributesRepositoryBlob.SaveEmailAttributeToBlob(emailAttribute);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveEmailAttributeToBlob error :{ex.Message}");
            }
        }
        #endregion
    }
}
