DROP DATABASE IF EXISTS magazzino;
CREATE DATABASE if NOT EXISTS magazzino;

USE magazzino;

CREATE TABLE account (
  IDlogin int(10) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  password varchar(255) NOT NULL
);

CREATE TABLE amministratore (
  email varchar(50) NOT NULL PRIMARY KEY,
  nome text NOT NULL,
  cognome text NOT NULL,
  fk_login int(10) NOT NULL Foreign Key REFERENCES account(IDlogin) ON DELETE CASCADE
);

CREATE TABLE categoria (
  IDcategoria int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nome varchar(255) NOT NULL
);

CREATE TABLE prodotto (
  IDprodotto int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nome varchar(255) NOT NULL,
  descrizione text NOT NULL,
  prezzo float NOT NULL,
  quantita int(11) NOT NULL,
  fk_categoria int(11) NOT NULL Foreign Key REFERENCES categoria(IDcategoria) ON DELETE CASCADE
);

CREATE TABLE ordine (
  IDordine int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  data date NOT NULL
);

CREATE TABLE composizione (
  IDcomposizione int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  fk_prodotto int(11) NOT NULL Foreign Key REFERENCES prodotto(IDprodotto) ON DELETE CASCADE,,
  fk_ordine int(11) NOT NULL Foreign Key REFERENCES ordine(IDordine) ON DELETE CASCADE,
  quantita int(11) NOT NULL
);

CREATE TABLE utente (
  IDutente int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nome text NOT NULL,
  cognome text NOT NULL,
  email text NOT NULL,
  indirizzo text NOT NULL,
  data_nascita date NOT NULL,
  telefono text NOT NULL,
  fk_login int(10) NOT NULL Foreign Key REFERENCES account(IDlogin) ON DELETE CASCADE
 );