-- ===========================
-- ELIMINAR LAS TABLAS SI EXISTEN (DROP)
-- ===========================
DROP TABLE IF EXISTS PAGO;
DROP TABLE IF EXISTS BOLETO;
DROP TABLE IF EXISTS HORARIO_PELICULA;
DROP TABLE IF EXISTS ASIENTO;
DROP TABLE IF EXISTS VERSION;
DROP TABLE IF EXISTS USUARIO;
DROP TABLE IF EXISTS REPARTO;
DROP TABLE IF EXISTS PELICULA_GENERO;
DROP TABLE IF EXISTS PELICULA_COMPANIA;
DROP TABLE IF EXISTS PELICULA_DIRECTOR;
DROP TABLE IF EXISTS PELICULA;
DROP TABLE IF EXISTS SALA_TIPO;
DROP TABLE IF EXISTS TIPOS_SALA;
DROP TABLE IF EXISTS SALA;
DROP TABLE IF EXISTS CINE;
DROP TABLE IF EXISTS DIRECTOR;
DROP TABLE IF EXISTS ACTOR;
DROP TABLE IF EXISTS GENERO;
DROP TABLE IF EXISTS COMPANIA;

-- ===========================
-- CREACIÓN DE TABLAS
-- ===========================

-- Tabla de Cines
CREATE TABLE CINE (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    UBICACION NVARCHAR(255) NOT NULL
);

-- Tabla de Salas
CREATE TABLE SALA (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    CINE_ID INT NOT NULL,
    CONSTRAINT FK_SALA_CINE FOREIGN KEY (CINE_ID) REFERENCES CINE(ID)
);

-- Tabla de Tipos de Sala
CREATE TABLE TIPOS_SALA (
    ID_TIPO_SALA INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE NVARCHAR(50) NOT NULL UNIQUE
);

-- Tabla intermedia para la relación muchos a muchos entre SALA y TIPOS_SALA
CREATE TABLE SALA_TIPO (
    SALA_ID INT NOT NULL,
    ID_TIPO_SALA INT NOT NULL,
    PRIMARY KEY (SALA_ID, ID_TIPO_SALA),
    CONSTRAINT FK_SALA_TIPO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(ID),
    CONSTRAINT FK_SALA_TIPO_TIPO FOREIGN KEY (ID_TIPO_SALA) REFERENCES TIPOS_SALA(ID_TIPO_SALA)
);

-- Tabla de Asientos
CREATE TABLE ASIENTO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    SALA_ID INT NOT NULL,
    NUMERO_ASIENTO NVARCHAR(10) NOT NULL,
    NUMERO_FILA NVARCHAR(5) NOT NULL,
    DISPONIBLE BIT DEFAULT 1,
    CONSTRAINT FK_ASIENTO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(ID)
);

-- Tabla de Actores
CREATE TABLE ACTOR (
    ID_ACTOR INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX) NULL,
    IMG_PERFIL NVARCHAR(255) NULL,
    FECHA_NACIMIENTO DATETIME NULL
);

-- Tabla de Directores
CREATE TABLE DIRECTOR (
    ID_DIRECTOR INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX),
    IMG_PERFIL NVARCHAR(255),
    FECHA_NACIMIENTO DATETIME
);

-- Tabla de Géneros
CREATE TABLE GENERO (
    ID_GENERO INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL
);

-- Tabla de Compañías
CREATE TABLE COMPANIA (
    ID_COMPANIA INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    SEDE NVARCHAR(255) NULL,
    WEB NVARCHAR(200) NULL,
    IMG_LOGO NVARCHAR(MAX) NULL,
    PAIS_ORIGEN NVARCHAR(10)
);

-- Tabla de Películas
CREATE TABLE PELICULA (
    ID_PELICULA INT PRIMARY KEY,
    TITULO NVARCHAR(255) NOT NULL,
    ANO_LANZAMIENTO INT,
    DURACION_MINUTOS INT,
    TITULO_ETIQUETA NVARCHAR(255),
    SINOPSIS NVARCHAR(MAX),
    IMG_FONDO NVARCHAR(255),
    IMG_POSTER NVARCHAR(255)
);

