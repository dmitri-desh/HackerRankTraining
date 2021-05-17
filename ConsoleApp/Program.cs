using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;


namespace ConsoleApp
{
    class Program
    {
        private const string URL = "https://jsonmock.hackerrank.com/api/article_users";
     
        static void Main(string[] args)
        {
            #region Rotate Left
            //string[] firstMultipleInput = Console.ReadLine().TrimEnd().Split(' ');

            //int n = Convert.ToInt32(firstMultipleInput[0]);

            //int d = Convert.ToInt32(firstMultipleInput[1]);

            //List<int> arr = Console.ReadLine().TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();

            //d = d % n;
            //List<int> result = Result.rotateLeft(d, arr);

            //Console.WriteLine(String.Join(" ", result));
            #endregion

            #region Sparse Arrays
            //int stringsCount = Convert.ToInt32(Console.ReadLine().Trim());

            //List<string> strings = new List<string>();

            //for (int i = 0; i < stringsCount; i++)
            //{
            //    string stringsItem = Console.ReadLine();
            //    strings.Add(stringsItem);
            //}

            //int queriesCount = Convert.ToInt32(Console.ReadLine().Trim());

            //List<string> queries = new List<string>();

            //for (int i = 0; i < queriesCount; i++)
            //{
            //    string queriesItem = Console.ReadLine();
            //    queries.Add(queriesItem);
            //}

            //List<int> res = Result2.matchingStrings(strings, queries);

            //Console.WriteLine(String.Join("\n", res));
            #endregion

            #region FizzBuzz
            //int n = Convert.ToInt32(Console.ReadLine().Trim());

            //Result3.fizzBuzz(n);
            #endregion

            #region Most Active Authors
            Console.Write("Input threshold > ");
            int threshold = Convert.ToInt32(Console.ReadLine().Trim());
            var list = PageObject.GetUsernames(threshold);
            Console.WriteLine(String.Join(" ", list));
            #endregion

            Console.Write("\nPress any key to stop...");
            Console.ReadKey();
        }

        #region for Active Authors
        public class DataObject
        {
            public long? Id { get; set; }
            public string Username { get; set; }
            public string About { get; set; }
            public int? Submitted { get; set; }
            public DateTime? Updated_At { get; set; }
            public int? Submission_Count { get; set; }
            public int? Comment_Count { get; set; }
            public int Created_At { get; set; }
        }

        public class PageObject
        {
            public int Page { get; set; }
            public int Per_Page { get; set; }
            public int Total { get; set; }
            public int Total_Pages { get; set; }
            public IEnumerable<DataObject> Data { get; set; }
            public static List<string> GetUsernames(int threshold)
            {
                string urlParameters = "?page=";
                int page = 1;
                int totalPages = 1;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var formatters = new List<MediaTypeFormatter>() {
                new JsonMediaTypeFormatter(),
                new XmlMediaTypeFormatter()
            };

                var data = new List<DataObject>();
                var pageObject = new PageObject();
                do
                {
                    HttpResponseMessage response = client.GetAsync($"{urlParameters}{page}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        pageObject = response.Content.ReadAsAsync<PageObject>(formatters).Result;
                        data.AddRange(pageObject.Data);
                        totalPages = pageObject.Total_Pages;
                        page++;
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                while (page <= totalPages);

                Console.WriteLine($"Total authors is {data.Count}. Threshold is {threshold}. Total Pages is {pageObject.Total_Pages}");

                foreach (var d in data)
                {
                    if (d.Submission_Count >= threshold)
                    {
                        Console.WriteLine($"{d.Username} has {d.Submission_Count} articles");
                    }
                }

                client.Dispose();

                return data.Where(x => x.Submission_Count >= threshold).Select(x => x.Username).ToList();
            }
        }
        #endregion

        #region for Rotate Left
        public class Result
        {

            /*
             * Complete the 'rotateLeft' function below.
             *
             * The function is expected to return an INTEGER_ARRAY.
             * The function accepts following parameters:
             *  1. INTEGER d
             *  2. INTEGER_ARRAY arr
             */

            public static List<int> rotateLeft(int d, List<int> arr)
            {
                var temp = new List<int>();
            
                for (int i = d+1; i < arr.Count; i++)
                {
                    temp.Add(arr[i]);
                }
                for (int i = 0; i <= d; i++)
                {
                    temp.Add(arr[i]);
                }

                return temp;
            }
        }
        #endregion

        #region for Sparse Arrays
        public class Result2
        {
            /*
             * Complete the 'matchingStrings' function below.
             *
             * The function is expected to return an INTEGER_ARRAY.
             * The function accepts following parameters:
             *  1. STRING_ARRAY strings
             *  2. STRING_ARRAY queries
             */

            public static List<int> matchingStrings(List<string> strings, List<string> queries)
            {
                var stringMap = new Dictionary<string, int>();
                foreach(var str in strings)
                {
                    if (stringMap.ContainsKey(str))
                        stringMap[str]++;
                    else
                        stringMap.Add(str, 1);
                }

                var output = new int[queries.Count];
                int i = 0;
                foreach (var queryString in queries)
                {
                    if (stringMap.ContainsKey(queryString))
                        output[i] = stringMap[queryString];

                    i++;
                }

                return output.ToList();
            }
        }
        #endregion

        #region for FizzBuzz
        public class Result3
        {
            /*
            * Complete the 'fizzBuzz' function below.
            *
            * The function accepts INTEGER n as parameter.
            */

            public static void fizzBuzz(int n)
            {
                for (int i = 1; i <= n; i++)
                {
                    bool mult3 = i % 3 == 0;
                    bool mult5 = i % 5 == 0;

                    if (mult3 && mult5)
                        Console.WriteLine("FizzBuzz");
                    if (mult3 && !mult5)
                        Console.WriteLine("Fizz");
                    if (!mult3 && mult5)
                        Console.WriteLine("Buzz");
                    if (!mult3 && !mult5)
                        Console.WriteLine($"{i}");
                }
            }
        }
        #endregion
    }
}
