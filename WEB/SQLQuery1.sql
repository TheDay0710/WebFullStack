USE DBADIDAS
GO

-- 2. TẠO CÁC BẢNG (Theo thứ tự chuẩn để tránh lỗi khóa ngoại)

-- Bảng AdminUser
CREATE TABLE [dbo].[AdminUser] (
    [ID]           INT IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (50) NULL,
    [RoleUser]     NVARCHAR (50) NULL,
    [PasswordUser] NCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

-- Bảng Category
CREATE TABLE [dbo].[Category] (
    [CateID]   INT IDENTITY (1, 1) NOT NULL,
    [NameCate] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([CateID] ASC)
);

-- Bảng Customer
CREATE TABLE [dbo].[Customer] (
    [IDCus]    INT IDENTITY (1, 1) NOT NULL,
    [NameCus]  NVARCHAR (50) NULL,
    [PhoneCus] NVARCHAR (15) NULL,
    [EmailCus] NVARCHAR (50) NULL,
    [UserName] VARCHAR(50) NULL,
    [Password] NVARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([IDCus] ASC)
);

-- Bảng Product (Đã gộp các cột mới vào đây)
CREATE TABLE [dbo].[Product] (
    [ProductID]       INT IDENTITY (1, 1) NOT NULL,
    [NamePro]         NVARCHAR (50)  NULL,
    [Type]   NVARCHAR (50)  NULL,
    [CateID]          INT            NULL,
    [Price]           DECIMAL (18, 2) NULL,
    [ImagePro]        NVARCHAR (50)   NULL,
    
    [ViewCount]       INT DEFAULT 0,
    [NumOfReview]     INT DEFAULT 0,
    
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([CateID]) REFERENCES [dbo].[Category] ([CateID])
);

-- Bảng ProductSize (Phải tạo SAU bảng Product)
CREATE TABLE [dbo].[ProductSize] (
    [ID]        INT IDENTITY(1,1) PRIMARY KEY,
    [ProductID] INT NOT NULL,  
    [DecriptionPro] NVARCHAR(100),
    [SizeName]      NVARCHAR(20) NULL,     
    [Quantity]  INT DEFAULT 0,     
    
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product]([ProductID])
);

-- Bảng OrderPro
CREATE TABLE [dbo].[OrderPro] (
    [ID]               INT IDENTITY (1, 1) NOT NULL,
    [DateOrder]        DATETIME DEFAULT GETDATE(), -- Sửa thành DateTime để lưu cả giờ
    [IDCus]            INT NULL,
    [AddressDeliverry] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDCus]) REFERENCES [dbo].[Customer] ([IDCus])
);

-- Bảng OrderDetail
CREATE TABLE [dbo].[OrderDetail] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [IDProduct] INT NULL,
    [IDOrder]   INT NULL,
    [Quantity]  INT NULL,
    [UnitPrice] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDProduct]) REFERENCES [dbo].[Product] ([ProductID]),
    FOREIGN KEY ([IDOrder]) REFERENCES [dbo].[OrderPro] ([ID])
);
GO

-- 3. INSERT DỮ LIỆU MẪU

-- Admin
Insert into AdminUser (UserName, RoleUser, PasswordUser) values ('HUY', 'HuyDay', '081106');
Insert into AdminUser (UserName, RoleUser, PasswordUser) values ('TRI', 'TRIFLOP', '366769');

-- Customer
Insert into Customer (NameCus, PhoneCus, EmailCus, UserName, Password)
	values (N'Nguyễn Quốc Huy', '0902637150', 'huy@gmail.com', 'HuyNguyen', 'huy08112006');
Insert into Customer (NameCus, PhoneCus, EmailCus, UserName, Password)
	values (N'Minh Trí', '0913678345', 'tri@gmail.com', 'MinhTri', 'tri123456789');

-- Category
Insert into Category (NameCate) Values(N'Giày Nam');
Insert into Category (NameCate) Values(N'Giày Nữ');
Insert into Category (NameCate) Values(N'Quần Áo');
Insert into Category (NameCate) Values(N'Sale');


Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Buộc Dây CLIMACOOL', N'Sportswear', 2, 4200000, 'ps1.jpg', 150, 10);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Adizero EVO SL', N'Run', 2, 4000000, 'ps2.jpg', 414, 11);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Golf Đinh Liền Retrocross 25', N'Golf', 2, 3000000, 'ps3.jpg', 89, 5);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Samba OG', N'Originals', 2, 2700000, 'ps4.jpg', 1205, 50);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Buộc Dây adidas Taekwondo', N'Originals', 2, 2400000, 'ps5.jpg', 300, 8);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày VL Court 2.0', N'Sportswear', 2, 2000000, 'ps6.jpg', 50, 2);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'GIÀY JAPAN', N'Originals', 1, 2900000, 'ps7.jpg', 67, 4);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'Giày Adizero Aruku', N'Originals', 1, 2790000, 'ps8.jpg', 99, 9);
Insert into Product (NamePro, Type, CateID, Price, ImagePro, ViewCount, NumOfReview)
    values (N'SUPERSKATE X KADER', N'Originals', 1, 2500000, 'ps9.jpg', 12, 1);

-- ProductSize (Dữ liệu mẫu cho bảng Size)

Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '31', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '32', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '33', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '34', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '35', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '36', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '37', 10);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '38', 0); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '39', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '40', 1);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '41', 1);

Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '31', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '32', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '33', 7);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '34', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '36', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '37', 16);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '39', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '40', 1);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '31', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '32', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '33', 7);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '36', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '39', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '40', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '31', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '32', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '33', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '36', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '39', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '40', 2);
Insert into ProductSize (ProductID, SizeName, Quantity) values (4, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '31', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '32', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '33', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '36', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '39', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '40', 2);
Insert into ProductSize (ProductID, SizeName, Quantity) values (6, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '31', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '32', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '33', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '36', 1);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '39', 8);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '40', 2);
Insert into ProductSize (ProductID, SizeName, Quantity) values (7, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '31', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '32', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '33', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '36', 1);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '38', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '39', 8);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '40', 2);
Insert into ProductSize (ProductID, SizeName, Quantity) values (8, '41', 2);

Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '31', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '32', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '33', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '34', 4);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '35', 3);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '36', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '37', 6);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '38', 4); 
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '39', 8);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '40', 0);
Insert into ProductSize (ProductID, SizeName, Quantity) values (9, '41', 2);
-- OrderPro
Insert into OrderPro (DateOrder, IDCus, AddressDeliverry)
	values ('2025-11-20', 1, N'191 Bùi Điền,q8');

-- OrderDetail
-- Lưu ý: Đảm bảo IDProduct 1,2,3 đã tồn tại ở trên
Insert into OrderDetail (IDProduct, IDOrder, Quantity, UnitPrice)
	values (1, 1, 5, 4200000);
Insert into OrderDetail (IDProduct, IDOrder, Quantity, UnitPrice)
	values (2, 1, 10, 4000000);
Insert into OrderDetail (IDProduct, IDOrder, Quantity, UnitPrice)
	values (3, 1, 12, 3000000);