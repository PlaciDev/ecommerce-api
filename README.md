# 🛒 EcommerceApi

API RESTful para gerenciamento de um sistema de e-commerce. Oferece funcionalidades para administração de usuários, produtos, categorias, clientes, pedidos, pagamentos e autenticação.

## Implementações

- Cache em memória
- Cadastro e login de usuários
- Atribuição e gerenciamento de papéis (roles)
- Autenticação com JWT
- Ocultação de credenciais senvíveis (SMTP, JWT e KEY API)
- Documentação via Swagger
- Padrão RESTful
- Paginação

---

## Endpoints Principais

### Users

| Método | Rota                  | Descrição                       |
|--------|------------------------|----------------------------------|
| GET    | `/api/users`           | Lista todos os usuários          |
| POST   | `/api/users`           | Cria um novo usuário             |
| GET    | `/api/users/{id}`      | Busca usuário por ID             |
| PUT    | `/api/users/{id}`      | Atualiza um usuário              |
| DELETE | `/api/users/{id}`      | Remove um usuário                |
| POST   | `/api/users/login`     | Login de usuário                 |
| POST   | `/api/users/register`  | Registro de novo usuário         |

---

### Customers

| Método | Rota                        | Descrição                      |
|--------|-----------------------------|-------------------------------|
| GET    | `/api/customers`            | Lista todos os clientes        |
| POST   | `/api/customers/register`   | Cria novo cliente              |
| POST   | `/api/customers/login`      | Login de cliente               |
| GET    | `/api/customers/{id}`       | Busca cliente por ID           |
| PUT    | `/api/customers/{id}`       | Atualiza cliente               |
| DELETE | `/api/customers/{id}`       | Remove cliente                 |

---

### Products

| Método | Rota                  | Descrição                       |
|--------|------------------------|----------------------------------|
| GET    | `/api/products`        | Lista produtos com paginação    |
| POST   | `/api/products`        | Cria novo produto               |
| GET    | `/api/products/{id}`   | Detalhes de um produto          |
| PUT    | `/api/products/{id}`   | Atualiza um produto             |
| DELETE | `/api/products/{id}`   | Remove um produto               |

---

###  Categories

| Método | Rota                             | Descrição                                 |
|--------|----------------------------------|--------------------------------------------|
| GET    | `/api/categories`                | Lista todas as categorias                  |
| POST   | `/api/categories`                | Cria nova categoria                        |
| GET    | `/api/categories/with-products`  | Lista categorias com seus produtos         |
| GET    | `/api/categories/{id}`           | Busca categoria por ID                     |
| PUT    | `/api/categories/{id}`           | Atualiza categoria                         |
| DELETE | `/api/categories/{id}`           | Remove categoria                           |

---

### Orders

| Método | Rota                      | Descrição                       |
|--------|---------------------------|----------------------------------|
| GET    | `/api/orders`             | Lista todos os pedidos           |
| POST   | `/api/orders`             | Cria novo pedido                 |
| GET    | `/api/orders/{id}`        | Detalhes de um pedido            |
| POST   | `/api/orders/{id}/cancel` | Cancela o pedido especificado    |

---

### Roles

| Método | Rota               | Descrição                     |
|--------|--------------------|-------------------------------|
| GET    | `/api/roles`       | Lista todas as funções (roles)|
| POST   | `/api/roles`       | Cria nova função              |
| GET    | `/api/roles/{id}`  | Detalha uma função            |
| PUT    | `/api/roles/{id}`  | Atualiza uma função           |
| DELETE | `/api/roles/{id}`  | Remove uma função             |
