# Event Sourcing — Ticketing Microservices

Proyecto de práctica que implementa una arquitectura de **microservicios con CQRS y Event Sourcing**
para un sistema de gestión de tickets (soporte/incidencias), usando MongoDB como *Event Store*.

> 🚧 **Proyecto en desarrollo.** La estructura base (solución, proyectos, modelos de dominio y
> repositorio Mongo) ya está definida; los endpoints y casos de uso se están construyendo de forma
> incremental.

## ¿De qué se trata?

La idea central es separar las responsabilidades de **escritura** y **lectura** en dos servicios
independientes:

- **`Ticketing.Command`** — recibe las acciones que cambian el estado del sistema (crear un ticket,
  actualizarlo, etc.). En lugar de sobrescribir el estado actual, cada cambio se persiste como un
  **evento inmutable** en el *Event Store* (colección `eventStores` en MongoDB). El estado de un
  ticket se reconstruye reproduciendo su historial de eventos.
- **`Ticketing.Query`** — expone los datos ya proyectados/optimizados para lectura, sin tocar el
  modelo de escritura.

Este patrón permite tener un historial completo y auditable de todo lo que ocurrió (event sourcing),
desacoplar el modelo de lectura del de escritura (CQRS), y escalar cada lado de forma independiente.

## Estructura del proyecto

```
event-sourcing/
├── Common/
│   └── Common.Core/                 # Librería compartida entre microservicios
│       ├── Events/                  #   Eventos de dominio (BaseEvent, TicketCreatedEvent, ...)
│       └── Messages/                #   Contrato base de mensajes (Message)
│
├── Projects/
│   ├── Ticketing.Command/           # Servicio de escritura (Command side)
│   │   ├── Application/Models/      #   DTOs / settings (MongoSettings, ...)
│   │   ├── Domain/
│   │   │   ├── Abstracts/           #   Interfaces (IMongoRepository, ISession)
│   │   │   ├── Common/              #   Document base, atributos Bson
│   │   │   └── EventModels/         #   EventModel (documento del event store)
│   │   └── Infraestructure/
│   │       └── Repositories/        #   Implementación del repositorio Mongo
│   │
│   └── Ticketing.Query/             # Servicio de lectura (Query side)
│
├── docker-compose.yml               # MongoDB (con replica set, requerido para transacciones)
├── Microservices.slnx               # Solución (.slnx, formato nuevo de Visual Studio)
└── global.json                      # Pin del SDK de .NET
```

## Tecnologías

| Tecnología | Uso |
|---|---|
| **.NET 10** (C#) | Plataforma y lenguaje base |
| **ASP.NET Core Web API** | Hosting de los microservicios `Command` y `Query` |
| **MongoDB 7** | Persistencia — actúa como *Event Store* (colección `eventStores`) |
| **MongoDB.Driver / MongoDB.Bson** | Acceso a datos y mapeo de documentos |
| **Newtonsoft.Json** | Serialización |
| **Docker Compose** | Orquestación local de MongoDB (con *replica set* `rs0`, necesario para transacciones) |
| **OpenAPI (Microsoft.AspNetCore.OpenApi)** | Documentación de los endpoints |

## Cómo levantar el proyecto

1. **Requisitos**: [.NET 10 SDK](https://dotnet.microsoft.com/) (ver `global.json`) y Docker.

2. **Levantar MongoDB** (con replica set ya configurado):
   ```bash
   docker compose up -d
   ```

3. **Ejecutar los servicios**:
   ```bash
   dotnet run --project Projects/Ticketing.Command
   dotnet run --project Projects/Ticketing.Query
   ```

4. Cada servicio expone su documentación OpenAPI en modo desarrollo (`/openapi`).

## Conceptos clave

- **Event Store**: en vez de guardar el estado actual de un ticket, se guarda cada evento que le
  ocurrió (`TicketCreatedEvent`, etc.), con su `AggregateIdentifier`, `Version` y `Timestamp`. El
  estado se obtiene reproduciendo (*replaying*) esos eventos.
- **CQRS**: los modelos y flujos de escritura (`Command`) y lectura (`Query`) están completamente
  separados, incluso en servicios distintos.

## Roadmap

- [ ] Implementar los endpoints de creación/actualización de tickets en `Ticketing.Command`
- [ ] Completar el repositorio Mongo (transacciones, inserción de eventos)
- [ ] Implementar las proyecciones y endpoints de `Ticketing.Query`
- [ ] Comunicación entre servicios (mensajería/eventos)
