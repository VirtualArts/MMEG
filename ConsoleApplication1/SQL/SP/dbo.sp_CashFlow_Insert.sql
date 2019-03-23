CREATE PROCEDURE [dbo].[sp_CashFlow_Insert]
@id_cashflow int,
@cuotas int,
@tipo nvarchar(50),
@concepto nvarchar(100),
@descripcion nvarchar(200),
@fecha datetime,
@monto decimal(18,2)
AS
declare @id int

set @id = (SELECT ISNULL((MAX(id)+1),1) AS id FROM CashFlow)

	INSERT INTO dbo.CashFlow(id,id_cashflow,cuotas,tipo,concepto,descripcion,fecha,monto) values(@id, @id_cashflow, @cuotas, @tipo, @concepto, @descripcion, @fecha, @monto)

RETURN 0
