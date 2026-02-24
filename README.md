# 🏥 Turnos Clínica - Sistema de Gestión de Turnos Médicos

Aplicación web para la gestión de turnos médicos en consultorios/clínicas.

Permite administrar entidades principales (países, provincias, ciudades, consultorios, especialidades, estados, pacientes, médicos, turnos.) y registrar turnos con validaciones de negocio, incluyendo envío de notificaciones por email.

---

## 🚀 Estado del proyecto

### ✅ Versión actual: V1 (funcional)
La V1 se encuentra enfocada en la **lógica de negocio**, validaciones y flujo completo de registro de turnos.

### 🔜 Próxima versión: V2 (en planificación / desarrollo)
La V2 estará orientada a **mejoras visuales (UI/UX)**, experiencia de usuario y nuevas funcionalidades.

---

## ✨ Funcionalidades implementadas (V1)

### 📌 Gestión de entidades
- [x] ABM de Países
- [x] ABM de Provincias
- [x] ABM de Ciudades
- [x] ABM de Consultorios
- [x] ABM de Especialidades
- [x] ABM de Estados
- [x] ABM de Pacientes
- [x] ABM de Médicos
- [x] ABM de Turnos

### 📅 Gestión de turnos
- [x] Registro de turnos
- [x] Validaciones de negocio para turnos
- [x] Verificación de existencia de entidades relacionadas (paciente, médico, etc.)
- [x] Prevención de datos inválidos / inconsistentes
- [x] Manejo de errores y respuestas HTTP adecuadas

### 🛡️ Validaciones / Reglas de negocio
- [x] Validaciones de integridad en servicios
- [x] Validaciones antes de persistir datos
- [x] Soft delete (en todas las entidades)
- [x] Prevención de duplicados 

### 📧 Notificaciones por email
- [x] Implementación de `IEmailSender`
- [x] Envío de email al registrar turno
- [x] Integración del servicio de email con el flujo de negocio

### 🖥️ UI / Frontend (V1)
- [x] Pantallas funcionales para ABM
- [x] Formularios de carga/edición
- [x] Listados y navegación básica
- [x] Diseño funcional con Bootstrap

---

## 🧱 Arquitectura / Stack utilizado

### Backend / Lógica
- **.NET / ASP.NET Core**
- **Entity Framework Core**
- **API REST**
- **SQL Server**

### Frontend
- **Blazor Web App**
- **Bootstrap 5**

### Otros
- **Identity**
- **Servicio de Email (`IEmailSender`)**

---

## ✅ Objetivo de la V1

El objetivo principal de esta versión fue construir una base sólida de:
- **lógica de negocio**
- **validaciones**
- **estructura del sistema**
- **flujo real de turnos**

priorizando funcionalidad y consistencia antes que diseño avanzado.

---

## 🛣️ Roadmap V2 (checklist)

> La idea de esta versión es mejorar experiencia de usuario, diseño visual y sumar funcionalidades nuevas.

### 🎨 UI/UX y diseño
- [ ] Migrar UI actual a **MudBlazor**
- [ ] Mejorar diseño general (layout, tarjetas, formularios, tablas)
- [ ] Mejorar experiencia de usuario en creación de turnos
- [ ] Componentes reutilizables (modales, alerts, loaders, etc.)
- [ ] Mejorar responsive para mobile/tablet

### 📊 Dashboard / Métricas
- [ ] Dashboard con métricas principales
- [ ] Cantidad de turnos por día/semana/mes
- [ ] Turnos por médico / especialidad
- [ ] Estados de turnos (pendiente, confirmado, cancelado, etc.)

### 👨‍⚕️ Funcionalidades nuevas (ideas)
- [ ] Sistema de fichaje para empleados / médicos
- [ ] Panel por rol (Recepción / Admin / Médico)
- [ ] Mejoras de agenda / calendario
- [ ] Historial de turnos por paciente
- [ ] Notificaciones adicionales por email

### 🔐 Seguridad / autenticación
- [ ] Mejorar autenticación/autorización
- [ ] Roles y permisos más detallados
- [ ] Protección de vistas y endpoints por rol

### 🧪 Calidad / mantenimiento
- [ ] Tests unitarios de servicios
- [ ] Tests de integración para endpoints críticos
- [ ] Refactor de componentes UI
- [ ] Mejoras en manejo global de errores
- [ ] Logging / trazabilidad

---

## 📸 Capturas
> Podés agregar screenshots de la app acá cuando quieras mostrar UI.

### Home
<p align="center">
  <img src="docs/img/home.png" alt="Home" width="850" />
</p>
### Gestion de medicos
<p align="center">
  <img src="docs/img/medicos.png" alt="Medicos" width="850" />
</p>
### Registro de turnos
<p align="center">
  <img src="docs/img/turnos.png" alt="Turnos" width="850" />
</p>

---

## ⚙️ Cómo ejecutar el proyecto (local)

### Requisitos
- .NET SDK (versión X)
- SQL Server (si aplica)
- Visual Studio / VS Code

### Pasos generales
1. Clonar el repositorio
2. Configurar cadena de conexión en `appsettings.json`
3. Ejecutar migraciones (si aplica)
4. Ejecutar el proyecto

```bash
dotnet restore
dotnet build
dotnet run