CREATE TABLE REPARTO (
    ID_REPARTO INT IDENTITY(1,1) PRIMARY KEY,
    ID_PELICULA INT NOT NULL,
    ID_PERSONA INT NOT NULL,
	NOMBRE NVARCHAR(255) NULL,
    
    PERSONAJE NVARCHAR(255) NULL,
	IMG_PERFIL NVARCHAR(255) NULL,
    CONSTRAINT FK_REPARTO_PELICULA FOREIGN KEY (ID_PELICULA) REFERENCES PELICULA(ID_PELICULA),
    CONSTRAINT FK_REPARTO_PERSONA_ACTOR FOREIGN KEY (ID_PERSONA) REFERENCES ACTOR(ID_ACTOR),
    CONSTRAINT FK_REPARTO_PERSONA_DIRECTOR FOREIGN KEY (ID_PERSONA) REFERENCES DIRECTOR(ID_DIRECTOR),
);

-- Tabla intermedia Películas - Géneros
CREATE TABLE PELICULA_GENERO (
    ID_PELICULA INT NOT NULL,
    ID_GENERO INT NOT NULL,
    PRIMARY KEY (ID_PELICULA, ID_GENERO),
    CONSTRAINT FK_PELICULA_GENERO_PELICULA FOREIGN KEY (ID_PELICULA) REFERENCES PELICULA(ID_PELICULA),
    CONSTRAINT FK_PELICULA_GENERO_GENERO FOREIGN KEY (ID_GENERO) REFERENCES GENERO(ID_GENERO)
);

-- Tabla intermedia Películas - Compañías
CREATE TABLE PELICULA_COMPANIA (
    ID_PELICULA INT NOT NULL,
    ID_COMPANIA INT NOT NULL,
    PRIMARY KEY (ID_PELICULA, ID_COMPANIA),
    CONSTRAINT FK_PELICULA_COMPANIA_PELICULA FOREIGN KEY (ID_PELICULA) 
    REFERENCES PELICULA(ID_PELICULA),
    CONSTRAINT FK_PELICULA_COMPANIA_COMPANIA FOREIGN KEY (ID_COMPANIA) 
    REFERENCES COMPANIA(ID_COMPANIA)
);
-- Tabla intermedia Películas - Directores
CREATE TABLE PELICULA_DIRECTOR (
    ID_PELICULA INT NOT NULL,
    ID_DIRECTOR INT NOT NULL,
    PRIMARY KEY (ID_PELICULA, ID_DIRECTOR),
    CONSTRAINT FK_PELICULA_DIRECTOR_PELICULA FOREIGN KEY (ID_PELICULA) REFERENCES PELICULA(ID_PELICULA),
    CONSTRAINT FK_PELICULA_DIRECTOR_DIRECTOR FOREIGN KEY (ID_DIRECTOR) REFERENCES DIRECTOR(ID_DIRECTOR)
);
-- Tabla de Usuarios
CREATE TABLE USUARIO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    CORREO_ELECTRONICO NVARCHAR(100) UNIQUE NOT NULL,
    CONTRASENIA_HASH NVARCHAR(255) NOT NULL,
    FECHA_CREACION DATETIME DEFAULT GETDATE()
);

-- Tabla de Versiones
CREATE TABLE VERSION (
    ID_VERSION INT IDENTITY(1,1) PRIMARY KEY,
    IDIOMA NVARCHAR(50) NOT NULL,
    TIPO_VERSION NVARCHAR(50) NOT NULL
);

-- Tabla de Horarios de Películas
CREATE TABLE HORARIO_PELICULA (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PELICULA_ID INT NOT NULL,
    SALA_ID INT NOT NULL,
    ID_VERSION INT NOT NULL,
    HORA_FUNCION DATETIME NOT NULL,
    ASIENTOS_DISPONIBLES INT NOT NULL,
    CONSTRAINT FK_HORARIO_PELICULA_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(ID_PELICULA),
    CONSTRAINT FK_HORARIO_PELICULA_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(ID),
    CONSTRAINT FK_HORARIO_PELICULA_VERSION FOREIGN KEY (ID_VERSION) REFERENCES VERSION(ID_VERSION)
);

