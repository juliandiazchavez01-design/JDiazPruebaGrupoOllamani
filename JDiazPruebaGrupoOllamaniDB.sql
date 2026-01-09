CREATE DATABASE JDiazPruebaGrupoOllamani

CREATE TABLE Departamento 
(
	IdDepartamento INT IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(50) NOT NULL
)

CREATE TABLE Empleado  
(
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    ApellidoPaterno VARCHAR(50) NOT NULL,
    ApellidoMaterno VARCHAR(50) NOT NULL,
    Salario DECIMAL(10,4) NOT NULL,
    Activo BIT,
    FechaRegistro DATE,
    IdDepartamento INT NOT NULL,
	FOREIGN KEY (IdDepartamento) REFERENCES Departamento(IdDepartamento)
)

CREATE PROCEDURE EmpleadoCRUD
(
	@Accion VARCHAR(20), 
    @IdEmpleado INT = NULL,
    @Nombre VARCHAR(50) = NULL,
    @ApellidoPaterno VARCHAR(50) = NULL,
    @ApellidoMaterno VARCHAR(50) = NULL,
    @Salario DECIMAL(18,2) = NULL,
    @Activo BIT = NULL,
    @IdDepartamento INT = NULL
)
AS
BEGIN
SET NOCOUNT ON;
	IF @Accion = 'Add'
	BEGIN 
		INSERT INTO Empleado (
			Nombre, 
			ApellidoPaterno, 
			ApellidoMaterno, 
			Salario, 
			Activo, 
			FechaRegistro, 
			IdDepartamento) 
		VALUES(@Nombre,
			@ApellidoPaterno, 
			@ApellidoMaterno, 
			@Salario, @Activo, 
			GETDATE(), 
			@IdDepartamento);
	END 

	ELSE IF @Accion = 'Update'
	BEGIN
		UPDATE	Empleado 
		SET Nombre = @Nombre, 
			ApellidoPaterno = @ApellidoPaterno, 
			ApellidoMaterno = @ApellidoMaterno,
			Salario = @Salario, 
			Activo = @Activo, 
			IdDepartamento = @IdDepartamento
		WHERE IdEmpleado = @IdEmpleado
	END

	ELSE IF @Accion = 'Delete'
	BEGIN
		DELETE FROM Empleado
		WHERE IdEmpleado = @IdEmpleado
	END

	ELSE IF @Accion = 'GetById'
	BEGIN
		SELECT IdEmpleado, 
			Nombre,
			ApellidoPaterno,
			ApellidoMaterno,
			Salario, Activo, 
			FechaRegistro, 
			IdDepartamento
		FROM Empleado
		WHERE IdEmpleado = @IdEmpleado
	END

	ELSE IF @Accion = 'GetAll'
	BEGIN
		SELECT Empleado.IdEmpleado, 
			Empleado.Nombre AS NombreEmpleado, 
			Empleado.ApellidoPaterno, 
			Empleado.ApellidoMaterno,
			Empleado.Salario, Empleado.Activo, 
			Empleado.FechaRegistro, 
			Departamento.IdDepartamento, 
			Departamento.Nombre AS NombreDepartamento
		FROM Empleado
		INNER JOIN Departamento ON Empleado.IdDepartamento = Departamento.IdDepartamento
	END
END

EXEC EmpleadoCRUD @Accion = 'GetAll'