# **MBA-ProjectTwo - Plataforma de Controle Financeiro Pessoal**

## **1. Apresentação**

Bem-vindo ao repositório do **MBA-ProjectTwo - Plataforma de Controle Financeiro Pessoal**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Desenvolvimento Full-Stack Avançado com
ASP.NET Core**.
O objetivo principal desenvolver uma plataforma de Controle Financeiro Pessoal web full-stack
projetada para ajudar usuários a gerenciar suas finanças de forma eficiente e organizada. A solução deve oferecer um painel integrado para registro de transações financeiras, relatórios interativos, e ferramentas de planejamento
financeiro, garantindo segurança, usabilidade e escalabilidade.

### **Autor(es)**
- **Filipe Alan Elias**

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
- **Banco de Dados:** SQL Server
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
  - CFP.SPA/ - Projeto Blazor
  - CFP.Api/ - API RESTful
  - CFP.Data/ - Modelos de Dados e Configuração do EF Core
     
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD completo para categorias de transações financeiras:** Permite criar, editar, visualizar e excluir categorias.
- **Autenticação e Autorização:** Permitir registro de novos usuários com dados como nome, e-mail,senha.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQL Server
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/seu-usuario/MBA-ProjectTwo.git`
   - `cd MBA-ProjectTwo`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQL Server.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a Aplicação MVC:**
   - `cd src/CFP.SPA/`
   - `dotnet run`
   - Acesse a aplicação em: http://localhost:5000

4. **Executar a API:**
   - `cd src/CFP.Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5001/swagger

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Usuário padrão**

A primeira vez que o projeto é inicializado deve-se criar um usúario de acesso

## **10. Canal do youtube com Testes**

TODO

