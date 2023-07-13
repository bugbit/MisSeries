using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MisSeries.Trakt.Build
{
    public class CreateTraktSecretsHFile : Microsoft.Build.Utilities.Task
    {
        [Required] public required string Include { get; set; }

        public override bool Execute()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets("TraktSecrets") //Nombre de la carpeta que hemos creado
                .Build();

            Log.LogMessage($"Include: {Include}");

            throw new Exception();

            //$(BuildDir)
            //this.BuildEngine5.

            return true;
        }
    }
}