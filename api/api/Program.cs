
using api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendURL!).AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddIdentityServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
