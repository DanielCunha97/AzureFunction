# AzureFunction
This is an Azure Function that insert logs into an Azure sql server. 
We can insert logs from two ways:
  - By Request Body: 
          - https://azurefunctionapp20211111105214.azurewebsites.net/api/HttpExample
          - {  
                "Log": "Daniel this is my online test Log"  
            }
   - By query parameter
           - https://azurefunctionapp20211111105214.azurewebsites.net/api/HttpExample?log="Daniel test online log"
