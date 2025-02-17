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
CREATE TABLE [AspNetRoles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(2048) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(2048) NULL,
    [SecurityStamp] nvarchar(2048) NULL,
    [ConcurrencyStamp] nvarchar(2048) NULL,
    [PhoneNumber] nvarchar(2048) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Usuario] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(250) NOT NULL,
    [CriadoEm] datetime2(0) NOT NULL,
    [ModificadoEm] datetime2(0) NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(2048) NULL,
    [ClaimValue] nvarchar(2048) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(2048) NULL,
    [ClaimValue] nvarchar(2048) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(2048) NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(2048) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Categoria] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(250) NOT NULL,
    [Descricao] nvarchar(500) NULL,
    [Padrao] bit NOT NULL DEFAULT CAST(0 AS bit),
    [UsuarioId] int NULL,
    [CriadoEm] datetime2(0) NOT NULL,
    [ModificadoEm] datetime2(0) NULL,
    CONSTRAINT [PK_Categoria] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categoria_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Orcamento] (
    [Id] int NOT NULL IDENTITY,
    [ValorLimite] decimal(18,2) NOT NULL,
    [UsuarioId] int NOT NULL,
    [CategoriaId] int NULL,
    [CriadoEm] datetime2(0) NOT NULL,
    [ModificadoEm] datetime2(0) NULL,
    CONSTRAINT [PK_Orcamento] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orcamento_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([Id]),
    CONSTRAINT [FK_Orcamento_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Transacao] (
    [Id] int NOT NULL IDENTITY,
    [Valor] decimal(18,2) NOT NULL,
    [Descricao] nvarchar(2048) NULL,
    [CategoriaId] int NOT NULL,
    [UsuarioId] int NOT NULL,
    [DataLancamento] datetime2 NOT NULL,
    [Tipo] int NOT NULL,
    [CriadoEm] datetime2(0) NOT NULL,
    [ModificadoEm] datetime2(0) NULL,
    CONSTRAINT [PK_Transacao] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transacao_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Transacao_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Categoria_UsuarioId] ON [Categoria] ([UsuarioId]);

CREATE INDEX [IX_Orcamento_CategoriaId] ON [Orcamento] ([CategoriaId]);

CREATE INDEX [IX_Orcamento_UsuarioId] ON [Orcamento] ([UsuarioId]);

CREATE INDEX [IX_Transacao_CategoriaId] ON [Transacao] ([CategoriaId]);

CREATE INDEX [IX_Transacao_UsuarioId] ON [Transacao] ([UsuarioId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250217140319_First', N'9.0.1');

COMMIT;
GO

SET IDENTITY_INSERT [dbo].[Categoria] ON
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (1, N'Alimentação', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (2, N'Transporte', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (3, N'Moradia', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (4, N'Investimento', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (5, N'Educação', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (6, N'Saúde', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (7, N'Lazer', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
INSERT INTO [dbo].[Categoria] ([Id], [Nome], [Descricao], [Padrao], [UsuarioId], [CriadoEm], [ModificadoEm]) VALUES (8, N'Salário', NULL, 1, NULL, N'2025-02-17 11:16:59', N'2025-02-17 11:16:59')
SET IDENTITY_INSERT [dbo].[Categoria] OFF
