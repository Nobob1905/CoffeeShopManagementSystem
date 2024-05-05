create database CoffeeShopManagement
go 

use CoffeeShopManagement
 
go

-- Drink
-- TablesList
-- DrinkCategory
-- Account
-- Invoice
-- InvoiceDetail

create table TablesList
(
	id int identity primary key,
	name nvarchar(100) not null,
	status nvarchar(100) null default N'Empty', -- empty || using
)
go

create table Account 
(
	username nvarchar(100) primary key,
	displayName nvarchar(100) not null,
	password varchar(1000) not null default '1962026656160185351301320480154111117132155',
	type int default 0, -- 1: admin & 0: staff
	phone varchar(100),
	email varchar(100),
	address nvarchar(100)
)
go

create table DrinkCategory 
(
	id int identity primary key,
	name nvarchar(100) not null,
)
go

create table Drink 
(
	id int identity primary key,
	name nvarchar(100) not null,
	idCategory int not null,
	price float not null default 0

	FOREIGN KEY (idCategory) REFERENCES dbo.DrinkCategory(id)
)
go

create table Invoice 
(
	id int identity primary key,
	dateCheckin date default getdate(),
	dateCheckout date,
	idTable int not null,
	status int  not null default 0, --1: paied & 0: unpaied 
	discount int default 0,
	totalPrice int default 0

	FOREIGN KEY (idTable) REFERENCES dbo.TablesList(id)
)
go

create table InvoiceDetail 
(
	id int identity primary key,
	idInvoice int not null,
	idDrink int not null,
	quantity int not null default 0

	foreign key (idInvoice) references Invoice(id),
	foreign key (idDrink) references Drink(id)
)
go

create proc USP_GetAccountByUsername
@username nvarchar(100)
as 
begin
	select * from Account where username = @username
end
go

create proc USP_Login
@username nvarchar(100), @password nvarchar(100)
as
begin
	select * from Account where Account.username = @username and Account.password = @password	
end
go

create proc USP_GetTableList
as select * from TablesList
go

create proc USP_InsertInvoice
@idTable int 
as 
begin
	insert dbo.Invoice values (GETDATE(), null, @idTable, 0, 0, 0)
end
go

create proc USP_InsertInvoiceDetail 
@idInvoice int, @idDrink int, @quantity int
as
begin
	declare @isExistInvoiceDetail int
	declare @DrinkQuantity int = 1;

	select @isExistInvoiceDetail = id, @DrinkQuantity = quantity 
	from dbo.InvoiceDetail 
	where idInvoice = @idInvoice and idDrink = @idDrink

	if (@isExistInvoiceDetail > 0)
	begin
		declare @newQuantiry int = @DrinkQuantity + @quantity
		if (@newQuantiry > 0) 
			update InvoiceDetail set quantity = @DrinkQuantity + @quantity where idDrink = @idDrink and idInvoice = @idInvoice
		else 
			delete InvoiceDetail where idInvoice = @idInvoice and idDrink = @idDrink
	end
	else
	begin 
		insert dbo.InvoiceDetail values (@idInvoice, @idDrink, @quantity)
	end
end
go

