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
DROP TABLE IF EXISTS TIPO_SALA;
DROP TABLE IF EXISTS SALA;
DROP TABLE IF EXISTS CINE;
DROP TABLE IF EXISTS DIRECTOR;
DROP TABLE IF EXISTS ACTOR;
DROP TABLE IF EXISTS GENERO;
DROP TABLE IF EXISTS COMPANIA;

-- ===========================
-- CREACI�N DE TABLAS
-- ===========================

-- Tabla de Cines
CREATE TABLE CINE (
    CINE_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    UBICACION NVARCHAR(255) NOT NULL
);

-- Tabla de Salas
CREATE TABLE SALA (
    SALA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    CINE_ID INT NOT NULL,
    CONSTRAINT FK_SALA_CINE FOREIGN KEY (CINE_ID) REFERENCES CINE(CINE_ID)
);

-- Tabla de Tipos de Sala
CREATE TABLE TIPO_SALA (
    TIPO_SALA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(50) NOT NULL UNIQUE
);

-- Tabla intermedia para la relaci�n muchos a muchos entre SALA y TIPO_SALA
CREATE TABLE SALA_TIPO (
    SALA_ID INT NOT NULL,
    TIPO_SALA_ID INT NOT NULL,
    PRIMARY KEY (SALA_ID, TIPO_SALA_ID),
    CONSTRAINT FK_SALA_TIPO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID),
    CONSTRAINT FK_SALA_TIPO_TIPO FOREIGN KEY (TIPO_SALA_ID) REFERENCES TIPO_SALA(TIPO_SALA_ID)
);

-- Tabla de Asientos
CREATE TABLE ASIENTO (
    ASIENTO_ID INT PRIMARY KEY,
    SALA_ID INT NOT NULL,
    NUMERO NVARCHAR(10) NOT NULL,
    FILA NVARCHAR(5) NOT NULL,
    DISPONIBLE BIT DEFAULT 1,
    CONSTRAINT FK_ASIENTO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID)
);

-- Tabla de Actores
CREATE TABLE ACTOR (
    ACTOR_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX) NULL,
    IMG_PERFIL NVARCHAR(255) NULL,
    FECHA_NACIMIENTO DATETIME NULL
);

-- Tabla de Directores
CREATE TABLE DIRECTOR (
    DIRECTOR_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    BIOGRAFIA NVARCHAR(MAX),
    IMG_PERFIL NVARCHAR(255),
    FECHA_NACIMIENTO DATETIME
);

-- Tabla de G�neros
CREATE TABLE GENERO (
    GENERO_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL
);

-- Tabla de Compa��as
CREATE TABLE COMPANIA (
    COMPANIA_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    SEDE NVARCHAR(255) NULL,
    WEB NVARCHAR(200) NULL,
    IMG_LOGO NVARCHAR(MAX) NULL,
    PAIS_ORIGEN NVARCHAR(10)
);

-- Tabla de Pel�culas
CREATE TABLE PELICULA (
    PELICULA_ID INT PRIMARY KEY,
    TITULO NVARCHAR(255) NOT NULL,
    FECHA_LANZAMIENTO DATE,
    DURACION_MINUTOS INT,
    TITULO_ETIQUETA NVARCHAR(255),
    SINOPSIS NVARCHAR(MAX),
    IMG_FONDO NVARCHAR(255),
    IMG_POSTER NVARCHAR(255)
);

-- Tabla de Reparto (solo actores)
CREATE TABLE REPARTO (
    REPARTO_ID INT PRIMARY KEY,
    PELICULA_ID INT NOT NULL,
    ACTOR_ID INT NOT NULL,
    NOMBRE NVARCHAR(255) NULL,
    PERSONAJE NVARCHAR(255) NULL,
    IMG_PERFIL NVARCHAR(255) NULL,
    CONSTRAINT FK_REPARTO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_REPARTO_ACTOR FOREIGN KEY (ACTOR_ID) REFERENCES ACTOR(ACTOR_ID)
);

-- Tabla intermedia Pel�culas - G�neros
CREATE TABLE PELICULA_GENERO (
    PELICULA_ID INT NOT NULL,
    GENERO_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, GENERO_ID),
    CONSTRAINT FK_PELICULA_GENERO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_GENERO_GENERO FOREIGN KEY (GENERO_ID) REFERENCES GENERO(GENERO_ID)
);

