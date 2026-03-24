# 🌸 SGEI - Sistema de Gestão de Estética Integrado

O **SGEI** é uma solução moderna e minimalista para o gerenciamento de clínicas de estética, desenvolvida com o ecossistema **.NET 9**. O foco do projeto é oferecer uma experiência fluida para o cliente final e uma gestão ágil e centralizada para o administrador.

## 🚀 Tecnologias e Padrões
* **Framework:** Blazor Web App (.NET 9).
* **Modo de Renderização:** Interactive Server Mode para alta interatividade.
* **Arquitetura:** Camada de Serviços (Service Layer) e padrão MVVM.
* **Banco de Dados:** SQL Server via Entity Framework Core (Migrations).
* **Segurança:** * Autenticação baseada em Claims e Cookies.
    * Criptografia de senhas com BCrypt.
    * Validação de dados via Data Annotations e DTOs.
* **UI/UX:** Design minimalista, responsivo e focado em ações rápidas.

## ✨ Funcionalidades Principais
* **Gestão de Agenda Admin:** Painel administrativo com botões de ação direta (Confirmar/Concluir/Cancelar) sem necessidade de dropdowns.
* **Inteligência de Status:** O sistema atualiza automaticamente agendamentos passados para o status "Concluído" ao carregar a lista.
* **Segurança Robusta:** Formulários de Login e Registro protegidos, evitando a entrada de dados inválidos no banco.
* **Navegação Inteligente:** Menu lateral (Drawer) com scroll independente e rodapé de usuário/logout fixo para melhor usabilidade.
* **Área do Cliente:** Interface para agendamento de serviços e consulta de horários marcados.

## 🛠️ Como Executar o Projeto
1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/HigorGerman/SGEI-Estetica.git](https://github.com/HigorGerman/SGEI-Estetica.git)
    ```
2.  **Configure a String de Conexão:** No arquivo `appsettings.json`, ajuste a conexão com o seu banco de dados SQL Server.
3.  **Atualize o Banco de Dados:** No terminal do seu ambiente de desenvolvimento, execute:
    ```bash
    dotnet ef database update
    ```
4.  **Rode a aplicação:**
    ```bash
    dotnet run
    ```

## 👨‍💻 Autor
**Higor Germano Cerqueira**
* Estudante de Sistemas de Informação na FIPP (Faculdade de Informática de Presidente Prudente).
* Desenvolvedor focado em soluções robustas com C# e .NET.

---
*Desenvolvido como projeto de estudo focado em boas práticas de arquitetura e segurança.*
