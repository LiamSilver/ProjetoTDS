# ProjetoTDS

# Aplicativo de Chat e Mensagens (em desenvolvimento)

## Descrição
Este é um aplicativo de chat e mensagens em tempo real, que permite aos usuários se comunicarem através de troca de mensagens de texto e envio de arquivos. O aplicativo implementa recursos essenciais de mensageria, como cadastro de usuários, gerenciamento de contatos e notificações para novas mensagens, com persistência de dados utilizando ADO.NET e EF Core.

## Funcionalidades
- **Cadastro de usuários**: Permite o registro de novos usuários e adição de contatos para comunicação.
- **Envio de mensagens**: Suporte ao envio de mensagens de texto e anexação de arquivos.
- **Notificações em tempo real**: Exibição de notificações quando novas mensagens são recebidas.
- **Persistência de conversas**: Armazenamento das conversas utilizando ADO.NET e EF Core para garantir a permanência dos dados entre sessões.
  
## Conceitos Técnicos
- **Polimorfismo**: Implementado para diferentes tipos de mensagens (ex.: texto, arquivos).
- **Padrões de Projeto**: Utilização do padrão Singleton para o gerenciamento de sessões de usuários e mensagens.
- **Interface Gráfica**: Desenvolvimento de uma interface intuitiva para facilitar a navegação e interação do usuário.
- **UML**: Diagramas UML foram utilizados durante a fase de planejamento e design do sistema.

## Tecnologias Utilizadas
- **ADO.NET**: Para persistência e acesso aos dados.
- **Entity Framework Core**: Para facilitar o mapeamento objeto-relacional (ORM).
- **C#**: Linguagem de programação principal.
- **Padrão Singleton**: Usado para gerenciar a sessão do usuário.
- **Padrão MVC**: Estrutura do projeto para separação das responsabilidades.
  
