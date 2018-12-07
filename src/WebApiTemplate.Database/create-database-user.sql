CREATE LOGIN db_user WITH PASSWORD = 'Fbx6zA^v5Qui', CHECK_POLICY = ON, DEFAULT_DATABASE = WEBAPITEMPLATE
GO

CREATE USER db_user FOR LOGIN db_user
EXEC sp_addrolemember 'db_datareader', 'db_user'
EXEC sp_addrolemember 'db_datawriter', 'db_user'
GO