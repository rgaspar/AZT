using Azure.TestProject.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Data
{
    public class DbEntityValidationExceptionHandler : ExceptionHandler<DbEntityValidationException>
    {
        public override AZTException CreateAZTException(DbEntityValidationException ex)
        {
            int validationErrorCount = 0;

            string[] messages =
                ex.EntityValidationErrors
                    .SelectMany(
                        dbEntityValidationResult =>
                        dbEntityValidationResult.ValidationErrors.Select(
                            dbValidationError =>
                            {
                                ++validationErrorCount;

                                DbEntityEntry entityEntry = dbEntityValidationResult.Entry;
                                string entityTypeName = entityEntry.Entity.GetType().Name;
                                string propertyName = dbValidationError.PropertyName;

                                object originalValue =
                                    entityEntry.State != EntityState.Added && entityEntry.State != EntityState.Detached
                                        ? entityEntry.OriginalValues[propertyName]
                                        : null;

                                object currentValue =
                                    entityEntry.State != EntityState.Deleted && entityEntry.State != EntityState.Detached
                                        ? entityEntry.CurrentValues[propertyName]
                                        : null;


                                string errorMessage = dbValidationError.ErrorMessage;

                                return
                                    new StringBuilder()
                                        .AppendLine()
                                        .AppendLine($"{entityTypeName}.{propertyName}:")
                                        .AppendLine($"    OriginalValue=[{originalValue ?? "(null)"}]")
                                        .AppendLine($"    CurrentValue=[{currentValue ?? "(null)"}]")
                                        .AppendLine($"    ErrorMessage=[{errorMessage}]")
                                        .ToString();
                            }
                        )
                    )
                    .ToArray();

            int entitiesAffected = ex.EntityValidationErrors.Count();

            var newException =
                new AZTException(
                    String.Format(
                        "Validation failure: {0} {1}, {2} {3}.",
                        entitiesAffected,
                        entitiesAffected == 1 ? "entity" : "entities",
                        validationErrorCount,
                        validationErrorCount == 1 ? "error" : "errors"
                    ),
                    ExceptionMessageCollection.Create(messages)
                );

            return newException;
        }
    }
}
