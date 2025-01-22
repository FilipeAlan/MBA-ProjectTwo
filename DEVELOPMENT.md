  # Notas de apoio para desenvolvedores

## Geração automática do API Client no build do projeto PCF.API

1. Instalar: 
```
dotnet tool install -g Swashbuckle.AspNetCore.Cli
```

2. Alterar a configuração corrente para `ApiClientGenerator`
3. Realizar build do projeto
4. O arquivo será gerado na pasta `\src\PCF\PCF.SPA\Services\WebApiClient.cs`
5. As especificações de geração estão no arquivo `\docs\nswag\nswag.json`

Documentação: 

https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#retrieve-swagger-directly-from-a-startup-assembly 

https://github.com/RicoSuter/NSwag/wiki/NSwag.MSBuild


## EF Core: Geração das migrations 
```
Add-Migration NAME -verbose -Context PCFDBContext -Project PCF.Core -Startup PCF.API
```