-- Tabla de Boletos
CREATE TABLE BOLETO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    USUARIO_ID INT NOT NULL,
    HORARIO_ID INT NOT NULL,
    ASIENTO_ID INT NOT NULL,
    FECHA_COMPRA DATETIME DEFAULT GETDATE(),
    ESTADO NVARCHAR(20) DEFAULT 'reservado',
    CONSTRAINT FK_BOLETO_ASIENTO FOREIGN KEY (ASIENTO_ID) REFERENCES ASIENTO(ID)
);

-- Tabla de Pagos
CREATE TABLE PAGO (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    BOLETO_ID INT NOT NULL,
    MONTO DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_PAGO_BOLETO FOREIGN KEY (BOLETO_ID) REFERENCES BOLETO(ID)
);


-- ====================================
-- Insertar Asientos (120 por sala)
-- ====================================
DECLARE @SALA_ID INT;
DECLARE @FILA CHAR(1);
DECLARE @NUMERO INT;

DECLARE CURSOR_SALAS CURSOR FOR 
SELECT ID FROM SALA;

OPEN CURSOR_SALAS;
FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Insertar 120 asientos por sala (10 filas, 12 asientos por fila)
    DECLARE @FILA_INDEX INT = 1;
    WHILE @FILA_INDEX <= 10
    BEGIN
        SET @FILA = CHAR(64 + @FILA_INDEX); -- Convierte 1 -> 'A', 2 -> 'B', ..., 10 -> 'J'
        SET @NUMERO = 1;
        
        WHILE @NUMERO <= 12
        BEGIN
            INSERT INTO ASIENTO (SALA_ID, NUMERO_ASIENTO, NUMERO_FILA, DISPONIBLE) 
            VALUES (@SALA_ID, CAST(@NUMERO AS NVARCHAR), @FILA, 1);
            SET @NUMERO = @NUMERO + 1;
        END
        
        SET @FILA_INDEX = @FILA_INDEX + 1;
    END
    
    FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;
END

CLOSE CURSOR_SALAS;
DEALLOCATE CURSOR_SALAS;

-- ====================================
-- Insertar generos (70 por sala)
-- ====================================

INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (28, 'Action');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (12, 'Adventure');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (16, 'Animation');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (35, 'Comedy');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (80, 'Crime');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (99, 'Documentary');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (18, 'Drama');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (10751, 'Family');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (14, 'Fantasy');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (36, 'History');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (27, 'Horror');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (10402, 'Music');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (9648, 'Mystery');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (10749, 'Romance');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (878, 'Science Fiction');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (10770, 'TV Movie');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (53, 'Thriller');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (10752, 'War');
INSERT INTO GENERO (ID_GENERO, NOMBRE) VALUES (37, 'Western');

-- ====================================
-- Insertar peliculas
-- ====================================
INSERT INTO PELICULA (ID_PELICULA, TITULO, ANO_LANZAMIENTO, DURACION_MINUTOS, TITULO_ETIQUETA, SINOPSIS, IMG_FONDO, IMG_POSTER)
VALUES
(762509, 'Mufasa: El rey león',  2024-12-18, 118, NULL, 'Rafiki debe transmitir la leyenda de Mufasa a la joven cachorro de león Kiara, hija de Simba y Nala, y con Timón y Pumba prestando su estilo característico. Mufasa, un cachorro huérfano, perdido y solo, conoce a un simpático león llamado Taka, heredero de un linaje real. Este encuentro casual pone en marcha un viaje de un extraordinario grupo de inadaptados que buscan su destino.', 'cVh8Af7a9JMOJl75ML3Dg2QVEuq.jpg', '/dmw74cWIEKaEgl5Dv3kUTcCob6D.jpg');

-- ====================================
-- Insertar companias
-- ====================================

INSERT INTO COMPANIA (ID_COMPANIA, NOMBRE, SEDE, WEB, IMG_LOGO, PAIS_ORIGEN) 
VALUES 
(694, 'StudioCanal', 'Paris', 'http://www.studiocanal.fr', '/5LEHONGkZBIoWvp1ygHOF8iyi1M.png', 'FR' ),
(238886, 'Marmalade Pictures', 'London, England', NULL, NULL, 'GB'),
(73025, 'Kinoshita Group', NULL, 'https://www.kinoshita-group.co.jp', '/zYb2OfWjKvNlngpq4LemY9JVC51.png', 'JP'),
(11341, 'Stage 6 Films', 'Culver City, California', 'https://www.stage6films.com', '/xytTBODEy3p20ksHL4Ftxr483Iv.png', 'US');


