CREATE VIEW UserWithCreditCard AS
SELECT 
    u.user_id AS UserId,
    u.first_name AS FirstName,
    u.last_name AS LastName,
    u.email AS Email,
    c.card_id AS CardId,
    c.card_number AS CardNumber,
    c.current_balance AS CurrentBalance,
    c.credit_limit AS CreditLimit,
    c.available_balance AS AvailableBalance
FROM 
    Users u
LEFT JOIN 
    CreditCards c ON u.user_id = c.user_id;

---combina informaci�n relevante de usuarios y sus tarjetas de cr�dito
---en una sola estructura de datos, facilitando consultas que necesiten 
---informaci�n de ambas entidades relacionadas.