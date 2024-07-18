-- Crear el trigger para actualizar el saldo de la tarjeta de cr�dito
CREATE TRIGGER trg_UpdateBalance
ON Transactions
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @card_id INT;
    DECLARE @amount DECIMAL(18, 2);
    DECLARE @transaction_type VARCHAR(20);

    -- Obtener los valores insertados
    SELECT @card_id = i.card_id, 
           @amount = i.amount, 
           @transaction_type = i.transaction_type
    FROM inserted i;

    -- Actualizar el saldo de la tarjeta de cr�dito dependiendo del tipo de transacci�n
    IF @transaction_type = 'Purchase'
    BEGIN
        UPDATE CreditCards
        SET current_balance = current_balance + @amount
        WHERE card_id = @card_id;
    END
    ELSE IF @transaction_type = 'Payment'
    BEGIN
        UPDATE CreditCards
        SET current_balance = current_balance - @amount
        WHERE card_id = @card_id;
    END
END;

---Explicaci�n del Trigger
---El trigger trg_UpdateBalance se ejecuta despu�s de que se inserte una nueva fila en la tabla Transactions.
---Dentro del trigger, se obtienen los valores insertados (@card_id, @amount, @transaction_type).
---Dependiendo del tipo de transacci�n (Purchase o Payment), se actualiza el saldo actual (current_balance) de la tarjeta de cr�dito correspondiente.
---Si la transacci�n es una compra (Purchase), se suma el monto al saldo actual.
---Si la transacci�n es un pago (Payment), se resta el monto del saldo actual.
---Con este trigger, cada vez que se inserte una nueva transacci�n en la tabla Transactions, el saldo actual de la tarjeta de cr�dito correspondiente se actualizar� autom�ticamente.