﻿using System.Net.Http.Headers;
var client = new HttpClient();
var request = new HttpRequestMessage
{
  Method = HttpMethod.Get,
  RequestUri = new Uri("https://localhost:7043/api/books"),
};
using (var response = await client.SendAsync(request))
{
  response.EnsureSuccessStatusCode();
  var body = await response.Content.ReadAsStringAsync();
  Console.WriteLine(body);
}
