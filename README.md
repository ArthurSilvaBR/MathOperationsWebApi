# MathOperations WebApi

 A simple web api to evaluate math expressions.
 

# Expression Rules

- Parenthesis are not allowed

- Only these operators are supported:

- Addition (`+`)

- Subtraction (`-`)

- Multiplication (`*`)

- Division (`/`)

- Negative numbers are not supported

  
  

# Examples of valid expressions

- `1+1`

- `1+1-1`

- `1+2*3-4/3`

  

# How to test

1. Clone this repository

2. Run the `MathOperations.WebApi` project

3. When Swagger page opens, test the `POST /api/MathOperations` route with the "Try it out" button

  

# Notes

1. All operations are executed using `decimal` C# type [Reference](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types)

2. For educational purposes, no authentication was implemented