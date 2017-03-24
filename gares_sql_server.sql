/*------------------------------------------------------------
*        Script SQLSERVER 
------------------------------------------------------------*/


/*------------------------------------------------------------
-- Table: ligne
------------------------------------------------------------*/
CREATE TABLE ligne(
	numero_ligne INT IDENTITY (1,1) NOT NULL ,
	code_ligne   INT   ,
	latitude     VARCHAR (20) NOT NULL ,
	longitude    VARCHAR (20) NOT NULL ,
	CONSTRAINT prk_constraint_ligne PRIMARY KEY NONCLUSTERED (numero_ligne)
);


/*------------------------------------------------------------
-- Table: gare
------------------------------------------------------------*/
CREATE TABLE gare(
	id_gare      INT IDENTITY (1,1) NOT NULL ,
	nom_gare     VARCHAR (50)  ,
	numero_ville INT  NOT NULL ,
	CONSTRAINT prk_constraint_gare PRIMARY KEY NONCLUSTERED (id_gare)
);


/*------------------------------------------------------------
-- Table: nature
------------------------------------------------------------*/
CREATE TABLE nature(
	numero_nature INT IDENTITY (1,1) NOT NULL ,
	nom_nature    VARCHAR (50)  ,
	CONSTRAINT prk_constraint_nature PRIMARY KEY NONCLUSTERED (numero_nature)
);


/*------------------------------------------------------------
-- Table: ville
------------------------------------------------------------*/
CREATE TABLE ville(
	numero_ville INT IDENTITY (1,1) NOT NULL ,
	nom_ville    VARCHAR (80)  ,
	CONSTRAINT prk_constraint_ville PRIMARY KEY NONCLUSTERED (numero_ville)
);


/*------------------------------------------------------------
-- Table: cp
------------------------------------------------------------*/
CREATE TABLE cp(
	code_postal INT  NOT NULL ,
	CONSTRAINT prk_constraint_cp PRIMARY KEY NONCLUSTERED (code_postal)
);


/*------------------------------------------------------------
-- Table: peut contenir
------------------------------------------------------------*/
CREATE TABLE peut_contenir(
	numero_ligne INT  NOT NULL ,
	id_gare      INT  NOT NULL ,
	CONSTRAINT prk_constraint_peut_contenir PRIMARY KEY NONCLUSTERED (numero_ligne,id_gare)
);


/*------------------------------------------------------------
-- Table: possède
------------------------------------------------------------*/
CREATE TABLE possede(
	id_gare       INT  NOT NULL ,
	numero_nature INT  NOT NULL ,
	CONSTRAINT prk_constraint_possede PRIMARY KEY NONCLUSTERED (id_gare,numero_nature)
);


/*------------------------------------------------------------
-- Table: inclus
------------------------------------------------------------*/
CREATE TABLE inclus(
	code_postal  INT  NOT NULL ,
	numero_ville INT  NOT NULL ,
	CONSTRAINT prk_constraint_inclus PRIMARY KEY NONCLUSTERED (code_postal,numero_ville)
);



ALTER TABLE gare ADD CONSTRAINT FK_gare_numero_ville FOREIGN KEY (numero_ville) REFERENCES ville(numero_ville);
ALTER TABLE peut_contenir ADD CONSTRAINT FK_peut_contenir_numero_ligne FOREIGN KEY (numero_ligne) REFERENCES ligne(numero_ligne);
ALTER TABLE peut_contenir ADD CONSTRAINT FK_peut_contenir_id_gare FOREIGN KEY (id_gare) REFERENCES gare(id_gare);
ALTER TABLE possede ADD CONSTRAINT FK_possede_id_gare FOREIGN KEY (id_gare) REFERENCES gare(id_gare);
ALTER TABLE possede ADD CONSTRAINT FK_possede_numero_nature FOREIGN KEY (numero_nature) REFERENCES nature(numero_nature);
ALTER TABLE inclus ADD CONSTRAINT FK_inclus_code_postal FOREIGN KEY (code_postal) REFERENCES cp(code_postal);
ALTER TABLE inclus ADD CONSTRAINT FK_inclus_numero_ville FOREIGN KEY (numero_ville) REFERENCES ville(numero_ville);
