[![.NET](https://github.com/FilipeAlan/MBA-ProjectTwo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/FilipeAlan/MBA-ProjectTwo/actions/workflows/dotnet.yml)

# **MBA-ProjectTwo - Plataforma de Controle Financeiro Pessoal**

## **1. Apresentação**

Bem-vindo ao repositório do **MBA-ProjectTwo - Plataforma de Controle Financeiro Pessoal**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Desenvolvimento Full-Stack Avançado com
ASP.NET Core**.
O objetivo principal desenvolver uma plataforma de Controle Financeiro Pessoal web full-stack
projetada para ajudar usuários a gerenciar suas finanças de forma eficiente e organizada. A solução deve oferecer um painel integrado para registro de transações financeiras, relatórios interativos, e ferramentas de planejamento
financeiro, garantindo segurança, usabilidade e escalabilidade.

### **Autores**
- **Eduardo Gimenes**
- **Filipe Alan Elias**
- **Jonatas Cruz**
- **Joseleno Santos** 
- **Leandro Andreotti** 
- **Paulo Cesar Carneiro**


## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação SPA:** Interface web para interação com a plataforma.
- **API RESTful:** Exposição dos recursos da plataforma para integração com aplicação SPA.
- **Autenticação e Autorização:** Implementação de controle de acesso, diferenciando administradores e usuários comuns.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQL Server e SQLite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Blazor
  - MudBlazor, HTML/CSS para estilização básica
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:


- src/
  - PCF.SPA/ - Projeto Blazor
  - PCF.Api/ - API RESTful
  - PCF.Core/ - Modelos de Dados e Configuração do EF Core
- tests/ - Testes unitários e de integração
     
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD completo para categorias de transações financeiras:** Permite criar, editar, visualizar e excluir categorias.
- **CRUD completo para orçamentos:** Permite criar, editar, visualizar e excluir orçamentos.
- **Relatórios e dashboards:** Permite visualizar de forma sintética e analítica das transações. 
- **Autenticação e Autorização:** Permitir registro de novos usuários com dados como nome, e-mail e senha.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.
- **Testes:** Unitários e de integração, validando os endpoints principais e regras de negócios.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 9.0 ou superior
- SQLite
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/FilipeAlan/MBA-ProjectTwo.git`
   - `cd MBA-ProjectTwo`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQLite.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a Aplicação Blazor:**
   - `cd src/PCF.SPA/`
   - `dotnet run`
   - Acesse a aplicação em: http://localhost:5049 ou https://localhost:7246

4. **Executar a API:**
   - `cd src/PCF.Api/`
   - `dotnet run`

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5084/swagger

## **9. Usuário padrão**

A primeira vez que o projeto é inicializado deve-se criar um usúario de acesso

