﻿using Newtonsoft.Json;
namespace Module5Homework1.Dtos.Responses;
public class AuthDto
{
    [JsonProperty("email")]
    public string Email { get; set; } = null!;

    [JsonProperty("password")]
    public string Password { get; set; } = null!;
}
