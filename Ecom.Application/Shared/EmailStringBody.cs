

namespace Ecom.Application.Shared
{
    public class EmailStringBody
    {
        /// <summary>
        /// component variable is used to determine Component name in angular app
        /// </summary>
       
        //for example if the component name is Active then the link will be: http://localhost:4200/Account/Active?email={email}&code={encodeToken}
        public static string SendEmailBody(string email, string token, string component,string message)
        {
            var encodeToken = Uri.EscapeDataString(token);

            return $@"
                     <html> 

                       <head>

                         <style>
                            .button
                            {{
                                border: none;
                                border-radius: 10px;
                                padding: 15px 30px;
                                color: #fff;
                                display: inline-block;
                                background: linear-gradient(45deg, #ff7e5f, #feb47b);
                                cursor: pointer;
                                text-decoration: none;
                                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
                                transition: all 0.3s ease;
                                font-size: 16px;
                                font-weight: bold;
                                font-family: 'Arial', sans-serif;
                                animation: glow 1.5s infinite alternate;
                             }}
                          </style>
                        </head>

                        <body>
                          <h1>{message}</h1>
                          <hr>
                          <br>
                          <a class=""button"" href=""http://localhost:4200/Account/{component}?email={email}&code={encodeToken}"">
                             {message}
                          </a>
                        </body>

                      </html>";

        }
    }
}
