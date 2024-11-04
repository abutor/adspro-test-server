DECLARE @Password VARCHAR(MAX) = '713BFDA78870BF9D1B261F565286F85E97EE614EFE5F0FAF7C34E7CA4F65BACA';

INSERT INTO [Users] ([Id], [Username], [Password], [Active])
VALUES 
    (NEWID(), 'admin', @Password, 1,
    (NEWID(), 'adam', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'john', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'mike', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'sara', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'lisa', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'mark', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'jane', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'paul', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'kate', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ryan', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'chris', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'anna', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'josh', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'emma', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'tom', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'amy', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'lucas', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'nina', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ben', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'alex', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'zoe', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'oliver', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'sophia', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'jack', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'isla', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'daniel', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'chloe', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'matt', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ella', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'tyler', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'grace', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'evan', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ruby', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'jason', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'bella', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'liam', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'mia', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ethan', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ava', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'noah', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'eva', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'logan', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'ivy', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'jacob', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'lily', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'caleb', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'zoey', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'owen', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'lucy', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0)),
    (NEWID(), 'blake', @Password, ROUND(RAND(CHECKSUM(NEWID())) * 1, 0));
