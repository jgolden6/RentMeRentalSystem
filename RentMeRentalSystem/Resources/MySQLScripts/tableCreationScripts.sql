CREATE TABLE employee(
    employeeId INTEGER NOT NULL AUTO_INCREMENT,
    fname VARCHAR(20) NOT NULL,
    lname VARCHAR(20) NOT NULL,
    gender VARCHAR(6) NOT NULL,
    birthdate DATE NOT NULL,
    phoneNumber CHAR(10) NOT NULL DEFAULT '0000000000',
    addressLine1 VARCHAR(20) NOT NULL,
    addressLine2 VARCHAR(20) NOT NULL DEFAULT '',
    zipcode CHAR(5) NOT NULL,
    city VARCHAR(20) NOT NULL,
    state CHAR(15) NOT NULL,
    username VARCHAR(10) NOT NULL,
    empPassword VARCHAR(15) NOT NULL,
    PRIMARY KEY(employeeId),
    UNIQUE(username)
);

Alter table `employee` AUTO_INCREMENT = 1000000

INSERT INTO employee(
    employeeId,
    fname,
    lname,
    gender,
    birthdate,
    phoneNumber,
    addressLine1,
    addressLine2,
    zipcode,
    city,
    state,
    username,
    empPassword	
)
VALUES(
    0,
    'Paul',
    'Wells',
    'Male',
    '1984-05-07',
    '7702127785',
    '753 Willing Way',
    '',
    '35447',
    'Carrollton',
    'Alabama',
    'pWells',
    'teranicaLove'
);

CREATE TABLE customer(
    customerId INTEGER NOT NULL AUTO_INCREMENT,
    fname VARCHAR(20) NOT NULL,
    lname VARCHAR(20) NOT NULL,
    gender VARCHAR(6) NOT NULL,
    birthdate DATE NOT NULL,
    phoneNumber CHAR(10) NOT NULL DEFAULT '0000000000',
    addressLine1 VARCHAR(20) NOT NULL,
    addressLine2 VARCHAR(20) NOT NULL DEFAULT '',
    zipcode CHAR(5) NOT NULL,
    city VARCHAR(20) NOT NULL,
    state CHAR(15) NOT NULL,
    registrationDate Date NOT NULL,
    PRIMARY KEY(customerId)
);

Alter table `customer` AUTO_INCREMENT = 100

INSERT INTO customer(
    customerId,
    fname,
    lname,
    gender,
    birthdate,
    phoneNumber,
    addressLine1,
    addressLine2,
    zipcode,
    city,
    state,
    registrationDate	
)
VALUES(
    0,
    'Teresa',
    'Walker',
    'Female',
    '1962-01-20',
    '6787361727',
    '6369 Forresterway',
    '',
    '30038',
    'Lithonia',
    'Georgia',
    '2016-08-30'
),(
    0,
    'Alberta',
    'Elam',
    'Female',
    '1953-03-28',
    '4043458789',
    '162 Cobb Rd',
    'Apt 1124',
    '30066',
    'Cobb',
    'Georgia',
    '2020-11-13'
);

