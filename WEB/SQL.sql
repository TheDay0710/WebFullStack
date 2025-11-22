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
    [IDCate]   INT IDENTITY (1, 1) NOT NULL,
    [NameCate] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([IDCate] ASC)
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
    CONSTRAINT [FK_Pro_Category] FOREIGN KEY ([CateID]) REFERENCES [dbo].[Category] ([IDCate])
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
-- Ví dụ cho giày ID=1
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '6 UK', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '7 UK', 10);
Insert into ProductSize (ProductID, SizeName, Quantity) values (1, '8 UK', 0); 
-- Ví dụ cho giày ID=2
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '39', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (2, '40', 5);
Insert into ProductSize (ProductID, SizeName, Quantity) values (3, '41', 8);
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