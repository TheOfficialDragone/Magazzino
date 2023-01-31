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
VALUES ('$2a$10$ueUorarNJiawj2nOTRx6OODZX51Yk7zUvG7.93crAqsHiSihyu41m', 'Admin', 'Admin', 'admin@test.local', 'parma', '2000/01/01', '1234567890', '1'),
('$2a$10$1lD4enpHueWUza6MpGTsj.5Y1RzCBvLGhSOTjQ5lEGYRk7xeJU7SC', 'magazziniere', 'test', 'magazziniere@test.local', 'parma', '2002/12/12', '1234567890', '2');

INSERT INTO categoria (nome)
VALUES
    ('Elettronica'),
    ('Abbigliamento'),
    ('Casa e giardino'),
    ('Alimentari'),
    ('Bellezza e salute');
	
INSERT INTO prodotto (nome, descrizione, prezzo, quantita, fk_categoria)
VALUES
    ('Smartphone', 'Un telefono intelligente di ultima generazione', 500, 10, 1),
    ('Tablet', 'Un tablet di ultima generazione con schermo da 10 pollici', 300, 5, 1),
    ('Computer portatile', 'Un computer portatile potente e leggero', 1000, 3, 1),
    ('Giacca invernale', 'Una giacca calda e confortevole per lo inverno', 100, 20, 2),
    ('Maglione', 'Un maglione caldo e confortevole', 50, 25, 2),
    ('Jeans', 'Un paio di jeans di alta qualità', 70, 15, 2),
    ('Lavatrice', 'Una lavatrice di ultima generazione con programmi speciali', 500, 10, 3),
    ('Aspirapolvere', 'Un aspirapolvere potente e silenzioso', 200, 5, 3),
    ('Sedie da giardino', 'Delle comode sedie da giardino in legno', 100, 20, 3),
    ('Pane', 'Un pane fresco e fragrante fatto in casa', 2, 50, 4),
    ('Latte', 'Un latte fresco e di alta qualità', 3, 40, 4),
    ('Uova', 'Delle uova fresche e di alta qualità', 4, 30, 4),
    ('Crema idratante', 'Una crema idratante per la pelle', 20, 20, 5),
    ('Shampoo', 'Uno shampoo per capelli forti e sani', 10, 25, 5),
    ('Dentifricio', 'Un dentifricio per denti forti e sani', 5, 30, 5);