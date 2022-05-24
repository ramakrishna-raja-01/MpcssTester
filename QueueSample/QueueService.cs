using Apache.NMS;
using Apache.NMS.ActiveMQ;
using MpcssApp.Dtos.MPCSS;
using QueueSample.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QueueSample
{
    public class QueueService
    {

        
        public async void TestMpcss()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                /*// HTTP GET
                HttpResponseMessage response = await client.GetAsync("api/products/1");
                if (response.IsSuccessStatusCode)
                {
                    Product product = await response.Content.ReadAsAsync<Product>();
                    Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }
*/
                // HTTP POST
                var mpcssInput = new MpcssRequest() 
                { 
                    MessageIdentificationCode = "123",
                    CustomerId = "111", 
                    ParticipantIdentificationCode = "test" };
                var response = await client.PostAsJsonAsync("weatherforecast/sample", mpcssInput);
                if (response.IsSuccessStatusCode)
                {
                    Uri gizmoUrl = response.Headers.Location;

                   /* // HTTP PUT
                    gizmo.Price = 80;   // Update price
                    response = await client.PutAsJsonAsync(gizmoUrl, gizmo);*/

                    // HTTP DELETE
                    response = await client.DeleteAsync(gizmoUrl);
                }
            }
        }



    }
}