create proc USP_SwitchTable 
@TableID1 int, @TableID2 int
as
begin
	declare @InvoiceID1 int
	declare @InvoiceID2 int
	declare @isTbl1Empty int = 1
	declare @isTbl2Empty int = 1

	select @InvoiceID2 = id from dbo.Invoice where idTable = @TableID2 and status = 0
	select @InvoiceID1 = id from dbo.Invoice where idTable = @TableID1 and status = 0

	if (@InvoiceID1 is not null and @InvoiceID2 is not null)
	begin
		select id into IDInvoiceTable from dbo.InvoiceDetail where idInvoice = @InvoiceID2

		update dbo.InvoiceDetail set idInvoice = @InvoiceID2 where idInvoice = @InvoiceID1

		update dbo.InvoiceDetail set idInvoice = @InvoiceID1 where idInvoice in (select * from IDInvoiceTable)

		drop table IDInvoiceTable

		-- First of all, quantity updation for each idDrink has a same idInvoice
		UPDATE InvoiceDetail
		SET quantity = (
			SELECT SUM(quantity)
			FROM InvoiceDetail AS src
			WHERE src.idInvoice = InvoiceDetail.idInvoice AND src.idDrink = InvoiceDetail.idDrink
		)
		WHERE id IN (
			SELECT MAX(id)
			FROM InvoiceDetail
			WHERE idInvoice = @InvoiceID2
			GROUP BY idDrink
		);

		-- Then, delete the whole records dont hav the largest ID
		DELETE FROM InvoiceDetail
		WHERE idInvoice = @InvoiceID2 AND id NOT IN (
			SELECT MAX(id)
			FROM InvoiceDetail
			WHERE idInvoice = @InvoiceID2
			GROUP BY idDrink
		);

		update dbo.TablesList set status = N'Empty' where id = @TableID1

		update dbo.TablesList set status = N'In use' where id = @TableID2

		return
	end

	if (@InvoiceID1 is null)
	begin
		insert dbo.Invoice (dateCheckin, dateCheckout, idTable, status) values (GETDATE(), null, @TableID1, 0)

		select @InvoiceID1 = MAX(id) from dbo.Invoice where idTable = @TableID1 and status = 0
	end

	select @isTbl1Empty = COUNT(*) from dbo.InvoiceDetail where idInvoice = @InvoiceID1
	
	if (@InvoiceID2 is null)
	begin
		insert dbo.Invoice (dateCheckin, dateCheckout, idTable, status) values (GETDATE(), null, @TableID2, 0)

		select @InvoiceID2 = MAX(id) from dbo.Invoice where idTable = @TableID2 and status = 0
	end

	select @isTbl2Empty = COUNT(*) from dbo.InvoiceDetail where idInvoice = @InvoiceID2

	select id into IDInvoiceTable from dbo.InvoiceDetail where idInvoice = @InvoiceID2

	update dbo.InvoiceDetail set idInvoice = @InvoiceID2 where idInvoice = @InvoiceID1

	update dbo.InvoiceDetail set idInvoice = @InvoiceID1 where id in (select * from IDInvoiceTable)

	drop table IDInvoiceTable

	if (@isTbl1Empty = 0)
		update dbo.TablesList set status = N'Empty' where id = @TableID2

	if (@isTbl2Empty = 0)
		update dbo.TablesList set status = N'Empty' where id = @TableID1
end
go

create proc USP_GetReportByDate
@DateCheckIn date, @DateCheckOut date
as 
begin
	select TBL.name as [Table], dateCheckin as [Date check-in], dateCheckout as [Date check-out], discount as [Discounted], I.totalPrice as [Total Price] 
	from dbo.Invoice as I, dbo.TablesList as TBL
	where I.dateCheckin >= @DateCheckIn and I.dateCheckout <= @DateCheckOut and I.status = 1 and I.idTable = TBL.id
end
go

create proc  USP_UpdateAccountInfo
@username nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100), @phone varchar(100), @email varchar(100), @address nvarchar(100)
as 
begin
	declare @isOrgPass int = 0

	select @isOrgPass = COUNT(*) from dbo.Account where username = @username and password = @password

	if (@isOrgPass = 1) 
	begin
		if (@newPassword = null or @newPassword = N'')
		begin
			update dbo.Account set displayName = @displayName, phone = @phone, email = @email, address = @address where username = @username
		end
		else 
			update dbo.Account set displayName = @displayName, phone = @phone, email = @email, address = @address, password = @newPassword where username = @username
	end
end
go

create proc USP_InvoiceExport
@invoiceID int, @discount int
as
begin
	select D.name as 'Name', D.price as 'Price', ID.quantity as 'Quantity', D.price*ID.quantity as 'TotalAmount'
	from dbo.Invoice as I, dbo.InvoiceDetail as ID, dbo.Drink as D
	where I.id = ID.idInvoice and D.id = ID.idDrink and idInvoice = @invoiceID
