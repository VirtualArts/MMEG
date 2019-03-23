CREATE TABLE [dbo].[CashFlow]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [id_cashflow] INT NOT NULL, 
    [cuotas] INT NOT NULL, 
    [tipo] NVARCHAR(50) NOT NULL, 
    [concepto] NVARCHAR(100) NOT NULL, 
    [descripcion] NVARCHAR(200) NULL, 
    [fecha] DATETIME NOT NULL, 
    [monto] DECIMAL(18, 2) NOT NULL
)
