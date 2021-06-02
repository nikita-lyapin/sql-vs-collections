set enable_hashjoin to 'off';
set enable_mergejoin to 'off';
set enable_material to 'off';

explain analyze
select sum(v.spent) from public.visit v
                             join public.person p on p.id = v.person_id
where v.visit_datetime <= '2020-12-31' and p.is_active = True
