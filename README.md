# GD Solutions API

## 📌 Sobre o Projeto

A GD Solutions desenvolve soluções voltadas para modernizar a gestão de pessoas e apoiar empresas na transição para o Futuro do Trabalho, um cenário marcado por digitalização, trabalho híbrido e uso intensivo de dados para tomada de decisão.

Esta API tem como objetivo oferecer uma base estruturada para o gerenciamento de funcionários e departamentos, permitindo que sistemas corporativos realizem operações de forma organizada, segura e escalável. Ela segue boas práticas REST, utiliza versionamento de API e emprega tecnologias modernas para garantir flexibilidade na evolução do sistema.

A GD Solutions API foi projetada para ser um ponto central de integração entre diferentes aplicações internas, como ferramentas de RH, dashboards de desempenho, plataformas de People Analytics e módulos administrativos. Com uma arquitetura limpa e orientada a serviços, a API facilita a automação de processos, melhora a qualidade dos dados e apoia estratégias de transformação digital.

---

## ⚙️ O que a API entrega

- ✔️ Cadastro, consulta e gerenciamento completo de Funcionários  
- ✔️ Administração estruturada de Departamentos  
- ✔️ Versionamento inteligente (v1 e v2) para evoluções futuras  
- ✔️ Paginação, filtros e atualizações parciais (PATCH)  
- ✔️ Documentação completa via Swagger/OpenAPI  
- ✔️ Persistência usando Entity Framework Core + MySQL  
- ✔️ Arquitetura robusta com inversão de dependência e separação de responsabilidades  

🔹 **Extensível**  
Permitindo crescimento com novas versões da API sem quebrar integrações existentes.

🔹 **Automatizado**  
Com migrations, validações e controle de entidades.

🔹 **Adaptável**  
Pensado para integrações com sistemas de IA, rotinas de análise de desempenho, gestão de habilidades, entre outros módulos corporativos.

---

## 🚀 Início Rápido

### Pré-requisitos
- .NET 9.0+  
- MySQL 8.0+  
- Git

### Instalação Completa

#### 1️⃣ Clonar o Repositório  
```bash
git clone https://github.com/RodrigooL10/GlobalSolution-CSharp.git
cd GlobalSolution-CSharp
```

#### 2️⃣ Acessar a Pasta da API  
```bash
cd FuturoDoTrabalho.Api
```

#### 3️⃣ Restaurar Dependências  
```bash
dotnet restore
```

#### 4️⃣ Criar Banco de Dados  
```bash
mysql -u root -padmin12@ -e "CREATE DATABASE futuro_trabalho CHARACTER SET utf8mb4;"
```
**Nota:** Se sua senha MySQL é diferente, substitua `admin12@` pela sua senha. Mude essa senha também nos arquivos `appsettings.Development.json` e `appsettings.json`.

#### 5️⃣ Criar e Aplicar Migrations  
```bash
dotnet ef migrations add Initial 

dotnet ef database update
```

#### 6️⃣ Executar a API  
```bash
dotnet run
```

#### 7️⃣ Acessar o Swagger  
```
http://localhost:5015
```

---

## 🧱 Estrutura do Projeto

```
FuturoDoTrabalho.Api/
├── Controllers/
│   ├── v1/
│   └── v2/
│
├── Services/
├── Repositories/
├── Models/
├── DTOs/
├── Data/
├── Mappings/
├── Migrations/
├── Program.cs
├── appsettings.json
└── README.md
```

---

## 📚 Versões da API

### v1 — Básica
Endpoints: `/api/v1/funcionario` e `/api/v1/departamento`

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/` | Listar todos |
| GET | `/{id}` | Obter um |
| POST | `/` | Criar |
| PUT | `/{id}` | Atualizar |
| DELETE | `/{id}` | Deletar |

### v2 — Avançada

Inclui tudo da v1 e mais:

- PATCH parcial  
- Paginação (`pageNumber`, `pageSize`)  

---

## 📌 Exemplos de Uso

### Listar Funcionários (v1)
```bash
curl http://localhost:5015/api/v1/funcionario
```

### Criar Funcionário
```bash
curl -X POST http://localhost:5015/api/v1/funcionario   -H "Content-Type: application/json"
```

---

## 🗄️ Banco de Dados

**Banco:** `futuro_trabalho_dev`

### Tabelas
- funcionarios  
- departamentos  

---

## 🛠️ Comandos Úteis

```bash
dotnet run
dotnet build
dotnet ef migrations add NomeMigracao
dotnet ef database update
dotnet ef migrations remove
```

---

## 🧬 Arquitetura

### Tecnologias
- .NET 9  
- ASP.NET Core  
- MySQL  
- AutoMapper  
- API Versioning  
- Swagger  

### Padrões
- Repository Pattern  
- Service Pattern  
- DTO Pattern  
- Dependency Injection  
- API Versioning  

### Fluxo
```
Request → Controller → Service → Repository → DbContext → MySQL → Response
```

### Status Codes
| Código | Significado |
|--------|------------|
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 404 | Not Found |
| 500 | Server Error |

---

## 🧑‍💻 Integrantes

- Adriano Lopes - RM98574  
- Henrique de Brito - RM98831  
- Rodrigo Lima - RM98326  
