using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data.SqlClient;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using NSSERP.Models;

public class AccessController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccessController> _logger;

    public AccessController(IConfiguration configuration, ILogger<AccessController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(NSSLogin nSSLogin)
    {
        string connectionString = _configuration.GetConnectionString("Constr");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlTransaction transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                string authenticationQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                string userQuery = "SELECT Fullname, Role, Department,REF_ID as UserID FROM Users WHERE Username = @Username AND Password = @Password";

                using (SqlCommand authenticationCommand = new SqlCommand(authenticationQuery, connection, transaction))
                {
                    authenticationCommand.Parameters.AddWithValue("@Username", nSSLogin.Username);
                    authenticationCommand.Parameters.AddWithValue("@Password", nSSLogin.Password);

                    int count = (int)authenticationCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Authentication successful

                        // Fetch additional user information
                        string fullname = null;
                        string role = null;
                        string department = null;
                        string UserID = null;
                        string FinYear = GetCurrentFiscalYear();
                        using (SqlCommand userCommand = new SqlCommand(userQuery, connection, transaction))
                        {
                            userCommand.Parameters.AddWithValue("@Username", nSSLogin.Username);
                            userCommand.Parameters.AddWithValue("@Password", nSSLogin.Password);

                            using (SqlDataReader reader = userCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Read additional user information from the database
                                    fullname = reader["Fullname"].ToString();
                                    role = reader["Role"].ToString();
                                    department = reader["Department"].ToString();
                                    UserID = reader["UserID"].ToString();
                                }
                            }
                        }

                        // Log IP address
                        var ipAddress = GetClientIpAddress(HttpContext);
                        _logger.LogInformation($"Login Device - IP Address: {ipAddress}");

                        // Log device information
                        var userAgent = HttpContext.Request.Headers["User-Agent"];
                        var deviceInfo = GetDeviceInfo(userAgent);
                        _logger.LogInformation($"Login Device: {deviceInfo}");

                        // Store IP address in a variable
                        string clientIpAddress = ipAddress;

                        // Log into the database using ADO.NET transaction
                        LogToDatabase(connection, transaction, nSSLogin.Username, clientIpAddress, deviceInfo);

                        // Continue with authentication
                        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nSSLogin.Username),
                // Add more claims as needed
                new Claim(ClaimTypes.Role, role),
                new Claim("Department", department),
                new Claim("UserID",UserID),
                new Claim("FinYear",FinYear),
                new Claim("DataFlag","GANGOTRI")
            };

                        // Add 'stay logged in' claim if the checkbox is checked
                        if (nSSLogin.KeepLoggedIn)
                        {
                            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "Persistent"));
                        }

                        // Add the full name claim
                        if (!string.IsNullOrEmpty(fullname))
                        {
                            claims.Add(new Claim(ClaimTypes.Name, fullname));
                        }

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity);

                        // Use SignInAsync to handle authentication
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IsPersistent = nSSLogin.KeepLoggedIn,
                        });

                        // Log user authentication AFTER SignInAsync
                        _logger.LogInformation($"User authenticated: {nSSLogin.Username}");

                        ViewData["User"] = nSSLogin.Username;
                        // Store FiscalYear in session



                        transaction.Commit(); // Commit the transaction if everything is successful
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log, and potentially roll back the transaction
                _logger.LogError($"Error during login transaction: {ex.Message}");
                transaction?.Rollback();
            }

            ViewData["validatemsg"] = "Invalid username or password";

            return View();
        }
    }

    private void LogToDatabase(SqlConnection connection, SqlTransaction transaction, string username, string ipAddress, string deviceInfo)
    {
        try
        {
            // Insert log information into the database using the existing connection and transaction
            string insertQuery = "INSERT INTO Logs (Username, IPAddress, DeviceInfo, LogDateTime) VALUES (@Username, @IPAddress, @DeviceInfo, GETDATE())";
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
            {
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@IPAddress", ipAddress);
                insertCommand.Parameters.AddWithValue("@DeviceInfo", deviceInfo);
                insertCommand.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, log, and potentially roll back the transaction
            _logger.LogError($"Error during database logging: {ex.Message}");
            transaction?.Rollback();
            throw; // Rethrow the exception for further handling at a higher level
        }
    }

    private string GetClientIpAddress(HttpContext context)
    {
        // Attempt to read the client IP address from the HTTP headers
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        // If the IP address is null, try to read it from the forwarded headers
        if (string.IsNullOrEmpty(ipAddress) && context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ipAddress = context.Request.Headers["X-Forwarded-For"];
        }

        // If the IP address is still null, use the connection's local address
        return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.LocalIpAddress?.ToString();
    }

    private string GetDeviceInfo(string userAgent)
    {
        // Extract basic information from the User-Agent header
        var browser = "Unknown Browser";
        var os = "Unknown OS";

        if (!string.IsNullOrEmpty(userAgent))
        {
            // Check for common browsers
            if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                browser = "Internet Explorer";
            }
            else if (userAgent.Contains("Edge"))
            {
                browser = "Microsoft Edge";
            }
            else if (userAgent.Contains("Chrome"))
            {
                browser = "Google Chrome";
            }
            else if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
            {
                browser = "Safari";
            }
            else if (userAgent.Contains("Firefox"))
            {
                browser = "Mozilla Firefox";
            }

            // Check for common operating systems
            if (userAgent.Contains("Windows"))
            {
                os = "Windows";
            }
            else if (userAgent.Contains("Mac OS"))
            {
                os = "Mac OS";
            }
            else if (userAgent.Contains("Android"))
            {
                os = "Android";
            }
            else if (userAgent.Contains("iOS"))
            {
                os = "iOS";
            }
            // Add more checks for other operating systems as needed
        }

        // Construct the device information string
        var deviceInfo = $"{os} - {browser}";

        return deviceInfo;
    }

    private string GetCurrentFiscalYear()
    {
        // Get the current date
        DateTime currentDate = DateTime.Today;

        // Determine the fiscal year based on your criteria
        int fiscalYear = currentDate.Month >= 4 ? currentDate.Year : currentDate.Year - 1;

        // Create a string representation of the fiscal year (e.g., "2023-2024")
        string fiscalYearString = $"{fiscalYear}{fiscalYear + 1}";

        return fiscalYearString;
    }
}
