-- 1. CREACIÓN DE LA BASE DE DATOS
CREATE DATABASE HotelDB;
GO

USE HotelDB;
GO

-- 2. TABLAS
-- Tabla Huesped
CREATE TABLE Huesped (
    IdHuesped INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE,
    Telefono NVARCHAR(20)
);

-- Tabla Habitacion
CREATE TABLE Habitacion (
    IdHabitacion INT PRIMARY KEY IDENTITY(1,1),
    Numero NVARCHAR(10) UNIQUE,
    Tipo NVARCHAR(50) NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Estado NVARCHAR(20) DEFAULT 'Disponible'
);

-- Tabla Reserva
CREATE TABLE Reserva (
    IdReserva INT PRIMARY KEY IDENTITY(1,1),
    IdHuesped INT FOREIGN KEY REFERENCES Huesped(IdHuesped),
    IdHabitacion INT FOREIGN KEY REFERENCES Habitacion(IdHabitacion),
    FechaEntrada DATE NOT NULL,
    FechaSalida DATE NOT NULL,
    Estado NVARCHAR(20) DEFAULT 'Pendiente'
);
GO

-- 3. PROCEDIMIENTOS ALMACENADOS

-- Listar habitaciones

CREATE PROCEDURE sp_ListarHabitaciones
AS
BEGIN
    SELECT * FROM Habitacion;
END
GO

-- Procedimiento para filtrar habitaciones por tipo
CREATE PROCEDURE sp_FiltrarHabitacionesPorTipo
    @Tipo NVARCHAR(50)
AS
BEGIN
    SELECT * FROM Habitacion 
    WHERE Tipo = @Tipo
    ORDER BY Numero;
END
GO

-- Procedimiento para cambiar el estado de una habitación
CREATE PROCEDURE sp_CambiarEstadoHabitacion
    @IdHabitacion INT,
    @Estado NVARCHAR(20)
AS
BEGIN
    UPDATE Habitacion 
    SET Estado = @Estado 
    WHERE IdHabitacion = @IdHabitacion;
END
GO

-- Procedimiento para crear una nueva habitación
CREATE PROCEDURE sp_CrearHabitacion
    @Numero NVARCHAR(10),
    @Tipo NVARCHAR(50),
    @Precio DECIMAL(10, 2),
    @Estado NVARCHAR(20) = 'Disponible'
AS
BEGIN
    INSERT INTO Habitacion (Numero, Tipo, Precio, Estado)
    VALUES (@Numero, @Tipo, @Precio, @Estado);
    
    SELECT SCOPE_IDENTITY() AS IdHabitacion;
END
GO

-- Procedimiento para actualizar una habitación
CREATE PROCEDURE sp_ActualizarHabitacion
    @IdHabitacion INT,
    @Numero NVARCHAR(10),
    @Tipo NVARCHAR(50),
    @Precio DECIMAL(10, 2),
    @Estado NVARCHAR(20)
AS
BEGIN
    UPDATE Habitacion 
    SET 
        Numero = @Numero,
        Tipo = @Tipo,
        Precio = @Precio,
        Estado = @Estado
    WHERE IdHabitacion = @IdHabitacion;
END
GO

-- Procedimiento para eliminar una habitación (solo si no tiene reservas)
CREATE PROCEDURE sp_EliminarHabitacion
    @IdHabitacion INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Reserva WHERE IdHabitacion = @IdHabitacion)
    BEGIN
        DELETE FROM Habitacion WHERE IdHabitacion = @IdHabitacion;
        SELECT 1 AS Resultado; -- Éxito
    END
    ELSE
    BEGIN
        SELECT 0 AS Resultado; -- Fallo (tiene reservas asociadas)
    END
END
GO


-- Listar huéspedes

CREATE PROCEDURE sp_ListarHuespedes
AS
BEGIN
    SELECT * FROM Huesped;
END
GO

