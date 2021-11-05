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