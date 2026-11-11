

IF DB_ID('GimnasioDB') IS NULL
    CREATE DATABASE GimnasioDB;
GO
USE GimnasioDB;
GO

IF OBJECT_ID('dbo.Socios', 'U') IS NOT NULL DROP TABLE dbo.Socios;
IF OBJECT_ID('dbo.Planes', 'U') IS NOT NULL DROP TABLE dbo.Planes;
GO

CREATE TABLE dbo.Planes (
    PlanId         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Planes PRIMARY KEY,
    Nombre         NVARCHAR(50)      NOT NULL CONSTRAINT UQ_Planes_Nombre UNIQUE,
    Precio         DECIMAL(8,2)      NOT NULL CONSTRAINT CK_Planes_Precio CHECK (Precio > 0),
    DuracionMeses  INT               NOT NULL CONSTRAINT CK_Planes_Duracion CHECK (DuracionMeses BETWEEN 1 AND 24),
    Activo         BIT               NOT NULL CONSTRAINT DF_Planes_Activo DEFAULT (1)
);
GO

CREATE TABLE dbo.Socios (
    SocioId           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Socios PRIMARY KEY,
    Nombres           NVARCHAR(100)     NOT NULL,
    Dni               CHAR(8)           NOT NULL CONSTRAINT UQ_Socios_Dni UNIQUE,
    Email             NVARCHAR(100)     NULL,
    Edad              INT               NOT NULL CONSTRAINT CK_Socios_Edad CHECK (Edad BETWEEN 14 AND 90),
    FechaInscripcion  DATE              NOT NULL CONSTRAINT DF_Socios_Fecha DEFAULT (CAST(GETDATE() AS DATE)),
    PlanId            INT               NOT NULL
        CONSTRAINT FK_Socios_Planes REFERENCES dbo.Planes(PlanId)
);
GO

CREATE INDEX IX_Socios_PlanId ON dbo.Socios(PlanId);
GO

INSERT INTO dbo.Planes (Nombre, Precio, DuracionMeses, Activo) VALUES
(N'Mensual Básico',   89.90,  1, 1),
(N'Trimestral',      239.90,  3, 1),
(N'Anual Premium',   899.00, 12, 1),
(N'Estudiante',       69.90,  1, 1),
(N'Promo Verano',    149.90,  2, 0);
GO

INSERT INTO dbo.Socios (Nombres, Dni, Email, Edad, FechaInscripcion, PlanId) VALUES
(N'Luis Mendoza Quispe',   '45781236', 'luis.mendoza@mail.com',   28, '2026-08-01', 1),
(N'Carla Huamán Rojas',    '71234598', 'carla.huaman@mail.com',   22, '2026-08-15', 4),
(N'Pedro Salazar Díaz',    '40125987', NULL,                      35, '2026-07-10', 3),
(N'Sofía Paredes León',    '72589641', 'sofia.paredes@mail.com',  19, '2026-09-03', 4),
(N'Jorge Castillo Vega',   '41569874', 'jorge.castillo@mail.com', 42, '2026-06-20', 2),
(N'Valeria Torres Ríos',   '74125963', 'valeria.torres@mail.com', 26, '2026-10-01', 1);
GO

SELECT * FROM dbo.Planes;
SELECT * FROM dbo.Socios;
GO
