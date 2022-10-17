DROP DATABASE IF EXISTS magazzino;
CREATE DATABASE IF NOT EXISTS magazzino;

USE magazzino;

CREATE TABLE categoria (
  IDcategoria int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nome varchar(255) NOT NULL
);

CREATE TABLE prodotto (
  IDprodotto int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  nome varchar(255) NOT NULL,
  descrizione text NOT NULL,
  prezzo text NOT NULL,
  quantita int(11) NOT NULL,
  fk_categoria int(11) NOT NULL,
  Foreign Key (fk_categoria) REFERENCES categoria(IDcategoria) ON DELETE CASCADE
);

CREATE TABLE account(
  IDutente int(11) NOT NULL AUTO_INCREMENT PRIMARY KEY,
  password varchar(255) NOT NULL,
  nome text NOT NULL,
  cognome text NOT NULL,
  email text NOT NULL,
  indirizzo text NOT NULL,
  data_nascita date NOT NULL,
  telefono text NOT NULL,
  TipoAccount int(10) NOT NULL
 );

-- assicurarsi di effettuare il bcrypt di 1 e test prima di updare sql
INSERT INTO account(password, nome, cognome, email, indirizzo, data_nascita, telefono, TipoAccount)
VALUES ('1', 'Admin', 'Admin', 'admin@test.local', 'parma', '2000/01/01', '1234567890', '1'),
('test', 'magazziniere', 'test', 'magazziniere@test.local', 'parma', '2002/12/12', '1234567890', '2')
