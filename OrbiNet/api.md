# API REST - OrbiNet

**Proyecto:** OrbiNet – Simulador de Red Satelital
**Autenticación:** Basic Authentication

---

# Tabla de Contenido

1. Descripción
2. Información General
3. Autenticación
4. Guía de Endpoints
5. Códigos de Respuesta
6. Resultados de las Pruebas
7. Consideraciones de Seguridad

---

# 1. Descripción

La API REST de **OrbiNet** permite administrar la simulación de una red satelital distribuida. Mediante esta API es posible cargar configuraciones XML, consultar el estado del sistema, administrar nodos, ejecutar la simulación y realizar el intercambio de mensajes entre satélites.

Los servicios implementados utilizan el protocolo HTTP y el formato JSON para el intercambio de información.

---

# 2. Información General

Propiedad          | Valor                 
------------------ | --------------------- 
Protocolo          | HTTP                  
Formato            | JSON                  
Autenticación      | Basic Authentication  
Métodos soportados | GET, POST             
URL Base           | http://localhost:5228 

---

# 3. Autenticación

Todos los endpoints protegidos requieren autenticación mediante **HTTP Basic Authentication**.

## Encabezado requerido

```http
Authorization: Basic <credenciales_base64>
```

### Credenciales utilizadas durante las pruebas

Usuario | Contraseña 
------- | ----------- 
admin   | orbitnet123 

---

# 4. Guía de Endpoints

## 4.1 Ingesta de Configuración XML

**Método**

```http
POST
```

**Endpoint**

```http
/api/space/config
```

Carga una configuración XML para inicializar la simulación.

### Payload de Entrada

```json
{
    "xmlContent":"<xml>...</xml>"
}
```

### Respuesta Exitosa (200 OK)

```json
{
    "estado":"Exitoso",
    "message":"Archivo cargado correctamente",
    "processedNodes":10
}
```

### Posible Error (400 Bad Request)

Se devuelve cuando el contenido XML no cumple con el formato esperado o contiene datos inválidos.

---

## 4.2 Estado del Sistema

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/state
```

Obtiene el estado actual de la simulación.

### Respuesta

```json
{
    "tickActual":0,
    "estadoTransaccion":"IDLE",
    "cantidadNodos":3
}
```

---

## 4.3 Topología de la Red

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/topology
```

Devuelve la representación actual de la red satelital.

### Respuesta

```json
{
    "topologia":"La red satelital está vacía. No hay nodos en órbita."
}
```

---

## 4.4 Avance de la Simulación

**Método**

```http
POST
```

**Endpoint**

```http
/api/space/simulation/step
```

Ejecuta un paso de la simulación e incrementa el tick actual.

### Respuesta Exitosa (200 OK)

```json
{
    "estado":"Exitoso",
    "mensaje":"Simulación ejecutada correctamente",
    "tickActual":1
}
```

---

## 4.5 Reinicio de la Simulación

**Método**

```http
POST
```

**Endpoint**

```http
/api/space/simulation/reset
```

Reinicia la simulación y restablece el estado inicial.

### Respuesta

```json
{
    "estado":"Exitoso",
    "mensaje":"Simulación reiniciada",
    "tickActual":0
}
```

---

## 4.6 Registro de Nodos

**Método**

```http
POST
```

**Endpoint**

```http
/api/space/node/register/{idNodo}
```

Registra un nuevo nodo satelital.

### Parámetro

Nombre | Tipo   | Descripción                             
------ | ------ | --------------------------------------- 
idNodo | String | Identificador único del nodo satelital. 

### Ejemplo

```http
POST /api/space/node/register/SAT-050
```

### Respuesta

```json
{
    "estado":"Exitoso",
    "nodo":"SAT-050",
    "registrado":true
}
```

---

## 4.7 Cantidad de Nodos

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/node/count
```

Obtiene la cantidad de nodos registrados.

### Respuesta

```json
{
    "cantidadNodos":4
}
```

---

## 4.8 Verificar Existencia de Nodo

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/node/exists/{idNodo}
```

Verifica si un nodo existe.

### Respuesta

```json
{
    "nodo":"SAT-050",
    "existe":true
}
```

---

## 4.9 Transferencia de Mensajes

**Método**

```http
POST
```

**Endpoint**

```http
/api/relay/send
```

Registra el envío de un mensaje entre dos nodos satelitales.

Este endpoint requiere autenticación Basic Authentication.

### Payload de Entrada

```json
{
    "origen":"SAT-001",
    "destino":"SAT-002",
    "mensaje":"Prueba historial"
}
```

