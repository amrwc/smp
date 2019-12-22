IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RelationshipTypes')
BEGIN
	CREATE TABLE RelationshipTypes (
		Id TINYINT IDENTITY(1,1) NOT NULL,
		Type VARCHAR(32) NOT NULL,
		CONSTRAINT PK_RelationshipTypes PRIMARY KEY (Id)
	);
END