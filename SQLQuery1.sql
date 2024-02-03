CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY,
    CategoryName NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(100) NOT NULL,
    CategoryId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

INSERT INTO Categories (CategoryName)
VALUES ('Electronics'),
       ('Clothing'),
       ('Books');

INSERT INTO Products (ProductName, CategoryId, CategoryName)
VALUES ('Tablet', 1, 'Electronics'),
    ('Smartwatch', 1, 'Electronics'),
    ('Headphones', 1, 'Electronics'),
    ('Speaker', 1, 'Electronics'),
    ('Digital Camera', 1, 'Electronics'),
    ('Printer', 1, 'Electronics'),
    ('Router', 1, 'Electronics'),
    ('External Hard Drive', 1, 'Electronics'),
	('Dress', 2, 'Clothing'),
    ('Sweater', 2, 'Clothing'),
    ('Jacket', 2, 'Clothing'),
    ('Skirt', 2, 'Clothing'),
    ('Shorts', 2, 'Clothing'),
    ('Blouse', 2, 'Clothing'),
    ('Coat', 2, 'Clothing'),
    ('Pants', 2, 'Clothing'),
	('Introduction to Algorithms', 3, 'Books'),
    ('The Great Gatsby', 3, 'Books'),
    ('To Kill a Mockingbird', 3, 'Books'),
    ('1984', 3, 'Books'),
    ('Pride and Prejudice', 3, 'Books'),
    ('The Catcher in the Rye', 3, 'Books'),
    ('Animal Farm', 3, 'Books'),
    ('Brave New World', 3, 'Books'),
    ('The Lord of the Rings', 3, 'Books'),
    ('Harry Potter and the Philosopher''s Stone', 3, 'Books');

