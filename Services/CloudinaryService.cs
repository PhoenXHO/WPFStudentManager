using CloudinaryDotNet;
using dotenv.net;

namespace StudentManager.Services
{
    public static class CloudinaryService
    {
        private static Cloudinary? _cloudinary;

        static CloudinaryService()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            var env = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            _cloudinary = new Cloudinary(env);
        }

        public static Cloudinary GetCloudinary()
        {
            return _cloudinary ?? throw new InvalidOperationException("Cloudinary is not initialized.");
        }
    }
}