-- ====================================
-- Insertar actores
-- ====================================
INSERT INTO ACTOR (BIOGRAFIA ,FECHA_NACIMIENTO,ID_ACTOR, NOMBRE,IMG_PERFIL)
VALUES
('',1980-10-14,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg'),
('',1963-11-10,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg'),
('',1971-12-01,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg'),
('',2002-01-18,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg'),
('',2001-04-28,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg'),
('Actor y director español que ha alcanzado gran fama dentro del panorama cinematográfico internacional. Nació en Málaga e inició su carrera como actor en el seno una compañía de teatro independiente. Tras fijar su residencia en Madrid, comenzó a trabajar en el cine como actor secundario. En 1982 rodó Laberinto de pasiones, su primera película con Pedro Almodóvar, director gracias al que alcanzó gran popularidad a partir de su intervención en filmes como La ley del deseo (1986), Mujeres al borde de un ataque de nervios (1987) y Átame (1989). También colaboró en producciones de otros realizadores españoles, como Los zancos (1984), de Carlos Saura, Bajarse al moro (1988), de Fernando Colomo, y Si te dicen que caí (1989), de Vicente Aranda. Con Los reyes del mambo tocan canciones de amor (1992, de Arne Glimcher), basada en la novela del mismo título de Oscar Hijuelos, Banderas comenzó a actuar en producciones extranjeras. Después de divorciarse de su primera mujer, Ana Leza, y de fijar su residencia en Estados Unidos, contrajo matrimonio con la popular actriz estadounidense Melanie Griffith. Entre su filmografía destacan algunos títulos muy conocidos que lo han lanzado al estrellato, como Philadelphia (1993, de Jonathan Demme); La casa de los espíritus (1993, de Bille August), basado en la novela de la escritora chilena Isabel Allende; Entrevista con el vampiro (1994, de Neil Jordan); Two much (1995, de Fernando Trueba); Evita (1996, de Alan Parker), con Madonna; La máscara del Zorro (1998, de Martin Campbell), en la que da vida al célebre mito literario y del cine de acción, y Frida (2002, de Julie Taymor), donde interpreta al artista mexicano David Alfaro Siqueiros. También ha prestado su voz al personaje del gato con botas en la popular película de dibujos animados Shrek 2 (2004). En 1999 estrenó su primera película como director, Locos en Albama, protagonizada por su mujer. Tras sufrir un ataque al corazón el 26 de enero de 2017 el actor asegura estar ya bien y vaticina que lo mejor de su carrera “está por venír”.',1960-08-10,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg'),
('',1974-01-30,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg'),
('',1950-02-22,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg'),
('Nació en Lincoln, hijo de Doreen "Dee" Findlay, una escultora, y Roy Laverick Broadbent, un artista, escultor y diseñador de interiores.1?2? Los padres de Broadbent fueron actores aficionados y fundaron el grupo teatral Holton Players.3? Tuvo una hermana melliza que murió al nacer. Broadbent estudió en la Leighton Park School, una escuela cuáquera ubicada en Reading.4? Posteriormente estudió en la London Academy of Music and Dramatic Art.',1949-05-24,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg'),
('Carla Tous (España, 2001) es una actriz española .',2001-01-01,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg'),
('',1976-01-19,65451,'Oliver Maltman','/wZgJ4BhseHlGOakiSMLCDUE07ic.jpg'),
('',1984-05-20,54811,'Joel Fry','/4nEKEWJpaTHncCTv6zeP98V0qGI.jpg'),
('',1970-03-24,1119,'Robbie Gee','/mzzfbA0e4DWgxvIYjaryMolkiqm.jpg'),
('',1963-10-31,145997,'Sanjeev Bhaskar','/gPFoGWj5VPm4IZVn9sMdBicVh2E.jpg'),
('',1956-01-09,11356,'Imelda Staunton','/tABRYOHUQeaUCAqrelJV5ZHjl1W.jpg'),
('',1966-02-24,18025,'Ben Miller','/ij5nhtBWhCaOl4IEn8XhOYAGqB1.jpg'),
('',1972-10-30,47730,'Jessica Hynes','/eyJ9aC1FXyPAI5xose0HqA26bnm.jpg'),
('',NULL,3037851,'Ella Dacres','/x9yadKWs3mWLXEZ6gcgvgCzhbr1.jpg'),
('',NULL,2329015,'Aloreia Spencer',NULL),
('',1977-01-01,110076,'Nicholas Burns','/x8ETzUUeTrmBRuVdvPEaZ23Focf.jpg'),
('',1995-08-12,2825348,'Ashleigh Reynolds','/jqZs6a76NM6Yks7fGy5azmaYbZD.jpg'),
('',1981-04-26,150072,'Amit Shah','/s6zx8CumxTGq0kULxp6H6nc1DDA.jpg'),
('',NULL,3250136,'Ella Bruccoleri','/xVqbUOELKqxhEdqAz5IXhFYp7Uy.jpg'),
('',1971-04-15,1456143,'Carlos Carlín','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg'),
('',1973-04-02,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg'),
('',1991-02-05,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg'),
('',1960-09-09,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg');

INSERT INTO REPARTO (ID_PELICULA, ID_PERSONA,NOMBRE ,IMG_PERFIL,PERSONAJE)
VALUES
(516729,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg','Paddington Brown (voice)'),
(516729,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg','Henry Brown'),
(516729,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg','Mary Brown'),
(516729,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg','Jonathan Brown'),
(516729,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg','Judy Brown'),
(516729,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg','Hunter Cabot'),
(516729,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg','Reverend Mother'),
(516729,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg','Mrs. Bird'),
(516729,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg','Samuel Gruber'),
(516729,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg','Gina Cabot'),
(516729,39459,'Hayley Atwell','/iQyJYuVWOA6tUxQEHt6dSuz1PoE.jpg','Madison'),
(516729,65451,'Oliver Maltman','/wZgJ4BhseHlGOakiSMLCDUE07ic.jpg','Photo Booth (voice)'),
(516729,54811,'Joel Fry','/4nEKEWJpaTHncCTv6zeP98V0qGI.jpg','Joe the Postman'),
(516729,1119,'Robbie Gee','/mzzfbA0e4DWgxvIYjaryMolkiqm.jpg','Mr. Barnes'),
(516729,145997,'Sanjeev Bhaskar','/gPFoGWj5VPm4IZVn9sMdBicVh2E.jpg','Dr. Jafri'),
(516729,11356,'Imelda Staunton','/tABRYOHUQeaUCAqrelJV5ZHjl1W.jpg','Aunt Lucy (voice)'),
(516729,18025,'Ben Miller','/ij5nhtBWhCaOl4IEn8XhOYAGqB1.jpg','Colonel Lancaster'),
(516729,47730,'Jessica Hynes','/eyJ9aC1FXyPAI5xose0HqA26bnm.jpg','Miss Kitts'),
(516729,3037851,'Ella Dacres','/x9yadKWs3mWLXEZ6gcgvgCzhbr1.jpg','Student Rep'),
(516729,2329015,'Aloreia Spencer',NULL,'Madison’s Assistant'),
(516729,110076,'Nicholas Burns','/x8ETzUUeTrmBRuVdvPEaZ23Focf.jpg','Mr. Wilson'),
(516729,2825348,'Ashleigh Reynolds','/jqZs6a76NM6Yks7fGy5azmaYbZD.jpg','Mr. Smith'),
(516729,150072,'Amit Shah','/s6zx8CumxTGq0kULxp6H6nc1DDA.jpg','Zayden'),
(516729,3250136,'Ella Bruccoleri','/xVqbUOELKqxhEdqAz5IXhFYp7Uy.jpg','Rosita'),
(516729,1456143,'Carlos Carlín','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg','Passport Control Officer'),
(516729,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg','Barry the Air Steward'),
(516729,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg','Elderly Resident Bear 3 (voice)'),
(516729,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg','Phoenix Buchanan');