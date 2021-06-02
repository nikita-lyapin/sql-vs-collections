create table public.visit
(
    id integer,
    person_id integer,
    visit_datetime timestamp without time zone,
    spent money
) tablespace pg_default;

create table public.person
(
    id integer,
    first_name character varying(100) COLLATE pg_catalog."default",
    last_name character varying(100) COLLATE pg_catalog."default",
    is_active boolean
) tablespace pg_default;