using System.Text;
using FluentValidation;
using ItemManagement.Data;
using ItemManagement.Services;
using ItemManagement.Endpoints;
using Microsoft.OpenApi.Models;
using ItemManagement.Repository;
using ItemManagement.Validators;
using ItemManagement.Middlewares;
using ItemManagement.Common.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ItemManagement.Domain.Interface;
using ItemManagement.Services.Interfaces;
using ItemManagement.ApplicationConstants;
using ItemManagement.Common.Service.Interface;
using ItemManagement.Domain.Models.RequestModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ItemManagement.Configuration;

public static class Configuration
{
	public static void ConnectDatabase(this IServiceCollection services)
	{
		services.AddDbContext<ItemManagementDbContext>(options => options.UseNpgsql(Constants.CONNECTION_STRING));
	}

	public static void AddCorsPolicy(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("AllowFrontEnd",
				builder =>
				{
					builder.WithOrigins("http://localhost:3000", "http://localhost:5101")
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				});
		});
	}


	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			var securityScheme = new OpenApiSecurityScheme
			{
				Name = "JWT Authentication",
				Description = "Enter your JWT token in this field",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT"
			};

			c.AddSecurityDefinition("Bearer", securityScheme);

			var securityRequirement = new OpenApiSecurityRequirement
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
					new string[] {}
				}
			};

			c.AddSecurityRequirement(securityRequirement);

			c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthCore API", Version = "v1" });
		});
	}

	public static void AddCustomAuthentication(this IServiceCollection services)
	{
		services
		.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.PRIVATE_KEY)),
				ValidateIssuer = false,
				ValidateAudience = false
			};

			x.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					var token = context.Request.Cookies["token"];
					if (!string.IsNullOrEmpty(token))
					{
						context.Token = token;
					}
					return Task.CompletedTask;
				}
			};

		});
	}

	public static void AddCustomAuthorization(this IServiceCollection services)
	{
		services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin", policy => policy.RequireRole("Network Admin"));
			options.AddPolicy("Developer", policy => policy.RequireRole("Developer"));
		});
	}

	public static void RegisterMiddleWares(this IServiceCollection services)
	{
		services.AddTransient<ExceptionMiddleware>();
	}

	public static void RegisterServices(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IItemTypeService, ItemTypeService>();
		services.AddScoped<IItemModelService, ItemModelService>();
		services.AddScoped<IPurchaseRequestService, PurchaseRequestService>();
		services.AddScoped<IUserItemRequestService, UserItemRequestService>();
		services.AddScoped<IUserItemReturnRequestService, UserItemReturnRequestService>();
		services.AddScoped<IUserItemService, UserItemService>();
		services.AddScoped<ICommonService, CommonService>();
		services.AddScoped<IOpenService, OpenService>();
	}

	public static void RegisterRepository(this IServiceCollection services)
	{
		services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
		services.AddScoped<IItemModelRepository, ItemModelRepository>();
		services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();
		services.AddScoped<IUserItemRequestRepository, UserItemRequestRepository>();
		services.AddScoped<IUserItemReturnRequestRepository, UserItemReturnRequestRepository>();
		services.AddScoped<IUserItemRepository, UserItemRepository>();
		services.AddScoped<IOpenRepository, OpenRepository>();
	}

	public static void RegisterAPIs(this WebApplication app)
	{
		app.RegisterAuthenticationAPIs();
		app.RegisterItemTypeAPIs();
		app.RegisterItemModelAPIs();
		app.RegisterPurchaseRequestAPIs();
		app.RegisterUserItemAPIs();
		app.RegisterUserItemRequestAPIs();
		app.RegisterUserItemReturnRequestAPIs();
		app.RegisterUserAPIs();
		app.RegisterOpenAPIs();
	}

	public static void RegisterValidators(this IServiceCollection services)
	{
		services.AddScoped<IValidator<AddUserRequestModel>, AddUserValidator>();
		services.AddValidatorsFromAssemblyContaining<AddUserValidator>();

		services.AddScoped<IValidator<ResetPasswordRequestModel>, ResetPasswordValidator>();
		services.AddValidatorsFromAssemblyContaining<ResetPasswordValidator>();

		services.AddScoped<IValidator<AddItemTypeRequestModel>, AddItemTypeValidator>();
		services.AddValidatorsFromAssemblyContaining<AddItemTypeValidator>();

		services.AddScoped<IValidator<AddItemModelRequestModel>, AddItemModelValidator>();
		services.AddValidatorsFromAssemblyContaining<AddItemModelValidator>();

		services.AddScoped<IValidator<PurchaseRequestWithItemModels>, AddPurchaseRequestModelValidator>();
		services.AddValidatorsFromAssemblyContaining<AddPurchaseRequestModelValidator>();

		services.AddScoped<IValidator<AddUserItemWithQuantityRequestModel>, AddUserItemRequestValidator>();
		services.AddValidatorsFromAssemblyContaining<AddUserItemRequestValidator>();

		services.AddScoped<IValidator<AddUserItemWithQuantityRequestModel>, AddUserItemReturnRequestValidator>();
		services.AddValidatorsFromAssemblyContaining<AddUserItemReturnRequestValidator>();

		services.AddScoped<IValidator<UpdateUserItemWithQuantityRequestModel>, UpdateUserItemRequestValidator>();
		services.AddValidatorsFromAssemblyContaining<UpdateUserItemRequestValidator>();

		services.AddScoped<IValidator<ForgotPasswordRequestModel>, ForgotPasswordValidator>();
		services.AddValidatorsFromAssemblyContaining<ForgotPasswordValidator>();
	}
}
