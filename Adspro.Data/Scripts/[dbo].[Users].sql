CREATE TABLE [dbo].[Users]
(
	[Id] uniqueidentifier primary key,
	[Username] varchar(64) not null unique,
	[Password] varchar(64) not null,
	[Active] bit default(1)
);
