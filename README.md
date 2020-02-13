# azure_test_project
This is a Finnish test project

# Remarks

First build, please build all project in the common folder and than you can build the whole solution.

Unfortunatelly I was ill I cannot finish the project!

Some parts are missing
- The service not check how many attributes has the email address
- SendGrid api is not implemented
  - If I would implement I would use Hangfire component and it would send the emails in the background
- No error handling
  - Elmah, log4net
- My idea was that the user set some azure specific variables and run the installer from Powershell and the azure environment was created automatically
  - You can see what I created in this folder : src\Azure.Deployment
- I am testing the web api from swagger

About the solution structure
- The solution has several projects, my idea was that I create 3 layers of structure and every project has responsibility and knowledge.
- I found that with this solution, the project would be much more manageable in the future and the tasks would be well separated.


Before run, please check the Azure.TestProject.WebApi web.config file! There you can set the azure specific values



