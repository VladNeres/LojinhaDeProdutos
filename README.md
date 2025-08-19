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
- Configure sua connectionString via Secrets.
- Execute a criação da tabela e após isso as procedures na pasta DataAccess > StoredProcedures
- Execute o projeto. 
