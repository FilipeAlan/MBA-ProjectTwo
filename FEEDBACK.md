# **Feedback 1a Entrega - Avaliação Geral e Recomendações**

## Front End

### Navegação

- Simples, mas objetiva

### Design

- Escolha assertiva pelo MudBlazor

### Funcionalidade

- Funcionalidades implementadas corretamente
- Componentes responsaveis, bem distribuidos

- Criticas:
    - O gráfico do histórico não é muito objetivo, poderia ser outro formato.
    - Ao ultrapassar o meu limite, poderia haver uma mensagem fixa no dashboard, o popup avisa, porém essa informação depois não fica disponível em outro lugar, assim criaria um sentido de alerta maior no lançamento das transações.
    - O relatório de gastos por categoria ficou "feio" com as linhas todas em vermelho
    - O popup de inclusão de transação poderia ter um valor em branco para o tipo (receita/despesa), evitaria induzir o usuario a lançar um tipo incorreto, obrigando ele a fazer a escolha no dropdown.
    - Algumas mensagens de erro de validação estão em inglês (formulário)


## Back End

### Arquitetura

- Distribuição de camadas adequadas
- Boa organização de pastas e configuração da aplicação

### Funcionalidade

- Funcionalidades implementadas corretamente
- Ótima distribuição de responsabilidades
- Bom tratamento de erros e validações no server`
- Criticas:
    - Não acredito ser interessante o ID do usuário como INT no Identity
    - Tratamento de erros do servidor está lançando a exception completa, não existe uma triagem de captura de erros e tratamento global.

### Modelagem

- Modelagem simples, porém funcional, de acordo com a complexidade
- Validações e fluxos de negócio tratados por serviços, tudo ok.

## Projeto

### Organização

- Organização de pastas e responsabilidades ok!

### Documentação

- Repositório e API bem documentados.

### Instalação

- Execução limpa, sem necessidade de setup de infra.