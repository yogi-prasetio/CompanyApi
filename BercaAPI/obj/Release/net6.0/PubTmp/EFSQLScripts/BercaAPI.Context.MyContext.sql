IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006025422_add_models')
BEGIN
    CREATE TABLE [Departments] (
        [Dept_Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY ([Dept_Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006025422_add_models')
BEGIN
    CREATE TABLE [Employees] (
        [NIK] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Status] bit NOT NULL,
        [Department_Id] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([NIK]),
        CONSTRAINT [FK_Employees_Departments_Department_Id] FOREIGN KEY ([Department_Id]) REFERENCES [Departments] ([Dept_Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006025422_add_models')
BEGIN
    CREATE INDEX [IX_Employees_Department_Id] ON [Employees] ([Department_Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231006025422_add_models')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231006025422_add_models', N'7.0.11');
END;
GO

COMMIT;
GO

