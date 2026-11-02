# Flotix Platform REST API Technical Stories

## Overview
This document contains API-focused technical stories intended for frontend or mobile developers integrating with the flotix-platform REST API. The stories are aligned with the User Stories (US01-US54) and the Technical Stories (TS01-TS31) of Chapter 3: Requirements Specification, and are organized by the bounded contexts of the Web Application.

Base resource route: `/api/v1/{resource}` (kebab-case route naming convention).

## Conventions
- Validation, business-rule and not-found errors return a localized error message (`Accept-Language`: `en`, `en-US`, `es`, `es-PE`); unexpected errors return a `ProblemDetails` payload.
- Status literals are shown in Spanish as the API returns them: vehicles (`Disponible`, `En mantenimiento`), maintenance requests (`Pendiente`, `Presupuestado`, `Aprobado`, `Rechazado`, `En reparación`, `Completado`), incidents (`Abierto`, `En revisión`, `Resuelto`) and IoT orders (`Pendiente de pago`, `En preparación`, `Entregado`, `Cancelado`).
- Business rules of Chapter 3 applied by the API: anomalous fuel consumption when the efficiency is lower than 70% of the historical average of the vehicle, speeding alert when the speed is higher than 90 km/h, and automatic synchronization of the vehicle status with the repair of its maintenance request.
- Real-time channels: `/hubs/monitoring` (`VehicleLocationUpdated`) and `/hubs/notifications` (`AlertActivated`, `NotificationReceived`).


## Identity and Access Management (IAM)

