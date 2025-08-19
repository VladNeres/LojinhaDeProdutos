## **Sobre o Projeto**

Inicialmente criei uma API básica para gerenciar **categorias**, **subcategorias**, **produtos** e **centro de distribuição** usando **MySQL** como banco de dados.
---

##  **Principais Tecnologias**

- **.NET 8**
- **C#**
- **EntityFrameWork**
- **SQL Server Express**
- **ASP.NET Core Identity**
- **Serilog** com sink para banco de dados
- **NSubistitute + xUnit + Moq**

---

##  **Principais Funcionalidades**

**Cadastrar ,Atualizar, Buscar e  Excluir ( Categoria, SubCategorias e Produtos)**  

- **Logs estruturados**
  - Toda ação relevante é logada (Controller + Service)
 
---
##  Tabelas e cardinalidades  ##

<img width="1181" height="422" alt="image" src="https://github.com/user-attachments/assets/750101b2-703c-455f-84e5-2e333e5fcc68" />

## **Como rodar**

**Clone o repositório:**
```bash
git clone https://github.com/VladNeres/LojinhaDeProdutos.git
```
- Configure sua connectionString no arquivo appsetigns. (deixei ela exposta, apenas a criterio de estudos)
- Após configurar a sua connection, entre no seu MSSQL e crie um banco de acordo com o nome que voce criou no arquivo appsettings, ou caso queira manter a mesma connection,
  Apenas crie um banco Ecommerce no seu SQL server.

 -Caso esteja utilizando o visual Studio para desenvolver o projeto abra o (""power shell do desenvolvedor"") -> aperte as teclas  ( CTRL + ' )

-  Navegue até a sua pasta principal onde o projeto está atraves do comando (**cd NomeDaPasta**) para entrar na pasta  Ou (**Cd..**) para retornar a pasta anterior


Vou deixar imagens ilustrando o caminho ao qual voce pode pegar o caminho onde vai colocar no comando para realizar a criação do banco 
 1 imagem <img width="486" height="200" alt="image" src="https://github.com/user-attachments/assets/493817e2-726e-478a-8d19-b422bda75b4f" />
 2 imagem <img width="458" height="354" alt="image" src="https://github.com/user-attachments/assets/1754ccae-5db4-4a64-bd3b-99a529a31f67" />

- Digite o seguinte comando colocando o caminho da suas pastas
**dotnet ef migrations add CriacaoEstruturaBanco --project "Caminho1magem\DataAccess.csproj" --startup-project "Caminho2imagem\AplicacaoProjeto.csproj"**

- Apos realizar esse comando de migration voce precisa efetivar a atualizacao no banco, então coloque o seguinte comando

- **dotnet ef database update --project "Caminho1imagem\DataAccess.csproj" --startup-project "caminho2imagem\AplicacaoProjeto.csproj"**

- Feito isso as tabelas no banco serão criadas automaticamente.


