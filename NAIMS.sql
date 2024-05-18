SET NOCOUNT ON
GO

SET DATEFORMAT MDY

USE master

DECLARE @dttm NVARCHAR(55)

SELECT @dttm=convert(VARCHAR(64), GETDATE(),113)
/*RAISEERROR('Beginning database at %s ....',1,1,@dttm_ WITH NOWAIT*/

GO

IF EXISTS (SELECT * FROM sysdatabases WHERE NAME='naimsdb')

BEGIN
		/*RAISEERROR('Dropping existing database....', 0,1)*/
		DROP DATABASE naimsdb
END

GO

CHECKPOINT

/*RAISEERROR('Creating database....',0,1)*/

GO

CREATE DATABASE naimsdb;
GO

CHECKPOINT

GO

USE [naimsdb ]

GO

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE BRANDS (
                BRAND_ID INT IDENTITY (1,1) UNIQUE NOT NULL,
                BNAME VARCHAR(64) NOT NULL,
                BDESCRIPTION VARCHAR(1024) NOT NULL,
                PRIMARY KEY CLUSTERED (BRAND_ID)
);


SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO


CREATE TABLE PRODUCTS (
                PRODUCT_ID INT IDENTITY (1,1) UNIQUE NOT NULL,
                BARCODE BIGINT,
                SKU VARCHAR(32) NOT NULL,
                BRAND_ID INT,
                PNAME VARCHAR(256) NOT NULL,
                SIZE VARCHAR(32) NOT NULL,
                PDESCRIPTION VARCHAR(1024) NOT NULL,
                PRICE FLOAT NOT NULL,
                PRICE_VAT FLOAT NOT NULL,
                WAREHOUSE_QTY INT NOT NULL,
                WAREHOUSE_STATUS VARCHAR(64) NOT NULL,
                LOCAL_QTY INT NOT NULL,
                LOCAL_STATUS VARCHAR(64),
                PRIMARY KEY CLUSTERED (PRODUCT_ID),
                                FOREIGN KEY (BRAND_ID) REFERENCES BRANDS(BRAND_ID)
                                ON DELETE NO ACTION
                                ON UPDATE NO ACTION
);

--SET IDENTITY_INSERT PRODUCTS ON

--GO


SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE EMPLOYEES (
                EMPLOYEE_ID INT IDENTITY (1,1) UNIQUE NOT NULL,
                E_FIRSTNAME VARCHAR(64) NOT NULL,
                E_LASTNAME VARCHAR(64) NOT NULL,
                E_ROLE VARCHAR(64) NOT NULL,
                E_EMAIL VARCHAR(256) NOT NULL,
                E_PHONENUMBER VARCHAR(64) NOT NULL,
                E_TARGET INT NOT NULL,
                E_COMISSION_PERC DECIMAL(10,2) NOT NULL,
                PRIMARY KEY CLUSTERED (EMPLOYEE_ID)
);

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE CONTACTS (
                CONTACT_ID INT IDENTITY (1,1) UNIQUE NOT NULL,
                CNAME VARCHAR(64) NOT NULL,
                CTYPE VARCHAR(64) NOT NULL,
                CADDRESS VARCHAR(256) NOT NULL,
                EMAIL VARCHAR(128) NOT NULL,
                PHONE_NUMBER VARCHAR(128) NOT NULL,
                PRIMARY KEY CLUSTERED (CONTACT_ID)
);


SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE ORDERS (
                ORDER_ID INT IDENTITY (1,1) UNIQUE NOT NULL,
                CONTACT_ID INT,
                ORDER_DATE DATE NOT NULL,
                EMPLOYEE_ID INT,
                PRIMARY KEY CLUSTERED (ORDER_ID),
                FOREIGN KEY (CONTACT_ID) REFERENCES CONTACTS (CONTACT_ID),
                FOREIGN KEY (EMPLOYEE_ID) REFERENCES EMPLOYEES (EMPLOYEE_ID)
                                ON DELETE NO ACTION
                                ON UPDATE NO ACTION
);

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE PRODUCTS_ORDERS (
                PRODUCTORDER_ID INT IDENTITY(1,1) UNIQUE NOT NULL,
                ORDER_ID INT,
                PRODUCT_ID INT NOT NULL,
                QTY INT NOT NULL,
                PRIMARY KEY (PRODUCTORDER_ID)
);

