CREATE DATABASE NanatsuDleDB;
GO

USE NanatsuDleDB;
GO

CREATE TABLE Genders (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(50)    NOT NULL
);

CREATE TABLE Races (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(50)    NOT NULL
);

CREATE TABLE Arcs (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(100)   NOT NULL
);

CREATE TABLE HairColors (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(50)    NOT NULL
);

CREATE TABLE Affiliations (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(100)   NOT NULL
);

CREATE TABLE TypesOfSkills (
    Id          INT             PRIMARY KEY IDENTITY(1,1),
    Name        NVARCHAR(50)    NOT NULL
);

CREATE TABLE Characters (
    Id              INT             PRIMARY KEY IDENTITY(1,1),
    Name            NVARCHAR(100)   NOT NULL,
    ImageUrl        NVARCHAR(500)   NOT NULL,
    Image2Url       NVARCHAR(500)   NOT NULL,
    Height          INT             NOT NULL,
    Magic           NVARCHAR(100)   NOT NULL,
    FirstAppearance NVARCHAR(100)   NOT NULL,
    GenderId        INT FOREIGN KEY REFERENCES Genders(Id),
    RaceId         INT FOREIGN KEY REFERENCES Races(Id),
    ArcId           INT FOREIGN KEY REFERENCES Arcs(Id),
    HairColorId     INT FOREIGN KEY REFERENCES HairColors(Id),
    AffiliationId   INT FOREIGN KEY REFERENCES Affiliations(Id),
    TypeOfSkillId   INT FOREIGN KEY REFERENCES TypesOfSkills(Id)
);
GO

INSERT INTO Genders (Name) VALUES ('Masculino'), ('Femenino');

INSERT INTO Races (Name) VALUES ('Demonio'), ('Diosa'), ('Humano'), ('Hada'), ('Gigante'), ('Muñeco');

INSERT INTO Arcs (Name) VALUES ('Introduccion'), ('Pecados Capitales'), ('Bosque de las Hadas'), ('Diez Mandamientos'), ('Guerra Santa');

INSERT INTO HairColors (Name) VALUES 
('Rubio'), ('Plateado'), ('Castaño'), ('Negro'), ('Gris'), ('Verde'), ('Rosado'), ('Naranja');

INSERT INTO Affiliations (Name) VALUES 
('Siete Pecados Capitales'), ('Caballeros Sagrados'), ('Diez Mandamientos'), ('Cuatro Arcangeles');

INSERT INTO TypesOfSkills (Name) VALUES 
('Ofensivo'), ('Apoyo'), ('Defensivo'), ('Ofensivo,Defensivo'), ('Apoyo,Ofensivo'), ('Defensivo,Ofensivo');

