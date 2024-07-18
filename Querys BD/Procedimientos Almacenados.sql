
-- =============================================
-- Author:		<Christofer Palacios>
-- Create date: <16/07/2024>
-- Description:	<Inserta registros que van para la tabla transacciones>
-- =============================================
-------------------------------
CREATE PROCEDURE InsertarPago
(

    @CardId INT,
    @TransactionDate DATETIME,
    @Description VARCHAR(255),
	@Amount DECIMAL(18, 2),
	@TransactionType VARCHAR(20)
	)
AS
BEGIN
    SET NOCOUNT ON; ---added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here
	INSERT INTO Transactions([card_id], transaction_date, description, amount, transaction_type)
    VALUES (@CardId, @TransactionDate, @Description, @Amount, @TransactionType);
END
GO


DROP PROCEDURE IF EXISTS InsertarPago;

EXEC InsertarPago
    1,
	'2024-07-16',
	'Tienda',
	100.00,
	'Pago'

select * from Transactions;
-------------------

CREATE PROCEDURE InsertarAbono
(

    @CardId INT,
    @TransactionDate DATETIME,
	@Amount DECIMAL(18, 2),
	@TransactionType VARCHAR(20)
	)
AS
BEGIN
    SET NOCOUNT ON; ---added to prevent extra result sets from
	-- interfering with SELECT statements.


    -- Insert statements for procedure here
	INSERT INTO Transactions([card_id], transaction_date, amount, transaction_type)
    VALUES (@CardId, @TransactionDate, @Amount, @TransactionType);
END
GO


DROP PROCEDURE IF EXISTS InsertarAbono;

EXEC InsertarAbono
    1,
	'2024-07-16',
	1000.00,
	'Abono'

select * from Transactions;

