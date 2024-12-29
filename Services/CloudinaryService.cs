using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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

        public static string UploadImage(string filePath)
        {
            var cloudinary = GetCloudinary();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public static void DeleteImage(string publicId)
        {
            var cloudinary = GetCloudinary();
            var result = cloudinary.DeleteResources(publicId);
        }
    }
}