-- Tabla intermedia Pel�culas - Compa��as
CREATE TABLE PELICULA_COMPANIA (
    PELICULA_ID INT NOT NULL,
    COMPANIA_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, COMPANIA_ID),
    CONSTRAINT FK_PELICULA_COMPANIA_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_COMPANIA_COMPANIA FOREIGN KEY (COMPANIA_ID) REFERENCES COMPANIA(COMPANIA_ID)
);

-- Tabla intermedia Pel�culas - Directores
CREATE TABLE PELICULA_DIRECTOR (
    PELICULA_ID INT NOT NULL,
    DIRECTOR_ID INT NOT NULL,
    PRIMARY KEY (PELICULA_ID, DIRECTOR_ID),
    CONSTRAINT FK_PELICULA_DIRECTOR_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_PELICULA_DIRECTOR_DIRECTOR FOREIGN KEY (DIRECTOR_ID) REFERENCES DIRECTOR(DIRECTOR_ID)
);

-- Tabla de Usuarios
CREATE TABLE USUARIO (
    USUARIO_ID INT PRIMARY KEY,
    NOMBRE NVARCHAR(100) NOT NULL,
    CORREO NVARCHAR(100) UNIQUE NOT NULL,
    CONTRASENA_HASH NVARCHAR(255) NOT NULL,
    FECHA_CREACION DATETIME DEFAULT GETDATE()
);

-- Tabla de Versiones
CREATE TABLE VERSION (
    VERSION_ID INT PRIMARY KEY,
    IDIOMA NVARCHAR(50) NOT NULL,
    FORMATO NVARCHAR(50) NOT NULL
);

-- Tabla de Horarios de Pel�culas
CREATE TABLE HORARIO_PELICULA (
    HORARIO_ID INT PRIMARY KEY,
    PELICULA_ID INT NOT NULL,
    SALA_ID INT NOT NULL,
    VERSION_ID INT NOT NULL,
    HORA_FUNCION DATETIME NOT NULL,
    ASIENTOS_DISPONIBLES INT NOT NULL,
    CONSTRAINT FK_HORARIO_PELICULA FOREIGN KEY (PELICULA_ID) REFERENCES PELICULA(PELICULA_ID),
    CONSTRAINT FK_HORARIO_SALA FOREIGN KEY (SALA_ID) REFERENCES SALA(SALA_ID),
    CONSTRAINT FK_HORARIO_VERSION FOREIGN KEY (VERSION_ID) REFERENCES VERSION(VERSION_ID)
);

-- Tabla de Boletos
CREATE TABLE BOLETO (
    BOLETO_ID INT PRIMARY KEY,
    USUARIO_ID INT NOT NULL,
    HORARIO_ID INT NOT NULL,
    ASIENTO_ID INT NOT NULL,
    FECHA_COMPRA DATETIME DEFAULT GETDATE(),
    ESTADO NVARCHAR(20) DEFAULT 'reservado',
    CONSTRAINT FK_BOLETO_ASIENTO FOREIGN KEY (ASIENTO_ID) REFERENCES ASIENTO(ASIENTO_ID)
);

-- Tabla de Pagos
CREATE TABLE PAGO (
    PAGO_ID INT PRIMARY KEY,
    BOLETO_ID INT NOT NULL,
    MONTO DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_PAGO_BOLETO FOREIGN KEY (BOLETO_ID) REFERENCES BOLETO(BOLETO_ID)
);

-- Insertar Cine
INSERT INTO CINE (CINE_ID, NOMBRE, UBICACION)
VALUES (1, 'Cine Central', 'Av. Principal 123');

-- Insertar 6 salas manualmente
INSERT INTO SALA (SALA_ID, NOMBRE, CINE_ID)
VALUES 
    (1, 'Sala 1', 1),
    (2, 'Sala 2', 1),
    (3, 'Sala 3', 1),
    (4, 'Sala 4', 1),
    (5, 'Sala 5', 1),
    (6, 'Sala 6', 1);

-- Insertar los 120 asientos por cada sala

DECLARE @SALA_ID INT;
DECLARE @FILA CHAR(1);
DECLARE @NUMERO INT;
DECLARE @ASIENTO_ID INT = 1; -- Control manual del ID de los asientos

DECLARE CURSOR_SALAS CURSOR FOR 
SELECT SALA_ID FROM SALA;