INSERT INTO BRANDS(BNAME, BDESCRIPTION)
VALUES ('milk_shake','this brand is milk_shake');
INSERT INTO BRANDS(BNAME, BDESCRIPTION)
VALUES ('depot','this brand is depot');
INSERT INTO BRANDS(BNAME, BDESCRIPTION)
VALUES ('simply zen','this brand is simply zen');



INSERT INTO PRODUCTS(BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) 
            VALUES (12345, 678910, 1, 'energizing shampoo', '300 ml', 'hello this is me' , 7.867, 8.8, 113,'in stock', 12, 'low stock');

            INSERT INTO EMPLOYEES(E_FIRSTNAME, E_LASTNAME,E_ROLE, E_EMAIL, E_PHONENUMBER, E_TARGET, E_COMISSION_PERC)
VALUES ('Amany','Aiy', 'Manager','amany@naturalapproachbh.com','36392394',10000,25.5);

INSERT INTO EMPLOYEES(E_FIRSTNAME, E_LASTNAME,E_ROLE, E_EMAIL, E_PHONENUMBER, E_TARGET, E_COMISSION_PERC)
VALUES ('Oskar','Alkhateeb', 'Sales','oskar@naturalapproachbh.com','39994304',2000,10);

INSERT INTO EMPLOYEES(E_FIRSTNAME, E_LASTNAME,E_ROLE, E_EMAIL, E_PHONENUMBER, E_TARGET, E_COMISSION_PERC)
VALUES ('Natalie','Arkhagha', 'Sales','natalie@naturalapproachbh.com','36392394',2000,10);

INSERT INTO EMPLOYEES(E_FIRSTNAME, E_LASTNAME,E_ROLE, E_EMAIL, E_PHONENUMBER, E_TARGET, E_COMISSION_PERC)
VALUES ('SuperAdmin','User', 'SuperAdmin','superadmin@naturalapproachbh.com','12345678',0,0);

INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274123606', 'Z110050CAMA025', 1, 'amazing', '200ml', '"A pre-styling spray capable of eliminating frizz with a long-lasting result. It is heat-activated, an will create a film-like effect over the hair that will protect ut from humidity, protect the hair colour and maintain the style for longer."', '14.85', '16.335', '0', 'pick up needed', '52', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274141075', 'Z110050CAMA010', 1, 'amazing', '50 ml', '"A pre-styling spray capable of eliminating frizz with a long-lasting result. It is heat-activated, an will create a film-like effect over the hair that will protect ut from humidity, protect the hair colour and maintain the style for longer."', '6', '6.6', '1', 'low stock', '51', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274052043', 'Z110050BGAT025', 1, 'argan deep treatment', '200ml', '"An organic argan oil & wheat protein formula that nourishes & eliminates frizz. Suitable for all hair types, it seals the cuticle, leaving hair soft, shiny, & easier to manage, while preserving the hair color."', '18.15', '19.965', '2', 'low stock', '50', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274051985', 'Z110050AGAR035', 1, 'argan shampoo', '300ml', '"Gentle, sulfate-free shampoo with a unique formula enriched with organic argan oil, wheat, & rice proteins provides a moisturising & nourishing cleanse. Combats frizz, leaving hair soft, shiny, & vibrant."', '15.4', '16.94', '3', 'low stock', '49', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274143888', 'Z110050BCBR030', 1, 'cold brunette conditioner', '250ml', '"Formulated with a specific blue pigment, it gives hair softness whilst counteracting reddish or orange tones in brunette hair. Milk proteins restructure the hair deeply, giving hair a healthy vibrant appearance."', '13.2', '14.52', '4', 'low stock', '48', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274143833', 'Z110050ACBR035', 1, 'cold brunette shampoo', '300ml', 'Cleanses hair gently with a special blue pigment developed to remove reddish-orange or orange tones from brunette hair. Milk proteins restructure the hair deeply giving hair a healthy vibrant appearance.', '13.2', '14.52', '5', 'low stock', '47', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147824', 'Z110050BMAN010', 1, 'color maintainer conditioner', '50 ml', '"This conditioner is specifically formulated to shield & detangle color-treated hair. It utilizes milk proteins to nourish & revitalize the hair, while the exclusive ingredient Integrity 41® enhances color longevity. The result is soft, shiny hair that is free from parabens."', '6', '6.6', '6', 'low stock', '46', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274051152', 'Z110050BMAI035', 1, 'colour maintainer conditioner', '300ml', '"This conditioner is specifically formulated to shield & detangle color-treated hair. It utilizes milk proteins to nourish & revitalize the hair, while the exclusive ingredient Integrity 41® enhances color longevity. The result is soft, shiny hair that is free from parabens."', '12.1', '13.31', '7', 'low stock', '45', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147695', 'Z110050AMAN035', 1, 'colour maintainer shampoo', '300ml', '"This gentle shampoo is designed to preserve the attractiveness & health of hair that has been treated with color. It includes milk proteins that deeply nourish & repair the hair, as well as the exclusive ingredient Integrity 41® to enhance color durability."', '12.1', '13.31', '8', 'low stock', '44', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147718', 'Z11050AMAN010', 1, 'colour maintainer shampoo', '50 ml', '"This gentle shampoo is designed to preserve the attractiveness & health of hair that has been treated with color. It includes milk proteins that deeply nourish & repair the hair, as well as the exclusive ingredient Integrity 41® to enhance color durability."', '6', '6.6', '9', 'low stock', '43', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274051244', 'Z110050BCOC025', 1, 'conditioning whipped cream', '200ml', '"A no rinse formula with a deep protective action, formulated to condition hair whilst maintaining the moisture balance of the hair & protecting the hair colour. Milk proteins condition & restructure the hair, whilst specific emollient ingredients give long-lasting hydration."', '12.1', '13.31', '10', 'low stock', '42', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274051688', 'Z110050BCOC010', 1, 'conditioning whipped cream', '50 ml', '"A no rinse formula with a deep protective action, formulated to condition hair whilst maintaining the moisture balance of the hair & protecting the hair colour. Milk proteins condition & restructure the hair, whilst specific emollient ingredients give long-lasting hydration."', '6', '6.6', '11', 'low stock', '41', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274104483', 'Z110050BCUN035', 1, 'curl passion conditioner', '300ml', '"Will help make curls bouncy, flexible and long-lasting. This specially formulated hydrating conditioner for curly hair is Paraben free. Ideal for all hair types to give softness and manageability without weighing the hair down."', '12.1', '13.31', '12', 'low stock', '40', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274105565', 'Z110050CCUN035', 1, 'curl passion leave in', '300ml', '"A specific leave-in conditioner for curly hair! Curl Passion leave in conditioning treatment is a treatment spray to help make curls soft, bouncy, flexible and long-lasting. For best results, use after Curl Passion Conditioner."', '12.1', '13.31', '13', 'low stock', '39', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274104476', 'Z110050ACUN035', 1, 'curl passion shampoo', '300ml', '"Specifically formulated to gently cleanse curly hair while hydrating, conditioning and counteracting frizz. To assist curl formation and keep curls in place for longer, leaving hair supple, shiny and in great condition."', '12.1', '13.31', '14', 'low stock', '38', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274078111', 'Z110050CCDE020', 1, 'curl perfectionist', '150ml', '"This styling cream will define and tame curls, banishing frizz and intensifying the natural texture of the hair. It will also give you long-lasting shape, softness and shine. It is a must have for anyone with curly or wavy hair."', '12.1', '13.31', '15', 'low stock', '37', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274060673', 'Z110050BDCO025', 1, 'deep color maintainer balm', '175ml', '"Crafted with precision, combines active ingredients like milk proteins, Integrity 41®, & quinoa proteins to amplify color stability. Imparts a consistent & structured shield for deep protection, resulting in irresistibly soft, glossy, & visibly vibrant hair."', '12.1', '13.31', '16', 'low stock', '36', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274059882', 'Z110050BENE035', 1, 'energizing blend conditioner', '300ml', '"A nourishing conditioner for fine, thinning, & fragile hair. Hydrates, detangles, & adds softness without weighing hair down. Contains organic rosemary & sage extracts, Fioravanti balm, glycerine, menthol, panthenol, rice bran oil, vitamin E, & 11 essential oils."', '12.1', '13.31', '17', 'low stock', '35', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274060376', 'Z110050HENE013', 1, 'energizing blend scalp treatment', '30ml', '"A revitalising lotion to replenish volume, thickness, & shine to weak, thinning, & delicate hair. Enriched with organic rosemary & organic sage extracts, Fioravanti balm, arnica extract, natural camphor, hydrolysed rice proteins, & panthenol."', '7.7', '8.47', '18', 'low stock', '34', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274059875', 'Z110050AENE035', 1, 'energizing blend shampoo', '300ml', '"A revitalising shampoo for fine, thinning, & fragile hair. Cleanses gently while promoting scalp & hair health. Enriched with organic rosemary & sage extracts, Fioravanti balm, glycerine, eucalyptol, panthenol, & 11 essential oils."', '12.1', '13.31', '19', 'low stock', '33', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274050476', 'Z110050HGAO010', 1, 'glistening argan oil', '50ml', '"With its exceptional organic argan formula, this product instantly nourishes & conditions hair, resulting in softness, vibrancy, added volume, & remarkable shine."', '27.5', '30.25', '20', 'low stock', '32', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147312', 'Z110050BHBL030', 1, 'icy blond conditioner', '250ml', '"Formulated with a specific black pigment, it gives hair softness whilst counteracting yellow or orange tones in blond or lightened hair, giving ash tones. Milk proteins restructure the hair deeply, giving hair a healthy vibrant appearance."', '13.2', '14.52', '21', 'low stock', '31', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147282', 'Z110050AHBL035', 1, 'icy blond shampoo', '300ml', '"Cleanses hair gently with a special black pigment developed to remove yellow/ yellow orange tones from blondes or lightened hair, whilst giving a delicate ash tone. Milk proteins restructure the hair deeply giving hair a healthy vibrant appearance."', '13.2', '14.52', '22', 'low stock', '30', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274059639', 'Z110050CICR010', 1, 'incredible milk', '50 ml', '"An intensive spray mask for all hair types with muru muru butter, fruit extracts, milk proteins. Benefits including frizz control, split end prevention, heat protection, improved hold, detangling, enhanced shine, added body & volume, styling, color maintenance, UV protection, & cuticle smoothing."', '12.1', '6.6', '23', 'low stock', '29', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274055556', 'Z110050CICR020', 1, 'incredible milk', '150 ml', '"An intensive spray mask for all hair types with muru muru butter, fruit extracts, milk proteins. Benefits including frizz control, split end prevention, heat protection, improved hold, detangling, enhanced shine, added body & volume, styling, color maintenance, UV protection, & cuticle smoothing."', '12.1', '13.31', '24', 'low stock', '28', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274109068', 'Z110050HINN010', 1, 'integrity incredible oil', '50ml', '"A film-forming leave-in treatment formulated to protect hair from the heat from blow-drying, straightening and other stress factors that weaken the hair."', '24.2', '26.62', '25', 'in stock', '27', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274106210', 'Z110050BINT025', 1, 'integrity intensive treatment', '200ml', '"A specific formula to nourish hair deeply. It seals the cuticle, eliminating frizz. Gives softness, shine and manageability to hair, whilst protecting hair colour. Ideal for normal or coarse hair."', '12.1', '13.31', '26', 'in stock', '26', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274106180', 'Z110050BINN035', 1, 'integrity nourishing conditioner', '300ml', 'Specifically formulated to condition and nourish all hair types. With organic muru muru butter to condition and nourish the hair.', '12.1', '13.31', '27', 'in stock', '25', 'in stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274142935', 'Z11050BINN010', 1, 'integrity nourishing conditioner', '50 ml', 'Specifically formulated to condition and nourish all hair types. With organic muru muru butter to condition and nourish the hair.', '6', '6.6', '28', 'in stock', '24', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274106234', 'Z110050BIMB025', 1, 'integrity nourishing muru muru butter', '200ml', '"Nourishing butter with organic muru muru for all hair types, a unique nourishing blend for a deep treatment. Leaves hair soft, nourished and lustrous immediately after the first application."', '24.2', '26.62', '29', 'in stock', '23', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274106159', 'Z110050AINN035', 1, 'integrity nourishing shampoo', '300ml', '"Its formula with organic muru muru butter cleanses gently, nourishing and conditioning the hair instantly, eliminating frizz, leaving hair soft and radiant."', '12.1', '13.31', '30', 'in stock', '22', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274142928', 'Z110050AINN010', 1, 'integrity nourishing shampoo', '50 ml', '"Its formula with organic muru muru butter cleanses gently, nourishing and conditioning the hair instantly, eliminating frizz, leaving hair soft and radiant."', '6', '6.6', '31', 'in stock', '21', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274106241', 'Z110050HIRL010', 1, 'integrity repairing hair lotion (8 fiale)', '12ml', '"A concentrated repairing protein treatment with amino acids to restructure and strengthen damaged and chemically treated hair, leaving it soft and shiny. With organic muru muru butter to condition and nourish the hair."', '18.15', '19.965', '32', 'in stock', '20', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274051534', 'Z110050BLIC040', 1, 'leave in conditioner', '350ml', '"Spray conditioner, gives manageability to damaged hair, protecting the hair shaft & enhancing the moisture balance of the hair. Milk proteins supplement & strengthen the hair shaft whilst fruit extracts & honey revitalize the hair, making it shiny & more manageable."', '12.1', '13.31', '33', 'in stock', '19', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274046721', 'Z110050BLIC013', 1, 'leave in conditioner', '75 ml', '"Spray conditioner, gives manageability to damaged hair, protecting the hair shaft & enhancing the moisture balance of the hair. Milk proteins supplement & strengthen the hair shaft whilst fruit extracts & honey revitalize the hair, making it shiny & more manageable."', '6', '6.6', '34', 'in stock', '18', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274076643', 'Z110050BMOS030', 1, 'moisture plus conditioner', '250ml', '"Gives softness and manageability to dry hair, maintaining the hair’s optimal moisture balance and maintains hair colour."', '12.1', '13.31', '35', 'in stock', '17', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274141129', 'Z110050BMOS010', 1, 'moisture plus conditioner', '50 ml', '"Gives softness and manageability to dry hair, maintaining the hair’s optimal moisture balance and maintains hair colour."', '6', '6.6', '36', 'in stock', '16', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274076582', 'Z110050AMOS035', 1, 'moisture plus shampoo', '300ml', 'Delicate shampoo ideal for giving softness and manageability to dry hair while maintaining its optimal water balance.', '12.1', '13.31', '37', 'in stock', '15', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274141112', 'Z110050AMOS010', 1, 'moisture plus shampoo', '50 ml', 'Delicate shampoo ideal for giving softness and manageability to dry hair while maintaining its optimal water balance.', '6', '6.6', '38', 'in stock', '14', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274076636', 'Z110050BMLF025', 1, 'moisture plus whipped cream', '200ml', '"Ideal to give softness and manageability to dry hair, maintaining the hair’s optimal moisture balance. Leaves hair soft, shiny and healthy."', '12.1', '13.31', '39', 'in stock', '13', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274172734', 'Z110PROM0247', 1, 'pochette', '', 'pochette for three mini sizes', '0', '0', '40', 'in stock', '12', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274076544', 'Z110050BNST030', 1, 'silver shine conditioner', '250ml', '"Formulated with a specific pigment that neutralises unwanted yellow, golden or brassy tones that are common in white, grey, blond or lightened hair. Leaves hair conditioned, soft & full of vitality."', '13.2', '14.52', '41', 'in stock', '11', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274141105', 'Z110050BNST010', 1, 'silver shine conditioner', '50 ml', '"Formulated with a specific pigment that neutralises unwanted yellow, golden or brassy tones that are common in white, grey, blond or lightened hair. Leaves hair conditioned, soft & full of vitality."', '6', '6.6', '42', 'in stock', '10', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274061892', 'Z110050ANST035', 1, 'silver shine shampoo', '300ml', '"A delicate cleanser with a specific violet pigment that has a balanced action, neutralising even the most subtle unwanted yellow tones typical of white, grey, blond or lightened hair."', '13.2', '14.52', '43', 'in stock', '9', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274141099', 'Z110050ANST010', 1, 'silver shine shampoo', '50 ml', '"A delicate cleanser with a specific violet pigment that has a balanced action, neutralising even the most subtle unwanted yellow tones typical of white, grey, blond or lightened hair."', '6', '6.6', '44', 'in stock', '8', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274061960', 'Z110050BSEW025', 1, 'silver shine whipped cream', '200ml', '"Formulated with milk proteins, organic berry extracts & Integrity 41® to condition the hair, maintaining its ideal moisture balance & protecting hair colour. Neutralises unwanted yellow & golden tones."', '13.2', '14.52', '45', 'in stock', '7', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147084', 'Z110700ASAN030', 1, 'sun and more - all over shampoo', '250ml', '"Cleansing and soothing formula for hair, ideal after swimming in the sea or pool, or year-round to leave skin soft and protect hair from the lightening and dehydrating effects of the sun."', '12', '13.2', '46', 'in stock', '6', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147091', 'Z110700BSMN025', 1, 'sun and more - beauty mask', '200ml', '"Intensive nourishing and conditioning mask for hair to be used after shampooing to soothe the damaging effects of the sun, salt, wind and chlorine. Leaves hair soft, radiant and manageable."', '12', '13.2', '47', 'in stock', '5', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274169390', 'Z110700BSI030', 1, 'sun and more - bi-phase leave in', '250ml', '"This bi-phase leave in conditioner is a no rinse conditioning spray. It provides protective action for hair that is exposed to sun, sea water and chlorine. It also improves manageability."', '12', '13.2', '48', 'in stock', '4', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274147107', 'Z110700CSMN020', 1, 'sun and more - incredible milk', '140ml', '"A spray mask with 12 cosmetic benefits in a ‘sun edition’, with added UV filters, to give all the necessary elements to protect hair during the summer. No rinse formula."', '12', '13.2', '49', 'in stock', '3', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274078067', 'Z110050BVLU035', 1, 'volumizing conditioner', '300ml', '"Indulge in the luxury of this paraben-free conditioner that deeply nourishes and adds softness and body to your hair, without weighing it down. Enriched with precious sugar derivatives, it effectively transforms and volumizes all hair types, delivering a deep revitalising action for truly rejuvenated locks."', '12.1', '13.31', '50', 'in stock', '2', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274078036', 'Z110050AVLU035', 1, 'volumizing shampoo', '300ml', '"Experience the freedom of volume and body with this paraben and SLES-free shampoo. Designed to cleanse gently, its special formula harnesses the power of precious sugar derivatives to transform your hair, giving it a fuller and more voluminous look without any unwanted heaviness."', '12.1', '13.31', '51', 'in stock', '1', 'low stock');
INSERT INTO PRODUCTS (BARCODE, SKU, BRAND_ID, PNAME, SIZE, PDESCRIPTION, PRICE, PRICE_VAT, LOCAL_QTY, LOCAL_STATUS, WAREHOUSE_QTY, WAREHOUSE_STATUS) VALUES ('8032274078340', 'Z110050CVBS025', 1, 'volumizing styling spray', '175ml', '"Elevate your hair game with this transformative styling spray. It adds structure and imparts a radiant shine while creating weightless volume with sugar derivatives. Experience long-lasting, flexible hold without compromising manageability and enjoy hair that stands out."', '12.1', '13.31', '52', 'in stock', '0', 'out of stock');


INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Nails n Things','Salon','Juffair','nails.things@hotmail.com',33121234);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Selena Gomez','Influencer','Saar','sel.gom@gmail.com',33872645);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Lawazon','Salon','Muharraq','lawazon@hotmail.com',36849205);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Betty White','Influencer','Manama','b.white@gmail.com',39472645);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Mello','Salon','Budaiya','mello@hotmail.com',66738495);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Dua Lipa','Influencer','Hamala','dualipa@gmail.com',77645392);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Blooming','Salon','Janabiya','blooming@hotmail.com',66384537);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Zendaya','Influencer','Diyar','thezendaya@gmail.com',33224455);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Diva','Salon','Marrassi','diva@hotmail.com',34758394);
INSERT INTO CONTACTS(CNAME,CTYPE,CADDRESS,EMAIL,PHONE_NUMBER) VALUES ('Gigi Hadid','Influencer','Zallaq','gigihdd@gmail.com',32849506);

INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'1-Jun-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'8-Jun-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'15-Jun-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'22-Jun-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'29-Jun-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'6-Jul-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'13-Jul-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'20-Jul-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'27-Jul-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'3-Aug-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'10-Aug-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'17-Aug-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'24-Aug-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'31-Aug-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'7-Sep-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'14-Sep-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'21-Sep-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'28-Sep-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'5-Oct-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'12-Oct-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'19-Oct-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'26-Oct-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'2-Nov-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'9-Nov-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'16-Nov-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'23-Nov-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'30-Nov-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'7-Dec-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'14-Dec-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'21-Dec-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'28-Dec-23',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'4-Jan-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'11-Jan-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'18-Jan-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'25-Jan-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'1-Feb-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'8-Feb-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'15-Feb-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'22-Feb-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'29-Feb-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'7-Mar-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'14-Mar-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'21-Mar-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'28-Mar-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'4-Apr-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'11-Apr-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'18-Apr-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'25-Apr-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'2-May-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'9-May-24',2);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'1-Jun-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'8-Jun-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'15-Jun-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'22-Jun-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'29-Jun-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'6-Jul-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'13-Jul-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'20-Jul-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'27-Jul-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'3-Aug-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'10-Aug-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'17-Aug-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'24-Aug-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'31-Aug-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'7-Sep-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'14-Sep-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'21-Sep-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'28-Sep-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'5-Oct-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'12-Oct-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'19-Oct-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'26-Oct-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'2-Nov-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'9-Nov-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'16-Nov-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'23-Nov-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'30-Nov-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'7-Dec-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'14-Dec-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'21-Dec-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'28-Dec-23',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'4-Jan-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'11-Jan-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'18-Jan-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'25-Jan-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'1-Feb-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'8-Feb-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'15-Feb-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'22-Feb-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'29-Feb-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (1,'7-Mar-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (2,'14-Mar-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (3,'21-Mar-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (4,'28-Mar-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (5,'4-Apr-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (6,'11-Apr-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (7,'18-Apr-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (8,'25-Apr-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (9,'2-May-24',3);
INSERT INTO ORDERS(CONTACT_ID,ORDER_DATE,EMPLOYEE_ID) VALUES (10,'9-May-24',3);

INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (1,1,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (1,47,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (1,39,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (1,31,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (2,2,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (2,48,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (2,40,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (2,32,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (3,3,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (3,49,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (3,41,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (3,33,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (4,4,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (4,50,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (4,42,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (4,34,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (5,5,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (5,51,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (5,43,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (5,35,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (6,6,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (6,52,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (6,44,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (6,36,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (7,7,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (7,53,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (7,45,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (7,37,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (8,8,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (8,54,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (8,46,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (8,38,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (9,9,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (9,1,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (9,47,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (9,39,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (10,10,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (10,2,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (10,48,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (10,40,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (11,11,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (11,3,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (11,49,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (11,41,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (12,12,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (12,4,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (12,50,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (12,42,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (13,13,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (13,5,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (13,51,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (13,43,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (14,14,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (14,6,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (14,52,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (14,44,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (15,15,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (15,7,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (15,53,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (15,45,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (16,16,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (16,8,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (16,54,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (16,46,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (17,17,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (17,9,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (17,1,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (17,47,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (18,18,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (18,10,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (18,2,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (18,48,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (19,19,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (19,11,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (19,3,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (19,49,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (20,20,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (20,12,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (20,4,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (20,50,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (21,21,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (21,13,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (21,5,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (21,51,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (22,22,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (22,14,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (22,6,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (22,52,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (23,23,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (23,15,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (23,7,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (23,53,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (24,24,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (24,16,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (24,8,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (24,54,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (25,25,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (25,17,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (25,9,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (25,1,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (26,26,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (26,18,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (26,10,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (26,2,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (27,27,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (27,19,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (27,11,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (27,3,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (28,28,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (28,20,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (28,12,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (28,4,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (29,29,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (29,21,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (29,13,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (29,5,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (30,30,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (30,22,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (30,14,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (30,6,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (31,31,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (31,23,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (31,15,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (31,7,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (32,32,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (32,24,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (32,16,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (32,8,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (33,33,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (33,25,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (33,17,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (33,9,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (34,34,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (34,26,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (34,18,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (34,10,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (35,35,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (35,27,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (35,19,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (35,11,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (36,36,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (36,28,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (36,20,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (36,12,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (37,37,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (37,29,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (37,21,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (37,13,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (38,38,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (38,30,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (38,22,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (38,14,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (39,39,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (39,31,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (39,23,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (39,15,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (40,40,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (40,32,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (40,24,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (40,16,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (41,41,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (41,33,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (41,25,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (41,17,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (42,42,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (42,34,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (42,26,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (42,18,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (43,43,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (43,35,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (43,27,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (43,19,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (44,44,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (44,36,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (44,28,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (44,20,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (45,45,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (45,37,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (45,29,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (45,21,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (46,46,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (46,38,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (46,30,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (46,22,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (47,47,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (47,39,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (47,31,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (47,23,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (48,48,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (48,40,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (48,32,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (48,24,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (49,49,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (49,41,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (49,33,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (49,25,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (50,50,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (50,42,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (50,34,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (50,26,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (51,51,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (51,43,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (51,35,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (51,27,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (52,52,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (52,44,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (52,36,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (52,28,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (53,53,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (53,45,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (53,37,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (53,29,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (54,54,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (54,46,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (54,38,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (54,30,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (55,1,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (55,47,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (55,39,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (55,31,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (56,2,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (56,48,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (56,40,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (56,32,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (57,3,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (57,49,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (57,41,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (57,33,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (58,4,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (58,50,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (58,42,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (58,34,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (59,5,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (59,51,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (59,43,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (59,35,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (60,6,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (60,52,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (60,44,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (60,36,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (61,7,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (61,53,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (61,45,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (61,37,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (62,8,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (62,54,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (62,46,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (62,38,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (63,9,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (63,1,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (63,47,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (63,39,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (64,10,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (64,2,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (64,48,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (64,40,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (65,11,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (65,3,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (65,49,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (65,41,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (66,12,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (66,4,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (66,50,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (66,42,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (67,13,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (67,5,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (67,51,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (67,43,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (68,14,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (68,6,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (68,52,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (68,44,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (69,15,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (69,7,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (69,53,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (69,45,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (70,16,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (70,8,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (70,54,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (70,46,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (71,17,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (71,9,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (71,1,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (71,47,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (72,18,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (72,10,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (72,2,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (72,48,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (73,19,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (73,11,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (73,3,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (73,49,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (74,20,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (74,12,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (74,4,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (74,50,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (75,21,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (75,13,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (75,5,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (75,51,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (76,22,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (76,14,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (76,6,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (76,52,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (77,23,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (77,15,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (77,7,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (77,53,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (78,24,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (78,16,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (78,8,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (78,54,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (79,25,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (79,17,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (79,9,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (79,1,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (80,26,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (80,18,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (80,10,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (80,2,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (81,27,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (81,19,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (81,11,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (81,3,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (82,28,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (82,20,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (82,12,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (82,4,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (83,29,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (83,21,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (83,13,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (83,5,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (84,30,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (84,22,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (84,14,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (84,6,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (85,31,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (85,23,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (85,15,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (85,7,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (86,32,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (86,24,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (86,16,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (86,8,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (87,33,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (87,25,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (87,17,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (87,9,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (88,34,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (88,26,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (88,18,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (88,10,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (89,35,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (89,27,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (89,19,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (89,11,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (90,36,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (90,28,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (90,20,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (90,12,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (91,37,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (91,29,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (91,21,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (91,13,2);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (92,38,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (92,30,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (92,22,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (92,14,4);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (93,39,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (93,31,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (93,23,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (93,15,6);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (94,40,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (94,32,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (94,24,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (94,16,8);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (95,41,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (95,33,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (95,25,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (95,17,10);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (96,42,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (96,34,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (96,26,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (96,18,1);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (97,43,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (97,35,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (97,27,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (97,19,3);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (98,44,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (98,36,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (98,28,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (98,20,5);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (99,45,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (99,37,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (99,29,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (99,21,7);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (100,46,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (100,38,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (100,30,9);
INSERT INTO PRODUCTS_ORDERS(ORDER_ID,PRODUCT_ID,QTY) VALUES (100,22,9);