INSERT INTO Characters 
(Name, ImageUrl, Image2Url, Height, Magic, FirstAppearance, GenderId, RaceId, ArcId, HairColorId, AffiliationId, TypeOfSkillId)
VALUES
('Meliodas', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774756129/Meliodas.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624733/Meliodas2_xb727m.jpg', 152, 'Full Counter', 'Capitulo 1', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Rubio'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Elizabeth', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774761968/Elizabeth_flupl3.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624733/Elizabeth2_z1f3qn.jpg', 162, 'Invocacion Divina', 'Capitulo 1', (SELECT Id FROM Genders WHERE Name='Femenino'), (SELECT Id FROM Races WHERE Name='Diosa'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Plateado'), (SELECT Id FROM Affiliations WHERE Name='Caballeros Sagrados'), (SELECT Id FROM TypesOfSkills WHERE Name='Apoyo')),

('Ban', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067958/Ban_fpsj0d.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624732/Ban2_nfyemi.png', 240, 'Snatch', 'Capitulo 2', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Plateado'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('King', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067958/King_rzw39y.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624732/King2_meeqxr.png', 160, 'Disaster', 'Capitulo 3', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Hada'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Castaño'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Defensivo,Ofensivo')),

('Diane', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774761969/Diane_ntxm6o.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624733/Diane2_sq8plh.jpg', 915, 'Creation', 'Capitulo 2', (SELECT Id FROM Genders WHERE Name='Femenino'), (SELECT Id FROM Races WHERE Name='Gigante'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Castaño'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo,Defensivo')),

('Gowther', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067956/Gowther_hmdach.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624732/Gowther2_w9siye.png', 175, 'Invasion', 'Capitulo 50', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Muñeco'), (SELECT Id FROM Arcs WHERE Name='Pecados Capitales'), (SELECT Id FROM HairColors WHERE Name='Rosado'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Apoyo')),

('Merlin', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067956/Merlin_cndhgm.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624732/Merlin2_hnlxkn.png', 177, 'Infinity', 'Capitulo 51', (SELECT Id FROM Genders WHERE Name='Femenino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Pecados Capitales'), (SELECT Id FROM HairColors WHERE Name='Negro'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Apoyo,Ofensivo')),

('Escanor', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067955/Escanor_rz6eqi.png', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775624732/Escanor2_xezc7b.png', 202, 'Sunshine', 'Capitulo 53', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Pecados Capitales'), (SELECT Id FROM HairColors WHERE Name='Castaño'), (SELECT Id FROM Affiliations WHERE Name='Siete Pecados Capitales'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Hendrickson', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067955/Hendrickson_e81ng9.png', 'HENDRICKSON', 187, 'Purge', 'Capitulo 4', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Gris'), (SELECT Id FROM Affiliations WHERE Name='Caballeros Sagrados'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo,Defensivo')),

('Gilthunder', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774847692/Gilthunder_p9owkx.png', 'GILTHUNDER', 186, 'Thunder', 'Capitulo 5', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Castaño'), (SELECT Id FROM Affiliations WHERE Name='Caballeros Sagrados'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Dreyfus', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067955/Dreyfus_c9uw5s.png', 'DREYFUS', 198, 'Break', 'Capitulo 40', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Humano'), (SELECT Id FROM Arcs WHERE Name='Introduccion'), (SELECT Id FROM HairColors WHERE Name='Negro'), (SELECT Id FROM Affiliations WHERE Name='Caballeros Sagrados'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Helbram', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774761371/Helbram_yscvcz.png', 'HELBRAM', 174, 'Link', 'Capitulo 72', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Hada'), (SELECT Id FROM Arcs WHERE Name='Bosque de las Hadas'), (SELECT Id FROM HairColors WHERE Name='Verde'), (SELECT Id FROM Affiliations WHERE Name='Caballeros Sagrados'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Zeldris', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Zeldris_kbia4j.png', 'ZELDRIS', 160, 'Ominous Nebula', 'Capitulo 173', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Negro'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo,Defensivo')),

('Estarossa', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Estarossa_guxtqe.png', 'ESTAROSSA', 198, 'Full Counter', 'Capitulo 173', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Negro'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Galand', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Galand_oxw5fk.png', 'GALAND', 540, 'Critical Over', 'Capitulo 150', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Gris'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Melascula', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Melascula_glhzqt.png', 'MELASCULA', 165, 'Faith', 'Capitulo 150', (SELECT Id FROM Genders WHERE Name='Femenino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Rubio'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Apoyo,Ofensivo')),

('Derieri', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Derieri_vkku9r.png', 'DERIERI', 173, 'Combo Star', 'Capitulo 150', (SELECT Id FROM Genders WHERE Name='Femenino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Naranja'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Monspiet', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Monspiet_cl3hxf.png', 'MONSPIET', 189, 'Trick Star', 'Capitulo 150', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Demonio'), (SELECT Id FROM Arcs WHERE Name='Diez Mandamientos'), (SELECT Id FROM HairColors WHERE Name='Negro'), (SELECT Id FROM Affiliations WHERE Name='Diez Mandamientos'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo')),

('Ludociel', 'https://res.cloudinary.com/dsidu0tej/image/upload/v1775067952/Ludociel_idbe9b.png', 'LUDOCIEL', 171, 'Flash', 'Capitulo 200', (SELECT Id FROM Genders WHERE Name='Masculino'), (SELECT Id FROM Races WHERE Name='Diosa'), (SELECT Id FROM Arcs WHERE Name='Guerra Santa'), (SELECT Id FROM HairColors WHERE Name='Rubio'), (SELECT Id FROM Affiliations WHERE Name='Cuatro Arcangeles'), (SELECT Id FROM TypesOfSkills WHERE Name='Ofensivo'));
GO

USE NanatsuDleDB;
GO

UPDATE Characters SET Image2Url = 'https://res.cloudinary.com/dsidu0tej/image/upload/v1774761968/Elizabeth_flupl3.png' WHERE Name = 'Elizabeth';
GO