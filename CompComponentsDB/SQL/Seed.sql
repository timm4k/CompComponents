USE CompComponentsDB
GO

TRUNCATE TABLE dbo.Components
GO

INSERT INTO dbo.Components (Name, Type, Supplier, Quantity, Cost, SupplyDate) VALUES
('Ryzen 5 7600X', 'CPU', 'AMD', 15, 299.99, '2024-10-01'),
('Ryzen 7 7800X3D', 'CPU', 'AMD', 8, 420.00, '2024-11-10'),
('Core i7-14700K', 'CPU', 'Intel', 12, 389.50, '2024-11-18'),
('RTX 4070', 'GPU', 'NVIDIA', 6, 599.99, '2024-10-15'),
('RTX 4080 Super', 'GPU', 'NVIDIA', 4, 999.99, '2024-11-05'),
('RX 7900 XT', 'GPU', 'AMD', 5, 749.99, '2024-11-12'),
('B650 AORUS', 'Motherboard', 'Gigabyte', 10, 210.50, '2024-09-20'),
('Z790 TUF', 'Motherboard', 'ASUS', 7, 279.99, '2024-10-02'),
('X670E Steel Legend', 'Motherboard', 'ASRock', 5, 319.99, '2024-11-01'),
('RTX 5090', 'GPU', 'NVIDIA', 3, 1999.99, '2025-02-01')
GO