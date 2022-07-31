using Cake.Core;

namespace Build.Context
{
    public class BuildContext : CoreContext
    {

        public BuildContext(ICakeContext context)
          : base(context)
        {
            Version = context.Arguments.GetArgument("app_version");
            Username = context.Arguments.GetArgument("docker_username");
            Password = context.Arguments.GetArgument("docker_password");
            Tags = context.Arguments.GetArguments("tags");
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
    }
}