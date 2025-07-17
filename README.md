# InfoneticaTask - Workflow Management System

A lightweight **workflow management system** built with **.NET 8.0 Minimal API**, providing features for workflow definition, instance management, state transitions, and history tracking.

---

## ✅ Technologies Used
- **.NET 8.0** - Modern, fast framework
- **ASP.NET Core Minimal API** - For lightweight REST API
- **C#** - Primary programming language
- **Swagger (Swashbuckle)** - Interactive API documentation

---

## ✅ Features
- **Workflow Definition Management** - Create and retrieve workflows
- **Workflow Execution** - Start instances and perform actions
- **State Transitions** - Configurable actions between states
- **History Tracking** - Keep track of transitions for audit
- **RESTful API** - Simple, clean endpoints with Swagger UI

---

## ✅ Project Structure
InfoneticaTask/
├── Models/
│ ├── State.cs # Workflow state model
│ ├── ActionTransition.cs # Actions connecting states
│ ├── WorkflowDefinition.cs # Workflow template
│ ├── WorkflowInstance.cs # Instance of workflow
│ └── HistoryEntry.cs # Transition history
├── Services/
│ └── WorkflowService.cs # Core business logic
├── Program.cs # Application entry point
└── InfoneticaWorkflow.csproj # Project configuration


---

## ✅ API Endpoints

### **Workflows**
| Method | Endpoint         | Description                   |
|--------|------------------|-------------------------------|
| POST   | `/workflows`     | Create a new workflow        |
| GET    | `/workflows`     | Get all workflows            |
| GET    | `/workflows/{id}`| Get a workflow by ID         |

### **Workflow Instances**
| Method | Endpoint                   | Description                    |
|--------|---------------------------|--------------------------------|
| POST   | `/instances/{workflowId}`| Start a workflow instance      |
| GET    | `/instances`             | Get all instances             |
| GET    | `/instances/{id}`        | Get instance details & history|

### **Execution**
| Method | Endpoint                                   | Description                 |
|--------|-------------------------------------------|-----------------------------|
| POST   | `/instances/{instanceId}/actions/{actionId}`| Execute an action          |

---

## ✅ Setup Instructions

### Prerequisites
- Install **.NET 8.0 SDK** → [Download here](https://dotnet.microsoft.com/en-us/download)

### Installation & Run
```bash
# Clone the repository
git clone <repository-url>
cd InfoneticaTask

# Restore dependencies
dotnet restore

# Add Swagger support (if not installed)
dotnet add package Swashbuckle.AspNetCore --version 6.5.0

# Build and run
dotnet build
dotnet run
```

## Access API & Documentation

    API Base URL: http://localhost:5153

    Swagger UI: http://localhost:5153/swagger