-- Procedimiento para crear un nuevo huésped
CREATE PROCEDURE sp_CrearHuesped
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20)
AS
BEGIN
    INSERT INTO Huesped (Nombre, Apellido, Email, Telefono)
    VALUES (@Nombre, @Apellido, @Email, @Telefono);
    
    -- Retornar el ID del nuevo huésped
    SELECT SCOPE_IDENTITY() AS IdHuesped;
END
GO

-- Procedimiento para obtener un huésped por ID
CREATE PROCEDURE sp_ObtenerHuesped
    @IdHuesped INT
AS
BEGIN
    SELECT * FROM Huesped WHERE IdHuesped = @IdHuesped;
END
GO

-- Procedimiento para actualizar un huésped
CREATE PROCEDURE sp_ActualizarHuesped
    @IdHuesped INT,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20)
AS
BEGIN
    UPDATE Huesped 
    SET 
        Nombre = @Nombre,
        Apellido = @Apellido,
        Email = @Email,
        Telefono = @Telefono
    WHERE IdHuesped = @IdHuesped;
END
GO

-- Procedimiento para eliminar un huésped
CREATE PROCEDURE sp_EliminarHuesped
    @IdHuesped INT
AS
BEGIN
    -- Verificar si el huésped tiene reservas antes de eliminar
    IF NOT EXISTS (SELECT 1 FROM Reserva WHERE IdHuesped = @IdHuesped)
    BEGIN
        DELETE FROM Huesped WHERE IdHuesped = @IdHuesped;
        SELECT 1 AS Resultado; -- Éxito
    END
    ELSE
    BEGIN
        SELECT 0 AS Resultado; -- Fallo (tiene reservas asociadas)
    END
END
GO

-- Listar reservas

CREATE PROCEDURE sp_ListarReservas
AS
BEGIN
    SELECT 
        r.IdReserva,
        h.Nombre + ' ' + h.Apellido AS Huesped,
        hab.Numero AS Habitacion,
        r.FechaEntrada,
        r.FechaSalida,
        r.Estado
    FROM Reserva r
    INNER JOIN Huesped h ON r.IdHuesped = h.IdHuesped
    INNER JOIN Habitacion hab ON r.IdHabitacion = hab.IdHabitacion;
END
GO

-- 4. INSERTAR DATOS 

-- Huespedes

INSERT INTO Huesped (Nombre, Apellido, Email, Telefono) VALUES
('Juan', 'Pérez', 'juan.perez@email.com', '555-1234'),
('María', 'Gómez', 'maria.gomez@email.com', '555-5678'),
('Carlos', 'López', 'carlos.lopez@email.com', '555-9012'),
('Ana', 'Martínez', 'ana.martinez@email.com', '555-3456'),
('Luis', 'Rodríguez', 'luis.rodriguez@email.com', '555-7890'),
('Laura', 'Hernández', 'laura.hernandez@email.com', '555-2345'),
('Pedro', 'Díaz', 'pedro.diaz@email.com', '555-6789'),
('Sofía', 'Torres', 'sofia.torres@email.com', '555-0123'),
('Jorge', 'Ramírez', 'jorge.ramirez@email.com', '555-4567'),
('Elena', 'Flores', 'elena.flores@email.com', '555-8901'),
('Ricardo', 'Vargas', 'ricardo.vargas@email.com', '555-6543'),
('Diana', 'Cruz', 'diana.cruz@email.com', '555-0987'),
('Fernando', 'Morales', 'fernando.morales@email.com', '555-4321'),
('Gabriela', 'Ortiz', 'gabriela.ortiz@email.com', '555-8765'),
('Roberto', 'Silva', 'roberto.silva@email.com', '555-2109'),
('Patricia', 'Reyes', 'patricia.reyes@email.com', '555-6543'),
('Miguel', 'Mendoza', 'miguel.mendoza@email.com', '555-0987'),
('Lucía', 'Guerrero', 'lucia.guerrero@email.com', '555-5432'),
('José', 'Navarro', 'jose.navarro@email.com', '555-9876'),
('Carmen', 'Santos', 'carmen.santos@email.com', '555-3210');
GO

