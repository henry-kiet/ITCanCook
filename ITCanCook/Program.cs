﻿using ITCanCook_BusinessObject.Helper.Implement;
using ITCanCook_BusinessObject;
using ITCanCook_BusinessObject.Service.Implement;
using ITCanCook_BusinessObject.Service.Interface;
using ITCanCook_DataAcecss;
using ITCanCook_DataAcecss.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ITCanCook_DataAcecss.Repository.Interface;
using ITCanCook_DataAcecss.Repository.Implement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));// cho phép mọi người truy cập endpoint


//Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ITCanCookContext>()
.AddDefaultTokenProviders();


// Đọc ConnectionString từ appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<ITCanCookContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("ITCanCook"));
});

builder.Services.AddAuthentication(op =>
{
	op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
	{
		op.SaveToken = true;
		op.RequireHttpsMetadata = false;
		op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = builder.Configuration["JWT:ValidAudience"],
			ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:IssuerSigningKey"]))
		};
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
		new OpenApiSecurityScheme
		{
			Reference = new OpenApiReference
			{
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			}
		},
		new string[]{ }
		}
	});
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IEmailService, SendMail>();


//đăng ký mapper
builder.Services.AddAutoMapper(typeof(MapperProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
