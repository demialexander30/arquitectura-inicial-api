
CREATE DATABASE SalesOnlineDb
GO

USE SalesOnlineDb
GO

CREATE TABLE Passengers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    DocumentNumber NVARCHAR(50) NOT NULL,
    DocumentType NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    DateOfBirth DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
)
GO


CREATE TABLE Flights (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FlightNumber NVARCHAR(20) NOT NULL,
    Origin NVARCHAR(100) NOT NULL,
    Destination NVARCHAR(100) NOT NULL,
    DepartureTime DATETIME NOT NULL,
    ArrivalTime DATETIME NOT NULL,
    BasePrice DECIMAL(18,2) NOT NULL,
    TotalSeats INT NOT NULL,
    AvailableSeats INT NOT NULL,
    AircraftType NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
)
GO


CREATE TABLE CrewMembers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(50) NOT NULL,
    LicenseNumber NVARCHAR(50) NOT NULL,
    LicenseExpiry DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0
)
GO


CREATE TABLE Bookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BookingNumber NVARCHAR(20) NOT NULL,
    PassengerId INT NOT NULL,
    FlightId INT NOT NULL,
    SeatNumber NVARCHAR(10) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (PassengerId) REFERENCES Passengers(Id),
    FOREIGN KEY (FlightId) REFERENCES Flights(Id)
)
GO


CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL UNIQUE,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    TransactionId NVARCHAR(100) NOT NULL,
    PaymentDate DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (BookingId) REFERENCES Bookings(Id)
)
GO


CREATE TABLE CrewAssignments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FlightId INT NOT NULL,
    CrewMemberId INT NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (FlightId) REFERENCES Flights(Id),
    FOREIGN KEY (CrewMemberId) REFERENCES CrewMembers(Id)
)
GO

INSERT INTO Passengers (FirstName, LastName, DocumentNumber, DocumentType, Email, PhoneNumber, DateOfBirth)
VALUES 
('John', 'Doe', '123456789', 'DNI', 'john@example.com', '1234567890', '1990-01-01'),
('Jane', 'Smith', '987654321', 'Passport', 'jane@example.com', '0987654321', '1985-05-15')
GO

INSERT INTO Flights (FlightNumber, Origin, Destination, DepartureTime, ArrivalTime, BasePrice, TotalSeats, AvailableSeats, AircraftType, Status)
VALUES 
('FL001', 'New York', 'Los Angeles', '2025-03-01 10:00', '2025-03-01 13:00', 299.99, 180, 180, 'Boeing 737', 'Scheduled'),
('FL002', 'Miami', 'Chicago', '2025-03-02 14:00', '2025-03-02 16:30', 199.99, 150, 150, 'Airbus A320', 'Scheduled')
GO