end
go

create trigger UTG_UpdateInvoiceDetail
on dbo.InvoiceDetail for insert, update
as
begin
	declare @InvoiceID int
	select @InvoiceID = idInvoice from inserted

	declare @TableID int
	select @TableID = Invoice.idTable from dbo.Invoice where Invoice.id = @InvoiceID and Invoice.status = 0

	declare @quantity int
	select @quantity = COUNT(*) from dbo.InvoiceDetail where idInvoice = @InvoiceID

	if (@quantity > 0)
		update dbo.TablesList set status = N'In use' where TablesList.id = @TableID
	else 
		update dbo.TablesList set status = N'Empty' where TablesList.id = @TableID
end
go

create trigger UTG_UpdateInvoice
on dbo.Invoice for update
as 
begin
	declare @InvoiceID int
	select @InvoiceID = id from inserted

	declare @TableID int
	select @TableID = Invoice.idTable from dbo.Invoice where Invoice.id = @InvoiceID

	declare @quantity int = 0
	select @quantity = COUNT(*) from dbo.Invoice where Invoice.idTable = @TableID and Invoice.status = 0

	if (@quantity = 0) 
		update dbo.TablesList set status = N'Empty' where id = @TableID
end
go

create trigger UTG_DeleteInvoiceDetail
on dbo.InvoiceDetail for delete
as
begin
	declare @idInvoiceDetail int
	declare @idInvoice int

	select @idInvoiceDetail = id, @idInvoice = idInvoice from deleted

	declare @TableID int
	select @TableID = idTable from dbo.Invoice where id = @idInvoice

	declare @count int = 0

	select @count = COUNT(*) from dbo.InvoiceDetail as ID, dbo.Invoice as I 
	where I.id = ID.idInvoice and I.id = @idInvoice and I.status = 0

	if (@count = 0)
		update dbo.TablesList set status = N'Empty' where id = @TableID
end
go

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
go

-- Add account
insert dbo.Account (username, displayName, password, type, phone, email, address)
values (N'admin01', N'Nguyễn Văn An', '1962026656160185351301320480154111117132155', 1, '0919616411', 'ngvanan@gmail.com', N'Quận 7, HCMC')
insert dbo.Account (username, displayName, password, type, phone, email, address)
values (N'admin02', N'Nguyễn Quốc Anh', '1962026656160185351301320480154111117132155', 1, '0916616511', 'ngquocanh@gmail.com', N'Quận 8, HCMC')
insert dbo.Account (username, displayName, password, type, phone, email, address)
values (N'staff01', N'Ngô Quốc An', '1962026656160185351301320480154111117132155', 0, '0619616211', 'ngoquocan@gmail.com', N'Quận 6, HCMC')
insert dbo.Account (username, displayName, password, type, phone, email, address)
values (N'staff02', N'Lê Quốc Huy', '1962026656160185351301320480154111117132155', 0, '0963716911', 'lequochuyn@gmail.com', N'Quận 5, HCMC')
go

-- Add table 
declare @i int = 1
while @i <= 10
begin
	insert dbo.TablesList (name) values (N'Table ' + cast(@i as nvarchar(100)))
	set @i += 1
end
go

-- Add category
insert dbo.DrinkCategory values (N'Coffee Classics')
insert dbo.DrinkCategory values (N'Specialty Coffee Drinks')
insert dbo.DrinkCategory values (N'Tea Delights')
insert dbo.DrinkCategory values (N'Hot Chocolate Options')
insert dbo.DrinkCategory values (N'Fruity Refreshments')
insert dbo.DrinkCategory values (N'Creamy Delights')
go

