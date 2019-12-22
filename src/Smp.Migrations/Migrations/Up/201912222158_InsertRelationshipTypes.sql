SET IDENTITY_INSERT [dbo].[RelationshipTypes] ON;

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RelationshipTypes')
BEGIN
	INSERT INTO RelationshipTypes (Id, Type) VALUES (1, 'Friend');
END

SET IDENTITY_INSERT [dbo].[RelationshipTypes] OFF;