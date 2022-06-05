using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Hubteste;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("v1/MensagemBroker")]
    public class MensagemBrokerController : ControllerBase
    {
        private readonly IHubContext<MyHub> hubContext;
       // List<MensagemBroker> ListaMensagem = new List<MensagemBroker>();

        public MensagemBrokerController(IHubContext<MyHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpGet]
        [Route("")]

        public async Task<ActionResult<List<MensagemBroker>>> Get([FromServices] DataContext context)
        {
            Thread.Sleep(1000);
            var mensagens = await context.MensagemBrokers.ToListAsync();
            return mensagens;
        }

        [HttpPost]
        [Route("")]

        public async Task<ActionResult<MensagemBroker>> Post([FromServices] DataContext context, [FromBody] MensagemBroker model)
        {
            if (ModelState.IsValid)
            {
                context.MensagemBrokers.Add(model);
                await hubContext.Clients.All.SendAsync("post", model);
                await context.SaveChangesAsync();
                return model;

            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
