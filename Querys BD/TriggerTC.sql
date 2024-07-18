-- Crear el trigger para actualizar el saldo de la tarjeta de crédito
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

    -- Actualizar el saldo de la tarjeta de crédito dependiendo del tipo de transacción
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

---Explicación del Trigger
---El trigger trg_UpdateBalance se ejecuta después de que se inserte una nueva fila en la tabla Transactions.
---Dentro del trigger, se obtienen los valores insertados (@card_id, @amount, @transaction_type).
---Dependiendo del tipo de transacción (Purchase o Payment), se actualiza el saldo actual (current_balance) de la tarjeta de crédito correspondiente.
---Si la transacción es una compra (Purchase), se suma el monto al saldo actual.
---Si la transacción es un pago (Payment), se resta el monto del saldo actual.
---Con este trigger, cada vez que se inserte una nueva transacción en la tabla Transactions, el saldo actual de la tarjeta de crédito correspondiente se actualizará automáticamente.