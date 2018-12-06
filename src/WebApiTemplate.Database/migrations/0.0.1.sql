USE WEBAPITEMPLATE;
GO

CREATE TABLE dbo.Customers(
	CustomerId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ExternalCustomerReference UNIQUEIDENTIFIER NOT NULL,
	FirstName [NVARCHAR](70) NOT NULL,
	Surname [NVARCHAR](70) NOT NULL,	
	Status TINYINT NOT NULL,
	CreatedDate DATETIME NOT NULL,
	LastModifiedDate DATETIME NULL
);

CREATE TABLE dbo.ApiAuthentication
(
    ApiAuthenticationId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    CreatedDate DATETIME NOT NULL,
    ApiKey [UNIQUEIDENTIFIER] NOT NULL,
    KeyHolder [NVARCHAR](200) NOT NULL
);

CREATE NONCLUSTERED INDEX IX_ApiAuthentication_ApiKey ON dbo.ApiAuthentication (ApiKey);
CREATE NONCLUSTERED INDEX IX_Customers.ExternalCustomerReference ON dbo.Customers (ExternalCustomerReference)