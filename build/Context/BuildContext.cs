using System.Threading.Tasks;
using Build.Steps.Deploy;
using Cake.Core;

namespace Build.Context
{
    public class BuildContext : CoreContext
    {

        public BuildContext(ICakeContext context)
          : base(context)
        {
            Version = context.Arguments.GetArgument("app_version");
            Username = context.Arguments.GetArgument("username");
            Password = context.Arguments.GetArgument("password");
            Tags = context.Arguments.GetArguments("tags");
            HelmRepositoryAddress = context.Arguments.GetArgument("helm_repository_address");
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        public string HelmRepositoryAddress { get; set; }
    }
}