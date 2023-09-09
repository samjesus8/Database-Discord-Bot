create schema if not exists data;

create table if not exists data."userinfo"
(
	"userno" bigint primary key,
	"username" varchar not null,
	"servername" varchar not null,
	"serverid" bigint not null
);