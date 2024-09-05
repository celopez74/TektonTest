# Proyecto Tekton.Products

Este proyecto es una aplicación basada en .NET 8.0 que implementa una arquitectura de Clean Code con el patrón CQRS (Command Query Responsibility Segregation) para el CRUD (Crear, Leer, Actualizar, Eliminar) de la entidad `Producto`. Se han utilizado otros patrones y técnicas, como Singleton para el acceso a la base de datos y Lazy Cache para el almacenamiento en caché.

## Arquitectura y Patrones

### Clean Code

El proyecto sigue los principios de Clean Code para asegurar un código limpio, legible y mantenible. Esto incluye el uso de nombres descriptivos, funciones pequeñas y la eliminación de código duplicado.

### CQRS

Se utiliza el patrón CQRS para separar las operaciones de lectura y escritura. Los comandos (Commands) manejan las operaciones de escritura, mientras que las consultas (Queries) manejan las operaciones de lectura.

### Singleton

El patrón Singleton se utiliza para garantizar que solo haya una instancia de las clases responsables del acceso a la base de datos, asegurando que el acceso a los recursos se maneje de manera eficiente.

### Lazy Cache

Se utiliza Lazy Cache para almacenar en caché los datos de manera perezosa, lo que mejora el rendimiento al evitar cálculos o accesos a datos redundantes. la informacón almacenada en Lazy Cache se mantienen con una duración de 5 minutos.

### Sqlite

Para el manejo de datos persistentes en la apliacación se utiliza SQLite junto con Entity Framework para facilitar el tratamiento de los datos.
El proyecto se carga a Github con la base de datos ya generada, por lo que la misma se encuentra en el proyecto Tekton.Products.Api y su nombre es products.db, adicionalmente en el proyeccto de infraestructura se puede encontrar la carpeta Migrations generada por Entity Framework a la hora de crear la base de datos, esta información se sube al repositorio para poder contar con una serie de Id de productos (tipo Guid) con valores de descuento en la api de MockApi.

### MockApi
La información correspondiente a los descuentos de los productos se obtiene de un api rest externo generada con MockApi, allí se tiene expuesto un Api-rest de tipo Get que permite consultar el descuento de un producto por medio del Id almacenado en la base de datos Sqlite.
los descuentos existentes en MockApi pueden ser consultados en: https://66d844bf37b1cadd80540f7f.mockapi.io/api/v1/product-discount


## Arquitectura Hexagonal

En el proyecto se ha implementado la Arquitectura Hexagonal, también conocida como Arquitectura de Puertos y Adaptadores. Este patrón de diseño permite un desacoplamiento efectivo entre el núcleo de la aplicación y las interfaces externas, como bases de datos, servicios web y otras dependencias. A continuación, se describen los componentes clave y cómo se estructuran en nuestra aplicación:

### Componentes Clave

1. **Núcleo de la Aplicación (Core)**
   - **Dominios y Entidades**: Contiene las entidades del dominio y la lógica de negocio central. Aquí es donde se definen las reglas y comportamientos que rigen la aplicación.
   - **Casos de Uso**: Representa las operaciones específicas que el sistema debe realizar, a menudo implementadas como comandos o consultas. 

2. **Puertos**
   - **Puertos de Entrada**: Interfaces que definen los casos de uso que el núcleo de la aplicación expone. Estos puertos son implementados por los servicios del dominio.
   - **Puertos de Salida**: Interfaces que definen las interacciones con sistemas externos, como bases de datos o servicios web. Estos puertos permiten al núcleo de la aplicación interactuar con el mundo exterior sin conocer detalles específicos de las implementaciones.

3. **Adaptadores**
   - **Adaptadores de Entrada**: Implementaciones de los puertos de entrada, tales como controladores de API o interfaces de usuario, que traducen las solicitudes externas en comandos o consultas que el núcleo de la aplicación puede procesar.
   - **Adaptadores de Salida**: Implementaciones de los puertos de salida, como repositorios o clientes de servicios externos, que traducen las solicitudes del núcleo de la aplicación en llamadas a sistemas externos.

### Beneficios de la Arquitectura Hexagonal