-- Habitaciones

INSERT INTO Habitacion (Numero, Tipo, Precio, Estado) VALUES
('101', 'Individual', 50.00, 'Disponible'),
('102', 'Individual', 50.00, 'Disponible'),
('103', 'Doble', 80.00, 'Disponible'),
('104', 'Doble', 80.00, 'Disponible'),
('105', 'Suite', 120.00, 'Disponible'),
('201', 'Individual', 55.00, 'Disponible'),
('202', 'Individual', 55.00, 'Disponible'),
('203', 'Doble', 85.00, 'Disponible'),
('204', 'Doble', 85.00, 'Disponible'),
('205', 'Suite', 130.00, 'Disponible'),
('301', 'Individual', 60.00, 'Disponible'),
('302', 'Individual', 60.00, 'Disponible'),
('303', 'Doble', 90.00, 'Disponible'),
('304', 'Doble', 90.00, 'Disponible'),
('305', 'Suite', 140.00, 'Disponible'),
('401', 'Individual', 65.00, 'Disponible'),
('402', 'Individual', 65.00, 'Disponible'),
('403', 'Doble', 95.00, 'Disponible'),
('404', 'Doble', 95.00, 'Disponible'),
('405', 'Suite', 150.00, 'Disponible');
GO

-- Reservas (usando IDs aleatorios de las tablas Huesped y Habitacion)

INSERT INTO Reserva (IdHuesped, IdHabitacion, FechaEntrada, FechaSalida, Estado) VALUES
(1, 3, '2023-10-01', '2023-10-05', 'Confirmada'),
(2, 5, '2023-10-02', '2023-10-07', 'Confirmada'),
(3, 7, '2023-10-03', '2023-10-06', 'Pendiente'),
(4, 9, '2023-10-04', '2023-10-08', 'Cancelada'),
(5, 11, '2023-10-05', '2023-10-10', 'Confirmada'),
(6, 13, '2023-10-06', '2023-10-09', 'Confirmada'),
(7, 15, '2023-10-07', '2023-10-12', 'Pendiente'),
(8, 17, '2023-10-08', '2023-10-11', 'Confirmada'),
(9, 19, '2023-10-09', '2023-10-14', 'Cancelada'),
(10, 2, '2023-10-10', '2023-10-13', 'Confirmada'),
(11, 4, '2023-10-11', '2023-10-15', 'Pendiente'),
(12, 6, '2023-10-12', '2023-10-16', 'Confirmada'),
(13, 8, '2023-10-13', '2023-10-17', 'Confirmada'),
(14, 10, '2023-10-14', '2023-10-18', 'Cancelada'),
(15, 12, '2023-10-15', '2023-10-19', 'Pendiente'),
(16, 14, '2023-10-16', '2023-10-20', 'Confirmada'),
(17, 16, '2023-10-17', '2023-10-21', 'Confirmada'),
(18, 18, '2023-10-18', '2023-10-22', 'Pendiente'),
(19, 20, '2023-10-19', '2023-10-23', 'Confirmada'),
(20, 1, '2023-10-20', '2023-10-24', 'Cancelada');
GO

-- 5. PROCEDIMIENTO PARA CREAR RESERVA (Extra)

CREATE PROCEDURE sp_CrearReserva
    @IdHuesped INT,
    @IdHabitacion INT,
    @FechaEntrada DATE,
    @FechaSalida DATE
AS
BEGIN
    INSERT INTO Reserva (IdHuesped, IdHabitacion, FechaEntrada, FechaSalida)
    VALUES (@IdHuesped, @IdHabitacion, @FechaEntrada, @FechaSalida);
    
    UPDATE Habitacion SET Estado = 'Ocupada' WHERE IdHabitacion = @IdHabitacion;
END
GO

PRINT '¡Base de datos HotelDB creada y poblada con éxito! 🎉';

