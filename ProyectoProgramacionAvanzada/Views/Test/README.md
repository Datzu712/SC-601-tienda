# Demo de Módulos JavaScript

Esta vista de prueba demuestra el uso de los módulos JavaScript personalizados del proyecto.

## Módulos Demostrados

### 1. ToastManager (`toast.js`)
- Notificaciones tipo toast con diferentes tipos (success, error, warning, info)
- Configuración personalizable de duración y autohide
- Ocultar todos los toasts a la vez

### 2. ModalManager (`modals.js`)
- Modales de confirmación con diferentes variantes
- Callbacks para confirmación y cancelación
- Modales predefinidos: delete, warning, success
- Personalización completa de contenido y estilos

### 3. DataFetcher (`api.js`)
- Sistema reactivo que detecta cambios en el estado
- Reintentos automáticos en caso de error
- Callbacks para éxito y error
- Manejo de estado de carga

### 4. Theme Manager (`theme.js`)
- Alternancia entre modo claro y oscuro
- Persistencia en localStorage
- Detección de preferencias del sistema

## Cómo Acceder

Navega a: `/Test/Index`

## Dependencias Externas

- **Bootstrap 5.3.8**: Framework CSS para estilos y componentes
- **Font Awesome 7.0.1**: Iconos

## Endpoint de Prueba

- `GET /Test/GetTestData`: Endpoint que devuelve datos de prueba para demostrar el DataFetcher
  - Parámetros: `search` (string), `page` (int)
  - Respuesta: JSON con datos simulados

## Notas de Seguridad

- Todas las URLs se generan con `@Url.Action()` para evitar inyección
- No se manipula HTML directamente, se usa el DOM API
- Los módulos JS utilizan ES6 imports para mejor encapsulación

