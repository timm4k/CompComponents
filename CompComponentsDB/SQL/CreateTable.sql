USE CompComponentsDB
GO

IF OBJECT_ID('dbo.Components', 'U') IS NOT NULL
    DROP TABLE dbo.Components
GO

CREATE TABLE dbo.Components (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Supplier NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    Cost DECIMAL(10,2) NOT NULL,
    SupplyDate DATE NOT NULL
)
GO