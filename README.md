# Social Network - Red Social con Battleship

## Descripción

Una red social completa desarrollada en ASP.NET Core MVC que permite a los usuarios conectarse, compartir contenido y jugar al clásico juego de Battleship. La aplicación incluye un sistema completo de autenticación, gestión de amistades, publicaciones multimedia y un juego interactivo integrado.

## Características Principales

### 🔐 Autenticación y Seguridad
- Sistema de registro con activación por correo electrónico
- Login seguro con validación de cuentas activas
- Recuperación de contraseña con tokens temporales
- Protección de rutas con autorización

### 📱 Gestión de Usuarios
- Perfiles personalizables con foto
- Edición de información personal
- Sistema de amistades con solicitudes
- Visualización de amigos comunes

### 📝 Publicaciones y Contenido
- Creación de publicaciones con texto e imágenes
- Integración con videos de YouTube (reproducción embebida)
- Sistema de comentarios anidados (replies)
- Reacciones "Me gusta" y "No me gusta"
- Edición y eliminación de contenido propio

### 🎮 Juego Battleship Integrado
- Partidas multijugador entre amigos
- Tablero 12x12 con posicionamiento estratégico de barcos
- Sistema de turnos en tiempo real
- Historial completo de partidas
- Estadísticas de victorias y derrotas

### 🤝 Sistema de Amistades
- Envío y gestión de solicitudes de amistad
- Visualización de publicaciones de amigos
- Eliminación de amistades
- Feed personalizado por amistad

## Tecnologías Utilizadas

### Backend
- **ASP.NET Core MVC** (.NET 8/9)
- **Entity Framework Core** (Code-First)
- **ASP.NET Core Identity** (Autenticación y autorización)
- **AutoMapper** (Mapeo de objetos)
- **SMTP** (Envío de correos electrónicos)

### Frontend
- **Bootstrap** (Framework CSS responsivo)
- **JavaScript** (Interactividad)
- **HTML5/CSS3** (Estructura y estilos)
- **YouTube API** (Integración de videos)

### Arquitectura
- **Arquitectura Onion** (Separación de responsabilidades)
- **Repository Pattern** (Repositorios genéricos)
- **Service Layer** (Servicios genéricos)
- **DTO Pattern** (Transferencia de datos)
- **ViewModels** (Validaciones y presentación)

## Estructura del Proyecto

```
SocialNetwork/
├── Core/
│   ├── Domain/              # Entidades y modelos de dominio
│   └── Application/         # Servicios y DTOs
├── Infrastructure/
│   ├── Data/               # Contexto EF y repositorios
│   └── Shared/             # Servicios compartidos (correo)
├── Presentation/
│   └── Web/                # Controladores y vistas MVC
└── README.md
```

## Funcionalidades Detalladas

### Sistema de Publicaciones
- **Creación**: Texto + imagen o texto + video de YouTube
- **Interacción**: Comentarios, replies anidados, reacciones
- **Gestión**: Edición y eliminación de contenido propio
- **Visualización**: Feed cronológico personalizado

### Battleship Game
- **Configuración**: Posicionamiento de 5 barcos de diferentes tamaños
- **Gameplay**: Sistema de turnos alternados con validaciones
- **Controles**: Tablero interactivo 12x12 con feedback visual
- **Resultados**: Historial completo con estadísticas detalladas

### Gestión de Amistades
- **Solicitudes**: Envío, aceptación y rechazo de solicitudes
- **Restricciones**: Prevención de solicitudes duplicadas
- **Visualización**: Amigos comunes y perfiles públicos
- **Interacción**: Invitaciones a jugar y gestión de amistades

## Instalación y Configuración

### Prerrequisitos
- .NET 8 o 9 SDK
- SQL Server (LocalDB o instancia completa)
- Visual Studio 2022 o VS Code

### Configuración
1. Clona el repositorio
2. Configura la cadena de conexión en `appsettings.json`
3. Configura el servicio SMTP para envío de correos
4. Ejecuta las migraciones: `dotnet ef database update`
5. Ejecuta la aplicación: `dotnet run`

## Arquitectura Técnica

### Capas de la Arquitectura Onion
- **Domain**: Entidades de negocio y interfaces
- **Application**: Servicios, DTOs y lógica de aplicación
- **Infrastructure**: Implementación de repositorios y servicios externos
- **Presentation**: Controladores, vistas y ViewModels

### Patrones Implementados
- **Repository Pattern**: Acceso a datos genérico
- **Service Layer**: Lógica de negocio encapsulada
- **DTO Pattern**: Transferencia de datos optimizada
- **MVVM**: Separación entre vista y lógica

## Seguridad

- Autenticación basada en Identity
- Autorización por roles y claims
- Validación de entrada en todos los formularios
- Protección CSRF integrada
- Sanitización de contenido HTML

## Características de UX/UI

- **Responsive Design**: Compatible con dispositivos móviles
- **Interfaz Intuitiva**: Navegación clara y accesible
- **Feedback Visual**: Indicadores de estado y confirmaciones
- **Optimización**: Carga eficiente de contenido multimedia

## Contribución

Este proyecto fue desarrollado como parte del programa académico del Instituto Tecnológico de Las Américas (ITLA) siguiendo las mejores prácticas de desarrollo de software.

## Licencia

© ITLA 2025 - Proyecto Académico

---

**Desarrollado con ❤️ usando ASP.NET Core MVC y Arquitectura Onion**
