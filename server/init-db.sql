IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FinanceTracker')
BEGIN
    CREATE DATABASE FinanceTracker;
END
GO