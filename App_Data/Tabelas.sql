--CREATE TABLE [dbo].[Categoria] (
--    [ID_Categoria] INT           NOT NULL,
--    [Categoria]    NVARCHAR (50) NOT NULL,
--    PRIMARY KEY CLUSTERED ([ID_Categoria] ASC),
--    UNIQUE NONCLUSTERED ([Categoria] ASC)
--);







--CREATE TABLE [dbo].[Cliente] (
--    [NUM_CC]         INT            NOT NULL,
--    [NomeProprio]    VARCHAR (50)   NULL,
--    [NomeApelido]    VARCHAR (50)   NULL,
--    [Email]          VARCHAR (50)   NULL,
--    [Contacto]       INT            NULL,
--    [DataNascimento] DATE           NULL,
--    [Morada]         VARCHAR (50)   NULL,
--    [Foto]           NVARCHAR (255) NULL,
--    PRIMARY KEY CLUSTERED ([NUM_CC] ASC)
--);






CREATE TABLE [dbo].[Material] (
    [SKU]             VARCHAR (50)    NOT NULL,
    [Marca]           VARCHAR (50)    NULL,
    [Modelo]          VARCHAR (50)    NULL,
    [Categoria]       VARCHAR (50)    NOT NULL,
    [Quantidade]      INT             NULL,
    [Caracteristicas] VARCHAR (50)    NULL,
    [Foto]            NVARCHAR (255)  NULL,
    [Preço]           DECIMAL (16, 2) NULL,
    PRIMARY KEY CLUSTERED ([SKU] ASC)
);