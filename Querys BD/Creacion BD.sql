-- Crear la base de datos
CREATE DATABASE CreditCardDB;
GO

USE CreditCardDB;
GO

-- Crear la tabla de usuarios
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE
);

-- Crear la tabla de tarjetas de crédito
CREATE TABLE CreditCards (
    card_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT NOT NULL,
    card_number VARCHAR(16) UNIQUE NOT NULL,
    current_balance DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    credit_limit DECIMAL(18, 2) NOT NULL,
    available_balance AS (credit_limit - current_balance) PERSISTED,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- Crear la tabla de transacciones
CREATE TABLE Transactions (
    transaction_id INT PRIMARY KEY IDENTITY(1,1),
    card_id INT NOT NULL,
    transaction_date DATETIME NOT NULL,
    description VARCHAR(255),
    amount DECIMAL(18, 2) NOT NULL,
    transaction_type VARCHAR(20) NOT NULL, -- Puede ser 'Purchase' o 'Payment'
    FOREIGN KEY (card_id) REFERENCES CreditCards(card_id)
);
