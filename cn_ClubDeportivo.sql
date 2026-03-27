/* Tablas */
CREATE TABLE Miembros (
    ID_Miembro NUMBER PRIMARY KEY,
    Nombre VARCHAR2(50) NOT NULL,
    Apellido VARCHAR2(50) NOT NULL,
    Fecha_Nacimiento DATE NOT NULL,
    Telefono VARCHAR2(15),
    Email VARCHAR2(100),
    Direccion VARCHAR2(150),
    Estado VARCHAR2(10) DEFAULT 'ACTIVO' NOT NULL
);

CREATE TABLE Entrenadores (
    ID_Entrenador NUMBER PRIMARY KEY,
    Nombre VARCHAR2(50) NOT NULL,
    Apellido VARCHAR2(50) NOT NULL,
    Especialidad VARCHAR2(100),
    Teléfono VARCHAR2(15),
    Email VARCHAR2(100)
);

CREATE TABLE Actividades (
    ID_Actividad NUMBER PRIMARY KEY,
    Nombre_Actividad VARCHAR2(100) NOT NULL,
    Descripción VARCHAR2(250),
    Horario VARCHAR2(50),
    Duración NUMBER,
    ID_Entrenador NUMBER,
    CONSTRAINT fk_Entrenador FOREIGN KEY (ID_Entrenador) REFERENCES Entrenadores(ID_Entrenador)
);

CREATE TABLE Participaciones (
    ID_Miembro NUMBER,
    ID_Actividad NUMBER,
    Fecha_Inscripción DATE DEFAULT SYSDATE,
    PRIMARY KEY (ID_Miembro, ID_Actividad),
    CONSTRAINT fk_Miembro FOREIGN KEY (ID_Miembro) REFERENCES Miembros(ID_Miembro),
    CONSTRAINT fk_Actividad FOREIGN KEY (ID_Actividad) REFERENCES Actividades(ID_Actividad)
);

/* Secuencias */
CREATE SEQUENCE seq_idmiembro
  START WITH 1
  INCREMENT BY 1
  NOCACHE;

CREATE SEQUENCE seq_identrenador
  START WITH 1
  INCREMENT BY 1
  NOCACHE;

CREATE SEQUENCE seq_idactividad
  START WITH 1
  INCREMENT BY 1
  NOCACHE;

/* Vistas y Procedimientos */
CREATE OR REPLACE VIEW vw_MiembrosActivos AS
SELECT ID_Miembro, Nombre, Apellido, Fecha_Nacimiento, Telefono, Email, Direccion, Estado
FROM Miembros
WHERE UPPER(Estado) = 'ACTIVO';
-------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE sp_InsertarMiembro(
    p_Nombre IN Miembros.Nombre%TYPE,
    p_Apellido IN Miembros.Apellido%TYPE,
    p_Fecha_Nacimiento IN Miembros.Fecha_Nacimiento%TYPE,
    p_Telefono IN Miembros.Telefono%TYPE DEFAULT NULL,
    p_Email IN Miembros.Email%TYPE DEFAULT NULL,
    p_Direccion IN Miembros.Direccion%TYPE DEFAULT NULL,
    p_Estado IN Miembros.Estado%TYPE DEFAULT 'ACTIVO'
)
AS
BEGIN
    INSERT INTO Miembros (ID_Miembro,Nombre,Apellido,Fecha_Nacimiento,Telefono,Email,Direccion,Estado) 
    VALUES (seq_idmiembro.NEXTVAL,p_Nombre,p_Apellido,p_Fecha_Nacimiento,p_Telefono,p_Email,p_Direccion,p_Estado);
    COMMIT;
END sp_InsertarMiembro;

CREATE OR REPLACE PROCEDURE sp_ActualizarMiembro(
    p_ID_Miembro IN Miembros.ID_Miembro%TYPE,
    p_Nombre IN Miembros.Nombre%TYPE,
    p_Apellido IN Miembros.Apellido%TYPE,
    p_Fecha_Nacimiento IN Miembros.Fecha_Nacimiento%TYPE,
    p_Telefono IN Miembros.Telefono%TYPE,
    p_Email IN Miembros.Email%TYPE,
    p_Direccion IN Miembros.Direccion%TYPE
)
AS
BEGIN
    UPDATE Miembros
    SET
        Nombre = p_Nombre,
        Apellido = p_Apellido,
        Fecha_Nacimiento = p_Fecha_Nacimiento,
        Telefono = p_Telefono,
        Email = p_Email,
        Direccion = p_Direccion
    WHERE ID_Miembro = p_ID_Miembro
      AND UPPER(Estado) = 'ACTIVO';

    COMMIT;
END sp_ActualizarMiembro;

CREATE OR REPLACE PROCEDURE sp_EliminarMiembro (
    p_id IN Miembros.ID_Miembro%TYPE
)
AS
BEGIN
    UPDATE Miembros
    SET Estado = 'INACTIVO'
    WHERE ID_Miembro = p_id;

    COMMIT;
END sp_EliminarMiembro;

CREATE OR REPLACE PROCEDURE sp_ObtenerMiembroPorId (
    p_id IN Miembros.ID_Miembro%TYPE,
    p_cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_cursor FOR
        SELECT
            ID_Miembro,
            Nombre,
            Apellido,
            Fecha_Nacimiento,
            Telefono,
            Email,
            Direccion,
            Estado
        FROM Miembros
        WHERE ID_Miembro = p_id
          AND UPPER(Estado) = 'ACTIVO';
END sp_ObtenerMiembroPorId;



