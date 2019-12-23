IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Relationships')
BEGIN
	CREATE TABLE Relationships (
		UserOneId UNIQUEIDENTIFIER NOT NULL,
		UserTwoId UNIQUEIDENTIFIER NOT NULL,
		RelationshipTypeId TINYINT NOT NULL, 
		AcceptedDate DATETIME NOT NULL,
		CONSTRAINT PK_Relationships PRIMARY KEY (UserOneId, UserTwoId, RelationshipTypeId),
		CONSTRAINT FK_Relationships_UserOneId_Users_Id FOREIGN KEY (UserOneId) REFERENCES Users(Id),
		CONSTRAINT FK_Relationships_UserTwoId_Users_Id FOREIGN KEY (UserTwoId) REFERENCES Users(Id),
		CONSTRAINT FK_Relationships_RelationshipTypeId_RelationshipTypes_Id FOREIGN KEY (RelationshipTypeId) REFERENCES RelationshipTypes(Id)
	);
END