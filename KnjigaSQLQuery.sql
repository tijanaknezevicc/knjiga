create database knjiga

use knjiga

create table knjiga(
    id int identity(0,1),
    naslov nvarchar(30),
    autor nvarchar(30),
    brStrana int,
    povez nvarchar(30)
)

