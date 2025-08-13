## **Sobre o Projeto**

Inicialmente criei uma API básica para gerenciar **categorias**, **subcategorias**, **produtos** e **centro de distribuição** usando **MySQL** como banco de dados.
---

##  **Principais Tecnologias**

- **.NET 8**
- **C#**
- **Dapper**
- **SQL Server Express**
- **RabbitMQ**
- **ASP.NET Core Identity**
- **Serilog** com sink para banco de dados
- **MSTest + xUnit + Moq**

---

##  **Principais Funcionalidades**

- **Cadastrar Categoria**  
  - Validações de nome (tamanho, caracteres especiais)
  - Armazena data de criação e modificação
- **Buscar Categorias**
  - Filtros dinâmicos por ID, Nome, Status
  - Ordenação dinâmica (ASC/DESC) validada na Service
- **Logs estruturados**
  - Toda ação relevante é logada (Controller + Service)
 
---

## **Como rodar**

**Clone o repositório:**
```bash
git clone https://github.com/VladNeres/LojinhaDeProdutos.git
```
- Configure sua connectionString via Secrets.
- Execute a criação da tabela e após isso as procedures na pasta DataAccess > StoredProcedures
- Execute o projeto. 