-- Add drinks
insert dbo.Drink (name, idCategory, price) values (N'Caffe Latte (M)', 1, 38000)
insert dbo.Drink (name, idCategory, price) values (N'Caffe Latte (L)', 1, 45000)
insert dbo.Drink (name, idCategory, price) values (N'Cappuccino (M)', 1, 42000)
insert dbo.Drink (name, idCategory, price) values (N'Cappuccino (L)', 1, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Americano (M)', 1, 32000)
insert dbo.Drink (name, idCategory, price) values (N'Americano (L)', 1, 38000)
insert dbo.Drink (name, idCategory, price) values (N'Mocha (M)', 1, 45000)
insert dbo.Drink (name, idCategory, price) values (N'Mocha (L)', 1, 52000)

insert dbo.Drink (name, idCategory, price) values (N'Caramel Macchiato (M)', 2, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Caramel Macchiato (L)', 2, 55000)
insert dbo.Drink (name, idCategory, price) values (N'Vanilla Latte (M)', 2, 40000)
insert dbo.Drink (name, idCategory, price) values (N'Vanilla Latte (L)', 2, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Caffe Breve (M)', 2, 52000)
insert dbo.Drink (name, idCategory, price) values (N'Caffe Breve (L)', 2, 58000)
insert dbo.Drink (name, idCategory, price) values (N'Flat White (M)', 2, 44000)
insert dbo.Drink (name, idCategory, price) values (N'Flat White (L)', 2, 50000)

insert dbo.Drink (name, idCategory, price) values (N'Black Tea (pot)', 3, 25000)
insert dbo.Drink (name, idCategory, price) values (N'Green Tea (pot)', 3, 28000)
insert dbo.Drink (name, idCategory, price) values (N'Earl Grey Tea (pot)', 3, 30000)
insert dbo.Drink (name, idCategory, price) values (N'Herbal Tea', 3, 22000)

insert dbo.Drink (name, idCategory, price) values (N'Classic Hot Chocolate (M)', 4, 35000)
insert dbo.Drink (name, idCategory, price) values (N'Classic Hot Chocolate (L)', 4, 42000)
insert dbo.Drink (name, idCategory, price) values (N'White Hot Chocolate (M)', 4, 40000)
insert dbo.Drink (name, idCategory, price) values (N'White Hot Chocolate (L)', 4, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Mexican Hot Chocolate (M)', 4, 42000)
insert dbo.Drink (name, idCategory, price) values (N'Mexican Hot Chocolate (L)', 4, 50000)
insert dbo.Drink (name, idCategory, price) values (N'Peppermint Hot Chocolate (M)', 4, 40000)
insert dbo.Drink (name, idCategory, price) values (N'Peppermint Hot Chocolate (L)', 4, 48000)

insert dbo.Drink (name, idCategory, price) values (N'Freshly Squeezed Orange Juice (M)', 5, 45000)
insert dbo.Drink (name, idCategory, price) values (N'Freshly Squeezed Orange Juice (L)', 5, 52000)
insert dbo.Drink (name, idCategory, price) values (N'Lemonade (M)', 5, 28000)
insert dbo.Drink (name, idCategory, price) values (N'Lemonade (L)', 5, 35000)
insert dbo.Drink (name, idCategory, price) values (N'Iced Tea (various flavors) (M)', 5, 42000)
insert dbo.Drink (name, idCategory, price) values (N'Iced Tea (various flavors) (L)', 5, 50000)
insert dbo.Drink (name, idCategory, price) values (N'Smoothies', 5, 55000)

insert dbo.Drink (name, idCategory, price) values (N'Hot Chocolate with Whipped Cream (M)', 6, 40000)
insert dbo.Drink (name, idCategory, price) values (N'Hot Chocolate with Whipped Cream (L)', 6, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Chai Latte (M)', 6, 48000)
insert dbo.Drink (name, idCategory, price) values (N'Chai Latte (L)', 6, 55000)
insert dbo.Drink (name, idCategory, price) values (N'Matcha Latte (M)', 6, 50000)
insert dbo.Drink (name, idCategory, price) values (N'Matcha Latte (L)', 6, 58000)
insert dbo.Drink (name, idCategory, price) values (N'Vanilla Bean Frappuccino (M)', 6, 52000)
go