OPEN CURSOR_SALAS;
FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Recorrer filas (A - J)
    DECLARE @FILA_INDEX INT = 1;
    WHILE @FILA_INDEX <= 10
    BEGIN
        SET @FILA = CHAR(64 + @FILA_INDEX); -- Convierte 1 -> 'A', 2 -> 'B', ..., 10 -> 'J'
        SET @NUMERO = 1;
        
        -- Recorrer asientos (1 - 12)
        WHILE @NUMERO <= 12
        BEGIN
            INSERT INTO ASIENTO (ASIENTO_ID, SALA_ID, NUMERO, FILA, DISPONIBLE) 
            VALUES (@ASIENTO_ID, @SALA_ID, CAST(@NUMERO AS NVARCHAR(10)), @FILA, 1);
            
            SET @NUMERO = @NUMERO + 1;
            SET @ASIENTO_ID = @ASIENTO_ID + 1; -- Incrementar manualmente el ID
        END
        
        SET @FILA_INDEX = @FILA_INDEX + 1;
    END
    
    FETCH NEXT FROM CURSOR_SALAS INTO @SALA_ID;
END

CLOSE CURSOR_SALAS;
DEALLOCATE CURSOR_SALAS;

-- ====================================
-- Insertar Versiones
-- ====================================

INSERT INTO VERSION (VERSION_ID, IDIOMA, FORMATO)
VALUES (1, 'Espa�ol', '2D'),
       (2, 'Ingl�s', '3D');

-- ====================================
-- Insertar peliculas
-- ====================================
SET DATEFORMAT dmy;
INSERT INTO PELICULA (PELICULA_ID, SINOPSIS, FECHA_LANZAMIENTO, DURACION_MINUTOS, TITULO_ETIQUETA, TITULO, IMG_FONDO, IMG_POSTER)
VALUES
(950396, 'Dos agentes de �lite son secretamente asignados a torres de vigilancia en los lados opuestos de un vasto desfiladero, para proteger al mundo de un misterioso mal que acecha en su interior. Se unen en la distancia, pero han de mantenerse alerta para defenderse del enemigo invisible. Cuando se les revela una amenaza fatal para la humanidad, deben trabajar juntos y poner a prueba su fuerza f�sica y mental para mantener el secreto del desfiladero antes de que sea demasiado tarde.', '2025-02-13',127, 'Nadie entra. Nada sale.', 'El abismo secreto', '', ''),
(762509,'Rafiki debe transmitir la leyenda de Mufasa a la joven cachorro de le�n Kiara, hija de Simba y Nala, y con Tim�n y Pumba prestando su estilo caracter�stico. Mufasa, un cachorro hu�rfano, perdido y solo, conoce a un simp�tico le�n llamado Taka, heredero de un linaje real. Este encuentro casual pone en marcha un viaje de un extraordinario grupo de inadaptados que buscan su destino.','2024-12-18', 118,'','Mufasa: El rey le�n','',''),
(1126166,'En este claustrof�bico thriller, un piloto (Mark Wahlberg) transporta en su avioneta a una teniente general (Michelle Dockery) que custodia a un testigo (Topher Grace) que va a testificar en un juicio contra la mafia. A medida que atraviesan las monta�as de Alaska, las tensiones se disparan, ya que no todo el mundo a bordo es quien parece ser. Y a 3.000 metros de altura no hay escapatoria posible.','2025-01-22',91,'','Amenaza en el aire','',''),
(939243,'Sonic, Knuckles y Tails se re�nen para enfrentarse a un nuevo y poderoso adversario, Shadow, un misterioso villano cuyos poderes no se parecen a nada de lo que nuestros h�roes han conocido hasta ahora. Con sus facultades superadas en todos los sentidos, el Equipo Sonic tendr� que establecer una ins�lita alianza con la esperanza de detener a Shadow y proteger el planeta.','2024-12-19',110,'','Sonic 3: La pel�cula','',''),
(1241982,'Tras recibir una inesperada llamada de sus antepasados, Vaiana debe viajar a los lejanos mares de Ocean�a y adentrarse en peligrosas aguas perdidas para vivir una aventura sin precedentes.','2024-11-21',99,'','Vaiana 2','','');

-- ====================================
-- Insertar companias
-- ====================================