### Respuesta Exitosa (200 OK)

```json
{
    "estado":"Encolado",
    "mensaje":"Mensaje registrado para transmisión",
    "origen":"SAT-001",
    "destino":"SAT-002"
}
```

### Error de Autenticación (401 Unauthorized)

Se devuelve cuando las credenciales Basic Authentication son inválidas o no se incluyen en la solicitud.

---

## 4.10 Historial de Mensajes

**Método**

```http
GET
```

**Endpoint**

```http
/api/relay/history
```

Obtiene el historial de mensajes registrados.

### Respuesta

```json
{
    "historial":[
        "SAT-001 -> SAT-002: Prueba historial"
    ]
}
```

---

## 4.11 Graphviz Logs

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/graph/logs
```

Genera la representación Graphviz de los registros.

### Respuesta

```json
{
    "dot":"digraph LogsAuditoria {...}"
}
```

---

## 4.12 Estado de la Transacción

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/transaction/status
```

Consulta el estado actual de la transacción XML.

### Respuesta

```json
{
    "estado":"IDLE"
}
```

---

## 4.13 Tabla de la Red

**Método**

```http
GET
```

**Endpoint**

```http
/api/space/network/table
```

Obtiene una representación textual de la red satelital.

---

# 5. Códigos de Respuesta

 Código HTTP              | Descripción                             
------------------------- | --------------------------------------- 
200 OK                    | Solicitud procesada correctamente.      
400 Bad Request           | Solicitud inválida o datos incorrectos. 
401 Unauthorized          | Credenciales inválidas o inexistentes.  
404 Not Found             | Recurso no encontrado.                  
500 Internal Server Error | Error interno del servidor.             

---

# 6. Resultados de las Pruebas

Las pruebas funcionales fueron realizadas utilizando **PowerShell** mediante el comando `Invoke-RestMethod`.

## Configuración utilizada

* Usuario: **admin**
* Contraseña: **orbitnet123**
* Autenticación: **Basic Authentication**

## Endpoints verificados

 Endpoint                             | Resultado                                       
------------------------------------ | ----------------------------------------------- 
Basic Authentication                 | Funciona correctamente                        
POST `/api/space/config`             | Integrado correctamente         
GET `/api/space/state`               | Funciona correctamente                        
GET `/api/space/topology`            | Funciona correctamente                       
POST `/api/space/simulation/step`    | Tick incrementado correctamente               
POST `/api/space/simulation/reset`   | Reinicia correctamente la simulación          
POST `/api/space/node/register/{id}` | Registra correctamente nuevos nodos           
GET `/api/space/node/count`          | Devuelve la cantidad correcta de nodos        
GET `/api/space/node/exists/{id}`    | Verifica correctamente la existencia del nodo 
POST `/api/relay/send`               | Registra correctamente el mensaje             
GET `/api/relay/history`             | Devuelve correctamente el historial           
GET `/api/space/graph/logs`          | Genera correctamente la salida Graphviz       
GET `/api/space/transaction/status`  | Devuelve el estado de la transacción          
GET `/api/space/network/table`       | Devuelve la representación textual de la red  

## Evidencia de pruebas

Durante las pruebas se ejecutó la siguiente solicitud:

```powershell
Invoke-RestMethod `
-Uri http://localhost:5228/api/relay/send `
-Method POST `
-Headers $headers `
-ContentType "application/json" `
-Body $body
```

Obteniendo la respuesta:

```text
estado   mensaje                             origen   destino
------   ----------------------------------  -------  --------
Encolado Mensaje registrado para transmisión SAT-001  SAT-002
```

Posteriormente se verificó el historial mediante:

```powershell
Invoke-RestMethod `
-Uri http://localhost:5228/api/relay/history `
-Headers $headers
```

confirmando que el mensaje había sido almacenado correctamente.

También se verificó el incremento del valor de `tickActual` mediante el endpoint `/api/space/simulation/step`, así como el reinicio exitoso utilizando `/api/space/simulation/reset`.

Todas las pruebas finalizaron de manera correctas.

---

# 7. Consideraciones de Seguridad

La API implementa autenticación mediante **HTTP Basic Authentication** para restringir el acceso a los endpoints protegidos.

Como parte del proyecto, se consideraron las siguientes prácticas de seguridad:

* Validación de datos recibidos mediante la API.
* Control de acceso mediante autenticación Basic Authentication.
* Procesamiento seguro de configuraciones XML.
* Integración con los servicios de simulación y enrutamiento sin exponer su implementación interna.

---

