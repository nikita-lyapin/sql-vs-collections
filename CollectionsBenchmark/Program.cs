using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dapper;
using Npgsql;

namespace CollectionsBenchmark
{
    [SimpleJob(1, 1, 3)]
    public class GetMostActiveVisistors
    {
        internal static Person[] persons;
        internal static Visit[] visits;

        internal static LinkedList<Person> personsList = new LinkedList<Person>();
        internal static LinkedList<Visit> visitsList = new LinkedList<Visit>();
        
        private static string connectionStringToDb = "Server=localhost;Database=auto_db;User Id=postgres;Password=1;";

        [GlobalSetup]
        public void GlobalSetup()
        {
            using (var connection = new NpgsqlConnection(connectionStringToDb))
            {
                var selectPersonSQL =
                    "select id as \"Id\", first_name as \"FirstName\", last_name  as \"LastName\", is_active  as \"IsActive\" from public.person";
                persons = connection.Query<Person>(selectPersonSQL).ToArray();
                foreach (var person in persons) personsList.AddLast(person);

                var selectVisitSQL =
                    "select id as \"Id\", person_id as \"PersonId\", visit_datetime as \"Date\", spent  as \"Spent\" from public.visit";
                visits = connection.Query<Visit>(selectVisitSQL).ToArray();
                foreach (var visit in visits) visitsList.AddLast(visit);
            }
        }

        [Benchmark]
        public decimal LinqJoin()
        {
            return
                persons.Where(x => x.IsActive)
                    .Join(visits.Where(x => x.Date <= new DateTime(2020, 12, 31)),
                        person => person.Id,
                        visit => visit.PersonId,
                        (person, visit) => visit.Spent).Sum();
        }

        [Benchmark]
        public decimal LinqJoin_LinkedList()
        {
            return
                personsList.Where(x => x.IsActive)
                    .Join(visitsList.Where(x => x.Date <= new DateTime(2020, 12, 31)),
                        person => person.Id,
                        visit => visit.PersonId,
                        (person, visit) => visit.Spent).Sum();
        }


        private bool IsActive(Person person, Visit visit, DateTime upperLimit)
        {
            return person.IsActive && visit.Date <= upperLimit;
        }

        [Benchmark]
        public decimal MergeJoin()
        {
            decimal result = 0;
            var upperLimit = new DateTime(2020, 12, 31);

            Array.Sort(persons);
            Array.Sort(visits);

            int personIndex = 0,
                visitIndex = 0;

            while (true)
            {
                if (personIndex >= persons.Length || visitIndex >= visits.Length) break;

                var person = persons[personIndex];
                var visit = visits[visitIndex];

                if (person.Id == visit.PersonId)
                {
                    if (IsActive(person, visit, upperLimit)) result += visit.Spent;

                    var visitIndexNext = visitIndex + 1;
                    while (true)
                    {
                        if (visitIndexNext >= visits.Length) break;

                        var visitNext = visits[visitIndexNext];

                        if (person.Id == visitNext.PersonId)
                        {
                            if (IsActive(person, visitNext, upperLimit)) result += visitNext.Spent;

                            visitIndexNext++;
                            continue;
                        }

                        break;
                    }

                    var personIndexNext = personIndex + 1;
                    while (true)
                    {
                        if (personIndexNext >= persons.Length) break;

                        var personNext = persons[personIndexNext];

                        if (personNext.Id == visit.PersonId)
                        {
                            if (IsActive(personNext, visit, upperLimit)) result += visit.Spent;

                            personIndexNext++;
                            continue;
                        }

                        break;
                    }

                    personIndex++;
                    visitIndex++;
                }
                else if (person.Id > visit.PersonId)
                {
                    visitIndex++;
                }
                else if (person.Id < visit.PersonId)
                {
                    personIndex++;
                }
            }

            return result;
        }

        [Benchmark]
        public decimal NestedLoop()
        {
            decimal result = 0;
            var upperLimit = new DateTime(2020, 12, 31);

            foreach (var person in persons)
            {
                if (person.IsActive == false)
                {
                    continue;
                }
                
                foreach (var visit in visits)
                {
                    if (person.Id == visit.PersonId && visit.Date <= upperLimit)
                    {
                        result += visit.Spent;
                    }
                }
            }
            return result;
        }

        [Benchmark]
        public decimal NestedLoop_LinkedList()
        {
            decimal result = 0;
            var upperLimit = new DateTime(2020, 12, 31);

            foreach (var person in personsList)
            {
                if (person.IsActive)
                {
                    foreach (var visit in visitsList)
                    {
                        if (person.Id == visit.PersonId && visit.Date <= upperLimit)
                        {
                            result += visit.Spent;
                        }
                    }
                }
            }

            return result;
        }


        [Benchmark]
        public void DataLoad()
        {
            using (var connection =
                new NpgsqlConnection(connectionStringToDb))
            {
                var persons = connection
                    .Query<Person>(
                        "select id as \"Id\", first_name as \"FirstName\", last_name  as \"LastName\", is_active  as \"IsActive\" from public.person")
                    .ToList();
                var visits = connection
                    .Query<Visit>(
                        "select id as \"Id\", person_id as \"PersonId\", visit_datetime as \"Date\", spent  as \"Spent\" from public.visit")
                    .ToList();
            }
        }

        [Benchmark]
        public decimal RunInSQL()
        {
            using (var connection =
                new NpgsqlConnection(connectionStringToDb))
            {
                return connection
                    .Query<decimal>(
                        @"select sum(v.spent) from public.visit v join public.person p on p.id = v.person_id where v.visit_datetime <= '2020-12-31' and p.is_active = True")
                    .Single();
            }
        }
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var bm = new GetMostActiveVisistors();
            bm.GlobalSetup();
            Console.WriteLine(bm.LinqJoin_LinkedList());
            Console.WriteLine(bm.LinqJoin());
            Console.WriteLine(bm.MergeJoin());
            
            var summary = BenchmarkRunner.Run<GetMostActiveVisistors>();
         
        }
    }
}