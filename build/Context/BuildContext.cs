using Cake.Core;

namespace Build.Context
{
    public class BuildContext : CoreContext
    {
        public BuildContext(ICakeContext context) :
            base(context)
        {
            EntryPoint = context.Arguments.GetArgument("entry_point");
            Version = context.Arguments.GetArgument("app_version");
            Username = context.Arguments.GetArgument("docker_username");
            Password = context.Arguments.GetArgument("docker_password");
            Tags = context.Arguments.GetArguments("tags");
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        public string EntryPoint { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
