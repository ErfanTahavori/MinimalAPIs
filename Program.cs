using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIs;
using MinimalAPIs.Data;
using MinimalAPIs.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) =>
{
    APIResponse Responce = new();
    _logger.Log(LogLevel.Information, "Getting All Coupons");
    Responce.Result = CouponStore.couponList;
    Responce.IsSuccess = true;
    Responce.StatusCode = HttpStatusCode.OK;
    return Results.Ok(Responce);
}).WithName("GetCoupons").Produces<APIResponse>(200);



app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    APIResponse Responce = new();
    Responce.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    Responce.IsSuccess = true;
    Responce.StatusCode = HttpStatusCode.OK;
    return Results.Ok(Responce);
}).WithName("GetCoupon").Produces<APIResponse>(200);



app.MapPost("/api/coupon", async (IMapper _mapper, IValidator<CouponCreateDTO> _validator, [FromBody] CouponCreateDTO coupon_C_DTO) =>
{
    //Response
    APIResponse Responce = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    return Results.Ok(Responce);

    //validation Check
    var ValidationResult = await _validator.ValidateAsync(coupon_C_DTO);
    if (!ValidationResult.IsValid)
    {
        Responce.ErrorMessages.Add(ValidationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(Responce);
    }
    if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
    {
        Responce.ErrorMessages.Add("Coupon Name already Exists!!");
        return Results.BadRequest(Responce);

    }
    Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

    coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);
    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);


    Responce.Result = couponDTO;
    Responce.IsSuccess = true;
    Responce.StatusCode = HttpStatusCode.Created;
    return Results.Ok(Responce);
    //return Results.CreatedAtRoute("GetCoupon",new { id = coupon.Id }, couponDTO);
    //return Results.Created($"/api/coupon/{coupon.Id}", couponDTO);
}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);



app.MapPut("/api/coupon", async (IMapper _mapper, IValidator<CouponUpdateDTO> _validator, [FromBody] CouponUpdateDTO coupon_Up_DTO) =>
{
    //Response
    APIResponse Responce = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };



    //validation Check
    var ValidationResult = await _validator.ValidateAsync(coupon_Up_DTO);
    if (!ValidationResult.IsValid)
    {
        Responce.ErrorMessages.Add(ValidationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(Responce);
    }

    /*if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == coupon_Up_DTO.Name.ToLower()) != null)
    {
        Responce.ErrorMessages.Add("Coupon Name already Exists!!");
        return Results.BadRequest(Responce);

    }*/

    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == coupon_Up_DTO.Id);


    couponFromStore.IsActive = coupon_Up_DTO.IsActive;
    couponFromStore.Name = coupon_Up_DTO.Name;
    couponFromStore.Percent = coupon_Up_DTO.Percent;
    couponFromStore.LastUpdated = DateTime.Now;



    Responce.Result = _mapper.Map<CouponDTO>(couponFromStore);
    Responce.IsSuccess = true;
    Responce.StatusCode = HttpStatusCode.OK;
    return Results.Ok(Responce);

}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400); ;



app.MapDelete("/api/coupon/{id:int}", (int Id) =>
{
    APIResponse Responce = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };





    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == Id);
    if (couponFromStore != null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        Responce.IsSuccess = true;
        Responce.StatusCode = HttpStatusCode.OK;
        return Results.Ok(Responce);
    }
    else
    {
        Responce.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(Responce);
    }

});


app.UseHttpsRedirection();


app.Run();