### TS001 — Sign up a user
As a frontend developer, I want to register a user with the `Owner`, `Driver` or `Mechanic` role through the API so that I can implement the registration feature in my application.
#### Acceptance criteria:
- Scenario: Successful sign-up
    - Given a request `POST /api/v1/authentication/sign-up` that includes required fields (`email`, `password`, `roleType`, `name`) is received
    - When the API validates the data, hashes the password and persists the new user successfully
    - Then the API responds with `201 Created` and returns the created user resource (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`) without the password.
- Scenario: Validation error
    - Given a request `POST /api/v1/authentication/sign-up` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Driver without license
    - Given a request `POST /api/v1/authentication/sign-up` with `roleType` `Driver` and without `licenseNumber` or `licenseExpiration` is received
    - When the API validates the driver profile
    - Then the API responds with `400 Bad Request` and a localized error message.
- Scenario: Duplicate email
    - Given a request `POST /api/v1/authentication/sign-up` for an `email` that already exists is received
    - When the API detects the duplicate email before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/authentication/sign-up` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS002 — Sign in a user
As a frontend developer, I want to sign in with my credentials through the API so that I can obtain a token to call the protected endpoints from my application.
#### Acceptance criteria:
- Scenario: Valid credentials
    - Given a request `POST /api/v1/authentication/sign-in` with a registered `email` and its `password` is received
    - When the API verifies the credentials
    - Then the API responds with `200 OK` and returns the authenticated user resource (`id`, `email`, `roleType`, `name`, `token`), where `token` is a JWT that includes the user id and role.
- Scenario: Invalid credentials
    - Given a request `POST /api/v1/authentication/sign-in` with an unknown `email` or a wrong `password` is received
    - When the API cannot verify the credentials
    - Then the API responds with `401 Unauthorized` and a localized error message.
- Scenario: Validation error
    - Given a request `POST /api/v1/authentication/sign-in` with missing required fields is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
---

### TS003 — List and search users by `roleType`
As a frontend developer, I want to request the API to list users, optionally filtered by `{roleType}` and by a `{search}` text so that I can implement the user and driver listing features and find a specific driver or mechanic quickly.
#### Acceptance criteria:
- Scenario: Returns one or more users
    - Given a request `GET /api/v1/users?roleType={roleType}&search={search}` is received
    - When the API finds one or more matching user resources
    - Then the API responds with `200 OK` and returns a non-empty array of user resources, with each resource including `id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`; for users with `roleType` `Driver` each resource also includes `assignedVehicleId`, `assignedLicensePlate` and `status`.
- Scenario: Driver status
    - Given a request `GET /api/v1/users?roleType=Driver` is received
    - When the API builds each driver resource
    - Then the `status` is `En ruta` when the driver is assigned to a vehicle and `Activo` otherwise.
- Scenario: Search with results
    - Given a request `GET /api/v1/users?search={search}` is received
    - When the `{search}` text matches the `name`, `email` or `licenseNumber` of one or more users (case-insensitive, partial match)
    - Then the API responds with `200 OK` and returns only the users that match.
- Scenario: Search without results
    - Given a request `GET /api/v1/users?search={search}` is received
    - When the `{search}` text does not match any user
    - Then the API responds with `200 OK` and returns an empty array.
- Scenario: Returns no users
    - Given a request `GET /api/v1/users?roleType={roleType}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS004 — Retrieve the drivers summary
As a frontend developer, I want to request the summary of drivers so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/users/drivers-summary` is received
    - When the API finds drivers
    - Then the API responds with `200 OK` and returns the summary (`totalDrivers`, `activeDrivers`, `enRouteDrivers`). `activeDrivers` counts the drivers with `status` `Activo` and `enRouteDrivers` the drivers with `status` `En ruta`.
- Scenario: No drivers
    - Given a request `GET /api/v1/users/drivers-summary` is received
    - When the API finds no drivers
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

### TS005 — Retrieve a user by id
As a frontend developer, I want to request a user by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/users/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the user resource (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`).
- Scenario: Not found
    - Given a request `GET /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS006 — Update a user
As a frontend developer, I want to update a user through the API so that I can implement the user edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/users/{id}` that includes required fields (`name`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `email`, `roleType`, `name`, `licenseNumber`, `licenseExpiration`, `companyName`, `workshopName`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/users/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Driver without license
    - Given a request `PUT /api/v1/users/{id}` for a driver without `licenseNumber` or `licenseExpiration` is received
    - When the API validates the driver profile
    - Then the API responds with `400 Bad Request` and a localized error message.
---

### TS007 — Delete a user
As a frontend developer, I want to delete a user through the API so that I can implement the user removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/users/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/users/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: A user with related records
    - Given a request `DELETE /api/v1/users/{id}` for a user that still has vehicles, fuel records, incidents, maintenance requests, notifications, IoT orders or reports is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---

## Vehicle and Fleet Management

### TS008 — List vehicles by `ownerId` or `driverId`
As a frontend developer, I want to request the API to list vehicles, optionally filtered by `{ownerId}` or `{driverId}`, so that I can implement the vehicle listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more vehicles
    - Given a request `GET /api/v1/vehicles?ownerId={ownerId}&driverId={driverId}` is received
    - When the API finds one or more matching vehicle resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`, `driverName`.
- Scenario: Returns no vehicles
    - Given a request `GET /api/v1/vehicles?ownerId={ownerId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS009 — Retrieve the fleet summary
As a frontend developer, I want to request the summary of vehicles so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/vehicles/summary?ownerId={ownerId}` is received
    - When the API finds vehicles
    - Then the API responds with `200 OK` and returns the summary (`totalVehicles`, `availableVehicles`, `inMaintenanceVehicles`).
- Scenario: No vehicles
    - Given a request `GET /api/v1/vehicles/summary?ownerId={ownerId}` is received
    - When the API finds no vehicles
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

### TS010 — Retrieve a vehicle by id
As a frontend developer, I want to request a vehicle by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/vehicles/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the vehicle resource (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`, `driverName`).
- Scenario: Not found
    - Given a request `GET /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS011 — Register a vehicle
As a frontend developer, I want to register a vehicle of my fleet through the API so that I can implement the vehicle creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/vehicles` that includes required fields (`ownerId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`). The `vehicleType` is one of `Automóvil`, `Camioneta`, `Mototaxi`, `Camión de carga` or `Van de pasajeros`.
- Scenario: Vehicle without driver
    - Given a request `POST /api/v1/vehicles` without `driverId` is received
    - When the API persists the new resource
    - Then the API responds with `201 Created` and the resource has a `null` `driverId` (shown as "Sin asignar").
- Scenario: Validation error
    - Given a request `POST /api/v1/vehicles` with missing required fields or invalid values (including an unsupported `vehicleType` or a negative `currentMileage`) is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Duplicate license plate
    - Given a request `POST /api/v1/vehicles` for a `licensePlate` that already exists is received
    - When the API detects the duplicate license plate before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/vehicles` with a `ownerId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/vehicles` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS012 — Update a vehicle
As a frontend developer, I want to update a vehicle through the API so that I can implement the vehicle edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/vehicles/{id}` that includes required fields (`licensePlate`, `vehicleType`, `currentMileage`, `status`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/vehicles/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Mileage decrease
    - Given a request `PUT /api/v1/vehicles/{id}` with a `currentMileage` lower than the stored one is received
    - When the API applies the vehicle rules
    - Then the API responds with `400 Bad Request` and a localized error message.
- Scenario: Duplicate license plate
    - Given a request `PUT /api/v1/vehicles/{id}` with a `licensePlate` of another vehicle is received
    - When the API detects the duplicate license plate
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `PUT /api/v1/vehicles/{id}` with a `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS013 — Assign a driver to a vehicle
As a frontend developer, I want to assign a registered driver to a vehicle through the API so that I can enable the operation of that unit.
#### Acceptance criteria:
- Scenario: Successful assignment
    - Given a request `POST /api/v1/vehicles/{id}/driver-assignment` with a `driverId` of a registered driver is received
    - When the API persists the assignment
    - Then the API responds with `200 OK` and returns the vehicle resource (`id`, `ownerId`, `driverId`, `licensePlate`, `vehicleType`, `currentMileage`, `status`) with that `driverId`; from then on the driver resource has `status` `En ruta`.
- Scenario: Validation error
    - Given a request `POST /api/v1/vehicles/{id}/driver-assignment` without `driverId` is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Driver not found
    - Given a request `POST /api/v1/vehicles/{id}/driver-assignment` with a `driverId` that does not correspond to any registered driver is received
    - When the API cannot find the driver
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/vehicles/{id}/driver-assignment` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS014 — Unassign the driver of a vehicle
As a frontend developer, I want to remove the driver assigned to a vehicle through the API so that the vehicle shows as without driver.
#### Acceptance criteria:
- Scenario: Successful unassignment
    - Given a request `DELETE /api/v1/vehicles/{id}/driver-assignment` for a vehicle with an assigned driver is received
    - When the API removes the assignment
    - Then the API responds with `200 OK` and returns the vehicle resource with a `null` `driverId` (shown as "Sin asignar"); the former driver resource has `status` `Activo`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/vehicles/{id}/driver-assignment` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS015 — Delete a vehicle
As a frontend developer, I want to delete a vehicle through the API so that I can implement the vehicle removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/vehicles/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/vehicles/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: A vehicle with related records
    - Given a request `DELETE /api/v1/vehicles/{id}` for a vehicle that still has fuel records, maintenance requests, incidents, an IoT device or alerts is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---

## Fuel Control

### TS016 — List fuel records by `vehicleId` or `driverId`
As a frontend developer, I want to request the API to list fuel records, optionally filtered by `{vehicleId}` or `{driverId}`, so that I can implement the fuel record listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more fuel records
    - Given a request `GET /api/v1/fuel-records?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds one or more matching fuel record resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources ordered from the most recent to the oldest `recordedDate`, with each resource including `id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`, `fuelEfficiency`, `isAnomalous`.
- Scenario: Fuel efficiency calculated
    - Given a vehicle has at least two fuel records
    - When the API builds the resources
    - Then each record after the first one includes `fuelEfficiency` in km/L, calculated with the kilometers traveled since the previous refill (difference of `mileageAtRefill`) divided by the `liters` of the refill.
- Scenario: First refill
    - Given a fuel record is the first one of its vehicle
    - When the API builds the resource
    - Then `fuelEfficiency` is `null` and `isAnomalous` is `false`.
- Scenario: Anomalous consumption
    - Given the `fuelEfficiency` of a fuel record is lower than 70% of the historical average `fuelEfficiency` of its vehicle
    - When the API builds the resource
    - Then `isAnomalous` is `true`.
- Scenario: Normal consumption
    - Given the `fuelEfficiency` of a fuel record is equal to or higher than 70% of the historical average
    - When the API builds the resource
    - Then `isAnomalous` is `false`.
- Scenario: Returns no fuel records
    - Given a request `GET /api/v1/fuel-records?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS017 — Retrieve a fuel record by id
As a frontend developer, I want to request a fuel record by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/fuel-records/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the fuel record resource (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`, `fuelEfficiency`, `isAnomalous`).
- Scenario: Not found
    - Given a request `GET /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS018 — Register a fuel record
As a frontend developer, I want to register a refuel of a vehicle through the API so that I can implement the fuel record creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/fuel-records` that includes required fields (`vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`, `fuelEfficiency`, `isAnomalous`).
- Scenario: Validation error
    - Given a request `POST /api/v1/fuel-records` with missing required fields or with `liters` or `cost` lower than or equal to zero is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Inconsistent mileage
    - Given a request `POST /api/v1/fuel-records` with a `mileageAtRefill` lower than the one of the last refill of the vehicle is received
    - When the API applies the fuel record rules
    - Then the API responds with `400 Bad Request` and a localized error message indicating the inconsistency.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/fuel-records` with a `vehicleId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/fuel-records` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS019 — Update a fuel record
As a frontend developer, I want to update a fuel record through the API so that I can implement the fuel record edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/fuel-records/{id}` that includes required fields (`liters`, `cost`, `mileageAtRefill`, `recordedDate`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK`, returns the updated resource representation (`id`, `vehicleId`, `driverId`, `liters`, `cost`, `mileageAtRefill`, `recordedDate`, `fuelEfficiency`, `isAnomalous`) and recalculates the `fuelEfficiency` of the vehicle records.
- Scenario: Validation error
    - Given a request `PUT /api/v1/fuel-records/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS020 — Delete a fuel record
As a frontend developer, I want to delete a fuel record through the API so that I can keep the consumption history accurate.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/fuel-records/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content` and recalculates the `fuelEfficiency` and the fuel summary.
- Scenario: Not found
    - Given a request `DELETE /api/v1/fuel-records/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS021 — Retrieve the fuel summary
As a frontend developer, I want to request the summary of fuel records so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/fuel-records/summary?ownerId={ownerId}` is received
    - When the API finds fuel records
    - Then the API responds with `200 OK` and returns the summary (`totalLiters`, `totalCost`, `anomalousCount`). `totalCost` is expressed in soles and `anomalousCount` is the number of fuel records with `isAnomalous` `true`.
- Scenario: No fuel records
    - Given a request `GET /api/v1/fuel-records/summary?ownerId={ownerId}` is received
    - When the API finds no fuel records
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

## Maintenance Management

### TS022 — List maintenance requests by `vehicleId` or `mechanicId`
As a frontend developer, I want to request the API to list maintenance requests, optionally filtered by `{vehicleId}` or `{mechanicId}`, so that I can implement the maintenance request listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more maintenance requests
    - Given a request `GET /api/v1/maintenance-requests?vehicleId={vehicleId}&mechanicId={mechanicId}` is received
    - When the API finds one or more matching maintenance request resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`, `licensePlate`, `workshopName`.
- Scenario: Returns no maintenance requests
    - Given a request `GET /api/v1/maintenance-requests?vehicleId={vehicleId}&mechanicId={mechanicId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS023 — Retrieve the maintenance summary
As a frontend developer, I want to request the summary of maintenance requests so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/maintenance-requests/summary?ownerId={ownerId}` is received
    - When the API finds maintenance requests
    - Then the API responds with `200 OK` and returns the summary (`openRequests`, `inRepairRequests`, `completedRequests`). `openRequests` counts the requests in `Pendiente`, `Presupuestado` or `Aprobado` status; a request in `Rechazado` status is not counted as open.
- Scenario: No maintenance requests
    - Given a request `GET /api/v1/maintenance-requests/summary?ownerId={ownerId}` is received
    - When the API finds no maintenance requests
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

### TS024 — Retrieve a maintenance request by id
As a frontend developer, I want to request a maintenance request by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/maintenance-requests/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the maintenance request resource (`id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`, `licensePlate`, `workshopName`).
- Scenario: Not found
    - Given a request `GET /api/v1/maintenance-requests/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS025 — Submit a maintenance request
As a frontend developer, I want to submit a maintenance request of a vehicle, optionally addressed to a chosen workshop, through the API so that I can implement the maintenance request creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/maintenance-requests` that includes required fields (`vehicleId`, `description`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `mechanicId`, `description`, `estimatedCost`, `status`, `requestDate`) with status `Pendiente`.
- Scenario: Request addressed to a workshop
    - Given a request `POST /api/v1/maintenance-requests` that also includes a `mechanicId` is received
    - When the API persists the new resource
    - Then the API responds with `201 Created` and the resource is assigned to that workshop.
- Scenario: Validation error
    - Given a request `POST /api/v1/maintenance-requests` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/maintenance-requests` with a `vehicleId` or `mechanicId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/maintenance-requests` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS026 — Register the diagnostic and quote of a maintenance request
As a frontend developer, I want to register the diagnostic and estimated cost of a maintenance request so that I can implement the mechanic quotation feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` with `estimatedCost` (and `mechanicId` when the request has no workshop yet) for a maintenance request in `Pendiente` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Presupuestado`.
- Scenario: Validation error
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` with a negative or missing `estimatedCost`, or without `mechanicId` for a request with no workshop, is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` for a maintenance request that is not in `Pendiente` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS027 — Approve or reject the budget of a maintenance request
As a frontend developer, I want to approve or reject the quoted budget of a maintenance request so that I can implement the owner budget decision feature in my application.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` with `isApproved` for a maintenance request in `Presupuestado` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Aprobado` or `Rechazado`; a `Rechazado` request is no longer counted as open.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` for a maintenance request that is not in `Presupuestado` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/budget-decision` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS028 — Start the repair of a maintenance request
As a frontend developer, I want to start the repair of an approved maintenance request so that the vehicle availability reflects the service in progress.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a maintenance request in `Aprobado` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `En reparación`; in the same operation the vehicle changes to `En mantenimiento`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a maintenance request that is not in `Aprobado` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message; the status of the vehicle does not change.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/repair-start` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS029 — Complete a maintenance request
As a frontend developer, I want to complete a maintenance request under repair so that the vehicle is available again for the operation.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a maintenance request in `En reparación` status is received
    - When the API applies the transition to the maintenance request
    - Then the API responds with `200 OK` and returns the maintenance request resource with status `Completado`; in the same operation the vehicle changes to `Disponible`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a maintenance request that is not in `En reparación` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message; the status of the vehicle does not change.
- Scenario: Not found
    - Given a request `POST /api/v1/maintenance-requests/{id}/completion` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS030 — Delete a maintenance request
As a frontend developer, I want to delete a maintenance request through the API so that I can implement the maintenance request removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/maintenance-requests/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/maintenance-requests/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS031 — List and search affiliated workshops
As a frontend developer, I want to request the API to list the affiliated workshops, optionally filtered by a `{search}` text so that I can help the owner choose the most suitable workshop for a vehicle.
#### Acceptance criteria:
- Scenario: Returns one or more workshops
    - Given a request `GET /api/v1/workshops?search={search}` is received
    - When the API finds one or more matching workshop resources
    - Then the API responds with `200 OK` and returns a non-empty array of workshop resources, with each resource including `id`, `name`, `city`, `address`, `phone`, `rating`, `specialties`; the `id` of a workshop is the value to send as `mechanicId` when submitting a maintenance request, and the number of available workshops is the length of the array.
- Scenario: Search with results
    - Given a request `GET /api/v1/workshops?search={search}` is received
    - When the `{search}` text matches the `name` or `city` of one or more workshops (case-insensitive, partial match)
    - Then the API responds with `200 OK` and returns only the workshops that match.
- Scenario: Returns no workshops
    - Given a request `GET /api/v1/workshops?search={search}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

## Incident Management

### TS032 — List incidents by `vehicleId` or `driverId`
As a frontend developer, I want to request the API to list incidents, optionally filtered by `{vehicleId}` or `{driverId}`, so that I can implement the incident listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more incidents
    - Given a request `GET /api/v1/incidents?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds one or more matching incident resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`, `licensePlate`.
- Scenario: Returns no incidents
    - Given a request `GET /api/v1/incidents?vehicleId={vehicleId}&driverId={driverId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS033 — Retrieve the incidents summary
As a frontend developer, I want to request the summary of incidents so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/incidents/summary?ownerId={ownerId}` is received
    - When the API finds incidents
    - Then the API responds with `200 OK` and returns the summary (`totalIncidents`, `openIncidents`, `resolvedIncidents`).
- Scenario: No incidents
    - Given a request `GET /api/v1/incidents/summary?ownerId={ownerId}` is received
    - When the API finds no incidents
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

### TS034 — Retrieve an incident by id
As a frontend developer, I want to request an incident by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/incidents/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the incident resource (`id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`, `licensePlate`).
- Scenario: Not found
    - Given a request `GET /api/v1/incidents/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS035 — Report an incident
As a frontend developer, I want to report an incident of a vehicle through the API so that I can implement the incident creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/incidents` that includes required fields (`vehicleId`, `driverId`, `description`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `driverId`, `description`, `status`, `reportedDate`) with status `Abierto`; in the same operation the API generates a notification for the owner of the vehicle with the description of the incident.
- Scenario: Validation error
    - Given a request `POST /api/v1/incidents` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/incidents` with a `vehicleId` or `driverId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/incidents` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS036 — Start the review of an incident
As a frontend developer, I want to mark an incident as in review so that I can show that it is being attended.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/incidents/{id}/review` for an incident in `Abierto` status is received
    - When the API applies the transition
    - Then the API responds with `200 OK` and returns the incident resource with status `En revisión`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/incidents/{id}/review` for an incident that is not in `Abierto` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/incidents/{id}/review` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS037 — Resolve an incident
As a frontend developer, I want to mark an incident as resolved so that I can close its follow-up.
#### Acceptance criteria:
- Scenario: Successful transition
    - Given a request `POST /api/v1/incidents/{id}/resolution` for an incident in `En revisión` status is received
    - When the API applies the transition
    - Then the API responds with `200 OK` and returns the incident resource with status `Resuelto`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/incidents/{id}/resolution` for an incident in `Abierto` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message indicating that the incident must first pass to `En revisión`.
- Scenario: Not found
    - Given a request `POST /api/v1/incidents/{id}/resolution` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS038 — Delete an incident
As a frontend developer, I want to delete an incident through the API so that I can implement the incident removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/incidents/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/incidents/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

## Real-Time Monitoring / Fleet Tracking

### TS039 — List IoT devices by `vehicleId`
As a frontend developer, I want to request the API to list IoT devices, optionally filtered by `{vehicleId}`, so that I can implement the IoT device listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more IoT devices
    - Given a request `GET /api/v1/iot-devices?vehicleId={vehicleId}` is received
    - When the API finds one or more matching IoT device resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `vehicleId`, `serialNumber`, `isConnected`.
- Scenario: Returns no IoT devices
    - Given a request `GET /api/v1/iot-devices?vehicleId={vehicleId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS040 — Retrieve an IoT device by id
As a frontend developer, I want to request an IoT device by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-devices/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the IoT device resource (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Not found
    - Given a request `GET /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS041 — Link an IoT device to a vehicle
As a frontend developer, I want to link an IoT device to a vehicle through the API so that the telemetry received is attributed to the correct unit.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/iot-devices` that includes required fields (`vehicleId`, `serialNumber`, `isConnected`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Validation error
    - Given a request `POST /api/v1/iot-devices` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Vehicle already equipped
    - Given a request `POST /api/v1/iot-devices` for a `vehicleId` that already has an IoT device is received
    - When the API detects the duplicate vehicle before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Device already linked
    - Given a request `POST /api/v1/iot-devices` for a `serialNumber` that already exists (device linked to another vehicle) is received
    - When the API detects the duplicate serial number before persistence or during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/iot-devices` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/iot-devices` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS042 — Update an IoT device
As a frontend developer, I want to update an IoT device through the API so that I can implement the IoT device edition feature in my application.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/iot-devices/{id}` that includes required fields (`vehicleId`, `isConnected`) is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `vehicleId`, `serialNumber`, `isConnected`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/iot-devices/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Vehicle already equipped
    - Given a request `PUT /api/v1/iot-devices/{id}` with a `vehicleId` that already has another IoT device is received
    - When the API detects the duplicate vehicle
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Referenced resource not found
    - Given a request `PUT /api/v1/iot-devices/{id}` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS043 — Delete an IoT device
As a frontend developer, I want to delete an IoT device through the API so that I can implement the IoT device removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/iot-devices/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/iot-devices/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: An IoT device with related records
    - Given a request `DELETE /api/v1/iot-devices/{id}` for an IoT device that still has telemetry data is received
    - When the API detects the related records during persistence confirmation
    - Then the API responds with `409 Conflict` and a localized error message; the resource is not removed.
---

### TS044 — Receive IoT telemetry data
As a frontend developer, I want to send the telemetry packets of an IoT device to the API so that I can feed the real-time monitoring of the fleet.
#### Acceptance criteria:
- Scenario: Packet accepted
    - Given a request `POST /api/v1/telemetry-data` that includes required fields (`deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`) is received
    - When the API validates the packet and queues it for processing
    - Then the API responds with `202 Accepted` without a body.
- Scenario: Packet processed
    - Given a queued telemetry packet of an IoT device linked to a vehicle
    - When the IoT Telemetry Processor processes the packet
    - Then the API stores the reading, updates the position of the vehicle, marks the IoT device as connected and sends `VehicleLocationUpdated` (`vehicleId`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`) to the clients of `/hubs/monitoring`; the connected devices send a new reading every 3 seconds.
- Scenario: Corrupt packet
    - Given a request `POST /api/v1/telemetry-data` with missing fields or out-of-range coordinates or speed is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request`, discards the packet and writes it to the error log.
- Scenario: Unlinked device
    - Given a queued telemetry packet of a `deviceId` that is not linked to a vehicle
    - When the IoT Telemetry Processor processes the packet
    - Then the API discards the packet and writes it to the error log.
---

### TS045 — List telemetry data records by `deviceId`
As a frontend developer, I want to request the API to list telemetry data records, optionally filtered by `{deviceId}`, so that I can implement the telemetry data record listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more telemetry data records
    - Given a request `GET /api/v1/telemetry-data?deviceId={deviceId}` is received
    - When the API finds one or more matching telemetry data record resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources ordered from the most recent to the oldest `recordedAt`,, with each resource including `id`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`.
- Scenario: Returns no telemetry data records
    - Given a request `GET /api/v1/telemetry-data?deviceId={deviceId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS046 — Retrieve a telemetry data record by id
As a frontend developer, I want to request a telemetry data record by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/telemetry-data/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the telemetry data record resource (`id`, `deviceId`, `latitude`, `longitude`, `speed`, `recordedAt`).
- Scenario: Not found
    - Given a request `GET /api/v1/telemetry-data/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS047 — Retrieve the fleet monitoring status
As a frontend developer, I want to request the last position, the speed and the monitoring indicators of the fleet so that I can implement the real-time monitoring view in my application.
#### Acceptance criteria:
- Scenario: Current status of the fleet
    - Given a request `GET /api/v1/fleet-monitoring?ownerId={ownerId}` is received and there are connected devices with received telemetry
    - When the API builds the monitoring status
    - Then the API responds with `200 OK` and returns, for each vehicle with an IoT device, its `vehicleId`, `licensePlate`, `deviceId`, `latitude`, `longitude`, `speed` and `recordedAt` of the last reading, together with `connectedDevices`, `averageSpeed` and `packetsReceived`.
- Scenario: Most recent reading
    - Given the API processes a new packet of a device
    - When a request `GET /api/v1/fleet-monitoring?ownerId={ownerId}` is received again
    - Then the response includes the most recent reading of that device.
- Scenario: No connected devices
    - Given a request `GET /api/v1/fleet-monitoring?ownerId={ownerId}` is received
    - When there are no connected devices
    - Then the API responds with `200 OK`, an empty list of vehicles and every indicator in zero.
---

## Alerts & Notifications

### TS048 — Generate speeding alerts from telemetry
As a frontend developer, I want the API to evaluate the speed of every processed telemetry reading so that I can show the owner the alerts and notifications generated for risky driving.
#### Acceptance criteria:
- Scenario: Speed limit exceeded
    - Given a queued telemetry packet with a `speed` higher than 90 km/h of an IoT device linked to a vehicle
    - When the IoT Telemetry Processor evaluates the reading
    - Then the API creates an alert of type `Exceso de velocidad` for the vehicle, creates a notification for the owner of the vehicle with the license plate and the registered speed, and sends `AlertActivated` and `NotificationReceived` through `/hubs/notifications`.
- Scenario: Speed within the limit
    - Given a queued telemetry packet with a `speed` of 90 km/h or less
    - When the IoT Telemetry Processor evaluates the reading
    - Then the API creates neither an alert nor a notification.
---

### TS049 — List alerts by `vehicleId`
As a frontend developer, I want to request the API to list alerts, optionally filtered by `{vehicleId}`, so that I can implement the alert listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more alerts
    - Given a request `GET /api/v1/alerts?vehicleId={vehicleId}` is received
    - When the API finds one or more matching alert resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `vehicleId`, `alertType`, `triggerDate`.
- Scenario: Returns no alerts
    - Given a request `GET /api/v1/alerts?vehicleId={vehicleId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS050 — Retrieve an alert by id
As a frontend developer, I want to request an alert by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/alerts/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the alert resource (`id`, `vehicleId`, `alertType`, `triggerDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/alerts/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS051 — Create an alert
As a frontend developer, I want to create an alert of a vehicle through the API so that I can implement the alert creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/alerts` that includes required fields (`vehicleId`, `alertType`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `vehicleId`, `alertType`, `triggerDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/alerts` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/alerts` with a `vehicleId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/alerts` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS052 — List notifications by `userId`
As a frontend developer, I want to request the API to list notifications, optionally filtered by `{userId}`, so that I can implement the notification listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more notifications
    - Given a request `GET /api/v1/notifications?userId={userId}` is received
    - When the API finds one or more matching notification resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources ordered from the most recent to the oldest `sentDate`,, with each resource including `id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`.
- Scenario: Returns no notifications
    - Given a request `GET /api/v1/notifications?userId={userId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS053 — Retrieve the alerts and notifications summary
As a frontend developer, I want to request the summary of alerts and notifications so that I can show the totals without computing them in my application.
#### Acceptance criteria:
- Scenario: Summary available
    - Given a request `GET /api/v1/notifications/summary?userId={userId}` is received
    - When the API finds alerts and notifications
    - Then the API responds with `200 OK` and returns the summary (`activeAlerts`, `totalNotifications`, `unreadNotifications`). `activeAlerts` counts the alerts of the vehicles of the user.
- Scenario: No alerts and notifications
    - Given a request `GET /api/v1/notifications/summary?userId={userId}` is received
    - When the API finds no alerts and notifications
    - Then the API responds with `200 OK` and returns every counter and amount in zero.
---

### TS054 — Retrieve a notification by id
As a frontend developer, I want to request a notification by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/notifications/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the notification resource (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS055 — Create a notification
As a frontend developer, I want to create a notification for a user through the API so that I can implement the notification creation feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/notifications` that includes required fields (`userId`, `message`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`).
- Scenario: Validation error
    - Given a request `POST /api/v1/notifications` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/notifications` with a `userId` or `alertId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/notifications` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS056 — Mark a notification as read
As a frontend developer, I want to update the read state of a notification through the API so that the owner can tell apart the notifications already reviewed.
#### Acceptance criteria:
- Scenario: Successful update
    - Given a request `PUT /api/v1/notifications/{id}` with `isRead` `true` for an unread notification is received
    - When the API validates and persists the changes successfully
    - Then the API responds with `200 OK` and returns the updated resource (`id`, `userId`, `alertId`, `message`, `isRead`, `sentDate`); `unreadNotifications` of the summary decreases by one.
- Scenario: Notification already read
    - Given a request `PUT /api/v1/notifications/{id}` with `isRead` `true` for a notification that is already read is received
    - When the API applies the update
    - Then the API responds with `200 OK` without modifying the unread counter.
- Scenario: Validation error
    - Given a request `PUT /api/v1/notifications/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS057 — Delete a notification
As a frontend developer, I want to delete a notification through the API so that I can implement the notification removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/notifications/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/notifications/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS058 — Receive alerts and notifications in real time
As a frontend developer, I want to connect to the notifications hub so that I can show new alerts and notifications in my application without polling.
#### Acceptance criteria:
- Scenario: Alert created
    - Given a client connected to `/hubs/notifications`
    - When an alert is created
    - Then the API sends `AlertActivated` with the alert resource to the connected clients.
- Scenario: Notification created
    - Given a client connected to `/hubs/notifications` as the recipient user
    - When a notification is created
    - Then the API sends `NotificationReceived` with the notification resource only to the recipient user.
---

## Digital Experience / IoT Commerce

### TS059 — List the IoT device catalog
As a frontend developer, I want to request the catalog of IoT devices so that I can implement the device store in my application.
#### Acceptance criteria:
- Scenario: Catalog available
    - Given a request `GET /api/v1/iot-products` is received
    - When the API finds the products of the catalog
    - Then the API responds with `200 OK` and returns the Rastreador GPS Flotix (S/ 149.90), the Odómetro digital Flotix (S/ 89.90) and the Sensor de combustible Flotix (S/ 119.90), each one with `id`, `name` and `unitPrice`.
- Scenario: Empty catalog
    - Given a request `GET /api/v1/iot-products` is received
    - When the API finds no products
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS060 — Retrieve an IoT product by id
As a frontend developer, I want to request an IoT product by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-products/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the IoT product resource (`id`, `name`, `unitPrice`).
- Scenario: Not found
    - Given a request `GET /api/v1/iot-products/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS061 — List IoT orders by `ownerId`
As a frontend developer, I want to request the API to list IoT orders, optionally filtered by `{ownerId}`, so that I can implement the IoT order listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more IoT orders
    - Given a request `GET /api/v1/iot-orders?ownerId={ownerId}` is received
    - When the API finds one or more matching IoT order resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources ordered from the most recent to the oldest `orderDate`,, with each resource including `id`, `ownerId`, `productId`, `productName`, `quantity`, `totalAmount`, `status`, `orderDate`.
- Scenario: Returns no IoT orders
    - Given a request `GET /api/v1/iot-orders?ownerId={ownerId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS062 — Retrieve an IoT order by id
As a frontend developer, I want to request an IoT order by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/iot-orders/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the IoT order resource (`id`, `ownerId`, `productId`, `productName`, `quantity`, `totalAmount`, `status`, `orderDate`).
- Scenario: Not found
    - Given a request `GET /api/v1/iot-orders/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS063 — Place an IoT order
As a frontend developer, I want to place an order of IoT devices through the API so that I can implement the purchase feature in my application.
#### Acceptance criteria:
- Scenario: Successful create
    - Given a request `POST /api/v1/iot-orders` that includes required fields (`ownerId`, `productId`, `quantity`) is received
    - When the API validates and persists the new resource successfully
    - Then the API responds with `201 Created`, and returns the created resource representation (`id`, `ownerId`, `productId`, `productName`, `quantity`, `totalAmount`, `status`, `orderDate`) with status `Pendiente de pago` and a `totalAmount` equal to the `unitPrice` of the product multiplied by the `quantity`.
- Scenario: Invalid quantity
    - Given a request `POST /api/v1/iot-orders` with a `quantity` lower than or equal to zero, or not an integer, is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/iot-orders` with a `ownerId` or `productId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/iot-orders` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS064 — Confirm the payment of an IoT order
As a frontend developer, I want to notify the API that the payment of an IoT order was confirmed so that the order passes to preparation.
#### Acceptance criteria:
- Scenario: Payment confirmed
    - Given a request `POST /api/v1/iot-orders/{id}/payment-confirmation` for an order in `Pendiente de pago` status is received
    - When the API processes the payment confirmation
    - Then the API responds with `200 OK` and returns the order resource with status `En preparación`.
- Scenario: Invalid transition
    - Given a request `POST /api/v1/iot-orders/{id}/payment-confirmation` for an order that is not in `Pendiente de pago` status is received
    - When the API rejects the transition
    - Then the API responds with `409 Conflict` and a localized error message.
- Scenario: Not found
    - Given a request `POST /api/v1/iot-orders/{id}/payment-confirmation` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS065 — Advance the status of an IoT order
As a frontend developer, I want to advance the status of an IoT order until its delivery so that the follow-up of the purchase stays up to date.
#### Acceptance criteria:
- Scenario: Successful advance
    - Given a request `PUT /api/v1/iot-orders/{id}` that includes required fields (`status`) for an order in `Pendiente de pago` or `En preparación` status is received
    - When the API applies the transition to the next state of the flow (`Pendiente de pago`, `En preparación`, `Entregado`)
    - Then the API responds with `200 OK` and returns the updated resource representation (`id`, `ownerId`, `productId`, `productName`, `quantity`, `totalAmount`, `status`, `orderDate`).
- Scenario: Validation error
    - Given a request `PUT /api/v1/iot-orders/{id}` with missing required fields or invalid values is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Not found
    - Given a request `PUT /api/v1/iot-orders/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Closed order
    - Given a request `PUT /api/v1/iot-orders/{id}` for an order in `Entregado` or `Cancelado` status is received
    - When the API applies the order rules
    - Then the API responds with `409 Conflict` and a localized error message; the status does not change.
---

## Reporting & Analytics

### TS066 — Retrieve the operations dashboard
As a frontend developer, I want to request the consolidated operational indicators of the fleet so that I can show the owner what requires attention as soon as they enter the platform.
#### Acceptance criteria:
- Scenario: Operational summary
    - Given a request `GET /api/v1/dashboard?ownerId={ownerId}` is received and there are fleet records
    - When the API consolidates the indicators
    - Then the API responds with `200 OK` and returns `availableVehicles`, `inMaintenanceVehicles`, `fuelSpend`, `openAlerts`, `fleetStatus` and `pendingMaintenance`.
- Scenario: Urgent incident
    - Given at least one incident of the fleet is in `Abierto` status
    - When a request `GET /api/v1/dashboard?ownerId={ownerId}` is received
    - Then the response marks that incident as urgent.
- Scenario: No pending maintenance
    - Given there are no open maintenance requests
    - When a request `GET /api/v1/dashboard?ownerId={ownerId}` is received
    - Then the response indicates zero `pendingMaintenance`.
- Scenario: Referenced resource not found
    - Given a request `GET /api/v1/dashboard?ownerId={ownerId}` with a `ownerId` that does not exist is received
    - When the API does not find the owner
    - Then the API responds with `404 Not Found` and a localized error message.
---

### TS067 — Retrieve the consolidated fleet summary
As a frontend developer, I want to request the consolidated data of vehicles, fuel and maintenance so that I can show the total cost of the operation of the owner.
#### Acceptance criteria:
- Scenario: Consolidated data
    - Given a request `GET /api/v1/fleet-summary?ownerId={ownerId}` is received and there are fleet records
    - When the API consolidates the data
    - Then the API responds with `200 OK` and returns `vehicleCount`, `totalFuelSpend`, `totalMaintenanceSpend` and `completedMaintenance`.
- Scenario: No records
    - Given a request `GET /api/v1/fleet-summary?ownerId={ownerId}` is received
    - When the API finds no records
    - Then the API responds with `200 OK` with every counter and amount in zero.
---

### TS068 — Compare the fuel performance by vehicle
As a frontend developer, I want to request the comparison of the fuel spend and the average efficiency of each vehicle so that I can identify the least efficient units.
#### Acceptance criteria:
- Scenario: Comparison generated
    - Given a request `GET /api/v1/fleet-performance?ownerId={ownerId}` is received and there are vehicles with at least two fuel records
    - When the API builds the comparison
    - Then the API responds with `200 OK` and returns, for each vehicle, its `licensePlate`, `totalFuelCost` and `averageFuelEfficiency` in km/L.
- Scenario: Insufficient history
    - Given a vehicle has fewer than two fuel records
    - When the API builds the comparison
    - Then the API excludes that vehicle from the response.
---

### TS069 — Generate a report
As a frontend developer, I want to generate a fuel consumption or maintenance cost report of my fleet for a date range so that I can implement the reports feature in my application.
#### Acceptance criteria:
- Scenario: Report with records
    - Given a request `POST /api/v1/reports` that includes required fields (`ownerId`, `reportType`, `from`, `to`) is received
    - When the API finds fuel records or maintenance requests of the owner vehicles within the period
    - Then the API responds with `201 Created` and returns the generated report (`id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`, `lines`, `totalRecords`, `totalQuantity`, `totalCost`), with one line per vehicle; the report is registered with its type and generation date.
- Scenario: Report without records
    - Given a request `POST /api/v1/reports` for a period without records is received
    - When the API finds no records
    - Then the API responds with `201 Created`, an empty `lines` array and a localized informative `message`.
- Scenario: Validation error
    - Given a request `POST /api/v1/reports` with an unsupported `reportType` (only `FuelConsumption` and `MaintenanceCost` are supported) or `from` after `to` is received
    - When the API validation fails
    - Then the API responds with `400 Bad Request` and a localized error message describing the validation failure.
- Scenario: Referenced resource not found
    - Given a request `POST /api/v1/reports` with a `ownerId` that does not exist is received
    - When the API cannot find the referenced resource during persistence confirmation
    - Then the API responds with `404 Not Found` and a localized error message.
- Scenario: Unexpected server error
    - Given a request `POST /api/v1/reports` is received
    - When an unexpected error occurs during persistence
    - Then the API responds with `500 Internal Server Error` and a `ProblemDetails` payload.
---

### TS070 — Export a report
As a frontend developer, I want to download a generated report as a file so that the owner can share or archive it.
#### Acceptance criteria:
- Scenario: Successful export
    - Given a request `GET /api/v1/reports/{id}/export` is received
    - When the API finds the report
    - Then the API responds with `200 OK` and a downloadable file attachment that contains the consolidated summary and the detail per vehicle.
- Scenario: Not found
    - Given a request `GET /api/v1/reports/{id}/export` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS071 — List reports by `ownerId`
As a frontend developer, I want to request the API to list reports, optionally filtered by `{ownerId}`, so that I can implement the report listing feature in my application.
#### Acceptance criteria:
- Scenario: Returns one or more reports
    - Given a request `GET /api/v1/reports?ownerId={ownerId}` is received
    - When the API finds one or more matching report resources
    - Then the API responds with `200 OK` and returns a non-empty array of resources with each resource including `id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`.
- Scenario: Returns no reports
    - Given a request `GET /api/v1/reports?ownerId={ownerId}` is received
    - When the API finds no matching resources
    - Then the API responds with `200 OK` and returns an empty array.
---

### TS072 — Retrieve a report by id
As a frontend developer, I want to request a report by its `{id}` so that I can implement the details view in my application.
#### Acceptance criteria:
- Scenario: Found
    - Given a request `GET /api/v1/reports/{id}` is received
    - When the API finds the resource
    - Then the API responds with `200 OK` and returns the report resource (`id`, `ownerId`, `reportType`, `generatedDate`, `filterParams`).
- Scenario: Not found
    - Given a request `GET /api/v1/reports/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found`.
---

### TS073 — Delete a report
As a frontend developer, I want to delete a report through the API so that I can implement the report removal feature in my application.
#### Acceptance criteria:
- Scenario: Successful delete
    - Given a request `DELETE /api/v1/reports/{id}` is received
    - When the API finds and removes the resource
    - Then the API responds with `204 No Content`.
- Scenario: Not found
    - Given a request `DELETE /api/v1/reports/{id}` for a non-existent `{id}` is received
    - When the API does not find the resource
    - Then the API responds with `404 Not Found` and a localized error message.
---

## API Documentation

### TS074 — Read the API description
As a frontend developer, I want to request the OpenAPI description of the API, rendered with Scalar so that I can consume the API without ambiguity.
#### Acceptance criteria:
- Scenario: Documentation available
    - Given the service is deployed and a request `GET /openapi/v1.json` is received
    - When the API builds the description document
    - Then the API responds with `200 OK` and the endpoints, parameters, schemas and response codes of each bounded context.
- Scenario: Documentation updated
    - Given an endpoint is added or modified
    - When the description document is generated
    - Then the document reflects the change.
---

### TS075 — Read the API description in my language
As a frontend developer, I want the API description document to be served in my preferred language so that I can understand the endpoints and their responses without translating them.
#### Acceptance criteria:
- Scenario: Supported language requested
    - Given a request `GET /openapi/v1.json` with an `Accept-Language` header for a supported culture (`en`, `en-US`, `es`, `es-PE`) is received
    - When the API builds the description document
    - Then the API responds with `200 OK` and the operation summaries, descriptions, response descriptions and resource property descriptions are in that language.
- Scenario: Unsupported or missing language
    - Given a request `GET /openapi/v1.json` without an `Accept-Language` header, or with an unsupported one, is received
    - When the API builds the description document
    - Then the API responds with `200 OK` and the texts are in the default language.
---

## Traceability with Chapter 3

| Technical Story | Title | Endpoint | Chapter 3 |
|---|---|---|---|
| TS001 | Sign up a user | `POST /api/v1/authentication/sign-up` | EP01, US05-US06 |
| TS002 | Sign in a user | `POST /api/v1/authentication/sign-in` | EP01 |
| TS003 | List and search users by `roleType` | `GET /api/v1/users` | US06, US07, TS07 |
| TS004 | Retrieve the drivers summary | `GET /api/v1/users/drivers-summary` | US06, TS07 |
| TS005 | Retrieve a user by id | `GET /api/v1/users/{id}` | TS07 |
| TS006 | Update a user | `PUT /api/v1/users/{id}` | TS07 |
| TS007 | Delete a user | `DELETE /api/v1/users/{id}` | TS07 |
| TS008 | List vehicles by `ownerId` or `driverId` | `GET /api/v1/vehicles` | US04, TS01 |
| TS009 | Retrieve the fleet summary | `GET /api/v1/vehicles/summary` | US04 |
| TS010 | Retrieve a vehicle by id | `GET /api/v1/vehicles/{id}` | US04, TS01 |
| TS011 | Register a vehicle | `POST /api/v1/vehicles` | US01, TS01 |
| TS012 | Update a vehicle | `PUT /api/v1/vehicles/{id}` | US02, TS05 |
| TS013 | Assign a driver to a vehicle | `POST /api/v1/vehicles/{id}/driver-assignment` | US05, TS06 |
| TS014 | Unassign the driver of a vehicle | `DELETE /api/v1/vehicles/{id}/driver-assignment` | US05, TS06 |
| TS015 | Delete a vehicle | `DELETE /api/v1/vehicles/{id}` | US03, TS05 |
| TS016 | List fuel records by `vehicleId` or `driverId` | `GET /api/v1/fuel-records` | US09, TS09 |
| TS017 | Retrieve a fuel record by id | `GET /api/v1/fuel-records/{id}` | US09, TS09 |
| TS018 | Register a fuel record | `POST /api/v1/fuel-records` | US08, TS08 |
| TS019 | Update a fuel record | `PUT /api/v1/fuel-records/{id}` | US09, TS08 |
| TS020 | Delete a fuel record | `DELETE /api/v1/fuel-records/{id}` | US12, TS11 |
| TS021 | Retrieve the fuel summary | `GET /api/v1/fuel-records/summary` | US11, TS11 |
| TS022 | List maintenance requests by `vehicleId` or `mechanicId` | `GET /api/v1/maintenance-requests` | US18, TS12 |
| TS023 | Retrieve the maintenance summary | `GET /api/v1/maintenance-requests/summary` | US18, TS12 |
| TS024 | Retrieve a maintenance request by id | `GET /api/v1/maintenance-requests/{id}` | US18, TS12 |
| TS025 | Submit a maintenance request | `POST /api/v1/maintenance-requests` | US13, US22, TS12 |
| TS026 | Register the diagnostic and quote of a maintenance request | `POST /api/v1/maintenance-requests/{id}/diagnostic-and-quote` | US14, TS03 |
| TS027 | Approve or reject the budget of a maintenance request | `POST /api/v1/maintenance-requests/{id}/budget-decision` | US15, TS03 |
| TS028 | Start the repair of a maintenance request | `POST /api/v1/maintenance-requests/{id}/repair-start` | US16, TS13 |
| TS029 | Complete a maintenance request | `POST /api/v1/maintenance-requests/{id}/completion` | US17, TS13 |
| TS030 | Delete a maintenance request | `DELETE /api/v1/maintenance-requests/{id}` | US19, TS15 |
| TS031 | List and search affiliated workshops | `GET /api/v1/workshops` | US20, US21, TS14 |
| TS032 | List incidents by `vehicleId` or `driverId` | `GET /api/v1/incidents` | US26, TS18 |
| TS033 | Retrieve the incidents summary | `GET /api/v1/incidents/summary` | US26, TS18 |
| TS034 | Retrieve an incident by id | `GET /api/v1/incidents/{id}` | US26, TS18 |
| TS035 | Report an incident | `POST /api/v1/incidents` | US23, US31, TS16 |
| TS036 | Start the review of an incident | `POST /api/v1/incidents/{id}/review` | US24, TS17 |
| TS037 | Resolve an incident | `POST /api/v1/incidents/{id}/resolution` | US25, TS17 |
| TS038 | Delete an incident | `DELETE /api/v1/incidents/{id}` | US27, TS18 |
| TS039 | List IoT devices by `vehicleId` | `GET /api/v1/iot-devices` | TS19 |
| TS040 | Retrieve an IoT device by id | `GET /api/v1/iot-devices/{id}` | TS19 |
| TS041 | Link an IoT device to a vehicle | `POST /api/v1/iot-devices` | TS19 |
| TS042 | Update an IoT device | `PUT /api/v1/iot-devices/{id}` | TS19 |
| TS043 | Delete an IoT device | `DELETE /api/v1/iot-devices/{id}` | TS19 |
| TS044 | Receive IoT telemetry data | `POST /api/v1/telemetry-data` | US28, TS02, TS19 |
| TS045 | List telemetry data records by `deviceId` | `GET /api/v1/telemetry-data` | US28, TS20 |
| TS046 | Retrieve a telemetry data record by id | `GET /api/v1/telemetry-data/{id}` | TS20 |
| TS047 | Retrieve the fleet monitoring status | `GET /api/v1/fleet-monitoring` | US28, US29, TS20 |
| TS048 | Generate speeding alerts from telemetry | `Internal rule (IoT Telemetry Processor)` | US30, TS21 |
| TS049 | List alerts by `vehicleId` | `GET /api/v1/alerts` | US32, TS22 |
| TS050 | Retrieve an alert by id | `GET /api/v1/alerts/{id}` | US32, TS22 |
| TS051 | Create an alert | `POST /api/v1/alerts` | TS21 |
| TS052 | List notifications by `userId` | `GET /api/v1/notifications` | US32, TS22 |
| TS053 | Retrieve the alerts and notifications summary | `GET /api/v1/notifications/summary` | US32, TS22 |
| TS054 | Retrieve a notification by id | `GET /api/v1/notifications/{id}` | US32, TS22 |
| TS055 | Create a notification | `POST /api/v1/notifications` | US31, TS16 |
| TS056 | Mark a notification as read | `PUT /api/v1/notifications/{id}` | US33, TS23 |
| TS057 | Delete a notification | `DELETE /api/v1/notifications/{id}` | US32 |
| TS058 | Receive alerts and notifications in real time | `WebSocket /hubs/notifications` | US30, US31, US32 |
| TS059 | List the IoT device catalog | `GET /api/v1/iot-products` | US34, TS24 |
| TS060 | Retrieve an IoT product by id | `GET /api/v1/iot-products/{id}` | TS24 |
| TS061 | List IoT orders by `ownerId` | `GET /api/v1/iot-orders` | US36, TS26 |
| TS062 | Retrieve an IoT order by id | `GET /api/v1/iot-orders/{id}` | US36, TS26 |
| TS063 | Place an IoT order | `POST /api/v1/iot-orders` | US35, TS25 |
| TS064 | Confirm the payment of an IoT order | `POST /api/v1/iot-orders/{id}/payment-confirmation` | US35, TS25 |
| TS065 | Advance the status of an IoT order | `PUT /api/v1/iot-orders/{id}` | US37, TS27 |
| TS066 | Retrieve the operations dashboard | `GET /api/v1/dashboard` | US38, TS28 |
| TS067 | Retrieve the consolidated fleet summary | `GET /api/v1/fleet-summary` | US39, TS29 |
| TS068 | Compare the fuel performance by vehicle | `GET /api/v1/fleet-performance` | US40, TS29 |
| TS069 | Generate a report | `POST /api/v1/reports` | US41, TS04, TS30 |
| TS070 | Export a report | `GET /api/v1/reports/{id}/export` | US41, TS30 |
| TS071 | List reports by `ownerId` | `GET /api/v1/reports` | US42, TS30 |
| TS072 | Retrieve a report by id | `GET /api/v1/reports/{id}` | US42, TS30 |
| TS073 | Delete a report | `DELETE /api/v1/reports/{id}` | US42, TS30 |
| TS074 | Read the API description | `GET /openapi/v1.json` | TS31 |
| TS075 | Read the API description in my language | `GET /openapi/v1.json` | TS31 |