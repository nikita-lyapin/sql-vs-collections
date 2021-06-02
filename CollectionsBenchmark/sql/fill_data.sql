select setseed(0.777);

delete from public.person;
insert into public.person(id, first_name, last_name, is_active)
select row_number() over () as id,
		substr(md5(random()::text), 1, 10) as first_name,
		substr(md5(random()::text), 1, 10) as last_name,
		((row_number() over ()) % 5 = 0) as is_active
from generate_series(1, 5000);/*<-- 5000 это число клиентов*/

delete from public.visit;
insert into public.visit(id, person_id, visit_datetime, spent)
select row_number() over () as id,
		(random()*5000)::integer as person_id, /*<-- 5000 это число клиентов*/
		DATE '2020-01-01' + (random() * 500)::integer as visit_datetime,
		(random()*10000)::integer as spent
from generate_series(1, 8000); /* 8000 - это общее число визитов в СТО*/