INSERT INTO COMPANIA (COMPANIA_ID, NOMBRE, SEDE, WEB, IMG_LOGO, PAIS_ORIGEN) 
VALUES 
(694, 'StudioCanal', 'Paris', 'http://www.studiocanal.fr', '/5LEHONGkZBIoWvp1ygHOF8iyi1M.png', 'FR' ),
(238886, 'Marmalade Pictures', 'London, England', NULL, NULL, 'GB'),
(73025, 'Kinoshita Group', NULL, 'https://www.kinoshita-group.co.jp', '/zYb2OfWjKvNlngpq4LemY9JVC51.png', 'JP'),
(11341, 'Stage 6 Films', 'Culver City, California', 'https://www.stage6films.com', '/xytTBODEy3p20ksHL4Ftxr483Iv.png', 'US');


-- ====================================
-- Insertar actores
-- ====================================
INSERT INTO ACTOR (BIOGRAFIA ,FECHA_NACIMIENTO,ACTOR_ID, NOMBRE,IMG_PERFIL)
VALUES
('',1980-10-14,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg'),
('',1963-11-10,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg'),
('',1971-12-01,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg'),
('',2002-01-18,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg'),
('',2001-04-28,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg'),
('Actor y director espa�ol que ha alcanzado gran fama dentro del panorama cinematogr�fico internacional. Naci� en M�laga e inici� su carrera como actor en el seno una compa��a de teatro independiente. Tras fijar su residencia en Madrid, comenz� a trabajar en el cine como actor secundario. En 1982 rod� Laberinto de pasiones, su primera pel�cula con Pedro Almod�var, director gracias al que alcanz� gran popularidad a partir de su intervenci�n en filmes como La ley del deseo (1986), Mujeres al borde de un ataque de nervios (1987) y �tame (1989). Tambi�n colabor� en producciones de otros realizadores espa�oles, como Los zancos (1984), de Carlos Saura, Bajarse al moro (1988), de Fernando Colomo, y Si te dicen que ca� (1989), de Vicente Aranda. Con Los reyes del mambo tocan canciones de amor (1992, de Arne Glimcher), basada en la novela del mismo t�tulo de Oscar Hijuelos, Banderas comenz� a actuar en producciones extranjeras. Despu�s de divorciarse de su primera mujer, Ana Leza, y de fijar su residencia en Estados Unidos, contrajo matrimonio con la popular actriz estadounidense Melanie Griffith. Entre su filmograf�a destacan algunos t�tulos muy conocidos que lo han lanzado al estrellato, como Philadelphia (1993, de Jonathan Demme); La casa de los esp�ritus (1993, de Bille August), basado en la novela de la escritora chilena Isabel Allende; Entrevista con el vampiro (1994, de Neil Jordan); Two much (1995, de Fernando Trueba); Evita (1996, de Alan Parker), con Madonna; La m�scara del Zorro (1998, de Martin Campbell), en la que da vida al c�lebre mito literario y del cine de acci�n, y Frida (2002, de Julie Taymor), donde interpreta al artista mexicano David Alfaro Siqueiros. Tambi�n ha prestado su voz al personaje del gato con botas en la popular pel�cula de dibujos animados Shrek 2 (2004). En 1999 estren� su primera pel�cula como director, Locos en Albama, protagonizada por su mujer. Tras sufrir un ataque al coraz�n el 26 de enero de 2017 el actor asegura estar ya bien y vaticina que lo mejor de su carrera �est� por ven�r�.',1960-08-10,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg'),
('',1974-01-30,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg'),
('',1950-02-22,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg'),
('Naci� en Lincoln, hijo de Doreen "Dee" Findlay, una escultora, y Roy Laverick Broadbent, un artista, escultor y dise�ador de interiores.1?2? Los padres de Broadbent fueron actores aficionados y fundaron el grupo teatral Holton Players.3? Tuvo una hermana melliza que muri� al nacer. Broadbent estudi� en la Leighton Park School, una escuela cu�quera ubicada en Reading.4? Posteriormente estudi� en la London Academy of Music and Dramatic Art.',1949-05-24,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg'),
('Carla Tous (Espa�a, 2001) es una actriz espa�ola .',2001-01-01,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg'),
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
('',1971-04-15,1456143,'Carlos Carl�n','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg'),
('',1973-04-02,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg'),
('',1991-02-05,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg'),
('',1960-09-09,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg');

--INSERT INTO REPARTO (ID_PELICULA, ID_PERSONA,NOMBRE ,IMG_PERFIL,PERSONAJE)
--VALUES
--(516729,17064,'Ben Whishaw','/2GBtQ6scGeSHkX1urOP1EJbmksx.jpg','Paddington Brown (voice)'),
--(516729,19923,'Hugh Bonneville','/skbxj8MUuNiI39ZkP38uEirU0bC.jpg','Henry Brown'),
--(516729,1246,'Emily Mortimer','/6tFo75Em20OKJqygfAlHuCl05bl.jpg','Mary Brown'),
--(516729,1304662,'Samuel Joslin','/gXthNRK0BN9IqXnhKIc4glxOB7P.jpg','Jonathan Brown'),
--(516729,1304661,'Madeleine Harris','/a0leGmuuQslxiuP7g52sMxET6rm.jpg','Judy Brown'),
--(516729,3131,'Antonio Banderas','/n8YlGookYzgD3cmpMP45BYRNIoh.jpg','Hunter Cabot'),
--(516729,39187,'Olivia Colman','/1KTXGJaqWRnsoA6qeaUa7U2zkHL.jpg','Reverend Mother'),
--(516729,477,'Julie Walters','/bCTkV2OUgzbJdQEoCk3GesE4DXq.jpg','Mrs. Bird'),
--(516729,388,'Jim Broadbent','/s7lXYfrsJoGA4vKmyv61SPgABmR.jpg','Samuel Gruber'),
--(516729,2610316,'Carla Tous','/8wriPsd59p97mH9vXCq4JW5DmYz.jpg','Gina Cabot'),
--(516729,39459,'Hayley Atwell','/iQyJYuVWOA6tUxQEHt6dSuz1PoE.jpg','Madison'),
--(516729,65451,'Oliver Maltman','/wZgJ4BhseHlGOakiSMLCDUE07ic.jpg','Photo Booth (voice)'),
--(516729,54811,'Joel Fry','/4nEKEWJpaTHncCTv6zeP98V0qGI.jpg','Joe the Postman'),
--(516729,1119,'Robbie Gee','/mzzfbA0e4DWgxvIYjaryMolkiqm.jpg','Mr. Barnes'),
--(516729,145997,'Sanjeev Bhaskar','/gPFoGWj5VPm4IZVn9sMdBicVh2E.jpg','Dr. Jafri'),
--(516729,11356,'Imelda Staunton','/tABRYOHUQeaUCAqrelJV5ZHjl1W.jpg','Aunt Lucy (voice)'),
--(516729,18025,'Ben Miller','/ij5nhtBWhCaOl4IEn8XhOYAGqB1.jpg','Colonel Lancaster'),
--(516729,47730,'Jessica Hynes','/eyJ9aC1FXyPAI5xose0HqA26bnm.jpg','Miss Kitts'),
--(516729,3037851,'Ella Dacres','/x9yadKWs3mWLXEZ6gcgvgCzhbr1.jpg','Student Rep'),
--(516729,2329015,'Aloreia Spencer',NULL,'Madison�s Assistant'),
--(516729,110076,'Nicholas Burns','/x8ETzUUeTrmBRuVdvPEaZ23Focf.jpg','Mr. Wilson'),
--(516729,2825348,'Ashleigh Reynolds','/jqZs6a76NM6Yks7fGy5azmaYbZD.jpg','Mr. Smith'),
--(516729,150072,'Amit Shah','/s6zx8CumxTGq0kULxp6H6nc1DDA.jpg','Zayden'),
--(516729,3250136,'Ella Bruccoleri','/xVqbUOELKqxhEdqAz5IXhFYp7Uy.jpg','Rosita'),
--(516729,1456143,'Carlos Carl�n','/uRPeNEIcDtyjBY00u0oDAW645PV.jpg','Passport Control Officer'),
--(516729,114253,'Simon Farnaby','/xslxzBfDYYHNFnAMWyjjxFUTlEk.jpg','Barry the Air Steward'),
--(516729,2540694,'Emma Sidi','/9eCqMZbLum1UBFiwSQzAGvcgr3Q.jpg','Elderly Resident Bear 3 (voice)'),
--(516729,3291,'Hugh Grant','/hsSfxSHzkKJ6ZKq1Ofngcp7aAnT.jpg','Phoenix Buchanan');