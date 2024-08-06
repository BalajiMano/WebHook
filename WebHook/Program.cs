
namespace WebHook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.MapPost("/WebHook", async context =>
            {
                if(!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; return;

                }
                var apikey = context.Request.Headers["Authorization"];
                if(apikey!="APIKEY")
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; return;
                }
                var requestbody = await context.Request.ReadFromJsonAsync<WebHookPayload>();
              
                Console.WriteLine($"header: {requestbody?.Header} and Body: {requestbody?.Body}");
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("WebHook processed the event");



            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
    public record WebHookPayload (string Header, String Body);
}