CREATE TABLE `transaction`(
    transactionId INTEGER AUTO_INCREMENT NOT NULL,
    employeeId INTEGER NOT NULL,
    customerId INTEGER NOT NULL,
    fee DECIMAL(9, 2) NOT NULL,
    dueDate DATE NOT NULL,
    PRIMARY KEY(transactionId),
    FOREIGN KEY(employeeId) REFERENCES employee(employeeId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY(customerId) REFERENCES customer(customerId) ON DELETE CASCADE ON UPDATE CASCADE
);

ALTER TABLE
    `transaction` AUTO_INCREMENT = 1000;

CREATE TABLE rental_transaction(
    rentalId INTEGER NOT NULL,
    rentalDate DATE NOT NULL,
    PRIMARY KEY(rentalId),
    FOREIGN KEY(rentalId) REFERENCES `transaction`(transactionId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE furniture(
    furnitureId INTEGER AUTO_INCREMENT NOT NULL,
    categoryName VARCHAR(30) NOT NULL DEFAULT "Miscellaneous",
    styleName VARCHAR(30) NOT NULL DEFAULT "Miscellaneous",
    daily_rental_rate DECIMAL(9, 2),
    quantity INTEGER NOT NULL,
    PRIMARY KEY(furnitureId),
    FOREIGN KEY(categoryName) REFERENCES category(categoryName) ON DELETE SET DEFAULT ON UPDATE CASCADE,
    FOREIGN KEY(styleName) REFERENCES style(styleName) ON DELETE SET DEFAULT ON UPDATE CASCADE
);

alter table `furniture` auto_increment = 10000;

INSERT INTO furniture
VALUES(0, "Beds", "Contemporary", 1.99, 7),(0, "Chairs", "Oriental", 2.99, 25),(
    0,
    "Couches",
    "Traditional",
    0.25,
    3
),(0, "Desks", "Victorian", 2.99, 12),(
    0,
    "Dining Tables",
    "Modern",
    0.75,
    9
),(0, "Beds", "Oriental", 2.39, 4),(0, "Chairs", "Contemporary", 1.55, 12),(
    0,
    "Couches",
    "Victorian",
    3.20,
    2
),(0, "Desks", "Modern", 3.19, 8),(
    0,
    "Dining Tables",
    "Traditional",
    0.55,
    6
),(0, "Beds", "Modern", 1.50, 20),(0, "Chairs", "Traditional", 0.88, 10),(
    0,
    "Couches",
    "Oriental",
    2.50,
    14
),(0, "Desks", "Contemporary", 2.24, 5),(
    0,
    "Dining Tables",
    "Victorian",
    3.57,
    13
);

CREATE TABLE rental_item(
    rentalId INTEGER NOT NULL,
    furnitureId INTEGER NOT NULL,
    quantity INTEGER NOT NULL,
    PRIMARY KEY(rentalId, furnitureId),
    FOREIGN KEY(rentalId) REFERENCES rental_transaction(rentalId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY(furnitureId) REFERENCES furniture(furnitureId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE category(
    categoryName VARCHAR(30) NOT NULL,
    PRIMARY KEY(categoryName)
);

INSERT INTO category
VALUES("Miscellaneous"),("Beds"),("Dining Tables"),("Desks"),("Couches"),("Chairs");

CREATE TABLE style(
    styleName VARCHAR(30) NOT NULL,
    PRIMARY KEY(styleName)
);

INSERT INTO style
VALUES("Miscellaneous"),("Modern"),("Victorian"),("Oriental"),("Traditional"),("Contemporary");

CREATE TABLE return_transaction(
    returnId INTEGER NOT NULL,
    returnDate DATE NOT NULL,
    PRIMARY KEY(returnId),
    FOREIGN KEY(returnId) REFERENCES `transaction`(transactionId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE return_item(
    returnId INTEGER NOT NULL,
    rentalId INTEGER NOT NULL,
    furnitureId INTEGER NOT NULL,
    quantity INTEGER NOT NULL,
    PRIMARY KEY(returnId, rentalId, furnitureId),
    FOREIGN KEY(returnId) REFERENCES return_transaction(returnId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY(rentalId) REFERENCES rental_transaction(rentalId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY(furnitureId) REFERENCES furniture(furnitureId) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP
PROCEDURE IF EXISTS retrieve_transactions_of_type;
DELIMITER
    $
CREATE PROCEDURE retrieve_transactions_of_type(transaction_type CHAR(6))
BEGIN
    DECLARE
        type_table_transaction_id INTEGER ; DECLARE type_table_id_column CHAR(8) ; DECLARE type_table_name CHAR(18) ;
    
    SELECT COLUMN_NAME,
        TABLE_NAME
    INTO type_table_id_column, type_table_name
FROM
    INFORMATION_SCHEMA.COLUMNS,
    INFORMATION_SCHEMA.TABLES
WHERE
    TABLE_SCHEMA = 'cs3230f21j' AND TABLE_NAME LIKE transaction_type AND COLUMN_KEY = "PRI" ;
    
    
SELECT
    type_table_id_column
INTO type_table_transaction_id
FROM
    type_table_name;
    
SELECT
    *
FROM
    `transaction`, type_table_name
WHERE
    type_table_transaction_id = `transaction`.`transactionId`; END$

DROP
PROCEDURE IF EXISTS SPLIT_STR;
CREATE PROCEDURE SPLIT_STR(
    X TEXT,
    delim TEXT,
    pos INT,
    OUT text_at_position TEXT
)
DROP
PROCEDURE IF EXISTS calculate_rental_transaction_cost;
DELIMITER
    $
CREATE PROCEDURE calculate_rental_transaction_cost(IN items TEXT)
BEGIN
    DECLARE
        cost DECIMAL(9, 2) ;
    SET
        cost =(
        SELECT
            SUM(
                f.daily_rental_rate * qty * TIMESTAMPDIFF(DAY, NOW(), due_date))
            FROM
                furniture f,
                JSON_TABLE(
                    items,
                    "$[*]" COLUMNS(
                        furitureId INTEGER PATH "$.id",
                        qty INTEGER PATH "$.qty",
                        due_date DATE PATH "$.dueDate"
                    )
                ) AS pre_rental_items
            WHERE
                f.furnitureId = furitureId
            ) ;
        SELECT
            cost ;
    END

CALL
    calculate_rental_transaction_cost(
        "[{\"id\":10000, \"qty\":5, \"dueDate\": \"2021-12-11\"}, {\"id\":10001, \"qty\":3, \"dueDate\": \"2021-12-11\"}, {\"id\":10004, \"qty\":6, \"dueDate\": \"2021-12-11\"}]");

        DROP
DROP
PROCEDURE IF EXISTS create_rental_transaction
DELIMITER
    $
CREATE PROCEDURE create_rental_transaction(
    IN rental TEXT,
    IN rental_items TEXT
)
BEGIN
    DECLARE EXIT
HANDLER FOR SQLEXCEPTION
BEGIN
    ROLLBACK
        ;
    SELECT
        "rolled back" ;
END ;
START TRANSACTION
    ;
INSERT INTO `transaction`(
    employeeId,
    customerId,
    fee,
    dueDate
)
SELECT
    employeeId,
    customerId,
    fee,
    dueDate
FROM
    JSON_TABLE(
        rental,
        "$[*]" COLUMNS(
            employeeId INTEGER PATH "$.employeeId",
            customerId INTEGER PATH "$.customerId",
            fee DECIMAL(9, 2) PATH "$.fee",
            dueDate DATE PATH "$.transactionDueDate"
        )
    ) AS a_rental_transaction ;
INSERT INTO rental_transaction(rentalId, rentalDate)
SELECT
    LAST_INSERT_ID(), NOW() ;
INSERT INTO rental_item(
    rentalId,
    furnitureId,
    quantity
)
SELECT
    LAST_INSERT_ID(), furnitureId, qty
FROM
    JSON_TABLE(
        rental_items,
        "$[*]" COLUMNS(
            furnitureId INTEGER PATH "$.furnitureId",
            qty INTEGER PATH "$.qty"
        )
    ) AS current_rental_items ;

COMMIT
    ;
    END

CALL
    create_rental_transaction(
        "[{\"employeeId\":1000000, \"customerId\":102, \"fee\": 234.20, \"transactionDueDate\": \"2021-12-11\"}]",
        "[{\"furnitureId\":10000, \"qty\":5}, {\"furnitureId\":10001, \"qty\":3}, {\"furnitureId\":10004, \"qty\":6}]"
    )

DROP
PROCEDURE IF EXISTS update_furniture_quanities;
DELIMITER
    $
CREATE PROCEDURE update_furniture_quanities()
BEGIN
    UPDATE
        furniture, rental_item
    SET
        furniture.quantity = furniture.quantity - rental_item.quantity
    WHERE
        rental_item.furnitureId = furniture.furnitureId AND rental_item.rentalId =( SELECT MAX(rentalId) FROM rental_item ) ;
END
CALL
    update_furniture_quanities()

SELECT
    ri.rentalId,
    f.furnitureId,
    f.categoryName,
    f.styleName,
    f.daily_rental_rate,
    ri.quantity
FROM
    furniture f,
    rental_item ri
WHERE
    f.furnitureId = ri.furnitureId AND ri.rentalId =( SELECT MAX(rentalId) FROM rental_item );