- **Desacoplamiento**: El núcleo de la aplicación está separado de las tecnologías y frameworks específicos. Esto facilita la prueba y el mantenimiento del código, así como la posibilidad de cambiar o actualizar las dependencias externas sin afectar la lógica de negocio central.
- **Flexibilidad**: Permite que diferentes adaptadores se conecten al núcleo de la aplicación sin necesidad de modificar el código central. Esto es útil para soportar múltiples interfaces o fuentes de datos.
- **Pruebas Simples**: Facilita la realización de pruebas unitarias en el núcleo de la aplicación, ya que se puede simular fácilmente los puertos de entrada y salida.

### Estructura del Proyecto

- **`Tekton.Products.Domain`**: Contiene las entidades y casos de uso del dominio. Aquí reside la lógica de negocio principal y las interfaces de puertos.
- **`Tekton.Products.Infrastructure`**: Implementa los adaptadores de salida y la persistencia de datos. Contiene implementaciones específicas de los puertos de salida.
- **`Tekton.Products.Api`**: Implementa los adaptadores de entrada, como controladores de API y otros puntos de entrada para la aplicación.

Este enfoque asegura que la aplicación pueda evolucionar y adaptarse a nuevas necesidades sin comprometer la integridad del núcleo de la lógica de negocio.



## Pasos para Levantar el Proyecto Localmente

1. **Clonar el Repositorio**

   Clona el repositorio desde GitHub:

   ```bash
   git clone https://github.com/celopez74/TektonTest.git

2. **Restaurar dependencias**
   Navega al directorio del proyecto y restaura las dependencias:
   ```bash
   cd Tekton.Products
   dotnet restore

3. **Ejecutar el proyecto**
   Ejecuta la aplicación:
   ```bash
   dotnet run --project dotnet run --project Tekton.Products.Api/Tekton.Products.Api.csproj

4. **Ejecutar las pruebas unitarias**
   Para ejecutar las pruebas unitarias, utiliza el siguiente comando:
   ```bash
   dotnet test
   ```
   Así mismo se puede observar el reporte de covertura del proyecto al momento de cargar en github en [Tekton.Products.UnitTest/coveragereport/index.html](Tekton.Products.UnitTest/coveragereport/index.html)

## Información Adicional
   * **Documentación del API**: La documentación del API está disponible en Swagger UI, accesible en http://localhost:5000/swagger cuando la aplicación esté en ejecución.
   En la base de datos existen los siguientes ProductId, los cuales pueden ser utilizados para llamar al api de consulta http://localhost:5000/api/ms-tekton/v1/Product/{ProductId}

         69b0fde4-1a24-451e-affe-2fb271dcd9e2
         1de560fc-b4c0-478d-a9da-fb74bed68690
         afc845eb-616b-4c52-bf09-c45436952eb2
         f8f6b05a-9f93-403d-823f-9cf10f6a7f8a
         53e531da-2df9-4f4f-baaf-a55fdef5551e
         27784d74-1bb6-41f1-8a55-627a4348119a
         077e5f19-4a52-4785-a0f1-2346236a6bde
         4dee5d9e-9306-412a-9fbe-b1c88bae8e9a
         f6146fed-8338-4893-bf14-3d23fe8a710e
         4dda88b1-622e-4254-91dc-82b79270e950
         e6da7c78-b9bf-4d2c-b248-c799425cc1e8
         f36ff71c-1915-4ea8-acff-792fd4d2038a
         3227baaa-04be-4d79-9ae1-bf0561c6e539
         1969c6c8-8209-4a06-b7ad-16c42d2f8db5
         577d14c2-a979-442d-a82c-f2829e2abf52
         1a0be34c-315e-4ed4-9e5e-c7d767fd8c7c 
      
  
   * **Configuración de Logging**: Cáda request realizado a la API-Rest, registra en un archivo de texto el tiempo de duración del request, el archivo se genera automáticamente dentro de la carpeta *ApiLogs* que se encuentra dentro de la carpeta Teckton.Producto.Api. El nombre  del archivo es configurable y se realiza mediante la configuración de logging se puede encontrar en el archivo appsettings.Local.json.
  
   * **Migraciones de Base de Datos**: Las migraciones se encuentran en la carpeta Migrations dentro del proyecto Tekton.Products.Infrastructure

